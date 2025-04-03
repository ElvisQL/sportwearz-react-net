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
using Eccomerce.Repositorio.Implementacion;


namespace Eccomerce.Servicio.VentaService
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;
        public VentaService(IMapper mapper, IVentaRepository ventaRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ventaRepository = ventaRepository ?? throw new ArgumentNullException(nameof(ventaRepository));
        }
        public async Task<bool> CambiarEstadoVenta(int idVenta, string nuevoEstado)
        {
            // Validar estado permitido
            var estadosValidos = new[] { "Pendiente", "Completada", "Cancelada" };
            if (!estadosValidos.Contains(nuevoEstado))
                throw new Exception("Estado no válido");

            return await _ventaRepository.ActualizarEstadoVenta(idVenta, nuevoEstado);
        }

        public async Task<List<VentaDTO>> ListarVentas()
        {
            try
            {
                var consultaLINQ = _ventaRepository.Consultar()
                    .Include(v => v.DetalleVenta) // Incluir detalles
                    .Include(v => v.IdUsuarioNavigation); // Incluir usuario
                var ventas = await consultaLINQ.ToListAsync(); // Obtener TODOS los registros

                if (ventas == null || !ventas.Any())
                {
                    throw new Exception("No se encontraron ventas");
                }

                return _mapper.Map<List<VentaDTO>>(ventas);
            }

            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<VentaDTO> Registrar(VentaDTO ventaDTO)
        {
            if (ventaDTO == null) throw new ArgumentNullException(nameof(ventaDTO));
            if (ventaDTO.DetalleVenta == null || !ventaDTO.DetalleVenta.Any())
                throw new Exception("El carrito está vacío");

            var venta = _mapper.Map<Venta>(ventaDTO);
            venta.DetalleVenta ??= new List<DetalleVenta>(); // Evita null

            venta.FechaCreacion = DateTime.UtcNow;
            venta.Estado = "Pendiente";

            var ventaRegistrada = await _ventaRepository.RegistrarVenta(venta);
            return _mapper.Map<VentaDTO>(ventaRegistrada);
        }

    }
}
