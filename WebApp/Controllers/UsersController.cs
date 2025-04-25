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


namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly RentalDBContext _context;

        public UsersController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index(string SearchString, string RoleFilter, string SortBy)
        {

            IEnumerable<User> userList;

            userList = _context.Users.Include(u => u.Role);

            if (!String.IsNullOrEmpty(SearchString))
            {
                userList = userList.Where(x => x.FirstName.ToLower().Contains(SearchString.ToLower()) || x.LastName.ToLower().Contains(SearchString.ToLower()));
            }

            if (!String.IsNullOrEmpty(RoleFilter))
            {
                userList = userList.Where(x => x.RoleId == Convert.ToInt32(RoleFilter));
            }

            userList = SortBy switch
            {
                "FirstNameAZ" => userList.OrderBy(x => x.FirstName),
                "FirstNameZA" => userList.OrderByDescending(x => x.FirstName),
                "RoleAZ" => userList.OrderBy(x => x.Role.RoleName),
                "EmailAZ" => userList.OrderBy(x => x.Email),
                _ => userList.OrderBy(x => x.Id) // default
            };

            var viewModel = new ListUsersViewModel
            {
               RolesList = _context.UserRoles,
               UsersList = userList,

            };



            return View(viewModel);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Image)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        

        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = _context.Users.Include(i => i.Role).SingleOrDefault(x => x.Id == id); 
            if (user == null)
            {
                return NotFound();
            }

            //ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", user.ImageId);
            //ViewData["RoleId"] = new SelectList(_context.UserRoles, "Id", "RoleName", user.RoleId);

            return View(user);
        }



        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.SaveChangesAsync();
                    TempData["CreateSuccess"] = "User Updated Successfully";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else {
                TempData["faild"] = "Faild to Update product";

                //ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", user.ImageId);
                //ViewData["RoleId"] = new SelectList(_context.UserRoles, "Id", "RoleName", user.RoleId);
                return View(user);
            }
           

            
        }

  

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Image)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'RentalDBContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
