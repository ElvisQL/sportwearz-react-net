using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.DetalleVenta
{
    public class DetalleVentaDTO
    {
        public int IdDetalleVenta { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        // Para mostrar en front
        public string? ProductoNombre { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
