using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Restaurante
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ingresa el nombre del restaurante")]
    [StringLength(30)]
    public string Nombre { get; set; } = null!;
    [Required(ErrorMessage = "Ingresa la direccion del restaurante")]
    [StringLength(50)]
    public string Direccion { get; set; } = null!;
    [Required(ErrorMessage = "Ingresa el telefono del restaurante")]
    [StringLength (30)]
    public string Telefono { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

    public virtual ICollection<Resenia> Resenia { get; set; } = new List<Resenia>();
}
