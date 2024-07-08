using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(50)]
    public string NombrePlato { get; set; } = null!;
    [StringLength(200)]
    public string? Descripcion { get; set; }

    public string Categoria { get; set; } = null!;
    [Required(ErrorMessage = "Campo obligatorio")]
    public double Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public string? ImagenUrl { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
