﻿using System;
using System.Collections.Generic;

namespace SistemaVenta.Modelos.Modelos;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
