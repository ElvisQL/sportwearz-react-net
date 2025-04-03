using Eccomerce.DTO.DetalleVenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Venta
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }

        public int? IdUsuario { get; set; }

        public decimal? Total { get; set; }
        public string Estado { get; set; }


        public virtual ICollection<DetalleVentaDTO> DetalleVenta { get; set; } = new List<DetalleVentaDTO>();

        
        
        public DateTime? FechaCreacion { get; set; }
    }
}
