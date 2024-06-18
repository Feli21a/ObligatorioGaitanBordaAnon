using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Cotizacione
{
    public int Id { get; set; }

    public string? NombreDivisa { get; set; }

    public int? CotizacionDivisa { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
