
namespace DotnetHeadStart;

/// <summary>
/// Base Identity User for all Identity users. Contains the CreatedAt, ModifiedAt, IsDeleted and DeletedAt properties. Inherits from IdentityUser.
/// </summary>
public class BaseIdentityUser : IdentityUser, IBaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public DateTime ModifiedAt { get; set; } = DateTime.MinValue;
    public DateTime DeletedAt { get; set; } = DateTime.MinValue;
    public bool IsDeleted { get; set; } = false;
}
