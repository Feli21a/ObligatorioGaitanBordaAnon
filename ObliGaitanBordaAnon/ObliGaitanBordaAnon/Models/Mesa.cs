using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Mesa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    public int NumeroMesa { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    public int Capacidad { get; set; }

    public string? Estado { get; set; }

    public int? RestauranteId { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Restaurante? Restaurante { get; set; }
}
