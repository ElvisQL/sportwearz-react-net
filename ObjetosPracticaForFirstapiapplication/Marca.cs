using System;
using System.Collections.Generic;

namespace Eccomerce.MODELO;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string NombreMarca { get; set; } = null!;

    public string? DescripcionMarca { get; set; }
}
