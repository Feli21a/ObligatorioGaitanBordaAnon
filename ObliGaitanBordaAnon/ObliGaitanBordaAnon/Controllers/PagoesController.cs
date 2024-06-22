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
    public class PagoesController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public PagoesController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Pagoes
        public async Task<IActionResult> Index()
        {
            var restoMalTiempoDbContext = _context.Pagos
                .Include(p => p.Clima)
                .Include(p => p.Reserva)
                .Include(p => p.Cotizacion)
                .Include(p => p.OrdenDetalle)
                .ThenInclude(od => od.Orden);

            return View(await restoMalTiempoDbContext.ToListAsync());
        }

        // GET: Pagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Clima)
                .Include(p => p.Reserva)
                .Include(c => c.Cotizacion)
                .Include(od => od.OrdenDetalle)
                .ThenInclude(o => o.Orden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagoes/Create
        public IActionResult Create()
        {
            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha");
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa");
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa");
            ViewData["OrdenDetalleId"] = new SelectList(_context.OrdenDetalles, "Id", "Id");
            return View();
        }

        // POST: Pagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,ClimaId,CotizacionId,OrdenDetalleId")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                var cotizacion = await _context.Cotizaciones.FindAsync(pago.CotizacionId);
                var ordenDetalle = await _context.OrdenDetalles
                                         .Include(od => od.Orden)
                                         .FirstOrDefaultAsync(od => od.Id == pago.OrdenDetalleId);

                pago.Monto = ordenDetalle.Orden.Total;

                if (cotizacion != null)
                {
                    pago.Monto = pago.Monto * cotizacion.CotizacionDivisa.Value;

                }
                else
                {
                    return NotFound();
                }

                pago.FechaPago = DateTime.Now;

                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa", pago.ReservaId);
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
            ViewData["OrdenDetalleId"] = new SelectList(_context.OrdenDetalles, "Id", "Id", pago.OrdenDetalleId);
            return View(pago);
        }

        // GET: Pagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
            ViewData["OrdenDetalleId"] = new SelectList(_context.OrdenDetalles, "Id", "Id", pago.OrdenDetalleId);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,ClimaId,CotizacionId,OrdenDetalleId")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cotizacion = await _context.Cotizaciones.FindAsync(pago.CotizacionId);

                if (cotizacion != null)
                {
                    pago.Monto = pago.Monto * cotizacion.CotizacionDivisa.Value;

                }
                else
                {
                    return NotFound();
                }

                pago.FechaPago = DateTime.Now;

                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
            ViewData["OrdenDetalleId"] = new SelectList(_context.OrdenDetalles, "Id", "Id", pago.OrdenDetalleId);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Clima)
                .Include(p => p.Reserva)
                .Include(c => c.Cotizacion)
                .Include(od => od.OrdenDetalle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}