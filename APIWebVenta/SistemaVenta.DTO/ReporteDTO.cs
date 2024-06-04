using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ReporteDTO
    {
        public string? numeroDocumento {  get; set; }
        public string? TipoPago { get; set; }
        public string? fechaRegistro { get; set; }
        public string? totalVenta { get; set; }
        public string? Producto { get; set; }
        public int Cantidad { get; set; }
        public string? Precio { get; set; }
        public string? Total { get; set; }
    }
}
