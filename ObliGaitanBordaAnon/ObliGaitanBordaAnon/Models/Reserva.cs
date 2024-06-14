using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public int? MesaId { get; set; }

    public string? Nombre { get; set; }

    public DateTime FechaReservada { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Mesa? Mesa { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
