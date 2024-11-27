namespace DotnetHeadStart.DB;

public class BaseIdentityUserSoftDeletable : BaseIdentityUser, ISoftDeletable
{
    public DateTime DeletedAt { get; set; }
}