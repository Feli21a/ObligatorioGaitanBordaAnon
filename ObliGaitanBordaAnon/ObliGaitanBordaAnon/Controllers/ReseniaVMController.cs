using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObliGaitanBordaAnon.Models;

namespace ObliGaitanBordaAnon.Controllers
{
    public class ReseniaVMController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public ReseniaVMController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: ReseniasVM
        public async Task<IActionResult> Index()
        {
            var resenias = await _context.Resenias
                .Include(r => r.Restaurante)
                .Select(reserva => new ReseniaViewModel
                {
                    Id = reserva.Id,
                    ClienteId = reserva.ClienteId,
                    RestauranteId = reserva.RestauranteId,
                    Puntaje = reserva.Puntaje,
                    Comentario = reserva.Comentario,
                    FechaResenia = reserva.FechaResenia,
                    Email = reserva.Email ?? "Cliente Generico"
                })
                .ToListAsync();

            return View(resenias);
        }

        // GET: ReseniasVM/Create
        public IActionResult Crear()
        {
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion");
            return View();
        }

        // POST: ReseniasVM/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ReseniaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resenia = new Resenia
                {
                    ClienteId = model.ClienteId,
                    RestauranteId = model.RestauranteId,
                    Puntaje = model.Puntaje,
                    Comentario = model.Comentario,
                    FechaResenia = DateTime.Now,
                    Email = model.Email
                };

                _context.Add(resenia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion");
            return View(model);
        }
    }
}
