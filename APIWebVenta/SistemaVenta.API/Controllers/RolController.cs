using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DTO;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService rolservicio;

        public RolController(IRolService rolservicio)
        {
            this.rolservicio = rolservicio;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> lista()
        {
            var rsp = new Response<List<RolDTO>>();
            try
            {
                rsp.status = true;
                rsp.Value = await rolservicio.Lista();
            }
            catch(Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
