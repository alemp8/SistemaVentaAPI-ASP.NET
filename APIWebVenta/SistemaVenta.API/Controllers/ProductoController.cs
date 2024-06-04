using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.Negocio.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly IProductoService productoService;

        public ProductoController(IProductoService productoService)
        {
            this.productoService = productoService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> lista()
        {
            var rsp = new Response<List<ProductoDTO>>();
            try
            {
                rsp.status = true;
                rsp.Value = await productoService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<ProductoDTO>();
            try
            {
                rsp.status = true;
                rsp.Value = await productoService.Crear(producto);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> EditarProducto([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.Value = await productoService.Editar(producto);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.Value = await productoService.Eliminar(id);
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
