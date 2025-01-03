using Eccomerce.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.DTO.Brand;

namespace Eccomerce.Servicio.BrandService
{
    public interface IBrandService : ICRUDService<BrandDTO>
    {
        Task<List<BrandDTO>> ListarMarcas(string busqueda);
    }
}
