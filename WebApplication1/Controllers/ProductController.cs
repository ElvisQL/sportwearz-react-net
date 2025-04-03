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

        [HttpGet("catalogo")]
        public async Task<IActionResult> CatalogoFiltrado(
            [FromQuery] string marcaId = "0",       // Cambiar a string y valor por defecto "0"
            [FromQuery] string categoriaId = "0",   // Cambiar a string y valor por defecto "0"
            [FromQuery] decimal precioMin = 0,
            [FromQuery] decimal precioMax = 0,
            [FromQuery] string busqueda = "")
        {
            var response = new ResponseDTO<List<ProductoDTO>>();
            try
            {
                // Convertir cadenas separadas por comas a listas de enteros, omitiendo "0"
                var marcaIds = marcaId == "0"
                    ? new List<int>()
                    : marcaId.Split(',').Select(int.Parse).ToList();

                var categoriaIds = categoriaId == "0"
                    ? new List<int>()
                    : categoriaId.Split(',').Select(int.Parse).ToList();

                response.Response = await _productService.Catalogo(
                    marcaIds: marcaIds,
                    categoriaIds: categoriaIds,
                    precioMin: precioMin,
                    precioMax: precioMax,
                    busqueda: busqueda
                );
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
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



        [HttpPut("Edit/{productId:int}")]
        public async Task<IActionResult> Edit([FromRoute] int productId,[FromBody] ProductoDTO modelo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _productService.Update(productId,modelo);
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
    

