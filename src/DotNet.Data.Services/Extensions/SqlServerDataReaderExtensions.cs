using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNet.Data.Services;

internal static class SqlServerDataReaderExtensions
{
    internal static int GetInt32(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetInt32(dataReader.GetOrdinal(columnName));

    internal static long GetInt64(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetInt64(dataReader.GetOrdinal(columnName));

    internal static ushort GetUnsignedInt16(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        Convert.ToUInt16(dataReader.GetInt16(dataReader.GetOrdinal(columnName)));

    internal static uint GetUnsignedInt32(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        Convert.ToUInt32(dataReader.GetInt32(dataReader.GetOrdinal(columnName)));

    internal static ulong GetUnsignedInt64(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        Convert.ToUInt64(dataReader.GetInt64(dataReader.GetOrdinal(columnName)));

    internal static ushort? GetNullableUnsignedInt16(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return Convert.ToUInt16(dataReader.GetValue(columnOrdinal));
    }

    internal static uint? GetNullableUnsignedInt32(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return Convert.ToUInt32(dataReader.GetValue(columnOrdinal));
    }

    internal static ulong? GetNullableUnsignedInt64(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return Convert.ToUInt64(dataReader.GetValue(columnOrdinal));
    }

    internal static decimal GetDecimal(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetDecimal(dataReader.GetOrdinal(columnName));

    internal static decimal? GetNullableDecimal(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return Convert.ToDecimal(dataReader.GetValue(columnOrdinal));
    }

    internal static bool GetBoolean(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetBoolean(dataReader.GetOrdinal(columnName));

    internal static bool? GetNullableBoolean(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return Convert.ToBoolean(dataReader.GetValue(columnOrdinal));
    }

    internal static string GetString(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetString(dataReader.GetOrdinal(columnName));

    internal static string? GetNullableString(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return dataReader.GetString(columnOrdinal);
    }

    internal static DateTimeOffset GetDateTimeOffset(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetDateTimeOffset(dataReader.GetOrdinal(columnName));

    internal static DateTimeOffset? GetNullableDateTimeOffset(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return dataReader.GetDateTimeOffset(columnOrdinal);
    }

    internal static Guid GetGuid(
        this SqlDataReader dataReader,
        string columnName
    ) =>
        dataReader.GetGuid(dataReader.GetOrdinal(columnName));

    internal static Guid? GetNullableGuid(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return null;

        return dataReader.GetGuid(columnOrdinal);
    }

    internal static byte[] GetBytes(
        this SqlDataReader dataReader,
        string columnName
    )
    {
        var columnOrdinal = dataReader.GetOrdinal(columnName);
        if (columnOrdinal == -1 || dataReader.IsDBNull(columnOrdinal))
            return Array.Empty<byte>();

        return dataReader.GetFieldValue<byte[]>(columnOrdinal);
    }

    internal static IList<SqlParameter> CreateParameters(
        this AbstractDataService _
    ) =>
        new List<SqlParameter>();

    internal static IList<SqlParameter> AddParameter(
        this IList<SqlParameter> sqlParameters,
        string parameterName,
        object? parameterValue
    )
    {
        sqlParameters.Add(
            new SqlParameter(
                parameterName,
                parameterValue ?? DBNull.Value
            )
        );

        return sqlParameters;
    }

    internal static IList<SqlParameter> AddParameter(
        this IList<SqlParameter> sqlParameters,
        string parameterName,
        object? parameterValue,
        SqlDbType dataType
    )
    {
        sqlParameters.Add(
            new SqlParameter(
                parameterName,
                dataType
            )
            {
                Value = parameterValue ?? DBNull.Value
            });

        return sqlParameters;
    }
}