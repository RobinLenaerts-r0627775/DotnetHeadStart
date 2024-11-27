namespace DotnetHeadStart.DB;
/// <summary>
/// Base model for all entities. Contains the Id, CreatedAt, ModifiedAt, IsDeleted and DeletedAt properties.
/// </summary>
public class BaseModel : IBaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public DateTime ModifiedAt { get; set; } = DateTime.MinValue;
    public bool IsDeleted { get; set; } = false;
}