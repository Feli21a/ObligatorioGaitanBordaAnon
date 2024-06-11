using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models;

public partial class Cotizacione
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Debes ingresar el tipo de divisa")]
    [StringLength(50)]
    public string? TipoMonedas { get; set; }
    [Required(ErrorMessage ="Debes ingresar el tipo de cambio")]
    public int? CotizacionMoneda { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
