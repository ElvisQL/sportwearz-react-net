using Eccomerce.DTO;
using Eccomerce.DTO.Venta;
using Eccomerce.Servicio.VentaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                if (modelo.DetalleVenta == null || !modelo.DetalleVenta.Any())
                    return BadRequest("El carrito está vacío");

                // Obtener usuario autenticado
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                modelo.IdUsuario = userId;
                modelo.FechaCreacion = DateTime.UtcNow;
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
        [Authorize(Roles = "Admin")]
        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            var response = new ResponseDTO<List<VentaDTO>>();
            try
            {
                response.Success = true;
                response.Response = await _ventaService.ListarVentas();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

            }

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{idVenta}/estado")]
        public async Task<IActionResult> ActualizarEstado(int idVenta, [FromBody] string nuevoEstado)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.Success = true;
                response.Response = await _ventaService.CambiarEstadoVenta(idVenta, nuevoEstado);
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
