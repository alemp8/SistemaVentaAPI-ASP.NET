using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SistemaVenta.DTO;
namespace SistemaVenta.Negocio.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO modelo);
        Task<List<VentaDTO>> Historial(string buscar, string numeroVenta, string fechaInicio, string fechafin);
        Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechafin);
    }
}
