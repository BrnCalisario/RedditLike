using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Group
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int? Image { get; set; }

    public virtual ImageDatum ImageNavigation { get; set; }

    public virtual User Owner { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
