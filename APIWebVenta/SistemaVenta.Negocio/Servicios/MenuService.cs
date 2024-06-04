using SistemaVenta.Negocio.Servicios.Contrato;
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
    // Clase que implementa la interfaz IMenuService para gestionar los menús del sistema
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> userRepo; // Repositorio genérico para acceder a los datos de usuario
        private readonly IGenericRepository<MenuRol> mrRepo; // Repositorio genérico para acceder a los datos de roles de menú
        private readonly IGenericRepository<Menu> menuRepo; // Repositorio genérico para acceder a los datos de menú
        private readonly IMapper mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe los repositorios y el objeto AutoMapper
        public MenuService(IGenericRepository<Usuario> userRepo, IGenericRepository<MenuRol> mrRepo, IGenericRepository<Menu> menuRepo, IMapper mapper)
        {
            this.userRepo = userRepo; // Inicializa el repositorio genérico de usuario
            this.mrRepo = mrRepo; // Inicializa el repositorio genérico de roles de menú
            this.menuRepo = menuRepo; // Inicializa el repositorio genérico de menú
            this.mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método público para obtener la lista de menús disponibles para un usuario específico
        public async Task<List<MenuDTO>> listaMenus(int idUsuario)
        {
            // Consulta el usuario, los roles de menú y los menús en la base de datos
            IQueryable<Usuario> tbUsuario = await userRepo.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tbRol = await mrRepo.Consultar();
            IQueryable<Menu> tbMenu = await menuRepo.Consultar();

            try
            {
                // Realiza una consulta LINQ para obtener los menús disponibles para el usuario
                IQueryable<Menu> tbResultado = (from u in tbUsuario
                                                join mr in tbRol on u.IdRol equals mr.IdRol
                                                join m in tbMenu on mr.IdMenu equals m.IdMenu
                                                select m).AsQueryable();
                var listaMenus = tbResultado.ToList(); // Convierte el resultado en una lista
                return mapper.Map<List<MenuDTO>>(listaMenus); // Mapea los menús a DTOs y devuelve la lista resultante
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }
    }
}
