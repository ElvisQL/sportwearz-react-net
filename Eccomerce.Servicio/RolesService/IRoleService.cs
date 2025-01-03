using Eccomerce.MODELO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.DTO.Role;
using Eccomerce.Repositorio.Contratos;

namespace Eccomerce.Servicio.RolesService
{
    public interface IRoleService
    {



        Task<List<RoleDTO>> GetRoles();
        Task<RoleDTO> GetRoleByID(int id);
    }
}
