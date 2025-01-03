using Eccomerce.DTO.Producto;
using Eccomerce.DTO;
using Eccomerce.DTO.Category;
using Eccomerce.Servicio.CategoryService;
using Eccomerce.Servicio.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriaController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("lista/{busqueda:alpha?}")]
        public async Task<IActionResult> Lista(string busqueda = "NA")
        {
            var response = new ResponseDTO<List<CategoryDTO>>();

            try
            {
                if (busqueda == "NA") busqueda = "";
                response.Success = true;
                response.Response = await _categoryService.ListarCategorias(busqueda);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);

        }


        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var response = new ResponseDTO<CategoryDTO>();
            try
            {
                response.Success = true;
                response.Response = await _categoryService.Read(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }





        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO modelo)
        {
            var response = new ResponseDTO<CategoryDTO>();
            try
            {
                response.Success = true;
                response.Response = await _categoryService.Create(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }



        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] CategoryDTO modelo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _categoryService.Update(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }


        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _categoryService.Delete(id);
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
