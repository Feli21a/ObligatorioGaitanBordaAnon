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
            .Select(m => new MenuViewModel
            {
                Id = m.Id,
                NombrePlato = m.NombrePlato,
                Descripcion = m.Descripcion,
                Precio = m.Precio,
                Categoria = m.Categoria,
                ImagenUrl = m.ImagenUrl
            });

        if (!string.IsNullOrEmpty(categoria))
        {
            menus = menus.Where(m => m.Categoria == categoria);
        }

        return View(await menus.ToListAsync());
    }

    // GET: Menu/Details/5
    public async Task<IActionResult> Detalles(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menu = await _context.Menus
            .Where(m => m.Id == id)
            .Select(m => new MenuViewModel
            {
                Id = m.Id,
                NombrePlato = m.NombrePlato,
                Descripcion = m.Descripcion,
                Precio = m.Precio,
                Categoria = m.Categoria,
                ImagenUrl = m.ImagenUrl
            })
            .FirstOrDefaultAsync();

        if (menu == null)
        {
            return NotFound();
        }

        return View(menu);
    }
}