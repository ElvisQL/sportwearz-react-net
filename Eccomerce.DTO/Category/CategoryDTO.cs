using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Category
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="ingrese nombre de categoria")]
        public string? Nombre { get; set; } 

        public string? Descripcion { get; set; }
    }
}
