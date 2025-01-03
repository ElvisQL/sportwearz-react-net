using Eccomerce.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.VentaService
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO venta);
        Task<List<VentaDTO>> ListarVentas(string busqueda);
    }
}
