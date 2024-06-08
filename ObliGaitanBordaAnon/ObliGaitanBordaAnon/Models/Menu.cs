using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string NombrePlato { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Precio { get; set; }

    public int? CotizacionId { get; set; }

    [NotMapped]
    public string ImagenNombre { get; set; }

    public virtual Cotizacione? Cotizacion { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
