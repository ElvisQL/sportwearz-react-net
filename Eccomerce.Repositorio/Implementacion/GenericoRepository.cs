using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.MODELO;
using Microsoft.EntityFrameworkCore;
namespace Eccomerce.Repositorio.Implementacion

{
    public class GenericoRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EccomerceDbContext _dbContext;

        public GenericoRepository(EccomerceDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public IQueryable<T> Consultar(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> consultaLINQ = (filtro == null) ? _dbContext.Set<T>() : _dbContext.Set<T>().Where(filtro);
            return consultaLINQ;
        }

        public async Task<T> Crear(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            
        }

        public async Task<bool> Editar(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public async Task<bool> Eliminar(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
        public async Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds)
        {
            return await _dbContext.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync();
        }
    }
}
