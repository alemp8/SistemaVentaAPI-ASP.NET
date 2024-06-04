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

namespace SistemaVenta.Negocio.Servicios
{
    // Clase que implementa la interfaz IUsuarioService para gestionar los usuarios
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> UsuarioRepo; // Repositorio genérico para acceder a los datos de usuarios
        private readonly IMapper _mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe el repositorio genérico de usuarios y el objeto AutoMapper
        public UsuarioService(IGenericRepository<Usuario> usuarioRepo, IMapper mapper)
        {
            UsuarioRepo = usuarioRepo; // Inicializa el repositorio genérico de usuarios
            _mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método público para obtener una lista de todos los usuarios
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                // Consulta todos los usuarios en la base de datos, incluyendo la información del rol
                var queryUsuario = await UsuarioRepo.Consultar();
                var listaUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                // Mapea los usuarios a DTOs y devuelve la lista resultante
                return _mapper.Map<List<UsuarioDTO>>(listaUsuario);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para validar las credenciales de inicio de sesión
        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                // Busca el usuario en la base de datos por correo y clave
                var queryUsuario = await UsuarioRepo.Consultar(u =>
                    u.Correo == correo &&
                    u.Clave == clave
                );

                if (queryUsuario.FirstOrDefault() == null)
                {
                    // Si el usuario no se encontró, lanza una excepción
                    throw new TaskCanceledException("El Usuario no existe");
                }

                // Obtiene el primer usuario encontrado con su rol asociado
                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                // Mapea el usuario a un DTO de sesión y lo devuelve
                return _mapper.Map<SesionDTO>(devolverUsuario);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para crear un nuevo usuario
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                // Crea el usuario en la base de datos
                var usuarioCreado = await UsuarioRepo.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioCreado.IdUsuario == 0)
                {
                    // Si el usuario no se pudo crear, lanza una excepción
                    throw new TaskCanceledException("El Usuario no se creó");
                }

                // Consulta el usuario recién creado de nuevo para incluir la información del rol asociado
                var query = await UsuarioRepo.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
                // Mapea el usuario a un DTO y lo devuelve
                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para editar un usuario existente
        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                // Mapea el DTO a una entidad de usuario
                var usuariomodelo = _mapper.Map<Usuario>(modelo);
                // Obtiene el usuario existente de la base de datos por su ID
                var usuarioEncontrado = await UsuarioRepo.Obtener(u => u.IdUsuario == usuariomodelo.IdUsuario);
                if (usuarioEncontrado == null)
                {
                    // Si el usuario no se encontró, lanza una excepción
                    throw new TaskCanceledException("El Usuario no se encontró");
                }

                // Actualiza los campos del usuario existente con los datos del DTO
                usuarioEncontrado.NombreCompleto = usuariomodelo.NombreCompleto;
                usuarioEncontrado.Correo = usuariomodelo.Correo;
                usuarioEncontrado.IdRol = usuariomodelo.IdRol;
                usuarioEncontrado.Clave = usuariomodelo.Clave;
                usuarioEncontrado.EsActivo = usuariomodelo.EsActivo;

                // Guarda los cambios en la base de datos
                bool respuesta = await UsuarioRepo.Editar(usuarioEncontrado);
                if (!respuesta)
                {
                    // Si no se pudieron guardar los cambios, lanza una excepción
                    throw new TaskCanceledException("El Usuario no se pudo editar");
                }

                return respuesta; // Devuelve el resultado de la operación de edición
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para eliminar un usuario
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                // Busca el usuario en la base de datos por su ID
                var usuarioEncontrado = await UsuarioRepo.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    // Si el usuario no se encontró, lanza una excepción
                    throw new TaskCanceledException("El Usuario no se encontró");
                }

                // Elimina el usuario de la base de datos
                bool respuesta = await UsuarioRepo.Eliminar(usuarioEncontrado);
                if (!respuesta)
                {
                    // Si no se pudo eliminar el usuario, lanza una excepción
                    throw new TaskCanceledException("El Usuario no se pudo eliminar");
                }

                return respuesta; // Devuelve el resultado de la operación de eliminación
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }

        }
    }
}
