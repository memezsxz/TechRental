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


        //unit of work "DBContext" refrence passed auto using the depandency injection
        public UsersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }



        //Index ==> Get method (Display the view)
        public async Task<IActionResult> Index(string? searchString, string? roleFilter, SortOption? sortBy, int page = 1, int pageSize = 10)
        {
            if (!User.IsInRole("Admin")) {
                return View("Forbidden");
            }

            var allUsers = await _unitOfWork.Users.GetUsersAsync(searchString, roleFilter, sortBy);
            var roles = await _unitOfWork.UserRoles.GetAllAsync();
            var totalUsers = allUsers.Count();
            var users = allUsers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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


        //Edit ==> Get method (Display the view of the edit)
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return View("Forbidden");
            }


            if (id == null || _unitOfWork.Users == null || id == 0)
            {
                return View("NotFound");
            }

            var user = await _unitOfWork.Users.GetUserWithRoleAsync(id.Value);
            if (user == null)
            {
                return View("NotFound");
            }

            var viewModel = new EditUserViewModel
            {
                User = user,
                RolesList = await _unitOfWork.UserRoles.GetAllAsync()
            };

            return View(viewModel);
        }



        //Index ==> Post method (handel the form submit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel editUser)
        {

            if (!User.IsInRole("Admin"))
            {
                return View("Forbidden");
            }

            //do some server side validation 
            ValidateUser(editUser.User);


            if (ModelState.IsValid)
            {
                try
                {

                    await _unitOfWork.Users.UpdateAsync(editUser.User);
                    await _unitOfWork.SaveChangesAsync();

                    var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == editUser.User.Id); // match using your foreign key


                    if (identityUser != null)
                    {
                        identityUser.FirstName = editUser.User.FirstName;
                        identityUser.LastName = editUser.User.LastName;
                        identityUser.Email = editUser.User.Email;
                        identityUser.UserName = editUser.User.Email;
                        identityUser.PhoneNumber = editUser.User.PhoneNumber;
                        identityUser.NormalizedEmail = editUser.User.Email.ToUpper();
                        identityUser.NormalizedUserName = editUser.User.Email.ToUpper();

                        var currentRoles = await _userManager.GetRolesAsync(identityUser);
                        await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

                       
                            var getRoleName = _unitOfWork.UserRoles.getRoleNameByID((int)editUser.User.RoleId);


                        if (editUser.User.RoleId != null)
                        {
                            await _userManager.AddToRoleAsync(identityUser, getRoleName);
                        }


                        var result = await _userManager.UpdateAsync(identityUser);
                        if (!result.Succeeded)
                        {
                            TempData["MessageText"] = "An error occurred while saving the equipment.";
                            TempData["MessageType"] = "error";
                            return RedirectToAction("Index");
                        }
                    }



                    //to show successful message at the top using the TempData
                    TempData["MessageText"] = "User was saved successfully!";
                    TempData["MessageType"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException) // TODO Fatima: check the for the other error from SaveChangesAsync 
                {
                    if (!await _unitOfWork.Users.UserExistsAsync(editUser.User.Id))
                        return View("NotFound");
                    else
                        throw;

                }
            }

            TempData["faild"] = "Failed to Update User";

            var roles = await _unitOfWork.UserRoles.GetAllAsync();
            editUser.RolesList = roles;
            return View(editUser);
        }





        //DeleteConfirmed ==> Post method (handel delete button click)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return View("Forbidden");
            }

            if (id == null || _unitOfWork.Users == null)
            {
                return View("NotFound");
            }

            var user = await _unitOfWork.Users.GetAsync(id.Value);

            if (user == null)
            {
                return View("NotFound");
            }

            try
            {
                var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == user.Id);

                if (dbUser != null)
                {
                    var identityResult = await _userManager.DeleteAsync(dbUser);

                    if (!identityResult.Succeeded)
                    {
                        return Json(new { success = false, message = "Failed to delete user from Identity DB", type = "error" });
                    }
                }

                user.IsActive = false;
                user.Email = "Deleted" + user.Id.ToString();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "User deleted successfully.",
                    redirectUrl = Url.Action("Index"),
                    type = "success"
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Unexpected error occurred.", type = "error" });
            }
        }


        private void ValidateUser(User user)
        {
            if (user.RoleId == null || user.RoleId == 0)
            {
                ModelState.AddModelError("User.RoleId", "User Role must be selected.");
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                ModelState.AddModelError("User.FirstName", "First name is required.");
            }
            else if (user.FirstName.Length < 3)
            {
                ModelState.AddModelError("User.FirstName", "First name must contain at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                ModelState.AddModelError("User.LastName", "Last name is required.");
            }
            else if (user.LastName.Length < 3)
            {
                ModelState.AddModelError("User.LastName", "Last name must contain at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError("User.Email", "Email is required.");
            }
            else
            {
                if (!user.Email.Contains('@') || !user.Email.Contains('.'))
                {
                    ModelState.AddModelError("User.Email", "Invalid email format. Email must contain '@' and a domain with a dot (e.g., example.com).");
                }
                else
                {
                    var parts = user.Email.Split('@');

                    if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                    {
                        ModelState.AddModelError("User.Email", "Invalid email format. Please enter a valid email address.");
                    }
                    else
                    {
                        var domainParts = parts[1].Split('.');

                        if (domainParts.Length < 2 || domainParts.Any(string.IsNullOrWhiteSpace))
                        {
                            ModelState.AddModelError("User.Email", "Invalid domain format. Email must have a valid domain and extension (e.g., example.com).");
                        }
                    }
                }

                // check on the provided email address if it valid after click on the save button
                try
                {
                    var addr = new System.Net.Mail.MailAddress(user.Email);
                    if (addr.Address != user.Email)
                    {
                        ModelState.AddModelError("User.Email", "Invalid email address format.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("User.Email", "Invalid email address format.");
                }
            }
        }



        public async Task<IActionResult> SendResetLink(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return View("Forbidden");
            }

            var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == id);

            if (identityUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(identityUser.Email, "Reset Password",
                $"Reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            TempData["MessageText"] = "A password reset link has been sent to the user's email.";
            TempData["MessageType"] = "success";

            return RedirectToAction("Index");
        }

    }
}
