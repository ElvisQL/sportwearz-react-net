using AutoMapper;
using Eccomerce.DTO.Cart;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.CartService
{
    public class CartService : ICartService
    {


        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;


        public CartService(
            IGenericRepository<Cart> cartRepository,
            IGenericRepository<CartItem> cartItemRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Product> productRepository,
            IMapper mapper
            )
        {
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _cartRepository = cartRepository;
        }

        public async Task<bool> SaveCart(int userId, CartDTO updatedCart)
        {
            try
            {
                // Obtener el usuario con su carrito y items
                var user = await _userRepository.Consultar(u => u.UserId == userId)
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync();

                if (user == null) throw new Exception("Usuario no encontrado");

                // 1. Manejar el carrito existente o nuevo
                if (user.Cart == null)
                {
                    var newCart = _mapper.Map<Cart>(updatedCart);
                    newCart.UserId = userId;
                    await _cartRepository.Crear(newCart);
                    return true;
                }

                var existingCart = user.Cart;

                // 2. Eliminar items que ya no están en el DTO
                var itemsToRemove = existingCart.CartItems
                    .Where(ci => !updatedCart.CartItems.Any(dto => dto.ProductId == ci.ProductId))
                    .ToList();

                foreach (var item in itemsToRemove)
                {
                    await _cartItemRepository.Eliminar(item); // Usar repositorio de CartItem
                }

                // 3. Actualizar o agregar nuevos items
                foreach (var itemDto in updatedCart.CartItems)
                {
                    var existingItem = existingCart.CartItems
                        .FirstOrDefault(ci => ci.ProductId == itemDto.ProductId);

                    if (existingItem != null)
                    {
                        // Actualizar cantidad
                        existingItem.Quantity = itemDto.Quantity;
                        await _cartItemRepository.Editar(existingItem);
                    }
                    else
                    {
                        // Crear nuevo item
                        var newItem = _mapper.Map<CartItem>(itemDto);
                        newItem.CartId = existingCart.CartId;
                        await _cartItemRepository.Crear(newItem);
                    }
                }

                // 4. Actualizar fechas del carrito
                existingCart.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.Editar(existingCart);

                return true;
            }
            catch (Exception ex)
            {
                // Loggear error
                Console.WriteLine($"Error guardando carrito: {ex.Message}");
                throw;
            }
        }

        public async Task<CartDTO> GetCartService(int userId)
        {
            var user = await _userRepository.Consultar(u => u.UserId == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (user?.Cart == null)
            {
                var newCart = new Cart { UserId = userId };
                await _cartRepository.Crear(newCart);
                return _mapper.Map<CartDTO>(newCart);
            }

            return _mapper.Map<CartDTO>(user.Cart);
        }
        public async Task<CartDTO> AddItemToCart(int userId, CartItemRequestDTO item)
        {
            // Obtener la entidad Cart, no el DTO
            var user = await _userRepository.Consultar(u => u.UserId == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (user?.Cart == null)
            {
                user.Cart = new Cart { UserId = userId };
                await _cartRepository.Crear(user.Cart);
            }

            var cart = user.Cart;

            // Buscar producto
            var product = await _productRepository.Consultar(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
            if (product == null) throw new Exception("Producto no existe");

            // Manejar el ítem
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                await _cartItemRepository.Editar(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CartId = cart.CartId, // Ahora cart es la entidad, tiene CartId
                    AddedAt = DateTime.UtcNow
                };
                await _cartItemRepository.Crear(newItem); // Usar repositorio de CartItem
            }
            var updatedCart = await _cartRepository.Consultar(c => c.CartId == cart.CartId)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) // <-- Recargar con relaciones
                .FirstOrDefaultAsync();

            return _mapper.Map<CartDTO>(updatedCart);

            
        }
        public async Task<CartDTO> RemoveItemFromCart(int userId, int itemId)
        {
            var user = await _userRepository.Consultar(u => u.UserId == userId)
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (user?.Cart == null)
            {
                user.Cart = new Cart { UserId = userId };
                await _cartRepository.Crear(user.Cart);
            }
            var cart = user.Cart;

            // Buscar el ítem a eliminar
            var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId);

            if (itemToRemove == null)
            {
                throw new Exception("El ítem no existe en el carrito");
            }

            // Eliminar el ítem
            cart.CartItems.Remove(itemToRemove);

            // Actualizar el carrito en la base de datos
            await _cartItemRepository.Eliminar(itemToRemove); // Asumiendo que tienes un repositorio para CartItem

            // Recargar el carrito actualizado
            var updatedCart = await _cartRepository.Consultar(c => c.CartId == cart.CartId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            return _mapper.Map<CartDTO>(updatedCart);


        }


        public async Task<CartDTO> UpdateItemQuantity(int userId, int itemId, int quantity)
        {
            // Obtener usuario con carrito y relaciones
            var user = await _userRepository.Consultar(u => u.UserId == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            if (user.Cart == null)
            {
                throw new Exception("El usuario no tiene un carrito activo");
            }

            var cart = user.Cart;

            // Buscar el ítem específico por CartItemId
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId);

            if (cartItem == null)
            {
                throw new Exception("Ítem no encontrado en el carrito");
            }

            // Validar cantidad
            if (quantity < 0)
            {
                throw new ArgumentException("La cantidad no puede ser negativa", nameof(quantity));
            }

            // Lógica de actualización/eliminación
            if (quantity == 0)
            {
                // Eliminar ítem
                cart.CartItems.Remove(cartItem);
                await _cartItemRepository.Eliminar(cartItem);
            }
            else
            {
                // Actualizar cantidad
                cartItem.Quantity = quantity;
                await _cartItemRepository.Editar(cartItem);
            }

            // Recargar datos actualizados con relaciones
            var updatedCart = await _cartRepository.Consultar(c => c.CartId == cart.CartId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            return _mapper.Map<CartDTO>(updatedCart);
        }
    }
}
