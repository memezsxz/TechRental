using Database.Core;
using Database.Core.Domain;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Main DB context connection 
builder.Services.AddDbContext<RentalDBContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity DB context connection
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("IdentityContextConnection")));


builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//// Seed roles and admin user
///

using (var scope = app.Services.CreateScope())
    //{
    //    var services = scope.ServiceProvider;
    //    try
    //    {
    //        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    //        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    //        await ContextSeed.SeedRoleAsync(userManager, roleManager);
    //        await ContextSeed.SeedAdminAsync(userManager, roleManager);
    //    }
    //    catch (Exception ex)
    //    {
    //        var logger = services.GetRequiredService<ILogger<Program>>();
    //        logger.LogError(ex, "An error occurred while seeding roles or users.");
    //    }
    //}



    app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
