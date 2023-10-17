namespace DotNet.Data.Services;

[Serializable]
public sealed class SqlServerInvalidOperationException : Exception
{
    public SqlServerInvalidOperationException(
        string message,
        Exception innerException
    ) : base(message, innerException)
    {
    }
}