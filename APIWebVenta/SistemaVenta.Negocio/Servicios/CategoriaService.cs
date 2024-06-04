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
    // Clase que implementa la interfaz ICategoriaService para manejar operaciones relacionadas con categorías
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> cateRepo; // Repositorio genérico para operaciones de base de datos relacionadas con categorías
        private readonly IMapper mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe el repositorio genérico de categorías y el objeto AutoMapper
        public CategoriaService(IGenericRepository<Categoria> cateRepo, IMapper mapper)
        {
            this.cateRepo = cateRepo; // Inicializa el repositorio genérico
            this.mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método asíncrono para obtener una lista de todas las categorías
        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategorias = await cateRepo.Consultar(); // Consulta todas las categorías en la base de datos
                return mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList()); // Mapea las categorías a DTOs y devuelve la lista resultante
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }
    }
}
