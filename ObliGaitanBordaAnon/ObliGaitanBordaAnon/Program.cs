using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ObliGaitanBordaAnon.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using ObliGaitanBordaAnon.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestoMalTiempoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conectionSQL")));

builder.Services.AddControllersWithViews().AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var externalImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Repositorio", "img");
if (!Directory.Exists(externalImagePath))
{
    Directory.CreateDirectory(externalImagePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(externalImagePath),
    RequestPath = "/ExternalImages"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

