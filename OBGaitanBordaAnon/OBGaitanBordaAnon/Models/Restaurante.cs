using System;
using System.Collections.Generic;

namespace OBGaitanBordaAnon.Models;

public partial class Restaurante
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

    public virtual ICollection<Resenia> Resenia { get; set; } = new List<Resenia>();
}
