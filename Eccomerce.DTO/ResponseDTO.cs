using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO
{
    public class ResponseDTO<T>
    {
        public T? Response { get; set; }
        public bool ? Success { get; set; }
        public string? Message { get; set; }

    }
}
