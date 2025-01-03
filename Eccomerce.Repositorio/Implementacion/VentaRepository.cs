using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.MODELO;

namespace Eccomerce.Repositorio.Implementacion
{
    public class VentaRepository : GenericoRepository<Venta>, IVentaRepository
    {
        private readonly EccomerceDbContext _dbContext;
        public VentaRepository(EccomerceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> RegistrarVenta(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            using (var transaction = _dbContext.Database.BeginTransaction())
                try

                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Product productoEncontrado =
                            _dbContext.Products.Where(p => p.ProductId == dv.IdProducto).First();

                        productoEncontrado.Stock = (int)(productoEncontrado.Stock - dv.Cantidad);
                        _dbContext.Products.Update(productoEncontrado);

                    }
                    await _dbContext.Venta.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();
                    ventaGenerada = modelo;
                    transaction.Commit();
                }
                catch 
                {
                    transaction.Rollback();
                }
            return ventaGenerada;

            {
                
            }
        }
    }
}
