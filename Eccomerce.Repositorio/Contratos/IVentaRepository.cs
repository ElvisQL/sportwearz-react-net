using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.MODELO;

namespace Eccomerce.Repositorio.Contratos
{
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> RegistrarVenta(Venta model);
        Task<bool> ActualizarEstadoVenta(int idVenta,string nuevoEstado);
    }
}
