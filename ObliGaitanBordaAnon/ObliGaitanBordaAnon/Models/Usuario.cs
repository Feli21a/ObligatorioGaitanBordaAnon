using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Contrasenia { get; set; }

    public int? RolId { get; set; }

    public virtual Role? Rol { get; set; }
}
