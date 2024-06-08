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
    public async Task<IActionResult> Index()
    {
        var menus = await _context.Menus
            .Select(m => new MenuViewModel
            {
                Id = m.Id,
                NombrePlato = m.NombrePlato,
                Descripcion = m.Descripcion,
                Precio = m.Precio
            })
            .ToListAsync();

        return View(menus);
    }

    // GET: MenuVM/Details/5
    public async Task<IActionResult> Details(int? id)
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
                Precio = m.Precio
            })
            .FirstOrDefaultAsync();

        if (menu == null)
        {
            return NotFound();
        }

        return View(menu);
    }
}