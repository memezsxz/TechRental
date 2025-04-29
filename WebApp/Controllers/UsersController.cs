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


namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //unit of work "DBContext" refrence passed auto using the depandency injection
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        //Index ==> Get method (Display the view)
        public async Task<IActionResult> Index(string? searchString, string? roleFilter, SortOption? sortBy)
        {
            var users = await _unitOfWork.Users.GetUsersAsync(searchString, roleFilter, sortBy);
            var roles = await _unitOfWork.UserRoles.GetAllAsync();

            var viewModel = new ListUsersViewModel
            {
                UsersList = users,
                RolesList = roles,
                SearchString = searchString,
                RoleFilter = roleFilter,
                CurrentSort = sortBy,
                SortOptions = Enum.GetValues(typeof(IUserRepository.SortOption)).Cast<IUserRepository.SortOption>()
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
            else
            {
                try
                {
                    user.IsActive =false; // Or set IsActive = false if soft delete
                    await _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    TempData["editSuccess"] = "User Deleted Successfully";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    if (!await _unitOfWork.Users.UserExistsAsync(user.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
        }



        public async Task<IActionResult> Profile(int? id)
        {

            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetUserWithProfileAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User editUser)
        {


            ValidateUser(editUser);



            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Users.UpdateAsync(editUser);
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.Users.UserExistsAsync(editUser.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(editUser);
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

    }
}
