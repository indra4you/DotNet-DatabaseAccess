namespace DotNet.Data.Services.Models;

public sealed class UpsertProfileModel
{
    public UpsertProfileModel(
        ulong? mobileNumber,
        string? emailAddress,
        string? firstName,
        string? lastName
    )
    {
        this.MobileNumber = mobileNumber;
        this.EmailAddress = emailAddress;
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public ulong? MobileNumber { get; private set; }

    public string? EmailAddress { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }
}