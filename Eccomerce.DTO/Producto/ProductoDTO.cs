using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.DTO.Brand;
using Eccomerce.DTO.Category;
namespace Eccomerce.DTO.Producto
{
    public class ProductoDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ingrese nombre")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Ingrese descripcion")]

        public string? Description { get; set; }
        [Required(ErrorMessage = "Ingrese precio")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ingrese stock")]
      
        public int Stock { get; set; }

        public string? ImageURL { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        
        [Required(ErrorMessage = "Seleccione una marca")]
        public int BrandId { get; set; }

        public List<int>? CategoriesIds { get; set; } = new List<int>();
        public List<CategoryDTO>? Categories { get; set; } = new List<CategoryDTO>();


        public BrandDTO? Brand { get; set; }


    }
}
