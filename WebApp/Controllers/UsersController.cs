using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using WebApp.ViewModel;
using Database.Core;
using static Database.Core.Repositories.IUserRepository;
using Database.Core.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity;
using Sprache;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Identity;


namespace WebApp.Controllers
{
    public class UsersController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly RentalDBContext _context;

        // Constructor - Dependency injection of UnitOfWork, UserManager, and EmailSender
        public UsersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender, RentalDBContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        // GET: Display list of users with optional search, filter, and pagination
        public async Task<IActionResult> Index(string? searchString, string? roleFilter, SortOption? sortBy, int page = 1, int pageSize = 10)
        {
            // Ensure user is authenticated and authorized
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized");

            if (!User.IsInRole("Admin"))
                return View("Forbidden");

            try
            {
                // Fetch all users with optional filtering and sorting
                var allUsers = await _unitOfWork.Users.GetUsersAsync(searchString, roleFilter, sortBy);
                var roles = await _unitOfWork.UserRoles.GetAllAsync();

                // Paginate the users list
                var totalUsers = allUsers.Count();
                var users = allUsers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Create the view model for the Index page
                var viewModel = new ListUsersViewModel
                {
                    UsersList = users,
                    RolesList = roles,
                    SearchString = searchString,
                    RoleFilter = roleFilter,
                    CurrentSort = sortBy,
                    SortOptions = Enum.GetValues(typeof(IUserRepository.SortOption)).Cast<IUserRepository.SortOption>(),
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );
                // In case of any unhandled exception, show the error view
                return View("Error");
            }
        }

        // GET: Display Edit view for selected user
        public async Task<IActionResult> Edit(int? id)
        {
            // Security checks
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized");

            if (!User.IsInRole("Admin"))
                return View("Forbidden");

            if (id == null || _unitOfWork.Users == null || id == 0)
                return View("NotFound");

            try
            {
                // Retrieve user by ID along with their role
                var user = await _unitOfWork.Users.GetUserWithRoleAsync(id.Value);
                if (user == null) return View("NotFound");

                // Populate view model with roles and selected user info
                var viewModel = new EditUserViewModel
                {
                    User = user,
                    RolesList = await _unitOfWork.UserRoles.GetAllAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );
                return View("Error");
            }
        }

