using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NuGet.Protocol.Plugins;
using ObliGaitanBordaAnon.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Linq;
using ConsoleAPI;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RestoMalTiempoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conectionSQL")));

builder.Services.AddControllersWithViews().AddViewOptions(options => { options.HtmlHelperOptions.ClientValidationEnabled = true; });


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// configuramos la ruta para los archivos estaticos (imagenes)
var externalImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Repositorio", "img"); //nos paramos en la carpeta
if (!Directory.Exists(externalImagePath))
{
    Directory.CreateDirectory(externalImagePath); //si no existe la creamos
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(externalImagePath),
    RequestPath = "/ExternalImages" //usamos la carpeta con esta ruta
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



