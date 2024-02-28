namespace DotnetHeadStart;

public class HeadStartException : Exception
{

    public HeadStartException(string message) : base(message)
    {
    }

    public HeadStartException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
