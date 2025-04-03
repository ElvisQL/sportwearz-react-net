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
using Eccomerce.DTO.Cart;
using Azure;
using Eccomerce.Utilidades;


namespace Eccomerce.Servicio.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly EccomerceDbContext _dbContext;

        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;



        public UserService(IGenericRepository<User> userrepository, IMapper mapper,IPasswordHasher passwordHasher,EccomerceDbContext dbContext,IGenericRepository<Cart> cartRepository) { 
            _mapper = mapper;
            _userRepository = userrepository;
            _cartRepository = cartRepository;
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

       
        public async Task<UserCreateDTO> Create(UserCreateDTO modelo)
        {
            try
            {
                if (modelo.PasswordHash != modelo.ConfirmPasswordHash)
                {
                    throw new Exception("Las contraseñas no coinciden");
                }
                var user = _mapper.Map<User>(modelo);
                user.PasswordHash = _passwordHasher.Hash(modelo.PasswordHash);
                // El carrito se crea automáticamente por la inicialización en el modelo
                user.Cart.CreatedAt = DateTime.UtcNow;
                user.Cart.UpdatedAt = DateTime.UtcNow;

                var response = await _userRepository.Crear(user);
                if (response.UserId != 0)
                {

                    
                    var userDto = _mapper.Map<UserCreateDTO>(response);
                    return userDto;
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
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Cargar usuario con todas sus dependencias
                var user = await _dbContext.Users
                    .Include(u => u.Cart)
                        .ThenInclude(c => c.CartItems)
                    .Include(u => u.Venta)
                        .ThenInclude(v => v.DetalleVenta)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    throw new KeyNotFoundException("Usuario no encontrado");
                }

                // Eliminar en orden correcto
                if (user.Cart != null)
                {
                    _dbContext.CartItems.RemoveRange(user.Cart.CartItems);
                    _dbContext.Carts.Remove(user.Cart);
                }

                if (user.Venta != null)
                {
                    foreach (var venta in user.Venta)
                    {
                        _dbContext.DetalleVenta.RemoveRange(venta.DetalleVenta);
                    }
                    _dbContext.Venta.RemoveRange(user.Venta);
                }

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error eliminando usuario", ex);
            }
        }


        public async Task<List<UserSessionDTO>> ListarUsuarios()
        {
            try
            {
                var consultaLINQ = _userRepository
                    .Consultar()
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
                var consultaLINQ = _userRepository.Consultar(u => u.Email == modelo.Email).Include(u => u.Role).Include(u=> u.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(cii => cii.Product);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    if (!_passwordHasher.Check(response.PasswordHash, modelo.PasswordHash))
                    {
                        throw new UnauthorizedAccessException("Credenciales inválidas");
                    }
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

       

        public async Task<bool> Update(int userId,UserCreateDTO modelo)
        {
            try
            {

                var response = await _userRepository.Consultar(p => p.UserId == userId).FirstOrDefaultAsync();
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
                throw ;
            }
        }

        public async Task<bool> UpdateUserByAdmin(int userId,UserUpdateDTO modelo)
        {

            try
            {

                var response = await _userRepository.Consultar(p => p.UserId == userId).FirstOrDefaultAsync();
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
                throw ;
            }
        }


        
    }
}
