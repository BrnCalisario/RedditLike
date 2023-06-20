namespace Reddit.Models;

public class Post
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int ParentPostID { get; set; } 
}

public class PostNode
{
    public Post Post { get; set; }
    public List<PostNode> Childs { get; set; }

    public static PostNode GetPostTree(IEnumerable<Post> table, int id)
    {
        PostNode top = new PostNode();
        top.Post = table.First(p => p.ID == id);

        if(top.Post is null)
            throw new ArgumentException("Invalid ID");
        
        var childs = table.Where(p => p.ParentPostID == top.Post.ID);

        foreach(var child in childs)
        {
            PostNode node = PostNode.GetPostTree(table, child.ID);
            top.Childs.Add(node);
        }
        return top;
    }
}