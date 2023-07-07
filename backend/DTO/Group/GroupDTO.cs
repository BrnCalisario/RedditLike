using Reddit.Model;

namespace Reddit.DTO;

public class GroupDTO
{
    public int Id { get; set;} = 0;
    public string Name { get; set; }
    public string Description { get; set; }
    public int OwnerID { get; set; }
    public int? ImageId { get; set; } 
    public bool isMember { get; set; }
    public int? UserQuantity { get; set; }
    public List<PermissionEnum> UserPermissions { get; set; }
    public string UserRole { get; set; }
    public string Jwt { get; set; } 
    public List<PostDTO> Posts { get; set; }
    
}
