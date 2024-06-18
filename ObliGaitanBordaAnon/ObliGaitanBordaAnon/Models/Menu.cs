using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresa el nombre del plato")]
    [StringLength(50)]
    public string NombrePlato { get; set; } = null!;

    [StringLength(200)]
    public string? Descripcion { get; set; }

    [Required]
    [StringLength(20)]
    public string Categoria { get; set; } = null!;

    [Required(ErrorMessage = "Ingresa un precio")]
    public int Precio { get; set; }

    [StringLength(250)]
    public string? ImagenUrl { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
