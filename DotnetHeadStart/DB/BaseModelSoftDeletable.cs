namespace DotnetHeadStart.DB;

public class BaseModelSoftDeletable : BaseModel, ISoftDeletable
{
    public DateTime DeletedAt { get; set; } = DateTime.MinValue;
}