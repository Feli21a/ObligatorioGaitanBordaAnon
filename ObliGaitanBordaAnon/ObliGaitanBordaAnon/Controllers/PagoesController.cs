using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ObliGaitanBordaAnon.Models;
using RestSharp;

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
        [VerificarPermisos("VerCrudPago")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Index()
        {
            var restoMalTiempoDbContext = _context.Pagos
                .Include(p => p.Clima)
                .Include(p => p.Reserva)
                .Include(p => p.Cotizacion)
                .Include(p => p.Orden);

            return View(await restoMalTiempoDbContext.ToListAsync());
        }

        // GET: Pagoes/Details/5
        [VerificarPermisos("VerCrudPago")]
        [VerificarPermisos("VerTodo")]
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
                .Include(o => o.Orden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagoes/Create
        [VerificarPermisos("VerCrudPago")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Create(int? ordenId)
        {
            // Llamar a la API del clima
            var climaApi = await ClimaActualApi();
            double? temperatura = climaApi?.Main.Temp;
            bool? lluvia = climaApi?.Weather.Any(w => w.Main.ToLower() == "rain");

            var orden = await _context.Ordenes.Include(o => o.Reserva).ThenInclude(r => r.Cliente).FirstOrDefaultAsync(o => o.Id == ordenId);
            double monto = orden?.Total ?? 0;
            var cliente = orden?.Reserva?.Cliente;

            double descuento = 0;
            List<string> descuentosAplicados = new List<string>();

            // Descuentos según el clima
            if (temperatura < 10)
            {
                descuento += 0.05;
                descuentosAplicados.Add("5% (" + temperatura + " ºC)");
            }
            if (temperatura < 0)
            {
                descuento += 0.10;
                descuentosAplicados.Add("10% (" + temperatura + " ºC)");
            }
            if (lluvia == true)
            {
                descuento += 0.05;
                descuentosAplicados.Add("5% (lluvias)");
            }

            // Descuentos según el tipo de cliente
            if (cliente != null)
            {
                if (cliente.TipoCliente == "Frecuente")
                {
                    descuento += 0.10;
                    descuentosAplicados.Add("10% (Frecuente)");
                }
                else if (cliente.TipoCliente == "VIP")
                {
                    descuento += 0.20;
                    descuentosAplicados.Add("20% (VIP)");
                }
            }

            ViewBag.MontoOriginal = monto;
            ViewBag.DescuentosAplicados = string.Join(" + ", descuentosAplicados);
            ViewBag.MontoConDescuento = monto * (1 - descuento);

            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha");
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa).Where
                (r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa");
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa");
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", ordenId);

            return View();
        }

        // POST: Pagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,CotizacionId,OrdenId,Estado")] Pago pago, double montoConDescuento)
        {
            if (ModelState.IsValid)
            {
                var cotizacion = await _context.Cotizaciones.FindAsync(pago.CotizacionId);
                var orden = await _context.Ordenes.Include(o => o.Reserva).ThenInclude(r => r.Cliente).FirstOrDefaultAsync(o => o.Id == pago.OrdenId);

                if (orden == null || cotizacion == null)
                {
                    return NotFound();
                }

                // Convertir a la moneda seleccionada
                pago.Monto = montoConDescuento * cotizacion.CotizacionDivisa.Value;
                pago.FechaPago = DateTime.Now;

                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa)
                .Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa", pago.ReservaId);
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", pago.OrdenId);

            return View(pago);
        }

        // GET: Pagoes/Edit/5
        [VerificarPermisos("VerCrudPago")]
        [VerificarPermisos("VerTodo")]
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
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa)
                .Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa");
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", pago.OrdenId);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,ClimaId,CotizacionId,OrdenId,Estado")] Pago pago)
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

                    if (cotizacion.NombreDivisa == "UYU")
                    {
                        pago.Monto = pago.Monto / cotizacion.CotizacionDivisa.Value;

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
            ViewData["ReservaId"] = new SelectList(_context.Reservas.Include(m => m.Mesa)
                .Where(r => r.Mesa.Estado == "Ocupada").Select(r => new { r.Id, NumeroMesa = r.Mesa.NumeroMesa }), "Id", "NumeroMesa");
            ViewData["OrdenId"] = new SelectList(_context.Ordenes, "Id", "Id", pago.OrdenId);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        [VerificarPermisos("VerCrudPago")]
        [VerificarPermisos("VerTodo")]
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
                .Include(od => od.Orden)
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

        public async Task<IActionResult> ConfirmarPago(int? idPago)
        {
            if (idPago == null)
            {
                return NotFound();
            }

            // Buscar el pago por su ID
            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .ThenInclude(r => r.Mesa)
                .Include(p => p.Orden)
                .Include(p => p.Clima)
                .FirstOrDefaultAsync(p => p.Id == idPago);

            if (pago == null)
            {
                return NotFound();
            }

            // Actualizar estado del pago
            pago.Estado = "Pagado";

            // Actualizar estado de la reserva
            if (pago.Reserva != null)
            {
                pago.Reserva.Estado = "Finalizada";

                // También puedes liberar la mesa si es necesario
                if (pago.Reserva.Mesa != null)
                {
                    pago.Reserva.Mesa.Estado = "Disponible";
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<ClimaApi> ClimaActualApi()
        {
            var client = new RestClient("http://api.openweathermap.org/data/2.5/weather");
            var request = new RestRequest("?q=Maldonado,UY&units=metric&appid=142e61b10d55592d847e6fb29fa1abcc", Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ClimaApi>(response.Content);
            }

            return null;
        }

    }


}