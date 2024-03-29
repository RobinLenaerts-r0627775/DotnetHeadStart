namespace DotnetHeadStart;
/// <summary>
/// Base model for all entities. Contains the Id, CreatedAt, ModifiedAt, IsDeleted and DeletedAt properties.
/// </summary>
public class BaseModel
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public DateTime ModifiedAt { get; set; } = DateTime.MinValue;
    public DateTime DeletedAt { get; set; } = DateTime.MinValue;
    public bool IsDeleted { get; set; } = false;
}