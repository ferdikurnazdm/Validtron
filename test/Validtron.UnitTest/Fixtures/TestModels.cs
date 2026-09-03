namespace Validtron.UnitTest.Fixtures;

internal sealed class Person
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }

    public int? Score { get; set; }

    public bool IsCompany { get; set; }

    public string? TaxNumber { get; set; }

    public string? ReferralCode { get; set; }

    public Address? Address { get; set; }

    public List<Address>? Addresses { get; set; }

    public List<string>? Tags { get; set; }

    public UserRole Role { get; set; }
}

internal sealed class Address
{
    public string? City { get; set; }
}

internal sealed class RegistrationRequest
{
    public string? UserName { get; set; }
}

internal enum UserRole
{
    User = 1,

    Admin = 2
}
