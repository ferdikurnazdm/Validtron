
using Validtron.Sample.Enums;

namespace Validtron.Sample.Models;

public sealed class Customer
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }

    public int Score { get; set; }

    public int? DiscountRate { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? PasswordAgain { get; set; }

    public CustomerType CustomerType { get; set; }

    public bool IsCompany { get; set; }

    public string? TaxNumber { get; set; }

    public string? ReferralCode { get; set; }

    public Address Address { get; set; } = new();

    public List<Address> PreviousAddresses { get; set; } = [];

    public List<string> Tags { get; set; } = [];
}