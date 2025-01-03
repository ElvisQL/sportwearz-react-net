using Eccomerce.DTO;
using Eccomerce.DTO.Venta;
using Eccomerce.Servicio.VentaService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO modelo)
        {
            var response = new ResponseDTO<VentaDTO>();
            try
            {
                response.Success = true;
                response.Response = await _ventaService.Registrar(modelo);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

            }

            return Ok(response);
        }

        [HttpGet("Listar/{busqueda:alpha}")]
        public async Task<IActionResult> Listar(string busqueda)
        {
            var response = new ResponseDTO<List<VentaDTO>>();
            try
            {
                response.Success = true;
                response.Response = await _ventaService.ListarVentas(busqueda);
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
