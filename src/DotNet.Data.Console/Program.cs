namespace DotNet.Data;

public static class Program
{
    public static async Task Main(
        string[] _
    )
    {
        // Set SQL Server Connection String OR read it from appSettings.json
        var sqlServerConnectionString = "Your Connection String";

        var profileDataService = new Services.ProfileDataService(sqlServerConnectionString);

        var profiles = await profileDataService
            .FetchProfilesByEmailAddress(
                "user@domain.com"
            );
        Console.WriteLine("Number of Profiles found: {0}", profiles.Count());

        await profileDataService
            .InsertProfileIfNotExistsByEmailAddress(
                "user@domain.com",
                "User",
                "Name"
            );
    }
}