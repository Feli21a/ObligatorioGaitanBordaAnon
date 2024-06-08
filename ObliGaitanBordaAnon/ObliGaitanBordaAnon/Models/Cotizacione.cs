using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Cotizacione
{
    public int Id { get; set; }

    public string? TipoMonedas { get; set; }

    public int? CotizacionMoneda { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
