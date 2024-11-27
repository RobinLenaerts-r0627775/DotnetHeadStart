using System;

namespace DotnetHeadStart.DB;

public interface ISoftDeletable
{
    public DateTime DeletedAt { get; set; }
}
