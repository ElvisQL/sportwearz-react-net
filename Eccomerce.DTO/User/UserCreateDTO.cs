using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.User
{
    public class UserCreateDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Ingrese nombre de usuario")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Ingrese Email ")]
        public string? Email { get; set; } 

        [Required(ErrorMessage ="Ingrese contraseña")]
        public string? PasswordHash { get; set; } 

        [Required(ErrorMessage = "Ingrese confirmacion")]
        public string? ConfirmPasswordHash { get; set; } 

       

        [Required(ErrorMessage = "Ingrese primer nombre")] 
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Ingrese Apellido")] 
        public string? LastName { get; set; } 

        [Required(ErrorMessage = "Ingrese numero de telefono")]
        public string? PhoneNumber { get; set; }

        


    }
}
