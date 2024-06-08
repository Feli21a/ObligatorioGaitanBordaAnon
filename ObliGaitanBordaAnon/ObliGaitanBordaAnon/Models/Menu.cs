using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string NombrePlato { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Precio { get; set; }

    public int? CotizacionId { get; set; }

    public virtual Cotizacione? Cotizacion { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
