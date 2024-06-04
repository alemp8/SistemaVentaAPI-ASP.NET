using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Datos.Repositorios.Contrato;
using SistemaVenta.Datos.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaVenta.Datos.Repositorios
{
    // Define la clase GenericRepository que implementa la interfaz IGenericRepository con el tipo T, donde T es una clase
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Campo privado que almacena el contexto de la base de datos
        private readonly DbventaContext dbcontext;

        // Constructor que inicializa el campo dbcontext con el contexto de base de datos proporcionado
        public GenericRepository(DbventaContext dbcontext)
        {
            this.dbcontext = dbcontext; // Corrige la asignación del parámetro al campo de clase
        }

        // Método asíncrono que consulta entidades en la base de datos, opcionalmente utilizando un filtro
        public async Task<IQueryable<T>> Consultar(Expression<Func<T, bool>> filtro = null)
        {
            try
            {
                // Si no se proporciona un filtro, devuelve todas las entidades; de lo contrario, aplica el filtro
                IQueryable<T> query = filtro == null ? dbcontext.Set<T>() : dbcontext.Set<T>().Where(filtro);
                return query;
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método asíncrono que crea una nueva entidad en la base de datos
        public async Task<T> Crear(T model)
        {
            try
            {
                // Añade la entidad al contexto y guarda los cambios en la base de datos
                dbcontext.Set<T>().Add(model);
                await dbcontext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método asíncrono que edita una entidad existente en la base de datos
        public async Task<bool> Editar(T model)
        {
            try
            {
                // Actualiza la entidad en el contexto y guarda los cambios en la base de datos
                dbcontext.Set<T>().Update(model);
                await dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método asíncrono que elimina una entidad de la base de datos
        public async Task<bool> Eliminar(T model)
        {
            try
            {
                // Elimina la entidad del contexto y guarda los cambios en la base de datos
                dbcontext.Set<T>().Remove(model);
                await dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }

        // Método asíncrono que obtiene una entidad de la base de datos utilizando un filtro
        public async Task<T> Obtener(Expression<Func<T, bool>> filtro)
        {
            try
            {
                // Busca la primera entidad que coincida con el filtro y la devuelve
                T modelo = await dbcontext.Set<T>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch
            {
                throw; // Lanza cualquier excepción que ocurra
            }
        }
    }

}

