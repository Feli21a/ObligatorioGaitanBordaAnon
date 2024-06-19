using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ObliGaitanBordaAnon.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestoMalTiempoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conectionSQL")));

builder.Services.AddControllersWithViews().AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

// Configure cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LoginVM/Login";
        options.LogoutPath = "/LoginVM/Logout";
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

// Configura la ruta para los archivos estáticos (imágenes)
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