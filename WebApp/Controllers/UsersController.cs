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
using WebApp.Areas.Identity.Data;
using Sprache;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;


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
            if (id == null || _unitOfWork.Users == null || id == 0)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetUserWithRoleAsync(id.Value);
            if (user == null)
            {
                return NotFound();
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

                        var result = await _userManager.UpdateAsync(identityUser);
                        if (!result.Succeeded)
                        {
                            TempData["faild"] = "Failed to update Identity User.";
                            return RedirectToAction("Index");
                        }
                    }



                    //to show successful message at the top using the TempData
                    TempData["editSuccess"] = "User Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException) // TODO Fatima: check the for the other error from SaveChangesAsync 
                {
                    if (!await _unitOfWork.Users.UserExistsAsync(editUser.User.Id))
                        return NotFound();
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
            if (id == null || _unitOfWork.Users == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == user.Id);

                if (dbUser != null)
                {
                    
                    var identityResult = await _userManager.DeleteAsync(dbUser);

                    if (!identityResult.Succeeded)
                    {
                        TempData["faild"] = "Failed to delete user from Identity DB.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Soft delete from your main DB
                user.IsActive = false;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                TempData["editSuccess"] = "User Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                if (!await _unitOfWork.Users.UserExistsAsync(user.Id))
                    return NotFound();
                else
                    throw;
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
            var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserID == id);

            if (identityUser == null)
            {
                return RedirectToAction("Index", "Users"); // or show a message
                //return NotFound("error like your head");
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

            TempData["editSuccess"] = "Password reset link sent to the user's email.";
            return RedirectToAction("Index");
        }

    }
}