        // POST: Handle Edit user form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel editUser)
        {
            // Only Admins can perform this action
            if (!User.IsInRole("Admin"))
                return View("Forbidden");

            // Run server-side validation on the User object
            ValidateUser(editUser.User);

            if (!ModelState.IsValid)
            {

                // Return with validation errors and reload role list
                editUser.RolesList = await _unitOfWork.UserRoles.GetAllAsync();
                return View(editUser);
            }

            try
            {


                var trackedUser = await _context.Users.FindAsync(editUser.User.Id);
                if (trackedUser == null)
                {
                    return NotFound();
                }

                var oldData = System.Text.Json.JsonSerializer.Serialize(trackedUser);

                // Update the tracked entity instead of replacing it
                trackedUser.FirstName = editUser.User.FirstName;
                trackedUser.LastName = editUser.User.LastName;
                trackedUser.PhoneNumber = editUser.User.PhoneNumber;
                trackedUser.UpdatedAt = DateTime.Now;

                var newData = System.Text.Json.JsonSerializer.Serialize(trackedUser);


                // Save changes to the custom user table (not Identity)
                await _unitOfWork.Users.UpdateAsync(editUser.User);
                await _unitOfWork.SaveChangesAsync();

                // Update Identity user to keep in sync
                var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == editUser.User.Id);
                if (identityUser != null)
                {
                    // Update basic info
                    identityUser.FirstName = editUser.User.FirstName;
                    identityUser.LastName = editUser.User.LastName;
                    identityUser.Email = editUser.User.Email;
                    identityUser.UserName = editUser.User.Email;
                    identityUser.PhoneNumber = editUser.User.PhoneNumber;
                    identityUser.NormalizedEmail = editUser.User.Email.ToUpper();
                    identityUser.NormalizedUserName = editUser.User.Email.ToUpper();

                    // Remove current roles and assign new role
                    var currentRoles = await _userManager.GetRolesAsync(identityUser);
                    await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

                    if (editUser.User.RoleId != null)
                    {
                        var getRoleName = _unitOfWork.UserRoles.getRoleNameByID((int)editUser.User.RoleId);
                        await _userManager.AddToRoleAsync(identityUser, getRoleName);
                    }

                    // Save changes in Identity system
                    var result = await _userManager.UpdateAsync(identityUser);
                    if (!result.Succeeded)
                    {
                        TempData["MessageText"] = "An error occurred while saving the user.";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index");
                    }
                }

                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await AuditLogger.LogActionAsync(
                    context: _context,
                    userId: userid,
                    actionType: "Update",
                    sourceEntity: "User",
                    dataBefore: oldData,
                    dataAfter: newData,
                    affectedRecordKey: editUser.User.Id.ToString()
                );

                TempData["MessageText"] = "User was saved successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {

                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );
                // Handle case where user may have been modified/deleted by someone else
                if (!await _unitOfWork.Users.UserExistsAsync(editUser.User.Id))
                    return View("NotFound");
                else
                    throw;
            }
            catch (Exception ex)
            {

                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );

                // Handle other exceptions
                TempData["faild"] = "Failed to update user.";
                editUser.RolesList = await _unitOfWork.UserRoles.GetAllAsync();
                return View(editUser);
            }
        }

        // POST: Handle user deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (!User.IsInRole("Admin"))
                return View("Forbidden");

            if (id == null || _unitOfWork.Users == null)
                return View("NotFound");

            try
            {

                var trackedUser = await _context.Users.FindAsync(id);
                if (trackedUser == null)
                {
                    return NotFound();
                }

                var oldData = System.Text.Json.JsonSerializer.Serialize(trackedUser);

                

                // Get user by ID
                var user = await _unitOfWork.Users.GetAsync(id.Value);
                if (user == null) return View("NotFound");

                // Delete from Identity system
                var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == user.Id);
                if (dbUser != null)
                {
                    var identityResult = await _userManager.DeleteAsync(dbUser);
                    if (!identityResult.Succeeded)
                        return Json(new { success = false, message = "Failed to delete user from Identity DB", type = "error" });
                }

                // Soft delete: mark inactive and change email in main DB
                user.IsActive = false;
                user.Email = "Deleted" + user.Id;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();



                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await AuditLogger.LogActionAsync(
                    context: _context,
                    userId: userid,
                    actionType: "Delete",
                    sourceEntity: "User",
                    dataBefore: oldData,
                    dataAfter:  " ",
                    affectedRecordKey: id.ToString()
                );

                return Json(new
                {
                    success = true,
                    message = "User deleted successfully.",
                    redirectUrl = Url.Action("Index"),
                    type = "success"
                });
            }
            catch (Exception ex)
            {

                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );

                return Json(new { success = false, message = "Unexpected error occurred.", type = "error" });
            }
        }

        // Server-side validation for user inputs
        private void ValidateUser(User user)
        {
            // Check role selection
            if (user.RoleId == null || user.RoleId == 0)
                ModelState.AddModelError("User.RoleId", "User Role must be selected.");

            // First name validation
            if (string.IsNullOrWhiteSpace(user.FirstName))
                ModelState.AddModelError("User.FirstName", "First name is required.");
            else if (user.FirstName.Length < 3)
                ModelState.AddModelError("User.FirstName", "First name must contain at least 3 characters.");

            // Last name validation
            if (string.IsNullOrWhiteSpace(user.LastName))
                ModelState.AddModelError("User.LastName", "Last name is required.");
            else if (user.LastName.Length < 3)
                ModelState.AddModelError("User.LastName", "Last name must contain at least 3 characters.");

            // Email validation
            if (string.IsNullOrWhiteSpace(user.Email))
                ModelState.AddModelError("User.Email", "Email is required.");
            else
            {
                try
                {
                    // Validate email format using .NET MailAddress
                    var addr = new System.Net.Mail.MailAddress(user.Email);
                    if (addr.Address != user.Email)
                        ModelState.AddModelError("User.Email", "Invalid email address format.");
                }
                catch(Exception ex)
                {
                    
                    ModelState.AddModelError("User.Email", "Invalid email address format.");
                }
            }
        }

        // Sends a reset password link to the user's email
        public async Task<IActionResult> SendResetLink(int id)
        {
            if (!User.IsInRole("Admin"))
                return View("Forbidden");

            try
            {
                // Retrieve Identity user by custom user ID
                var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == id);
                if (identityUser == null)
                    return RedirectToAction("Index", "Users");

                // Generate password reset token and encode it
                var code = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Generate reset URL using Razor Pages identity area
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    null,
                    new { area = "Identity", code },
                    protocol: Request.Scheme);

                // Send reset link via email
                await _emailSender.SendEmailAsync(identityUser.Email, "Reset Password",
                    $"Reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                TempData["MessageText"] = "A password reset link has been sent to the user's email.";
                TempData["MessageType"] = "success";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                await ErrorLogger.LogErrorAsync(
                    context: _context,
                    userId: userid,
                    errorMessage: ex.Message,
                    errorSource: ex.Source ?? "Unknown",
                    sourceProcedure: ex.TargetSite?.Name ?? "Unknown"
                );
                TempData["MessageText"] = "Failed to send reset link.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

    }
}
