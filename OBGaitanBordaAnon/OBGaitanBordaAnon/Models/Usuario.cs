using System;
using System.Collections.Generic;

namespace OBGaitanBordaAnon.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Contrasenia { get; set; }

    public bool? EsAdmin { get; set; }

    public bool? EsCliente { get; set; }

    public bool? EsMozo { get; set; }

    public bool? EsCajero { get; set; }

    public bool? EsRecepcionista { get; set; }
}
