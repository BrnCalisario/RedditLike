using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Post
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int GroupId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public int? IndexedImage { get; set; }

    public int LikeCount { get; set; }

    public DateTime PostDate { get; set; }

    public virtual User Author { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Group Group { get; set; }

    public virtual ImageDatum IndexedImageNavigation { get; set; }

    public virtual ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();
}
