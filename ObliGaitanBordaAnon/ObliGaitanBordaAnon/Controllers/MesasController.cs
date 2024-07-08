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
    public class MesasController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public MesasController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Mesas
        [VerificarPermisos("VerCrudMesa")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Index(string estado)
        {
            var mesa = from m in _context.Mesas.Include(r => r.Restaurante) select m;

            if (!string.IsNullOrEmpty(estado))
            {
                mesa = mesa.Where(m => m.Estado == estado);
            }

            return View(await mesa.ToListAsync());
        }

        // GET: Mesas/Details/5
        [VerificarPermisos("VerCrudMesa")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // GET: Mesas/Create
        [VerificarPermisos("VerCrudMesa")]
        [VerificarPermisos(("VerTodo"))]
        public IActionResult Create()
        {
            
            if(_context.Restaurantes.Count() == 0)
            {
                return RedirectToAction("ErrorAction", "Home");
            }

            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion");
            return View();
        }

        // POST: Mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroMesa,Capacidad,Estado,RestauranteId")] Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", mesa.RestauranteId);
            return View(mesa);
        }

        // GET: Mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", mesa.RestauranteId);
            return View(mesa);
        }

        // POST: Mesas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroMesa,Capacidad,Estado,RestauranteId")] Mesa mesa)
        {
            if (id != mesa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.Id))
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
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", mesa.RestauranteId);
            return View(mesa);
        }

        // GET: Mesas/Delete/5
        [VerificarPermisos("VerCrudMesa")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // POST: Mesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa != null)
            {
                _context.Mesas.Remove(mesa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.Id == id);
        }
    }
}
