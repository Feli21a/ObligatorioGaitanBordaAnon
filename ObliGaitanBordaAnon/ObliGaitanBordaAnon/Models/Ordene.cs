using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Ordene
{
    public int Id { get; set; }

    public int? ReservaId { get; set; }

    public double Total { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual Reserva? Reserva { get; set; }
}
