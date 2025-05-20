using Database.Core.Domain;
using Database.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sprache;

namespace Identity
{
    public class ContextSeed
    {
  
        public static async Task SeedRoleAsync(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager) {


            //check each role seperated , so if the role is not exsist create it.
            if (!await roleManager.RoleExistsAsync(RoleConstants.Admin))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin));

            if (!await roleManager.RoleExistsAsync(RoleConstants.Customer))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.Customer));

            if (!await roleManager.RoleExistsAsync(RoleConstants.Manager))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.Manager));


        }



        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, RentalDBContext _context)
        {

            try
            {
                // 1. Get Role from main DB
                var adminRole = _context.UserRoles.FirstOrDefault(r => r.RoleName == RoleConstants.Admin);
                if (adminRole == null)
                    throw new Exception("Role 'Admin' not found.");

                // 2. Check if Identity user already exists
                var existingUser = await userManager.FindByEmailAsync("Admin1@gmail.com");
                if (existingUser != null)
                {
                    Console.WriteLine("Admin user already exists");
                    return;
                }

                // 3. Insert into main app DB first
                var newAdmin = new User
                {
                    FirstName = "Default",
                    LastName = "Admin",
                    Email = "Admin1@gmail.com",
                    RoleId = adminRole.Id,
                    IsActive = true
                };

                _context.Users.Add(newAdmin);
                _context.SaveChanges();

                // 4. Create Identity user and link UserID to main DB
                var defaultUser = new ApplicationUser
                {
                    UserName = "Admin1@gmail.com",
                    Email = "Admin1@gmail.com",
                    FirstName = "Default",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserID = newAdmin.Id
                };

                var result = await userManager.CreateAsync(defaultUser, "Pa$$word123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, RoleConstants.Admin);
                    Console.WriteLine("Admin user created and linked successfully");
                }
                else
                {
                    Console.WriteLine("Failed to create Identity user:");
                    foreach (var error in result.Errors)
                        Console.WriteLine($"- {error.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during admin seeding: " + ex.Message);
            }

        }


        public static async Task SeedManagerAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, RentalDBContext _context)
        {

            try
            {
                // 1. Get Role from main DB
                var ManagerRole = _context.UserRoles.FirstOrDefault(r => r.RoleName == RoleConstants.Manager);
                if (ManagerRole == null)
                    Console.WriteLine("Role 'Manager' not found.");

                // 2. Check if Identity user already exists
                var existingUser = await userManager.FindByEmailAsync("Manager1@gmail.com");
                if (existingUser != null)
                {
                    Console.WriteLine("Manager user already exists");
                    return;
                }

                // 3. Insert into main app DB first
                var newManager = new User
                {
                    FirstName = "Default",
                    LastName = "Manager",
                    Email = "Manager1@gmail.com",
                    RoleId = ManagerRole.Id,
                    IsActive = true
                };

                _context.Users.Add(newManager);
                _context.SaveChanges();

                // 4. Create Identity user and link UserID to main DB
                var defaultUser = new ApplicationUser
                {
                    UserName = "Manager1@gmail.com",
                    Email = "Manager1@gmail.com",
                    FirstName = "Default",
                    LastName = "Manager",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserID = newManager.Id
                };

                var result = await userManager.CreateAsync(defaultUser, "Pa$$word123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, RoleConstants.Manager);
                    Console.WriteLine("Manager user created and linked successfully");
                }
                else
                {
                    Console.WriteLine("Failed to create Identity user:");
                    foreach (var error in result.Errors)
                        Console.WriteLine($"- {error.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during admin seeding: " + ex.Message);
            }

        }

    }
}
