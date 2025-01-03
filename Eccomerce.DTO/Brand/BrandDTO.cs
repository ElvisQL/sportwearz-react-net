using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Brand
{
    public class BrandDTO
    {

        public int BrandId { get; set; }
        [Required(ErrorMessage = "ingrese nombre de marca")]
        public string BrandName { get; set; }

        public string? Description { get; set; }
    }
}
