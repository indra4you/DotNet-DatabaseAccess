using Microsoft.Data.SqlClient;

namespace DotNet.Data.Services;

public abstract class AbstractDataService
{
    private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

    public AbstractDataService(
        string connectionString
    )
    {
        this._sqlConnectionStringBuilder =
            new SqlConnectionStringBuilder(connectionString);
    }

    protected async Task<int> ExecuteNonQuery(
        string sqlStatement,
        IEnumerable<SqlParameter> parameters
    ) =>
        await this
            .EnsureSuccess(
                async () =>
                {
                    using var connection = new SqlConnection(this._sqlConnectionStringBuilder.ConnectionString);
                    await connection.OpenAsync();

                    using var command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddRange(parameters.ToArray());

                    var noOfRows = await command.ExecuteNonQueryAsync();

                    return noOfRows;
                },
                TranslateSqlExceptionToException,
                nameof(this.ExecuteNonQuery)
            );

    protected async Task<int> ExecuteNonQuery(
        string sqlStatement
    ) =>
        await this.ExecuteNonQuery(sqlStatement, new List<SqlParameter>());

    protected async Task<IEnumerable<TReturn>> ExecuteReader<TReturn>(
        string sqlStatement,
        IEnumerable<SqlParameter> parameters,
        Func<SqlDataReader, TReturn> mapperMethod
    ) =>
        await this
            .EnsureSuccess(
                async () =>
                {
                    using var connection = new SqlConnection(this._sqlConnectionStringBuilder.ConnectionString);
                    await connection.OpenAsync();

                    using var command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddRange(parameters.ToArray());

                    var reader = await command.ExecuteReaderAsync();
                    var result = new List<TReturn>();

                    while (await reader.ReadAsync())
                    {
                        result.Add(mapperMethod(reader));
                    }

                    return result;
                },
                TranslateSqlExceptionToException,
                nameof(this.ExecuteReader)
            );

    protected async Task<IEnumerable<TReturn>> ExecuteReader<TReturn>(
        string sqlStatement,
        Func<SqlDataReader, TReturn> mapperMethod
    ) =>
        await this.ExecuteReader(sqlStatement, new List<SqlParameter>(), mapperMethod);

    private async Task<TReturn> EnsureSuccess<TReturn>(
        Func<Task<TReturn>> methodToWrap,
        Func<SqlException, string, Exception> exceptionFactory,
        string callingMethodName
    ) =>
        await this.WrapInTryCatch(methodToWrap, exceptionFactory, callingMethodName);

    private async Task<TReturn> WrapInTryCatch<TReturn>(
        Func<Task<TReturn>> methodToWrap,
        Func<SqlException, string, Exception> exceptionFactory,
        string callingMethodName
    )
    {
        try
        {
            return await methodToWrap();
        }
        catch (SqlException sqlException)
        {
            var exceptionToThrow = exceptionFactory(sqlException, callingMethodName);

            throw exceptionToThrow;
        }
        catch (Exception exception)
        {
            throw new UnexpectedException(
                $"Unhandled exception occured while trying to commuicate with the Sql Server: \"{this._sqlConnectionStringBuilder.InitialCatalog}\" with the exception details: {exception.Message}",
                exception
            );
        }
    }

    private Exception TranslateSqlExceptionToException(
        SqlException sqlException,
        string callingMethodName
    ) =>
        sqlException.ErrorCode switch
        {
            // TODO: Add more specific SQL Server Exceptions
            _ => new SqlServerInvalidOperationException(callingMethodName, sqlException),
        };
}