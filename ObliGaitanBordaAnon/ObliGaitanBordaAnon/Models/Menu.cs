using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string NombrePlato { get; set; } = null!;

    [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
    public string? Descripcion { get; set; }

    public int Precio { get; set; }

    [StringLength(50, ErrorMessage = "La descripción no puede tener más de 50 caracteres.")]
    public string? Categoria { get; set; }

    public int? CotizacionId { get; set; }

    [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
    public string? ImagenUrl { get; set; }

    public virtual Cotizacione? Cotizacion { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
