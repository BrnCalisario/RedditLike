using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class ImageDatum
{
    public int Id { get; set; }

    public byte[] Photo { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
