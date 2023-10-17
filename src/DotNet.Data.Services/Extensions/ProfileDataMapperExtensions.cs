using DotNet.Data.Services.Models;
using Microsoft.Data.SqlClient;

namespace DotNet.Data.Services;

internal static class ProfileDataMapperExtensions
{
    internal static ProfileDataModel ToProfileDataModel(
        SqlDataReader dataReader
    ) =>
        new(
            dataReader.GetUnsignedInt64("ProfileID"),
            dataReader.GetUnsignedInt64("MobileNumber"),
            dataReader.GetString("EmailAddress"),
            dataReader.GetNullableString("FirstName"),
            dataReader.GetNullableString("LastName")
        );
}