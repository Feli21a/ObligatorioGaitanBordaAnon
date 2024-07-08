using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ObliGaitanBordaAnon.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configurar la sesion
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// Habilitar la sesión
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();