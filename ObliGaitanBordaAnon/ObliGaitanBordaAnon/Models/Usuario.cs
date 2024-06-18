using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresa un nombre de usuario")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingresa un email para el usuario")]
    public string Email { get; set; } = null!;

    public string? Contrasenia { get; set; }

    public int? RolId { get; set; }

    public virtual Role? Rol { get; set; }
}
