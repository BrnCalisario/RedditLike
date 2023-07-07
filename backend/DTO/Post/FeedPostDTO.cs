using Reddit.Model;

public class FeedPostDTO
{
    public FeedPostDTO(Post post)
    {
        this.Id = post.Id;
        this.Title = post.Title;
        this.Content = post.Content;
        this.PostDate = post.PostDate;
        this.AuthorName = post.Author.Username;
        this.AuthorPhoto = post.Author.ProfilePicture ?? 0;
        this.IndexedImg = post.IndexedImage ?? 0;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PostDate { get; set; }
    public string AuthorName { get; set; }
    public int AuthorPhoto { get; set; }
    public string GroupName { get; set; }
    public int GroupId { get; set; }
    public int LikeCount { get; set; }
    public int VoteValue { get; set; }
    public int IndexedImg { get; set; }
    public bool IsAuthor { get; set; }
    public bool CanDelete { get; set; }
}
