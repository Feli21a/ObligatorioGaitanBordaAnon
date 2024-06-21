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
        public async Task<IActionResult> Index()
        {
            var restoMalTiempoDbContext = _context.Ordenes.Include(o => o.Reserva).ThenInclude(m => m.Mesa);
            return View(await restoMalTiempoDbContext.ToListAsync());
        }

        // GET: Ordenes/Details/5
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
        public IActionResult Create()
        {
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa");
            return View();
        }

        // POST: Ordenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservaId,Total")] Ordene ordene)
        {
            if (ModelState.IsValid)
            {

                _context.Add(ordene);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa", ordene.ReservaId);
            return View(ordene);
        }

        // GET: Ordenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordene = await _context.Ordenes.FindAsync(id);
            if (ordene == null)
            {
                return NotFound();
            }
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), ordene.ReservaId);
            return View(ordene);
        }

        // POST: Ordenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservaId,Total")] Ordene ordene)
        {
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
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), ordene.ReservaId);
            return View(ordene);
        }

        // GET: Ordenes/Delete/5
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
            var ordene = await _context.Ordenes.FindAsync(id);
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
