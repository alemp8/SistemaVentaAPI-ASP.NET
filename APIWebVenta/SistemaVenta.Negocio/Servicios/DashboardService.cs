using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.DTO;
using AutoMapper;
using SistemaVenta.Datos.Repositorios.Contrato;
using SistemaVenta.Modelos.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SistemaVenta.Negocio.Servicios
{
    // Clase que implementa la interfaz IDashboardService para proporcionar datos resumidos del dashboard
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository ventaRepository; // Repositorio de ventas para acceder a los datos de ventas
        private readonly IGenericRepository<Producto> detalleRepo; // Repositorio genérico para acceder a los datos de productos
        private readonly IMapper mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe los repositorios y el objeto AutoMapper
        public DashboardService(IVentaRepository ventaRepository, IGenericRepository<Producto> detalleRepo, IMapper mapper)
        {
            this.ventaRepository = ventaRepository; // Inicializa el repositorio de ventas
            this.detalleRepo = detalleRepo; // Inicializa el repositorio genérico de productos
            this.mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método privado que filtra las ventas en función de los últimos días especificados
        private IQueryable<Venta> retornarVentas(IQueryable<Venta> ventas, int restarDias)
        {
            DateTime? ultimaFecha = ventas.OrderByDescending(v => v.Fecha).Select(v => v.Fecha).First();
            ultimaFecha = ultimaFecha.Value.AddDays(restarDias);

            return ventas.Where(v => v.Fecha.Value.Date >= ultimaFecha.Value.Date);
        }

        // Método privado que calcula el total de ventas en los últimos 7 días
        private async Task<int> totalventa()
        {
            int total = 0;
            IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(ventaQuery, -7);
                total = tablaVenta.Count();
            }

            return total;
        }

        // Método privado que calcula el total de ingresos en los últimos 7 días
        private async Task<string> totalIngresos()
        {
            decimal resultado = 0;
            IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(ventaQuery, -7);
                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }

            return Convert.ToString(resultado, new CultureInfo("en-HN"));
        }

        // Método privado que calcula las ventas por día en la última semana
        private async Task<Dictionary<string, int>> ventasSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(ventaQuery, -7);
                resultado = tablaVenta.GroupBy(v => v.Fecha.Value.Date).OrderBy(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }
            return resultado;
        }

        // Método público que devuelve un resumen del dashboard
        public async Task<DashboardDTO> Resumen()
        {
            DashboardDTO dto = new DashboardDTO();
            try
            {
                // Calcula el total de ventas en la última semana
                dto.TotalVenta = await totalventa();
                // Calcula el total de ingresos en la última semana
                dto.TotalIngresos = await totalIngresos();
                // Calcula las ventas por día en la última semana
                List<VentaSemanaDTO> listaVenta = new List<VentaSemanaDTO>();
                foreach (KeyValuePair<string, int> item in await ventasSemana())
                {
                    listaVenta.Add(new VentaSemanaDTO()
                    {
                        fecha = item.Key,
                        total = item.Value,
                    });
                }
                dto.ventaSemanas = listaVenta;
            }
            catch
            {
                throw;
            }
            // Devuelve el DTO con el resumen del dashboard
            return dto;
        }
    }
}
