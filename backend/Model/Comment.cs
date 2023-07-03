using System;
using System.Collections.Generic;

namespace Reddit.Model;

public partial class Comment
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int PostId { get; set; }

    public string Content { get; set; }

    public DateTime PostDate { get; set; }

    public virtual User Author { get; set; }

    public virtual Post Post { get; set; }
}
