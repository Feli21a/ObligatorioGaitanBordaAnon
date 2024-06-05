using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class RolesPermiso
{
    public int Id { get; set; }

    public int? RolId { get; set; }

    public int? PermisoId { get; set; }

    public virtual Permiso? Permiso { get; set; }

    public virtual Role? Rol { get; set; }
}
