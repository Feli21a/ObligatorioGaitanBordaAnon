using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Clima
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Temperatura { get; set; }

    public bool Lluvia { get; set; }

    public string DescripcionClima { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
