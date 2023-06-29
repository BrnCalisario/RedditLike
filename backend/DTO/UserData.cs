namespace Reddit.DTO;

using Reddit.Model;

public class UserData {
    public string Username { get; set; }
    public string Email { get; set; }
    public int? ProfilePicture { get; set; }
    public List<Group> Groups { get; set; }
    public ICollection<Post> Posts { get; set; }

}