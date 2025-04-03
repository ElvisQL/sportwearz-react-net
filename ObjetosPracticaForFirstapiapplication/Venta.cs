using System;
using System.Collections.Generic;

namespace Eccomerce.MODELO;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? IdUsuario { get; set; }

    public decimal? Total { get; set; }
    public string Estado { get; set; } // <-- Campo agregado manualmente

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual User? IdUsuarioNavigation { get; set; }
}
