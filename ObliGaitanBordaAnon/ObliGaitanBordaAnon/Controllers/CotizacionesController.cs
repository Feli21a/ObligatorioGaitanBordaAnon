using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ObliGaitanBordaAnon.Models;
using RestSharp;

namespace ObliGaitanBordaAnon.Controllers
{
    public class CotizacionesController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public CotizacionesController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Cotizaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cotizaciones.ToListAsync());
        }

        // GET: Cotizaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotizacione = await _context.Cotizaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cotizacione == null)
            {
                return NotFound();
            }

            return View(cotizacione);
        }

        // GET: Cotizaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cotizaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreDivisa,CotizacionDivisa")] Cotizacione cotizacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cotizacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cotizacione);
        }

        // GET: Cotizaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotizacione = await _context.Cotizaciones.FindAsync(id);
            if (cotizacione == null)
            {
                return NotFound();
            }
            return View(cotizacione);
        }

        // POST: Cotizaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreDivisa,CotizacionDivisa")] Cotizacione cotizacione)
        {
            if (id != cotizacione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cotizacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CotizacioneExists(cotizacione.Id))
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
            return View(cotizacione);
        }

        // GET: Cotizaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotizacione = await _context.Cotizaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cotizacione == null)
            {
                return NotFound();
            }

            return View(cotizacione);
        }

        // POST: Cotizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cotizacione = await _context.Cotizaciones.FindAsync(id);
            if (cotizacione != null)
            {
                _context.Cotizaciones.Remove(cotizacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CotizacioneExists(int id)
        {
            return _context.Cotizaciones.Any(e => e.Id == id);
        }
    }
}
