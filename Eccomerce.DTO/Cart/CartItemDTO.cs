using Eccomerce.DTO.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Cart
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; } // Identificador del producto
        public string ProductName { get; set; } // Nombre del producto
        public int Quantity { get; set; } // Cantidad de este producto en el carrito
        public decimal Price { get; set;} // Precio unitario
        public string ImageUrl { get; set; }
        public decimal Total => Quantity * Price; // Total por este producto
        
    }
}
