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
    public class ReservasController : Controller
    {
        private readonly RestoMalTiempoDbContext _context;

        public ReservasController(RestoMalTiempoDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        [VerificarPermisos("VerCrudReservas")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Index(DateTime? fechafiltro)
        {
            // Creamos la consulta con los Includes necesarios
            var reservasPorFechas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Mesa).Include(r => r.Restaurante).AsQueryable(); // Convertimos a IQueryable para la consistencia de tipos

            // Aplicamos el filtro de fecha si está presente
            if (fechafiltro.HasValue)
            {
                reservasPorFechas = reservasPorFechas.Where(r => r.FechaReservada.Date == fechafiltro.Value.Date);
            }

            return View(await reservasPorFechas.ToListAsync());
        }

        // GET: Reservas/Details/5
        [VerificarPermisos("VerCrudReservas")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Include(r => r.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        [VerificarPermisos("VerCrudReservas")]
        [VerificarPermisos(("VerTodo"))]
        public IActionResult Create()
        {
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre");
            ViewData["MesaId"] = new SelectList(_context.Mesas.Where(r => r.Estado == "Disponible"), "Id", "NumeroMesa");

            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RestauranteId,ClienteId,Nombre,MesaId,FechaReservada,Estado")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                var mesa = await _context.Mesas.FindAsync(reserva.MesaId);

                if (mesa == null)
                {
                    return NotFound(ModelState);
                }

                if (reserva.Estado == "Pendiente")
                {
                    mesa.Estado = "Reservada";
                }
                else if (reserva.Estado == "Confirmada")
                {
                    mesa.Estado = "Ocupada";
                }

                reserva.FechaReservada = DateTime.Now;

                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", reserva.RestauranteId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", reserva.ClienteId);
            ViewData["MesaId"] = new SelectList(_context.Mesas.Where(m => m.Estado == "Disponible"), "Id", "NumeroMesa", reserva.MesaId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        [VerificarPermisos("VerCrudReservas")]
        [VerificarPermisos(("VerTodo"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", reserva.RestauranteId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", reserva.ClienteId);
            ViewData["MesaId"] = new SelectList(_context.Mesas.Where(m => m.Id == id), "Id", "NumeroMesa", reserva.MesaId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RestauranteId,ClienteId,Nombre,MesaId,FechaReservada,Estado")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mesa = await _context.Mesas.FindAsync(reserva.MesaId);
                    if (reserva.Estado == "Pendiente")
                    {
                        mesa.Estado = "Reservada";
                    }
                    else if (reserva.Estado == "Confirmada")
                    {
                        mesa.Estado = "Ocupada";
                    }
                    else
                    {
                        mesa.Estado = "Disponible";
                    }


                    reserva.FechaReservada = DateTime.Now;
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["RestauranteId"] = new SelectList(_context.Restaurantes, "Id", "Direccion", reserva.RestauranteId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", reserva.ClienteId);
            ViewData["MesaId"] = new SelectList(_context.Mesas.Where(m => m.Id == id), "Id", "NumeroMesa", reserva.MesaId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        [VerificarPermisos("VerCrudReservas")]
        [VerificarPermisos("VerTodo")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Include(r => r.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }

            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);

            if (mesa == null)
            {
                return NotFound();
            }

            mesa.Estado = "Disponible";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirmar(int? idReserva, int? nroMesa)
        {
            if (idReserva == null || nroMesa == null)
            {
                return NotFound();
            }

            // Buscar la reserva por su ID
            var reserva = await _context.Reservas.Include(r => r.Cliente).Include(r => r.Mesa).Include(r => r.Restaurante).FirstOrDefaultAsync(r => r.Id == idReserva);

            if (reserva == null)
            {
                return NotFound();
            }
            reserva.Estado = "Confirmada";

            reserva.Mesa.Estado = "Ocupada";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
