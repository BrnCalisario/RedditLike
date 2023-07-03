public class RoleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> PermissionsSet { get; set; }
    public int GroupId { get; set; }
    public string Jwt { get; set; }
}