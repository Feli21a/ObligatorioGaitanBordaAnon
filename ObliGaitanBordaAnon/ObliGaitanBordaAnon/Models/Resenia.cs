using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Resenia
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public int? RestauranteId { get; set; }

    public int? Puntaje { get; set; }
    [StringLength(250)]
    public string? Comentario { get; set; }

    public DateTime? FechaResenia { get; set; }
    [StringLength(50)]
    public string? Email { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Restaurante? Restaurante { get; set; }
}
