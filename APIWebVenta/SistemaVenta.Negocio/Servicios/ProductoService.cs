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
    // Clase que implementa la interfaz IProductoService para gestionar los productos
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> prodRepo; // Repositorio genérico para acceder a los datos de productos
        private readonly IMapper mapper; // Objeto AutoMapper para mapear entre entidades y DTOs

        // Constructor que recibe el repositorio genérico de productos y el objeto AutoMapper
        public ProductoService(IGenericRepository<Producto> prodRepo, IMapper mapper)
        {
            this.prodRepo = prodRepo; // Inicializa el repositorio genérico de productos
            this.mapper = mapper; // Inicializa el objeto AutoMapper
        }

        // Método público para crear un nuevo producto
        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                // Mapea el DTO a la entidad y crea el producto en la base de datos
                var productoCreado = await prodRepo.Crear(mapper.Map<Producto>(modelo));
                if (productoCreado == null)
                {
                    // Si el producto no se pudo crear, lanza una excepción
                    throw new TaskCanceledException("No se pudo crear el Producto");
                }

                // Mapea la entidad creada de vuelta a DTO y devuelve el resultado
                return mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para editar un producto existente
        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                // Mapea el DTO a la entidad
                var productoModelo = mapper.Map<Producto>(modelo);
                // Obtiene el producto existente de la base de datos
                var productoEncontrado = await prodRepo.Obtener(u => u.IdProducto == productoModelo.IdProducto);
                if (productoEncontrado == null)
                {
                    // Si el producto no se encontró, lanza una excepción
                    throw new TaskCanceledException("El Producto no se encontró");
                }

                // Actualiza los campos del producto existente con los datos del DTO
                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                // Guarda los cambios en la base de datos
                bool respuesta = await prodRepo.Editar(productoEncontrado);
                if (!respuesta)
                {
                    // Si no se pudieron guardar los cambios, lanza una excepción
                    throw new TaskCanceledException("El producto no se pudo editar");
                }

                return respuesta; // Devuelve el resultado de la operación de edición
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para eliminar un producto
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                // Busca el producto en la base de datos por su ID
                var productoEncontrado = await prodRepo.Obtener(u => u.IdProducto == id);
                if (productoEncontrado == null)
                {
                    // Si el producto no se encontró, lanza una excepción
                    throw new TaskCanceledException("El producto no se encontró");
                }

                // Elimina el producto de la base de datos
                bool respuesta = await prodRepo.Eliminar(productoEncontrado);
                if (!respuesta)
                {
                    // Si no se pudo eliminar el producto, lanza una excepción
                    throw new TaskCanceledException("El producto no se pudo eliminar");
                }

                return respuesta; // Devuelve el resultado de la operación de eliminación
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método público para obtener una lista de todos los productos
        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                // Consulta todos los productos en la base de datos, incluyendo la información de la categoría
                var queryProducto = await prodRepo.Consultar();
                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                // Mapea los productos a DTOs y devuelve la lista resultante
                return mapper.Map<List<ProductoDTO>>(listaProductos);
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }
    }
}
