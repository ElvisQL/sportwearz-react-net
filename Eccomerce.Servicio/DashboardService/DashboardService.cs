using AutoMapper;
using Eccomerce.DTO.Dashboard;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.Repositorio.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepository, IGenericRepository<User> userRepository, IGenericRepository<Product> productRepository, IMapper mapper) { 
            _mapper = mapper;
            _ventaRepository = ventaRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        private string Cliente() {
            var consultaLINQ = _userRepository.Consultar(u => u.Role.RoleName.ToLower() == "cliente");
            int total = consultaLINQ.Count();
            return total.ToString();
        
        }
        private string Ventas() {
            var consulta = _ventaRepository.Consultar();
            int total = consulta.Count();
            return total.ToString();
        }
        private  string Ingresos() {
            var consulta = _ventaRepository.Consultar();
            decimal? total = consulta.Sum(v => v.Total);
            return total.ToString();
        }
        private string Productos() {
            var consulta = _productRepository.Consultar();
            int total = consulta.Count();
            return total.ToString();
        }

        public DashboardDTO GetDashboardResume()
        {
            try
            {
                DashboardDTO dto = new DashboardDTO()
                {
                    TotalClientes = Cliente(),
                    TotalIngresos = Ingresos(),
                    TotalProductos = Productos(),
                    TotalVentas = Ventas(),

                };
                return dto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
    }
}
