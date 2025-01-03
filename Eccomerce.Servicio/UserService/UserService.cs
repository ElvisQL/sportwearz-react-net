using Eccomerce.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.Repositorio;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.MODELO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace Eccomerce.Servicio.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        

        public UserService(IGenericRepository<User> userrepository, IMapper mapper) { 
            _mapper = mapper;
            _userRepository = userrepository;
        }

       
        public async Task<UserCreateDTO> Create(UserCreateDTO modelo)
        {
            try
            {
                if (modelo.PasswordHash != modelo.ConfirmPasswordHash)
                {
                    throw new Exception("Las contraseñas no coinciden");
                }
                var userDB = _mapper.Map<User>(modelo);
                var response = await _userRepository.Crear(userDB);
                if (response.UserId != 0)
                {
                    return _mapper.Map<UserCreateDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("no se pudo crear el usuario");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var consultaLINQ = _userRepository.Consultar(u => u.UserId == id);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    var responseDelete = await _userRepository.Eliminar(response);
                    if (!responseDelete)
                    {
                        throw new TaskCanceledException("No se pudo eliminar el usuario");
                    }
                    return responseDelete;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el usuario a eliminar");
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        public async Task<List<UserSessionDTO>> ListarUsuarios(string rol, string busqueda)
        {
            try
            {
                var consultaLINQ = _userRepository
                    .Consultar(u => u.Role.RoleName == rol && (string.Concat(u.Email, u.Username, u.FirstName, u.LastName)).Contains(busqueda))
                    .Include(u => u.Role); //TODO NO MUESTRA EL ROLE PUES USERCREATE NO TIENE, HACER OTRO DTO PARA MUESTRA(?
                
                List<UserSessionDTO> lista = _mapper.Map<List<UserSessionDTO>>(await consultaLINQ.ToListAsync());
                return lista; 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //TODO: TOKEN HERE
        public async Task<UserSessionDTO> Logear(UserLoginDTO modelo)
        {
            try
            {
                var consultaLINQ = _userRepository.Consultar(u => u.Email == modelo.Email && u.PasswordHash == modelo.PasswordHash).Include(u => u.Role);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<UserSessionDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron coincidencias");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        public async Task<UserCreateDTO> Read(int id)
        {
            try
            {
                var consultaSQL = _userRepository.Consultar(u => u.UserId == id);
                var response = await consultaSQL.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<UserCreateDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el usuario");
                }
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }

        public async Task<bool> Update(UserCreateDTO modelo)
        {
            try
            {

                var consultaLINQ = _userRepository.Consultar(p => p.UserId == modelo.UserId);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    response.FirstName = modelo.FirstName;
                    response.LastName = modelo.LastName;
                    response.PhoneNumber = modelo.PhoneNumber;
                    response.Email = modelo.Email; // TODO :POSIBLEMENTE EDITAR PASSWORD
                    response.Username = modelo.Username;
                    var responseUser = await _userRepository.Editar(response);
                    if (!responseUser)
                    {
                        throw new TaskCanceledException("No se pudo editar el usuario");
                    }
                    return responseUser;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el usuario a editar");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUserByAdmin(UserUpdateDTO modelo)
        {

            try
            {

                var consultaLINQ = _userRepository.Consultar(p => p.UserId == modelo.UserId);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    response.FirstName = modelo.FirstName;
                    response.LastName = modelo.LastName;
                    response.PhoneNumber = modelo.PhoneNumber;
                    response.Email = modelo.Email;
                    response.Username = modelo.Username;
                    response.RoleId = modelo.RoleId;
                    var responseUser = await _userRepository.Editar(response);
                    if (!responseUser)
                    {
                        throw new TaskCanceledException("No se pudo editar el usuario");
                    }
                    return responseUser;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el usuario a editar");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
