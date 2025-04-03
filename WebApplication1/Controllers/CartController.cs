using Eccomerce.DTO;
using Eccomerce.DTO.Cart;
using Eccomerce.Servicio.CartService;
using Eccomerce.Servicio.UserService;
using Eccomerce.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;
        

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            
        }

        [Authorize]
        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            var response = new ResponseDTO<CartDTO>();
            try
            {
                var userId = User.GetUserId();
                var cart = await _cartService.GetCartService(userId);

                response.Success = true;
                response.Response = cart;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }
        [Authorize]
        [HttpPost("AddItem")] //TODO: no es rest api, borrar dto y hacer que reciba el id por params
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequestDTO itemDTO)
        {
            var response = new ResponseDTO<CartDTO>();
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var updatedCart = await _cartService.AddItemToCart(userId, itemDTO);

                response.Success = true;
                response.Response = updatedCart;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }
        [Authorize]
        [HttpDelete("RemoveItem/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var response = new ResponseDTO<CartDTO>();
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var updatedCart = await _cartService.RemoveItemFromCart(userId, cartItemId);

                response.Success = true;
                response.Response = updatedCart;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }
        [Authorize]
        [HttpPut("UpdateQuantity/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] int quantity)
        {
            var response = new ResponseDTO<CartDTO>();
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var updatedCart = await _cartService.UpdateItemQuantity(
                    userId,
                    cartItemId,
                    quantity
                );

                response.Success = true;
                response.Response = updatedCart;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }

    }
}
