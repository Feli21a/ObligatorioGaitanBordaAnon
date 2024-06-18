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
            var restoMalTiempoDbContext = _context.Pagos.Include(p => p.Clima).Include(p => p.Reserva).Include(c => c.Cotizacion);
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
            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id");
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa");
            return View();
        }

        // POST: Pagoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,ClimaId,CotizacionId")] Pago pago)
        {
            if (ModelState.IsValid)
            {

                var cotizacion = await _context.Cotizaciones.FindAsync(pago.CotizacionId);
                if (cotizacion != null && cotizacion.NombreDivisa == "USD")
                {
                    var client = new RestClient("http://api.currencylayer.com");
                    var request = new RestRequest("/live?access_key=771c89a8a3f3742bb23555ea425637b6", Method.Get);
                    RestResponse response = await client.ExecuteAsync(request);
                    Console.WriteLine(response.Content);

                    if (response.IsSuccessStatusCode)
                    {
                        Cotizacion cotizacionAPI = JsonConvert.DeserializeObject<Cotizacion>(response.Content); //cargamos con la api

                        if (cotizacionAPI.Quotes != null && cotizacionAPI.Quotes.ContainsKey("USDUYU")) //nos aseguramos de encontrar la cotizacion que queremos
                        {
                            double cotizacionUSDUYU = cotizacionAPI.Quotes["USDUYU"];
                            pago.Monto = (int)(pago.Monto / cotizacionUSDUYU); //convertimos en int (monto es int)
                        }
                        else
                        {
                            ModelState.AddModelError("", "No se pudo obtener la cotización de USDUYU.");
                            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
                            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
                            return View(pago);
                        }
                    }
                    else
                    {
                        // por si la api falla
                        ModelState.AddModelError("", "Error al obtener la cotización desde la API.");
                        ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
                        ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                        ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
                        return View(pago);
                    }
                }

                // Guardar el pago con el monto convertido
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "NombreDivisa", pago.CotizacionId);
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
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservaId,Monto,FechaPago,MetodoPago,ClimaId,CotizacionId")] Pago pago)
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
                    if (cotizacion != null && cotizacion.NombreDivisa == "USD")
                    {
                        var client = new RestClient("http://api.currencylayer.com");
                        var request = new RestRequest("/live?access_key=771c89a8a3f3742bb23555ea425637b6", Method.Get);
                        RestResponse response = await client.ExecuteAsync(request);
                        Console.WriteLine(response.Content);

                        if (response.IsSuccessStatusCode)
                        {
                            Cotizacion cotizacionAPI = JsonConvert.DeserializeObject<Cotizacion>(response.Content); // Cargamos con la API

                            if (cotizacionAPI.Quotes != null && cotizacionAPI.Quotes.ContainsKey("USDUYU")) // Aseguramos encontrar la cotización que queremos
                            {
                                double cotizacionUSDUYU = cotizacionAPI.Quotes["USDUYU"];
                                pago.Monto = (int)(pago.Monto / cotizacionUSDUYU); // Convertimos en dólares
                            }
                            else
                            {
                                ModelState.AddModelError("", "No se pudo obtener la cotización de USDUYU.");
                                ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
                                ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                                ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "Nombre", pago.CotizacionId);
                                return View(pago);
                            }
                        }
                        else
                        {
                            // Manejar el caso donde la solicitud a la API falle
                            ModelState.AddModelError("", "Error al obtener la cotización desde la API.");
                            ViewData["ClimaId"] = new SelectList(_context.Climas, "Id", "Fecha", pago.ClimaId);
                            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "Nombre", pago.CotizacionId);
                            return View(pago);
                        }
                    }

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
            ViewData["CotizacionId"] = new SelectList(_context.Cotizaciones, "Id", "Nombre", pago.CotizacionId);
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
