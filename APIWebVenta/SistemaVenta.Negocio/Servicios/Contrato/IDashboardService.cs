using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemaVenta.Negocio.Servicios.Contrato
{
    public interface IDashboardService
    {
        Task<DashboardDTO> Resumen();

    }
}
