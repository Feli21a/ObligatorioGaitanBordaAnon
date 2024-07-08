using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(30)]
    public string? Ci { get; set; }
    [StringLength(100)]
    public string? Email { get; set; }

    public string? TipoCliente { get; set; }

    public virtual ICollection<Resenia> Resenia { get; set; } = new List<Resenia>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
