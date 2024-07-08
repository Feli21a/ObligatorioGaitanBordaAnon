using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObliGaitanBordaAnon.Models;

namespace ObliGaitanBordaAnon.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public OrdenesController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Ordenes
        [VerificarPermisos("VerCrudOrden")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Index()
        {
            var restoMalTiempoDbContext = _context.Ordenes.Include(o => o.Reserva).ThenInclude(m => m.Mesa).Include(o => o.Pagos);
            return View(await restoMalTiempoDbContext.ToListAsync());
        }

        // GET: Ordenes/Details/5
        [VerificarPermisos("VerCrudOrden")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordene = await _context.Ordenes
                .Include(o => o.Reserva)                                
                .ThenInclude(m => m.Mesa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordene == null)
            {
                return NotFound();
            }

            return View(ordene);
        }

        // GET: Ordenes/Create
        [VerificarPermisos("VerCrudOrden")]
        [VerificarPermisos(("VerTodo"))]
        public IActionResult Create()
        {
            var reservaConMesasOcupadas = _context.Reservas.Where(R => R.Estado == "Confirmada").Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa });
            if(reservaConMesasOcupadas.Count() == 0)
            {
                return RedirectToAction("ErrorAction", "Home");
            }


            ViewData["ReservaId"] = new SelectList(reservaConMesasOcupadas, "Id", "NumeroMesa");
            return View();
        }

        // POST: Ordenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservaId,Total")] Ordene ordene)
        {

            // Verifica si hay reservas con mesas ocupadas y las reservas confirmadas
            var reservaConMesasOcupadas =_context.Reservas.Where(R => R.Estado == "Confirmada").Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa });

            if (!reservaConMesasOcupadas.Any())
            {
                ModelState.AddModelError("", "No se puede generar una orden sin clientes para atender.");
                ViewData["ReservaId"] = new SelectList(reservaConMesasOcupadas, "Id", "NumeroMesa", ordene.ReservaId);
                return View(ordene);
            }

            if (ModelState.IsValid)
            {

                ordene.Total = 0;

                _context.Add(ordene);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReservaId"] = new SelectList(reservaConMesasOcupadas, "Id", "NumeroMesa", ordene.ReservaId);
            return View(ordene);
        }

        // GET: Ordenes/Edit/5
        [VerificarPermisos("VerCrudOrden")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Edit(int? id)
        {
            var reservaConMesasOcupadas = _context.Reservas.Where(R => R.Estado == "Confirmada").Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa });

            if (id == null)
            {
                return NotFound();
            }

            var ordene = await _context.Ordenes.FindAsync(id);
            if (ordene == null)
            {
                return NotFound();
            }
            ViewData["ReservaId"] = new SelectList(reservaConMesasOcupadas, ordene.ReservaId);
            return View(ordene);
        }

        // POST: Ordenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservaId,Total")] Ordene ordene)
        {
            var reservaConMesasOcupadas = _context.Reservas.Where(R => R.Estado == "Confirmada").Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa });

            if (id != ordene.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordene);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdeneExists(ordene.Id))
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
                ViewData["ReservaId"] = new SelectList(reservaConMesasOcupadas, ordene.ReservaId);
                return View(ordene);
        }

        // GET: Ordenes/Delete/5
        [VerificarPermisos("VerCrudOrden")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordene = await _context.Ordenes
                .Include(o => o.Reserva)
                .ThenInclude(m => m.Mesa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordene == null)
            {
                return NotFound();
            }

            return View(ordene);
        }

        // POST: Ordenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordene = await _context.Ordenes.Include(o => o.Pagos).FirstOrDefaultAsync(o => o.Id == id);

            if (ordene == null)
            {
                return NotFound();
            }

            // Eliminar pagos asociados
            foreach (var pago in ordene.Pagos.ToList())
            {
                _context.Pagos.Remove(pago);
            }

            if (ordene != null)
            {
                _context.Ordenes.Remove(ordene);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdeneExists(int id)
        {
            return _context.Ordenes.Any(e => e.Id == id);
        }
    }
}
