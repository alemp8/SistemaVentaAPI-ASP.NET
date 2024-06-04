using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Datos.DBContext;
using SistemaVenta.Datos.Repositorios;
using SistemaVenta.Datos.Repositorios.Contrato;
using SistemaVenta.Modelos.Modelos;

namespace SistemaVenta.Datos.Repositorios
{
    // Define la clase VentaRepository que hereda de GenericRepository<Venta> e implementa IVentaRepository
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        // Campo privado que almacena el contexto de la base de datos
        private readonly DbventaContext dbcontext;

        // Constructor que inicializa el campo dbcontext y llama al constructor de la clase base
        public VentaRepository(DbventaContext dbventa) : base(dbventa)
        {
            this.dbcontext = dbventa; // Asigna el parámetro al campo de clase
        }

        // Método asíncrono que registra una venta en la base de datos
        public async Task<Venta> Registrar(Venta modelo)
        {
            // Crea una nueva instancia de Venta
            Venta nuevaVenta = new Venta();

            // Inicia una transacción de base de datos
            using (var transaction = dbcontext.Database.BeginTransaction())
            {
                try
                {
                    // Actualiza el stock de los productos en función de los detalles de la venta
                    foreach (DetalleVenta detalleVenta in modelo.DetalleVenta)
                    {
                        Producto productos = dbcontext.Productos.Where(p => p.IdProducto == detalleVenta.IdProducto).First();
                        productos.Stock = productos.Stock - detalleVenta.Cantidad;
                    }

                    // Guarda los cambios en la base de datos
                    await dbcontext.SaveChangesAsync();

                    // Actualiza el número de documento
                    NumeroDocumento correlativo = dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.Fecha = DateTime.Now;

                    // Actualiza el registro del número de documento en la base de datos
                    dbcontext.NumeroDocumentos.Update(correlativo);
                    await dbcontext.SaveChangesAsync();

                    // Genera el número de venta con ceros a la izquierda
                    int Digitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", Digitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - Digitos);

                    // Asigna el número de documento a la venta y la guarda en la base de datos
                    modelo.NumeroDocumento = numeroVenta;
                    await dbcontext.Venta.AddAsync(modelo);
                    await dbcontext.SaveChangesAsync();

                    // Asigna el modelo de venta registrado a nuevaVenta
                    nuevaVenta = modelo;

                    // Confirma la transacción
                    transaction.Commit();
                }
                catch
                {
                    // Revierte la transacción en caso de error
                    transaction.Rollback();
                    throw;
                }
            }

            // Devuelve la nueva venta registrada
            return nuevaVenta;
        }
    }

}

