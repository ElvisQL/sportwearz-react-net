using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.MODELO;
using Eccomerce.DTO;
using Eccomerce.Repositorio;
using Eccomerce.DTO.User;
using Eccomerce.Servicio;
using Eccomerce.DTO.Cart;

namespace Eccomerce.Servicio.UserService
{
    public interface IUserService: ICRUDService<UserCreateDTO>
    {
        Task<List<UserSessionDTO>> ListarUsuarios();
        Task<UserSessionDTO> Logear(UserLoginDTO modelo);
        Task<bool> UpdateUserByAdmin(int userId,UserUpdateDTO modelo);
        
        
    }
}
