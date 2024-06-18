using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Restaurante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresa el nombre del restaurante")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingresale la direccion de la sucursal")]
    public string Direccion { get; set; } = null!;

    [Required(ErrorMessage = "Ingresa el telefono de la sucursal")]
    public string Telefono { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

    public virtual ICollection<Resenia> Resenia { get; set; } = new List<Resenia>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
