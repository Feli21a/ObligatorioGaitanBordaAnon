using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class OrdenDetalle
{
    public int Id { get; set; }

    public int? OrdenId { get; set; }

    public int? MenuId { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    public int Cantidad { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Ordene? Orden { get; set; }
}
