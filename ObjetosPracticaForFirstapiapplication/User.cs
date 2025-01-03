using System;
using System.Collections.Generic;

namespace Eccomerce.MODELO;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; } = null!;

    public string? PasswordHash { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
