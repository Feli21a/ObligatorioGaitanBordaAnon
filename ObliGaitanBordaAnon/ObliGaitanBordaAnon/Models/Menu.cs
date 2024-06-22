using System;
using System.Collections.Generic;

namespace ObliGaitanBordaAnon.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string NombrePlato { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Categoria { get; set; } = null!;

    public double Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
