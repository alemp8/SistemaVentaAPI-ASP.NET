using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Modelos.Modelos;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre))
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                destino.IdRolNavigation,
                opt => opt.Ignore())
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));
            #endregion Usuario

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Producto
            CreateMap<Producto, ProductoDTO>()
            .ForMember(destino =>
            destino.CatDescripcion,
            opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre))
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value,new CultureInfo("es-HN"))))
            .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<ProductoDTO, Producto>()
            .ForMember(destino =>
            destino.IdCategoriaNavigation,
            opt => opt.Ignore())
            .ForMember(destino =>
            destino.Precio,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-HN"))))
            .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));

            #endregion Producto

            #region Venta
            CreateMap<Venta, VentaDTO>()
            .ForMember(destino =>
            destino.TotalTexto,
            opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-HN"))))
            .ForMember(destino =>
            destino.Fecha,
            opt => opt.MapFrom(origen => origen.Fecha.Value.ToString("dd/MM/yyyy")));

            CreateMap<VentaDTO, Venta>()
            .ForMember(destino =>
            destino.Total,
            opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-HN"))));
            #endregion Venta


            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destino =>
                destino.Descripcion,
                opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
                .ForMember(destino =>
                destino.PrecioTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-HN"))))
                 .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-HN"))));

            CreateMap<DetalleVentaDTO, DetalleVenta>()
               .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-HN"))))
               .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-HN"))));

            #endregion DetalleVenta

            #region Reportes
            CreateMap<DetalleVenta, ReporteDTO>()
                .ForMember(destino =>
                destino.fechaRegistro,
                opt => opt.MapFrom(origen => origen.IdVentaNavigation.Fecha.Value.ToString("dd/MM/yyyy")))
                .ForMember(destino =>
                destino.numeroDocumento,
                opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento))
                .ForMember(destino =>
                destino.TipoPago,
                opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago))
                .ForMember(destino =>
                destino.totalVenta,
                opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-HN"))))
                .ForMember(destino =>
                destino.Producto,
                opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-HN"))))
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-HN"))));

            #endregion Reportes

        }
    }
}
