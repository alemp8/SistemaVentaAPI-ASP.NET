using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Modelos.Modelos;

namespace SistemaVenta.Datos.Repositorios.Contrato
{
    // Define una interfaz IVentaRepository que hereda de IGenericRepository con el tipo Venta
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        // Declara un método asíncrono para registrar una venta, que toma un objeto Venta y devuelve una tarea que resulta en un objeto Venta
        Task<Venta> Registrar(Venta modelo);
    }

}
