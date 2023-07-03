public class CreatePostDTO
{
    public int Id { get; set; }
    public string Jwt { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int IndexedImg { get; set; }
    public int GroupID { get; set; }
}