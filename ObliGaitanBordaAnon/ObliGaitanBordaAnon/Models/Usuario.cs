using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresa el nombre del usuario")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingresa el email del usuario")]
    [StringLength (100)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    public string? Contrasenia { get; set; }

    public int? RolId { get; set; }

    public virtual Role? Rol { get; set; }
}
