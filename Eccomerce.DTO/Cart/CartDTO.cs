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
        public int CartId { get; set; } // Añadir esta propiedad
        
        public List<CartItemDTO> CartItems { get; set; }

        public decimal TotalPrice => CartItems.Sum(item => item.Total); // Total del carrito
    }
}
