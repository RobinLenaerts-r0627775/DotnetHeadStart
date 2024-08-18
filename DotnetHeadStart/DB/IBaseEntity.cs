namespace DotnetHeadStart.DB;

public interface IBaseEntity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

}
