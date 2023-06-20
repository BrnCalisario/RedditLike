using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public int? ProfilePicture { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ImageDatum ProfilePictureNavigation { get; set; }

    public virtual ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();
}
