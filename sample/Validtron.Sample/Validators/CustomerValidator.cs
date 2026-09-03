using Validtron.Configurations;
using Validtron.Extensions;
using Validtron.Sample.Enums;
using Validtron.Sample.Models;

namespace Validtron.Sample.Validators;

public sealed class CustomerValidator : Validator<Customer>
{
    public CustomerValidator()
    {

        _ = RuleFor(x => x.Name)
            .NotNull(
                "Name null olamaz.");

        _ = RuleFor(x => x.Name)
            .Cascade(CascadeMode.Continue)
            .NotEmpty(
                "İsim boş bırakılamaz.")
            .MinimumLength(
                3,
                "İsim en az 3 karakter olmalıdır.")
            .MaximumLength(
                30,
                "İsim en fazla 30 karakter olabilir.");

        _ = RuleFor(x => x.Surname)
            .Length(
                2,
                50,
                "Soyisim 2-50 karakter arasında olmalıdır.");

        _ = RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        _ = RuleFor(x => x.Age)
            .GreaterThanOrEqual(
                18,
                "Yaş en az 18 olmalıdır.")
            .LessThan(
                100,
                "Yaş 100'den küçük olmalıdır.");

        _ = RuleFor(x => x.Score)
            .InclusiveBetween(
                0,
                100,
                "Score 0-100 arasında olmalıdır.");

        _ = RuleFor(x => x.DiscountRate)
            .InclusiveBetween(
                0,
                100,
                "DiscountRate 0-100 arasında olmalıdır.");

        _ = RuleFor(x => x.UserName)
            .NotEmpty()
            .NotEqual(
                "admin",
                "'admin' kullanıcı adı kullanılamaz.");

        _ = RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(
                6,
                "Şifre en az 6 karakter olmalıdır.");


        _ = RuleFor(x => x.PasswordAgain)
            .Must(
                (customer, passwordAgain) =>
                    passwordAgain == customer.Password,
                "Şifreler eşleşmiyor.");

        _ = RuleFor(x => x.CustomerType)
            .IsInEnum(
                "Geçerli bir CustomerType giriniz.");

        _ = RuleFor(x => x.TaxNumber)
            .NotEmpty(
                "Şirket müşterilerinde vergi numarası zorunludur.")
            .Length(
                10,
                10,
                "Vergi numarası 10 karakter olmalıdır.")
            .When(
                x => x.IsCompany);

        _ = RuleFor(x => x.ReferralCode)
            .Must(
                referralCode =>
                    referralCode is null ||
                    referralCode.StartsWith(
                        "REF-",
                        StringComparison.OrdinalIgnoreCase),
                "ReferralCode REF- ile başlamalıdır.")
            .Unless(
                x => x.IsCompany);

        _ = RuleFor(x => x.UserName)
            .Must(
                userName =>
                    userName is null ||
                    !userName.Contains(' '),
                "Kullanıcı adı boşluk içeremez.");

        _ = RuleFor(x => x.Age)
            .Must(
                (customer, age) =>
                    customer.CustomerType != CustomerType.Premium ||
                    age >= 21,
                "Premium müşteri en az 21 yaşında olmalıdır.");

        _ = RuleFor(x => x.Address)
            .SetValidator(
                new AddressValidator());

        _ = RuleForEach(x => x.PreviousAddresses)
            .SetValidator(
                new AddressValidator());

        _ = RuleForEach(x => x.Tags)
            .Cascade(CascadeMode.Continue)
            .NotEmpty(
                "Tag boş olamaz.")
            .MinimumLength(
                2,
                "Tag en az 2 karakter olmalıdır.");
    }
}
