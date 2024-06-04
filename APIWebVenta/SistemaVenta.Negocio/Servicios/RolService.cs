using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Negocio.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Modelos; // No es necesario si estás usando Modelos.Modelos
using AutoMapper;
using SistemaVenta.Datos.Repositorios.Contrato;
using SistemaVenta.Modelos.Modelos;

namespace SistemaVenta.Negocio.Servicios
{
    // Clase que implementa la interfaz IRolService para gestionar los roles
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> rolRepo; // Repositorio genérico para acceder a los datos de roles
        private readonly IMapper _mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe el repositorio genérico de roles y el objeto AutoMapper
        public RolService(IGenericRepository<Rol> rolRepo, IMapper mapper)
        {
            this.rolRepo = rolRepo; // Inicializa el repositorio genérico de roles
            _mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método público para obtener una lista de todos los roles
        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                // Consulta todos los roles en la base de datos
                var listaRoles = await rolRepo.Consultar();
                // Mapea los roles a DTOs y devuelve la lista resultante
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }
    }
}
