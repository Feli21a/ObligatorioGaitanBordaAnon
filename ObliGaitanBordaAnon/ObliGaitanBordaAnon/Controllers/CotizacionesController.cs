using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ObliGaitanBordaAnon.Models;
using ObliGaitanBordaAnon;
using RestSharp;

public class CotizacionesController : Controller
{
    private readonly RestoMalTiempoDbContext _context;

    public CotizacionesController(RestoMalTiempoDbContext context)
    {
        _context = context;
    }

    // GET: Cotizaciones
    [VerificarPermisos("VerCrudCotizaciones")]
    [VerificarPermisos(("VerTodo"))]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Cotizaciones.ToListAsync());
    }

    // GET: Cotizaciones/Details/5
    [VerificarPermisos("VerCrudCotizaciones")]
    [VerificarPermisos(("VerTodo"))]
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
    [VerificarPermisos("VerCrudCotizaciones")]
    [VerificarPermisos(("VerTodo"))]
    public async Task<IActionResult> Create(string divisa)
    {
        divisa = divisa ?? "UYU"; // Si 'divisa' es nulo, asignar uyu
        var divisas = await GetDivisasDisponibles(divisa);
        ViewBag.Divisas = new SelectList(divisas, "Value", "Text");
        return View();
    }

    // POST: Cotizaciones/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NombreDivisa")] Cotizacione cotizacione)
    {
        if (ModelState.IsValid)
        {
            var cotizacionesCount = await _context.Cotizaciones.CountAsync();

            if (cotizacionesCount == 0)
            {
                cotizacione.NombreDivisa = "UYU";
                cotizacione.CotizacionDivisa = 1;

                _context.Add(cotizacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var cotizacionesApi = await GetCotizacionesApi("UYU");

            if (cotizacionesApi.ContainsKey(cotizacione.NombreDivisa))
            {
                cotizacione.CotizacionDivisa = cotizacionesApi[cotizacione.NombreDivisa];
                _context.Add(cotizacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "La divisa seleccionada no tiene una cotización disponible.");
            }
        }

        ViewBag.Divisas = new SelectList(await GetDivisasDisponibles("UYU"));
        return View(cotizacione);
    }

    // GET: Cotizaciones/Edit/5
    [VerificarPermisos("VerCrudCotizaciones")]
    [VerificarPermisos(("VerTodo"))]
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
                var cotizaciones = await GetCotizacionesApi("UYU");

                if (cotizaciones.ContainsKey(cotizacione.NombreDivisa))
                {
                    cotizacione.CotizacionDivisa = cotizaciones[cotizacione.NombreDivisa];
                    _context.Update(cotizacione);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("", "La divisa seleccionada no tiene una cotización disponible.");
                    return View(cotizacione);
                }
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
    [VerificarPermisos("VerCrudCotizaciones")]
    [VerificarPermisos(("VerTodo"))]
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

    private async Task<Dictionary<string, double>> GetCotizacionesApi(string source)
    {
        var client = new RestClient("http://api.currencylayer.com");
        var request = new RestRequest($"/live?access_key=771c89a8a3f3742bb23555ea425637b6&source={source}", Method.Get);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var cotizacion = JsonConvert.DeserializeObject<CotizacionAPI>(response.Content);
            if (cotizacion != null && cotizacion.Quotes != null)
            {
                return cotizacion.Quotes
                    .Where(q => !string.IsNullOrEmpty(q.Key) && q.Key.StartsWith(source))
                    .ToDictionary(q => q.Key.Substring(source.Length), q => q.Value);
            }
        }

        return new Dictionary<string, double>();
    }

    private async Task<List<SelectListItem>> GetDivisasDisponibles(string source)
    {
        var cotizaciones = await GetCotizacionesApi(source);
        var divisas = cotizaciones.Keys
            .Select(k => new SelectListItem
            {
                Value = k,
                Text = k
            })
            .ToList();

        return divisas;
    }
}