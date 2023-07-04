public class FeedPostDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PostDate { get; set; }
    public string AuthorName { get; set; }
    public string GroupName { get; set; }
    public int GroupId { get; set; }
    public int LikeCount { get; set;}
    public int IndexedImg { get; set; }
}