using Eccomerce.DTO.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.CartService
{
    public interface ICartService
    {
        Task<CartDTO> GetCartService(int userId);
        Task<bool> SaveCart(int userId, CartDTO updatedCart);
        Task<CartDTO> AddItemToCart(int userId, CartItemRequestDTO item);
        Task<CartDTO> RemoveItemFromCart(int userId, int itemId);
        Task<CartDTO> UpdateItemQuantity(int userId, int itemId, int quantity);
    }
}
