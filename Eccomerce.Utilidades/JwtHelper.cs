using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Utilidades
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

      
        public JwtHelper(IConfiguration configuration) { 
            _configuration = configuration;
        }


        public string GenerateToken(string userId,string userRole,string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole),
                new Claim(JwtRegisteredClaimNames.Sub,userId)
            };

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);


        }
    }
}
