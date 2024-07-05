using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObliGaitanBordaAnon.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ObliGaitanBordaAnon.Controllers
{
    public class LoginVMController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public LoginVMController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Usuarios
                    .Include(u => u.Rol)
                    .ThenInclude(r => r.RolesPermisos)
                    .ThenInclude(rp => rp.Permiso)
                    .SingleOrDefault(u => u.Email == model.Email && u.Contrasenia == model.Contrasenia);

                if (user != null)
                {
                    // Configurar la sesión
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserRole", user.Rol.Nombre);
                    var permisos = string.Join(",", user.Rol.RolesPermisos.Select(rp => rp.Permiso.Nombre));
                    HttpContext.Session.SetString("UserPermissions", permisos);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login inválido.");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "LoginVM");
        }
    }
}