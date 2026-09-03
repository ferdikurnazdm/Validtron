using Validtron.Extensions;
using Validtron.Sample.Models;

namespace Validtron.Sample.Validators;


public sealed class AddressValidator : Validator<Address>
{
    public AddressValidator()
    {
        _ = RuleFor(x => x.City)
            .NotEmpty(
                "Şehir boş bırakılamaz.");

        _ = RuleFor(x => x.Street)
            .NotEmpty(
                "Sokak boş bırakılamaz.")
            .MinimumLength(
                5,
                "Sokak en az 5 karakter olmalıdır.");

        _ = RuleFor(x => x.PostalCode)
            .NotEmpty()
            .Matches(
                @"^\d{5}$",
                "Posta kodu 5 rakamdan oluşmalıdır.");
    }
}
