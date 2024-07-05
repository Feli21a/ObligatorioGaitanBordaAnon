using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Permiso
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();
}
