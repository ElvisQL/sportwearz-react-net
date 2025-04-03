using Eccomerce.DTO;
using Eccomerce.DTO.User;
using Eccomerce.Servicio.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eccomerce.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Eccomerce.DTO.Cart;
using Eccomerce.MODELO;



namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUserService _userService;
        private IConfiguration _config;

        public UsuarioController(IUserService userService, IConfiguration config) {
            _userService = userService;
            _config = config;
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new ResponseDTO<List<UserSessionDTO>>();

            try
            {

                response.Success = true;
                response.Response = await _userService.ListarUsuarios();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);

        }
        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = new ResponseDTO<UserCreateDTO>();
            try
            {
                response.Success = true;
                response.Response = await _userService.Read(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);

        }





        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO modelo)
        {
            var response = new ResponseDTO<UserCreateDTO>();
            try
            {
                response.Success = true;
                response.Response = await _userService.Create(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }



        [HttpPost("Auth")]
        public async Task<IActionResult> AuthUser([FromBody] UserLoginDTO modelo)
        {
            var response = new ResponseDTO<UserSessionDTO>();
            try
            {
                response.Success = true;
                response.Response = await _userService.Logear(modelo);


                JwtHelper jwtHelper = new JwtHelper(_config);
                string token = jwtHelper.GenerateToken(response.Response.UserId.ToString(), response.Response.Role.RoleName, response.Response.Email);

                response.Response.Token = token;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }


            return Ok(response);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("Edit/{userId:int}")]
        public async Task<IActionResult> Edit([FromRoute] int userId,[FromBody] UserUpdateDTO modelo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _userService.UpdateUserByAdmin(userId,modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _userService.Delete(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }
        //[HttpPost("{userId}/cart")]
        //public async Task<IActionResult> SaveCart(int userId, [FromBody] CartDTO cartItems)
        //{
        //    var response = new ResponseDTO<bool>();
        //    try
        //    {
        //        response.Success = true;
        //        response.Response = await _userService.SaveCart(userId, cartItems);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Success = false;
        //        response.Message = e.Message;
        //    }
        //    return Ok(response);
        //}


        //[HttpGet("{userId}/cart")]
        //public async Task<IActionResult> GetCart(int userId)
        //{
        //    var response = new ResponseDTO<CartDTO>();
        //    try
        //    {
        //        response.Success = true;
        //        response.Response = await _userService.GetCart(userId);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Success = false;
        //        response.Message = e.Message;
        //    }
        //    return Ok(response);
        //}

    }
   

}
