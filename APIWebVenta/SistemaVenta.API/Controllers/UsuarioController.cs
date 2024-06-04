using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DTO;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.API.Utilidad;
using SistemaVenta.Negocio.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly IUsuarioService UsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            UsuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> lista()
        {
            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.Value = await UsuarioService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> iniciarSesion([FromBody]LoginDTO login)
        {
            var rsp = new Response<SesionDTO>();
            try
            {
                rsp.status = true;
                rsp.Value = await UsuarioService.ValidarCredenciales(login.correo, login.Clave);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<UsuarioDTO>();
            try
            {
                rsp.status = true;
                rsp.Value = await UsuarioService.Crear(usuario);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.mensage = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("EditarUsuario")]
        public async Task<IActionResult> EditarUsuario([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.Value = await UsuarioService.Editar(usuario);
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
                rsp.Value = await UsuarioService.Eliminar(id);
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
