using Eccomerce.DTO.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Cart
{
    public class CartDTO
    {
        public ProductoDTO Producto { get; set; }
        public int Cantidad {  get; set; }
        public decimal? Precio { get; set; }
        public decimal? Total { get; set; }
    }
}
