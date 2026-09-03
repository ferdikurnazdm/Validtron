using System.Globalization;

namespace Validtron.Configurations;

public static class ValidationMessages
{
    public static Func<string, CultureInfo, object[], string> Resolver { get; set; } = DefaultResolver;

    public static string Get(string key, params object[] args) =>
        Resolver(key, CultureInfo.CurrentUICulture, args);

    private static string DefaultResolver(string key, CultureInfo culture, object[] args)
    {
        var template = key switch
        {
            "NullInstance" => "The object to validate cannot be null.",
            "NotNull" => "This field cannot be null.",
            "NotEmpty" => "This field must not be empty.",
            "MinimumLength" => "Must be at least {0} characters long.",
            "MaximumLength" => "Must be at most {0} characters long.",
            "Length" => "Must be between {0} and {1} characters long.",
            "Matches" => "The value is not in a valid format.",
            "EmailAddress" => "Please enter a valid email address.",
            "Equal" => "The value must be equal to '{0}'.",
            "NotEqual" => "The value must not be equal to '{0}'.",
            "GreaterThan" => "The value must be greater than {0}.",
            "GreaterThanOrEqual" => "The value must be greater than or equal to {0}.",
            "LessThan" => "The value must be less than {0}.",
            "LessThanOrEqual" => "The value must be less than or equal to {0}.",
            "InclusiveBetween" => "The value must be between {0} and {1}.",
            "IsInEnum" => "The value must be a valid enum value.",
            _ => key
        };

        return args.Length == 0
            ? template
            : string.Format(culture, template, args);
    }
}