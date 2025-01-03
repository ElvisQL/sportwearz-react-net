using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.DTO.Role;
namespace Eccomerce.DTO.User
{
    public class UserSessionDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleDTO? Role { get; set; }

        public string? Token { get; set; }

    }
}
