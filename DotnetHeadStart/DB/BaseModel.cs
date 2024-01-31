namespace DotnetHeadStart;
/// <summary>
/// Base model for all entities. Contains the Id, CreatedAt, ModifiedAt and DeletedAt properties.
/// </summary>
public class BaseModel
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}