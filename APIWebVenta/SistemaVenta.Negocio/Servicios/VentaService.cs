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
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository ventaRepository; // Repositorio específico para ventas
        private readonly IGenericRepository<DetalleVenta> detalleRepo; // Repositorio genérico para detalles de venta
        private readonly IMapper mapper; // Objeto AutoMapper para mapeo de entidades a DTOs

        // Constructor que recibe los repositorios y el objeto AutoMapper
        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleRepo, IMapper mapper)
        {
            this.ventaRepository = ventaRepository; // Inicializa el repositorio de ventas
            this.detalleRepo = detalleRepo; // Inicializa el repositorio de detalles de venta
            this.mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método para obtener el historial de ventas
        public async Task<List<VentaDTO>> Historial(string buscar, string numeroVenta, string fechaInicio, string fechafin)
        {
            IQueryable<Venta> query = await ventaRepository.Consultar(); // Consulta las ventas en la base de datos
            var ListaResultado = new List<Venta>(); // Lista para almacenar el resultado

            try
            {
                if (buscar == "fecha") // Si se está buscando por fecha
                {
                    // Convierte las fechas a objetos DateTime
                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-HN"));
                    DateTime fech_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-HN"));

                    // Filtra las ventas por el rango de fechas y carga los detalles de venta
                    ListaResultado = await query.Where(v =>
                        v.Fecha.Value.Date >= fech_inicio.Date &&
                        v.Fecha.Value.Date <= fech_fin.Date)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else // Si se está buscando por número de venta
                {
                    // Filtra las ventas por el número de venta y carga los detalles de venta
                    ListaResultado = await query.Where(v =>
                        v.NumeroDocumento == numeroVenta)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }

            // Mapea las ventas a DTOs y devuelve la lista resultante
            return mapper.Map<List<VentaDTO>>(ListaResultado);
        }

        // Método para registrar una nueva venta
        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                // Registra la venta en la base de datos
                var ventagenerada = await ventaRepository.Registrar(mapper.Map<Venta>(modelo));

                if (ventagenerada.IdVenta == 0)
                {
                    // Si no se pudo generar la venta, lanza una excepción
                    throw new TaskCanceledException("No se pudo generar la venta");
                }

                // Mapea la venta generada a un DTO y la devuelve
                return mapper.Map<VentaDTO>(ventagenerada);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método para generar un informe de ventas
        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechafin)
        {
            IQueryable<DetalleVenta> query = await detalleRepo.Consultar(); // Consulta los detalles de venta en la base de datos
            var listaResultado = new List<DetalleVenta>(); // Lista para almacenar el resultado

            try
            {
                // Convierte las fechas a objetos DateTime
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-HN"));
                DateTime fech_fin = DateTime.ParseExact(fechafin, "dd/MM/yyyy", new CultureInfo("es-HN"));

                // Filtra los detalles de venta por el rango de fechas y carga la información del producto y la venta asociada
                listaResultado = await query.Include(p =>
                    p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.Fecha.Value.Date >= fech_inicio.Date &&
                        dv.IdVentaNavigation.Fecha.Value.Date <= fech_fin.Date)
                    .ToListAsync();
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }

            // Mapea los detalles de venta a DTOs y devuelve la lista resultante
            return mapper.Map<List<ReporteDTO>>(listaResultado);
        }
    }
}
