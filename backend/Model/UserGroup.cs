using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class UserGroup
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GroupId { get; set; }
}
