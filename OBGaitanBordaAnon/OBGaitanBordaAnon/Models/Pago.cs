using System;
using System.Collections.Generic;

namespace OBGaitanBordaAnon.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int? ReservaId { get; set; }

    public int Monto { get; set; }

    public DateTime FechaPago { get; set; }

    public string MetodoPago { get; set; } = null!;

    public int? ClimaId { get; set; }

    public virtual Clima? Clima { get; set; }

    public virtual Reserva? Reserva { get; set; }
}
