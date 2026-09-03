using Validtron.Configurations;
using Validtron.Extensions;

namespace Validtron.UnitTest.Fixtures;

internal sealed class NameValidator : Validator<Person>
{
    public NameValidator() => RuleFor(x => x.Name).NotEmpty();
}

internal sealed class ContinueNameValidator : Validator<Person>
{
    public ContinueNameValidator()
    {
        _ = RuleFor(x => x.Name)
            .Cascade(CascadeMode.Continue)
            .NotEmpty("required")
            .MinimumLength(3, "min");
    }
}

internal sealed class StopNameValidator : Validator<Person>
{
    public StopNameValidator()
    {
        _ = RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty("required")
            .MinimumLength(3, "min");
    }
}

internal sealed class ConditionalValidator : Validator<Person>
{
    public ConditionalValidator()
    {
        _ = RuleFor(x => x.TaxNumber)
            .NotEmpty("tax")
            .When(x => x.IsCompany);

        _ = RuleFor(x => x.ReferralCode)
            .NotEmpty("referral")
            .Unless(x => x.IsCompany);
    }
}

internal sealed class MultipleConditionValidator : Validator<Person>
{
    public MultipleConditionValidator()
    {
        _ = RuleFor(x => x.TaxNumber)
            .NotEmpty("tax")
            .When(x => x.IsCompany)
            .When(x => x.Age >= 18);
    }
}

internal sealed class AddressValidator : Validator<Address>
{
    public AddressValidator() => _ = RuleFor(x => x.City).NotEmpty("city");
}

internal sealed class NestedValidator : Validator<Person>
{
    public NestedValidator() => _ = RuleFor(x => x.Address!).SetValidator(new AddressValidator());
}

internal sealed class RequiredNestedValidator : Validator<Person>
{
    public RequiredNestedValidator()
    {
        _ = RuleFor(x => x.Address).NotNull("address");

        _ = RuleFor(x => x.Address!).SetValidator(new AddressValidator());
    }
}

internal sealed class CollectionValidator : Validator<Person>
{
    public CollectionValidator()
    {
        _ = RuleForEach(x => x.Addresses!).SetValidator(new AddressValidator());

        _ = RuleForEach(x => x.Tags!).NotEmpty("tag");
    }
}

internal sealed class AsyncUserNameValidator : Validator<RegistrationRequest>
{
    public AsyncUserNameValidator()
    {
        _ = RuleFor(x => x.UserName)
            .MustAsync(
                async (value, cancellationToken) =>
                {
                    await Task.Delay(10, cancellationToken);
                    return value != "taken";
                },
                "taken");
    }
}

internal sealed class AsyncChildAddressValidator : Validator<Address>
{
    public AsyncChildAddressValidator()
    {
        _ = RuleFor(x => x.City)
            .MustAsync(
                async (value, cancellationToken) =>
                {
                    await Task.Delay(10, cancellationToken);
                    return value != "blocked";
                },
                "blocked");
    }
}

internal sealed class AsyncChildParentValidator : Validator<Person>
{
    public AsyncChildParentValidator() => _ = RuleFor(x => x.Address!).SetValidator(new AsyncChildAddressValidator());
}

internal sealed class DuplicateFailureValidator : Validator<Person>
{
    public DuplicateFailureValidator()
    {
        _ = RuleFor(x => x.Name).Must(_ => false, "same");

        _ = RuleFor(x => x.Name).Must(_ => false, "same");
    }
}

internal sealed class ComparisonValidator : Validator<Person>
{
    public ComparisonValidator()
    {
        _ = RuleFor(x => x.Age)
            .GreaterThan(10, "gt")
            .GreaterThanOrEqual(11, "gte")
            .LessThan(20, "lt")
            .LessThanOrEqual(19, "lte")
            .InclusiveBetween(11, 19, "between");

        _ = RuleFor(x => x.Score)
            .InclusiveBetween(0, 100, "score");
    }
}

internal sealed class StringRulesValidator : Validator<Person>
{
    public StringRulesValidator()
    {
        _ = RuleFor(x => x.Name)
            .MinimumLength(2, "min")
            .MaximumLength(5, "max")
            .Length(2, 5, "length")
            .Matches(@"^[A-Z]+$", "matches");

        _ = RuleFor(x => x.Email)
            .EmailAddress("email");
    }
}

internal sealed class EnumValidator : Validator<Person>
{
    public EnumValidator() => _ = RuleFor(x => x.Role).IsInEnum("enum");
}

internal sealed class EqualityValidator : Validator<Person>
{
    public EqualityValidator()
    {
        _ = RuleFor(x => x.Name)
            .Equal("Ferdi", "equal")
            .NotEqual("Admin", "not-equal");
    }
}
