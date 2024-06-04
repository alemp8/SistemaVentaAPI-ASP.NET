using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.Datos.DBContext;
using SistemaVenta.Datos.Repositorios.Contrato;
using SistemaVenta.Datos.Repositorios;
using SistemaVenta.Utility;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.Negocio.Servicios;

namespace SistemaVenta.IOC
{
    // Clase estática que define métodos para inyectar dependencias
    public static class Dependencia
    {
        // Método que configura e inyecta las dependencias
        public static void InyectarDependencias(this IServiceCollection service, IConfiguration configuration)
        {
            // Configura el contexto de la base de datos utilizando la cadena de conexión del archivo de configuración
            service.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            // Registra los tipos de repositorio y su implementación
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IVentaRepository, VentaRepository>();

            // Configura AutoMapper
            service.AddAutoMapper(typeof(AutoMapperProfile));

            // Registra los servicios de negocio
            service.AddScoped<IRolService, RolService>();
            service.AddScoped<ICategoriaService, CategoriaService>();
            service.AddScoped<IProductoService, ProductoService>();
            service.AddScoped<IMenuService, MenuService>();
            service.AddScoped<IUsuarioService, UsuarioService>();
            service.AddScoped<IDashboardService, DashboardService>();
            service.AddScoped<IVentaService, VentaService>();
        }
    }
}
