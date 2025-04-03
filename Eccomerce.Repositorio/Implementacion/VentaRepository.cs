using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.MODELO;
using Microsoft.EntityFrameworkCore;

namespace Eccomerce.Repositorio.Implementacion
{
    public class VentaRepository : GenericoRepository<Venta>, IVentaRepository
    {
        private readonly EccomerceDbContext _dbContext;
        
        public VentaRepository(EccomerceDbContext dbContext  ) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<bool> ActualizarEstadoVenta(int idVenta, string nuevoEstado)
        {
            Venta venta = await _dbContext.Venta.FindAsync(idVenta);
            if (venta == null) throw new Exception("Venta no encontrada");
            venta.Estado = nuevoEstado;
            _dbContext.Venta.Update(venta);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Venta> RegistrarVenta(Venta modelo)
        {
           
            using (var transaction = _dbContext.Database.BeginTransaction())
                try

                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        var producto = await _dbContext.Products
                                        .FirstOrDefaultAsync(p => p.ProductId == dv.IdProducto);

                        if (producto == null) throw new Exception("Producto no encontrado");
                        if (producto.Stock < dv.Cantidad) throw new Exception($"Stock insuficiente para {producto.ProductName}");

                        producto.Stock -= dv.Cantidad;
                        
                    }
                    await _dbContext.Venta.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();
                    // 3. Vaciar carrito del usuario
                    var cart = await _dbContext.Carts
                        .Include(c => c.CartItems)
                        .FirstOrDefaultAsync(c => c.UserId == modelo.IdUsuario);

                    if (cart != null)
                    {
                        _dbContext.CartItems.RemoveRange(cart.CartItems);
                        await _dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return modelo;
                }
                catch 
                {
                    transaction.Rollback();
                    throw;
                }
           

            
        }
    }
}
