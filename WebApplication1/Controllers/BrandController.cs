using Eccomerce.DTO.Category;
using Eccomerce.DTO;
using Eccomerce.DTO.Brand;
using Eccomerce.Servicio.BrandService;
using Eccomerce.Servicio.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("lista/{busqueda:alpha?}")]
        public async Task<IActionResult> Lista(string busqueda = "NA")
        {
            var response = new ResponseDTO<List<BrandDTO>>();

            try
            {
                if (busqueda == "NA") busqueda = "";
                response.Success = true;
                response.Response = await _brandService.ListarMarcas(busqueda);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return Ok(response);

        }


        [HttpGet("Get/{id:int}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var response = new ResponseDTO<BrandDTO>();
            try
            {
                response.Success = true;
                response.Response = await _brandService.Read(id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }





        [HttpPost("Create")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDTO modelo)
        {
            var response = new ResponseDTO<BrandDTO>();
            try
            {
                response.Success = true;
                response.Response = await _brandService.Create(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return Ok(response);
        }



        [HttpPut("Edit/{brandId:int}")]
        public async Task<IActionResult> Edit([FromRoute] int brandId,[FromBody] BrandDTO modelo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _brandService.Update(brandId,modelo);
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
                response.Response = await _brandService.Delete(id);
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

