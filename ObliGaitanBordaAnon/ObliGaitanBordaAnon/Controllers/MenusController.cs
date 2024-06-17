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
    public class MenusController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public MenusController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index(string categoria)
        {

            var menu = from m in _context.Menus select m;

            if (!string.IsNullOrEmpty(categoria))
            {
                menu = menu.Where(m => m.Categoria == categoria);
            }

            return View(await menu.ToListAsync());
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombrePlato,Descripcion,Precio,CotizacionId,Categoria,ImagenUrl")] Menu menu, IFormFile ImagenFile)
        {
            if (ImagenFile != null && ImagenFile.Length > 0)
            {
                // Ruta fuera del proyecto
                var externalImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Repositorio", "img");
                if (!Directory.Exists(externalImagePath))
                {
                    Directory.CreateDirectory(externalImagePath);
                }

                var filePath = Path.Combine(externalImagePath, ImagenFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagenFile.CopyToAsync(stream);
                }

                menu.ImagenUrl = "/ExternalImages/" + ImagenFile.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombrePlato,Descripcion,Precio,CotizacionId,Categoria,ImagenUrl")] Menu menu, IFormFile ImagenFile)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ImagenFile != null && ImagenFile.Length > 0)
            {
                // Ruta fuera del proyecto
                var externalImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Repositorios", "img");
                if (!Directory.Exists(externalImagePath))
                {
                    Directory.CreateDirectory(externalImagePath);
                }

                var filePath = Path.Combine(externalImagePath, ImagenFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagenFile.CopyToAsync(stream);
                }

                menu.ImagenUrl = "/ExternalImages/" + ImagenFile.FileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
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
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}
