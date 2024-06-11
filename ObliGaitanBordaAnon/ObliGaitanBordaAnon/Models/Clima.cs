using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Clima
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Temperatura { get; set; }

    public bool? Lluvia { get; set; }
    [Required]
    [StringLength(50)]
    public string DescripcionClima { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
