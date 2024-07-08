using Microsoft.AspNetCore.Mvc;
using ObliGaitanBordaAnon.Models;
using System.Diagnostics;

namespace ObliGaitanBordaAnon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestoMalTiempoDbContext _context;

        public HomeController(ILogger<HomeController> logger, RestoMalTiempoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var reseñas = _context.Resenias
                .OrderByDescending(r => r.Puntaje)
                .Take(3)
                .ToList();
            return View(reseñas);
        }

        public IActionResult ErrorAction()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
