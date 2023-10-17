using System.Data;

namespace DotNet.Data.Services;

public sealed class ProfileDataService : AbstractDataService
{
    private const string PROFILES_TABLE = "[dbo].[Profiles]";

    public ProfileDataService(
    string connectionString
    ) : base(connectionString)
    {
    }

    public async Task<IEnumerable<Models.ProfileDataModel>> FetchProfilesByMobileNumber(
        ulong mobileNumber
    )
    {
        var parameters = this.CreateParameters()
            .AddParameter("@MobileNumber", mobileNumber, SqlDbType.BigInt);

        var profileDataModels = await base.ExecuteReader(
            @$"
                SELECT
                    [IP].[ProfileID],
                    [IP].[MobileNumber],
                    [IP].[EmailAddress],
                    [IP].[FirstName],
                    [IP].[LastName]
                FROM
                    {PROFILES_TABLE} [IP] WITH (NOLOCK)
                WHERE
                    [IP].[CountryPhoneCode] = @CountryPhoneCode
                AND [IP].[MobileNumber] = @MobileNumber
            ",
            parameters,
            ProfileDataMapperExtensions.ToProfileDataModel
        );

        return profileDataModels;
    }

    public async Task<IEnumerable<Models.ProfileDataModel>> FetchProfilesByEmailAddress(
        string emailAddress
    )
    {
        var parameters = this.CreateParameters()
            .AddParameter("@EmailAddress", emailAddress);

        var profileDataModels = await base.ExecuteReader(
            @$"
                SELECT
                    [IP].[ProfileID],
                    [IP].[MobileNumber],
                    [IP].[EmailAddress],
                    [IP].[FirstName],
                    [IP].[LastName]
                FROM
                    {PROFILES_TABLE} AS [IP] WITH (NOLOCK)
                WHERE
                    [IP].[EmailAddress] = @EmailAddress
            ",
            parameters,
            ProfileDataMapperExtensions.ToProfileDataModel
        );

        return profileDataModels;
    }

    public async Task<bool> IsProfileExistsByMobileNumber(
        ulong mobileNumber
    )
    {
        var profileDataModels = await this.FetchProfilesByMobileNumber(
            mobileNumber
        );

        return profileDataModels.Any();
    }

    public async Task<bool> IsProfileExistsByEmailAddress(
        string emailAddress
    )
    {
        var profileDataModels = await this.FetchProfilesByEmailAddress(
            emailAddress
        );

        return profileDataModels.Any();
    }

    async Task<Models.ProfileDataModel?> FetchProfileByProfileID(
        ulong profileID
    )
    {
        var parameters = this.CreateParameters()
            .AddParameter("@ProfileID", profileID, SqlDbType.BigInt);

        var profiles = await base.ExecuteReader(
            @$"
                SELECT
                    [IP].[ProfileID],
                    [IP].[MobileNumber],
                    [IP].[EmailAddress],
                    [IP].[FirstName],
                    [IP].[LastName]
                FROM
                    {PROFILES_TABLE} [IP] WITH (NOLOCK)
                WHERE
                    [IP].[ProfileID] = @ProfileID",
            parameters,
            ProfileDataMapperExtensions.ToProfileDataModel
        );

        var profile = profiles.SingleOrDefault();

        return profile;
    }

    public async Task InsertProfile(
        Models.UpsertProfileModel insertProfileModel
    )
    {
        var parameters = this.CreateParameters()
            .AddParameter("@MobileNumber", insertProfileModel.MobileNumber, SqlDbType.BigInt)
            .AddParameter("@EmailAddress", insertProfileModel.EmailAddress)
            .AddParameter("@FirstName", insertProfileModel.FirstName)
            .AddParameter("@LastName", insertProfileModel.LastName);

        await base.ExecuteNonQuery(
            @$"
                INSERT INTO {PROFILES_TABLE}
                (
                    [MobileNumber],
                    [EmailAddress],
                    [FirstName],
                    [LastName]
                )
                VALUES
                (
                    @MobileNumber,
                    @EmailAddress,
                    @FirstName,
                    @LastName
                )",
            parameters
        );
    }

    public async Task InsertProfileIfNotExistsByMobileNumber(
        ulong mobileNumber,
        string? firstName,
        string? lastName
    )
    {
        var profileExists = await this.IsProfileExistsByMobileNumber(
            mobileNumber
        );
        if (!profileExists)
            await this.InsertProfile(
                new(
                    mobileNumber,
                    null,
                    firstName,
                    lastName
                )
            );
    }

    public async Task InsertProfileIfNotExistsByEmailAddress(
        string emailAddress,
        string? firstName,
        string? lastName
    )
    {
        var profileExists = await this.IsProfileExistsByEmailAddress(
            emailAddress
        );
        if (!profileExists)
            await this.InsertProfile(
                new(
                    null,
                    emailAddress,
                    firstName,
                    lastName
                )
            );
    }

    public async Task UpdateProfileByProfileID(
        ulong profileID,
        Models.UpsertProfileModel upsertProfileModel
    )
    {
        var parameters = this.CreateParameters()
            .AddParameter("@ProfileID", profileID, SqlDbType.BigInt)
            .AddParameter("@MobileNumber", upsertProfileModel.MobileNumber, SqlDbType.BigInt)
            .AddParameter("@EmailAddress", upsertProfileModel.EmailAddress)
            .AddParameter("@FirstName", upsertProfileModel.FirstName)
            .AddParameter("@LastName", upsertProfileModel.LastName);

        await base.ExecuteNonQuery(
            @$"
                UPDATE {PROFILES_TABLE}
                SET
                    [CountryPhoneCode] = @CountryPhoneCode,
                    [MobileNumber] = @MobileNumber,
                    [EmailAddress] = @EmailAddress,
                    [FirstName] = @FirstName,
                    [MiddleName] = @MiddleName,
                    [LastName] = @LastName,
                    [Type] = @Type
                WHERE
                    [ProfileID] = @ProfileID",
            parameters
        );
    }
}