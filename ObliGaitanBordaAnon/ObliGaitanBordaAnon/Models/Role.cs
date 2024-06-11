using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Role
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ingresa el nombre del rol")]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
