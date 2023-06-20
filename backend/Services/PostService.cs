namespace Reddit.Services;

using Model;

public class PostNode
{
    public Post Post { get; set; }
    public List<PostNode> Childs { get; set; }
}

public class PostService
{
    public PostNode GetPostTree(IEnumerable<Post> table, int id)
    {
        PostNode top = new PostNode();
        top.Post = table.First(p => p.Id == id);

        if (top.Post is null)
            throw new ArgumentException("Invalid ID");

        var childs = table.Where(p => p.ParentPost == top.Post.Id);

        foreach (var child in childs)
        {
            PostNode node = GetPostTree(table, child.Id);
            top.Childs.Add(node);
        }
        return top;
    }

    // TODO: NEW IMPLEMENTANTION
    //Get Only First layer of childs 

}