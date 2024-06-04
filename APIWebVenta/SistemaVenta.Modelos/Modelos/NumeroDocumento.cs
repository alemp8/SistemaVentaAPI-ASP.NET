using System;
using System.Collections.Generic;

namespace SistemaVenta.Modelos.Modelos;

public partial class NumeroDocumento
{
    public int IdNumeroDocumento { get; set; }

    public int UltimoNumero { get; set; }

    public DateTime? Fecha { get; set; }
}
