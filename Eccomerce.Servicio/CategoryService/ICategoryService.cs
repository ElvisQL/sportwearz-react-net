using Eccomerce.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.CategoryService
{
    public interface ICategoryService: ICRUDService<CategoryDTO>
    {
        Task<List<CategoryDTO>> ListarCategorias(string busqueda);

    }
}
