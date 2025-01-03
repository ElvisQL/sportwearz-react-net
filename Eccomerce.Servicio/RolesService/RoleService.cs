using AutoMapper;
using Eccomerce.DTO.Role;
using Eccomerce.DTO.User;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.RolesService
{
    public class RoleService : IRoleService
    {

        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IGenericRepository<Role> roleRepository,IMapper mapper) { 
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        public async Task<RoleDTO> GetRoleByID(int id)
        {
            try
            {
                var consultaSQL = _roleRepository.Consultar(u => u.RoleId == id);
                var response = await consultaSQL.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<RoleDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el rol");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            try
            {
                var consultaLINQ = _roleRepository.Consultar();
                List<RoleDTO> lista = _mapper.Map<List<RoleDTO>>(await consultaLINQ.ToListAsync());
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
