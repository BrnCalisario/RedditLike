using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
