using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.Negocio.Servicios;
using SistemaVenta.Modelos.Modelos;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        public readonly IVentaService VentaService;

        public VentasController(IVentaService ventaService)
        {
            VentaService = ventaService;
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string Buscar,string? numerodeVenta, string? fechainicio, string? fechafin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numerodeVenta = numerodeVenta is null ? "" : numerodeVenta;
            fechainicio = fechainicio is null ? "" : fechainicio;
            fechafin = fechafin is null ? "" : fechafin;

            try
            {
                rsp.status = true;
                rsp.Value = await VentaService.Historial(Buscar, numerodeVenta,fechainicio,fechafin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechainicio, string? fechafin)
        {
            var rsp = new Response<List<ReporteDTO>>();
            fechainicio = fechainicio is null ? "" : fechainicio;
            fechafin = fechafin is null ? "" : fechafin;

            try
            {
                rsp.status = true;
                rsp.Value = await VentaService.Reporte(fechainicio, fechafin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();
            try
            {
                rsp.status = true;
                rsp.Value = await VentaService.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }


    }
}
