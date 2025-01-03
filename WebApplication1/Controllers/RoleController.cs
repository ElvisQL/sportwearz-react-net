using Eccomerce.DTO.User;
using Eccomerce.DTO;
using Eccomerce.Servicio.RolesService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eccomerce.DTO.Role;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) { 
            _roleService = roleService;
        }

        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var response = new ResponseDTO<RoleDTO>();
            try
            {
                response.Success = true;
                response.Response = await _roleService.GetRoleByID(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Lista(string busqueda = "NA")
        {
            var response = new ResponseDTO<List<RoleDTO>>();

            try
            {
                if (busqueda == "NA") busqueda = "";
                response.Success = true;
                response.Response = await _roleService.GetRoles(); //TODO: editar el service si se va agregar busqueda
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);

        }


    }
}
