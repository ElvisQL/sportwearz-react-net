using AutoMapper;
using Eccomerce.DTO.Venta;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.MODELO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Eccomerce.Servicio.VentaService
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _verntaRepository;
        private readonly IMapper _mapper;
        public async Task<List<VentaDTO>> ListarVentas(string busqueda)
        {
            try
            {
                var consultaLINQ = _verntaRepository.Consultar();
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response == null)
                {
                    throw new TaskCanceledException("Ocurrio un error al listar ventas");
                }
                return _mapper.Map<List<VentaDTO>>(response);
            }

            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<VentaDTO> Registrar(VentaDTO venta)
        {
            try {
                var dbModelo = _mapper.Map<Venta>(venta);
                var ventaGenerada = await _verntaRepository.RegistrarVenta(dbModelo);
                if (ventaGenerada.IdVenta == 0) {
                    throw new TaskCanceledException("No se pudo registrar la venta");
                }
                return _mapper.Map<VentaDTO>(ventaGenerada);

            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
