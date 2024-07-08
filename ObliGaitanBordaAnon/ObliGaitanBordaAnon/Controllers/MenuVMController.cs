using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ObliGaitanBordaAnon.Models;

public class MenuVMController : Controller
{
    private readonly RestoMalTiempoDbContext _context;

    public MenuVMController(RestoMalTiempoDbContext context)
    {
        _context = context;
    }

    // GET: MenuVM
    public async Task<IActionResult> Index(string categoria)
    {
        var menus = _context.Menus
            .Select(menu => new MenuViewModel
            {
                Id = menu.Id,
                NombrePlato = menu.NombrePlato,
                Descripcion = menu.Descripcion,
                Precio = menu.Precio,
                Categoria = menu.Categoria,
                ImagenUrl = menu.ImagenUrl
            });

        if (!string.IsNullOrEmpty(categoria))
        {
            menus = menus.Where(m => m.Categoria == categoria);
        }

        return View(await menus.ToListAsync());
    }

    // GET: Menu/Detalles/5
    public async Task<IActionResult> Detalles(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menu = await _context.Menus
            .Where(menu => menu.Id == id)
            .Select(menu => new MenuViewModel
            {
                Id = menu.Id,
                NombrePlato = menu.NombrePlato,
                Descripcion = menu.Descripcion,
                Precio = menu.Precio,
                Categoria = menu.Categoria,
                ImagenUrl = menu.ImagenUrl
            })
            .FirstOrDefaultAsync();

        if (menu == null)
        {
            return NotFound();
        }

        return View(menu);
    }
}