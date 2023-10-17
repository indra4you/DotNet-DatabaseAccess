namespace DotNet.Data.Services;

internal static class SqlServerExtensions
{
    internal static string ToInClause(
        this IEnumerable<string> values
    ) =>
        string.Join(", ", values.Select(s => $"'{s}'"));

    internal static string ToInClause(
        this IEnumerable<Guid> values
    ) =>
        string.Join(", ", values.Select(s => $"'{s}'"));
}