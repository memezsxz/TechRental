using Database.Core.Domain;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Webp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RentalDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


var context = new RentalDBContext();

foreach (var imageRecord in context.Images.Where(i => i.Guid != null && i.ImageName.Contains(".png")))
{
    if (imageRecord == null) return;

    using var getResponse = await S3Uploader.GetFileByGuidAsync(imageRecord.Guid.ToString()! + ".png");
    using var originalImage = await SixLabors.ImageSharp.Image.LoadAsync(getResponse);

    // Convert to WebP
    using var webpStream = new MemoryStream();
    await originalImage.SaveAsync(webpStream, new WebpEncoder { Quality = 80 });
    webpStream.Position = 0;

    // Generate new filename and GUID
    var newGuid = Guid.NewGuid();
    var newFileName = Path.GetFileNameWithoutExtension(imageRecord.ImageName) + ".webp";

    // Upload to S3
    if ((await S3Uploader.UploadFileAsync(webpStream, newGuid, ".webp")) == null)
        continue;

    imageRecord.ImageName = newFileName;
    imageRecord.ImageType = "image/webp";
    imageRecord.Guid = newGuid;
    imageRecord.CreatedAt = DateTime.UtcNow;
}
await context.SaveChangesAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
