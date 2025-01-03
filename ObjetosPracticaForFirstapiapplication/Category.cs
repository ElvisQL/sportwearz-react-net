using System;
using System.Collections.Generic;

namespace Eccomerce.MODELO;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
