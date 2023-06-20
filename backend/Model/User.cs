using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public byte[] ProfilePicture { get; set; }

    public byte[] Password { get; set; }

    public string Salt { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();
}
