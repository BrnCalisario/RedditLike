using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? GroupId { get; set; }

    public virtual Group Group { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
