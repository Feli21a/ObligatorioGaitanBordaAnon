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
    public class OrdenDetallesController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public OrdenDetallesController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: OrdenDetalles
        public async Task<IActionResult> Index()
        {
            var restoMalTiempoDbContext = _context.OrdenDetalles.Include(o => o.Menu).Include(o => o.Orden);
            return View(await restoMalTiempoDbContext.ToListAsync());
        }

        // GET: OrdenDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles
                .Include(o => o.Menu)
                .Include(o => o.Orden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }

            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "NombrePlato");
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id");
            return View();
        }

        // POST: OrdenDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdenId,MenuId,Cantidad")] OrdenDetalle ordenDetalle)
        {
            if (ModelState.IsValid)
            {
                var menu = await _context.Menus.FindAsync(ordenDetalle.MenuId); //menu pedido
                if (menu == null)
                {
                    return NotFound();
                }

                var orden = await _context.Ordenes.FindAsync(ordenDetalle.OrdenId);//orden ya generada
                if (orden == null)
                {
                    return NotFound();
                }

                orden.Total += (menu.Precio * ordenDetalle.Cantidad);

                // añade el detalle de la orden
                _context.Add(ordenDetalle);

                _context.Update(orden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "NombrePlato", ordenDetalle.MenuId);
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.OrdenId);
            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "NombrePlato", ordenDetalle.MenuId);
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.OrdenId);
            return View(ordenDetalle);
        }

        // POST: OrdenDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrdenId,MenuId,Cantidad")] OrdenDetalle ordenDetalle)
        {
            if (id != ordenDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenDetalleExists(ordenDetalle.Id))
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
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "NombrePlato", ordenDetalle.MenuId);
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", ordenDetalle.OrdenId);
            return View(ordenDetalle);
        }

        // GET: OrdenDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles
                .Include(o => o.Menu)
                .Include(o => o.Orden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }

            return View(ordenDetalle);
        }

        // POST: OrdenDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            var menu = await _context.Menus.FindAsync(ordenDetalle.MenuId); //menu pedido
            var orden = await _context.Ordenes.FindAsync(ordenDetalle.OrdenId);//orden ya generada
            if (ordenDetalle != null)
            {
                orden.Total -= (menu.Precio * ordenDetalle.Cantidad);
                _context.OrdenDetalles.Remove(ordenDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenDetalleExists(int id)
        {
            return _context.OrdenDetalles.Any(e => e.Id == id);
        }
    }
}
