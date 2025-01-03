using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.DTO.Tarjeta
{
    public class TarjetaDTO
    {
        [Required(ErrorMessage ="Ingrese titular")]
        public string? Titular {  get; set; }
        [Required(ErrorMessage = "Ingrese Numero de tarjeta")]
        public string? Numero { get; set; }
        [Required(ErrorMessage = "Ingrese vigencia")]
        public string? Vigencia { get; set; }

        [Required(ErrorMessage = "Ingrese cvv")]
        public string? CVV {  get; set; }

    }
}
