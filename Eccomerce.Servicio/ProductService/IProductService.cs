using Eccomerce.DTO.Producto;
using Eccomerce.MODELO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.Servicio;

namespace Eccomerce.Servicio.ProductService
{
    public interface IProductService: ICRUDService<ProductoDTO>
    {
        Task<List<ProductoDTO>> Listar(string busqueda);
        Task<List<ProductoDTO>> Catalogo(string categoria, string busqueda);

    }
}
