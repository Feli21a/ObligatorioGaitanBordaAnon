using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ObliGaitanBordaAnon.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ObliGaitanBordaAnon.Controllers
{
    public class LoginController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public LoginController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
                var user = _context.Usuarios.SingleOrDefault(u => u.Email == model.Email && u.Contrasenia == model.Password);
                var cliente = _context.Clientes.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                       
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Configura otras propiedades si es necesario
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else if(cliente != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, cliente.Email),

                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Configura otras propiedades si es necesario
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> AutoLogin()
        {
            var cliente = _context.Clientes.SingleOrDefault(u => u.Email == "ClienteRestoMalTiempo@gmail.com"); //usuario generico
            if (cliente != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, cliente.Email),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Ordenes");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}