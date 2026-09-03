using Microsoft.Extensions.DependencyInjection;
using Validtron;
using Validtron.DependencyInjection;
using Validtron.Sample.Enums;
using Validtron.Sample.Helpers;
using Validtron.Sample.Models;
using Validtron.Sample.Validators;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("========================================");
Console.WriteLine("VALIDTRON CONSOLE TEST");
Console.WriteLine("========================================");

await RunSyncValidation();
await RunAsyncValidation();
RunNullValidation();
RunDependencyInjectionTest();


static Task RunSyncValidation()
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("1 - SYNC VALIDATION");
    Console.WriteLine("========================================");

    var customer = new Customer
    {
        Name = "",
        Surname = "A",
        Email = "invalid-email",
        Age = 16,
        Score = 120,
        DiscountRate = 150,
        UserName = "admin",
        Password = "123",
        PasswordAgain = "456",
        CustomerType = (CustomerType)99,
        IsCompany = true,
        TaxNumber = "",
        ReferralCode = "INVALID",
        Address = new Address
        {
            City = "",
            Street = "A",
            PostalCode = "ABC"
        },
        PreviousAddresses =
        [
            new Address
            {
                City = "",
                Street = "Very short",
                PostalCode = "123"
            },
            new Address
            {
                City = "Istanbul",
                Street = "",
                PostalCode = "ABCDE"
            }
        ],
        Tags =
        [
            "",
            "a",
            "valid-tag"
        ]
    };

    var validator = new CustomerValidator();

    var result = validator.Validate(customer);

    ConsolePrinter.PrintResult("CustomerValidator", result);

    return Task.CompletedTask;
}

static async Task RunAsyncValidation()
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("2 - ASYNC VALIDATION");
    Console.WriteLine("========================================");

    var model = new RegistrationRequest
    {
        UserName = "already-exists",
        Email = "test@example.com"
    };

    var validator = new RegistrationRequestValidator();

    Console.WriteLine();
    Console.WriteLine("Önce Validate() çağırıyoruz:");

    try
    {
        _ = validator.Validate(model);
    }
    catch (InvalidOperationException exception)
    {
        Console.WriteLine($"Beklenen exception:");
        Console.WriteLine(exception.Message);
    }

    Console.WriteLine();
    Console.WriteLine("Şimdi ValidateAsync() çağırıyoruz:");

    var result =
        await validator.ValidateAsync(model);

    ConsolePrinter.PrintResult("RegistrationRequestValidator", result);
}

static void RunNullValidation()
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("3 - NULL INSTANCE");
    Console.WriteLine("========================================");

    var validator = new CustomerValidator();

    var result =
        validator.Validate(null!);

    ConsolePrinter.PrintResult("Null Customer", result);
}


// ============================================================
// DEPENDENCY INJECTION
// ============================================================

static void RunDependencyInjectionTest()
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("4 - DEPENDENCY INJECTION");
    Console.WriteLine("========================================");

    var services = new ServiceCollection();

    _ = services.AddValidtron(typeof(Program).Assembly);

    using var provider =
        services.BuildServiceProvider();

    using var scope =
        provider.CreateScope();

    var customerValidator =
        scope.ServiceProvider
            .GetRequiredService<IValidator<Customer>>();

    var registrationValidator =
        scope.ServiceProvider
            .GetRequiredService<IValidator<RegistrationRequest>>();

    Console.WriteLine(
        $"Customer validator: {customerValidator.GetType().Name}");

    Console.WriteLine(
        $"Registration validator: {registrationValidator.GetType().Name}");
}




public sealed class RegistrationRequest
{
    public string? UserName { get; set; }

    public string? Email { get; set; }
}
