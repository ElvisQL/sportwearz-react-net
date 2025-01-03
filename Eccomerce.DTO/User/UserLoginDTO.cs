using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.User
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage ="Correo no valido o no ingresado")]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Contraseña no valida")]
        public string? PasswordHash { get; set; }


    }
}
