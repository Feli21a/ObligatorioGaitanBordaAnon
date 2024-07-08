using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObliGaitanBordaAnon.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ObliGaitanBordaAnon.Controllers
{
    public class RestaurantesController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public RestaurantesController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Restaurantes
        [VerificarPermisos("VerCrudRestaurante")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Restaurantes.ToListAsync());
        }

        // GET: Restaurantes/Details/5
        [VerificarPermisos("VerCrudRestaurante")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurante == null)
            {
                return NotFound();
            }

            return View(restaurante);
        }

        // GET: Restaurantes/Create
        [VerificarPermisos("VerCrudRestaurante")]
        [VerificarPermisos("VerTodo")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Telefono")] Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurante);
        }

        // GET: Restaurantes/Edit/5
        [VerificarPermisos("VerCrudRestaurante")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurantes.FindAsync(id);
            if (restaurante == null)
            {
                return NotFound();
            }
            return View(restaurante);
        }

        // POST: Restaurantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,Telefono")] Restaurante restaurante)
        {
            if (id != restaurante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestauranteExists(restaurante.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurante);
        }

        // GET: Restaurantes/Delete/5
        [VerificarPermisos("VerCrudRestaurante")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurante == null)
            {
                return NotFound();
            }

            return View(restaurante);
        }

        // POST: Restaurantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurante = await _context.Restaurantes.FindAsync(id);
            if (restaurante != null)
            {
                _context.Restaurantes.Remove(restaurante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestauranteExists(int id)
        {
            return _context.Restaurantes.Any(e => e.Id == id);
        }
    }
}