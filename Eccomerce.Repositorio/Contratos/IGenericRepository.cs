
using Eccomerce.DTO.Category;
using Eccomerce.MODELO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Eccomerce.Repositorio.Contratos
{
    public interface IGenericRepository<T> where T :class
    {
        IQueryable<T> Consultar(Expression<Func<T,bool>>? filtro = null);

        Task<T> Crear(T modelo);
        Task<bool> Editar(T modelo);
        Task<bool> Eliminar(T modelo);
        Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds);

    }
}
