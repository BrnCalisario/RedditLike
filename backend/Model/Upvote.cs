using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Upvote
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public bool? Value { get; set; }

    public virtual Post Post { get; set; }

    public virtual User User { get; set; }
}
