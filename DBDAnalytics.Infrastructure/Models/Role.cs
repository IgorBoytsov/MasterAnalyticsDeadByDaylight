using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
