using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Utilidades
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public bool Check(string hash, string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password,13);
        }
    }
}
