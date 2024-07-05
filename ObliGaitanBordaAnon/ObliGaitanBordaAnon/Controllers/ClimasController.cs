using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObliGaitanBordaAnon.Models;
using RestSharp;
using Newtonsoft.Json;

namespace ObliGaitanBordaAnon.Controllers
{
    public class ClimasController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public ClimasController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Climas
        [VerificarPermisos("VerCrudClima")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Climas.ToListAsync());
        }

        // GET: Climas/Details/5
        [VerificarPermisos("VerCrudClima")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clima == null)
            {
                return NotFound();
            }

            return View(clima);
        }

        // GET: Climas/Create
        [VerificarPermisos("VerCrudClima")]
        [VerificarPermisos(("VerTodo"))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Climas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pais,Ciudad")] Clima clima)
        {
            if (string.IsNullOrEmpty(clima.Pais) || string.IsNullOrEmpty(clima.Ciudad))
            {
                ModelState.AddModelError("", "La ciudad y el país son obligatorios.");
                return View(clima);
            }

            var client = new RestClient("http://api.openweathermap.org/data/2.5/weather");
            var request = new RestRequest($"?q={clima.Ciudad},{clima.Pais}&units=metric&appid=142e61b10d55592d847e6fb29fa1abcc", Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var climaApi = JsonConvert.DeserializeObject<ClimaApi>(response.Content);
                if (climaApi != null)
                {
                    clima.Fecha = DateTime.Now;
                    clima.Temperatura = (int?)climaApi.Main.Temp;
                    clima.Lluvia = climaApi.Weather.Any(w => w.Main.ToLower().Contains("rain"));
                    clima.DescripcionClima = climaApi.Weather.FirstOrDefault()?.Description ?? "Sin descripción";

                    if (ModelState.IsValid)
                    {
                        _context.Add(clima);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo obtener la información del clima.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error al llamar a la API del clima.");
            }

            return View(clima);
        }

        // GET: Climas/Edit/5
        [VerificarPermisos("VerCrudClima")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas.FindAsync(id);
            if (clima == null)
            {
                return NotFound();
            }
            return View(clima);
        }

        // POST: Climas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Temperatura,Lluvia,DescripcionClima")] Clima clima)
        {
            if (id != clima.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClimaExists(clima.Id))
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
            return View(clima);
        }

        // GET: Climas/Delete/5
        [VerificarPermisos("VerCrudClima")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clima = await _context.Climas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clima == null)
            {
                return NotFound();
            }

            return View(clima);
        }

        // POST: Climas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clima = await _context.Climas.FindAsync(id);
            if (clima != null)
            {
                _context.Climas.Remove(clima);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClimaExists(int id)
        {
            return _context.Climas.Any(e => e.Id == id);
        }

        private async Task<ClimaApi> GetClimaApi(string ciudad, string pais)
        {
            var client = new RestClient("http://api.openweathermap.org");
            var request = new RestRequest($"/data/2.5/weather?q={ciudad},{pais}&units=metric&appid=142e61b10d55592d847e6fb29fa1abcc", Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ClimaApi>(response.Content);
            }

            return null;
        }
    }
}
