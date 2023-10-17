namespace DotNet.Data.Services;

[Serializable]
public sealed class UnexpectedException : Exception
{
    public UnexpectedException(
        string message,
        Exception innerException
    ) : base(message, innerException)
    {
    }
}