using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace SistemaVenta.Datos.Repositorios.Contrato
{
    public interface IGenericRepository<T> where T : class
    {
        /*
         Son todos los metodos para comunicarse con la informacion de las tablas
        */
        Task<T> Obtener(Expression<Func<T, bool>>filtro);
        Task<T> Crear(T model);
        Task<bool> Editar(T model);
        Task<bool> Eliminar(T model);
        Task<IQueryable<T>> Consultar(Expression<Func<T, bool>> filtro = null);

    }
}
