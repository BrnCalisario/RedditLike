namespace Reddit.DTO;

public class UserRegister
{
    public string Username { get; set;}
    public string Password { get; set;}
    public string Email { get; set;}
    public DateTime Birthdate { get; set;}
    public string Jwt { get; set; }
}