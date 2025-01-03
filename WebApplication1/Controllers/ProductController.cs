using Eccomerce.DTO.User;
using Eccomerce.DTO;
using Eccomerce.DTO.Producto;
using Eccomerce.Servicio.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("lista/{busqueda:alpha?}")]
        public async Task<IActionResult> Lista(string busqueda = "NA")
        {
            var response = new ResponseDTO<List<ProductoDTO>>();

            try
            {
                if (busqueda == "NA") busqueda = "";
                response.Success = true;
                response.Response = await _productService.Listar(busqueda);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);

        }

        [HttpGet("Catalogo/{categoria:alpha}/{busqueda:alpha?}")]
        public async Task<IActionResult> Catalogo(string categoria, string busqueda = "NA")
        {
            var response = new ResponseDTO<List<ProductoDTO>>();

            try
            {
                if (busqueda == "NA") busqueda = "";
                if (categoria.ToLower() == "todos") categoria = "";
                response.Success = true;
                response.Response = await _productService.Catalogo(categoria, busqueda);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);
        }


        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = new ResponseDTO<ProductoDTO>();
            try
            {
                response.Success = true;
                response.Response = await _productService.Read(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }





        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductoDTO modelo)
        {
            var response = new ResponseDTO<ProductoDTO>();
            try
            {
                response.Success = true;
                response.Response = await _productService.Create(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }



        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] ProductoDTO modelo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _productService.Update(modelo);
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
                response.Response = await _productService.Delete(id);
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
    

