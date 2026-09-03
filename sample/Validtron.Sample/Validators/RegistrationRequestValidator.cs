using Validtron.Extensions;

namespace Validtron.Sample.Validators;

public sealed class RegistrationRequestValidator : Validator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        _ = RuleFor(x => x.UserName)
            .NotEmpty()
            .MustAsync(
                async (userName, cancellationToken) =>
                {
                    await Task.Delay(
                        100,
                        cancellationToken);

                    return userName != "already-exists";
                },
                "Bu kullanıcı adı zaten kullanılıyor.");


        _ = RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(
                async (
                    request,
                    email,
                    cancellationToken) =>
                {
                    await Task.Delay(
                        100,
                        cancellationToken);

                    // Sadece ikinci MustAsync overload'ını
                    // test etmek için örnek.
                    return request.UserName != "blocked-user"
                           && email is not null;
                },
                "Kayıt isteği async kontrolden geçemedi.");
    }
}