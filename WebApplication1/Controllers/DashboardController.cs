using Eccomerce.DTO;
using Eccomerce.DTO.Dashboard;
using Eccomerce.Servicio.DashboardService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("Resumen")]
        public IActionResult Resumen()
        {
            var response = new ResponseDTO<DashboardDTO>();
            try
            {
                response.Success = true;
                response.Response = _dashboardService.GetDashboardResume();
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
