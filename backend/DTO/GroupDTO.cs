namespace Reddit.DTO;

public class GroupDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int OwnerID { get; set; }
    public int? ImageId { get; set; } 
    public bool UserParticipates { get; set; }
    public int UserQuantity { get; set; }
}
