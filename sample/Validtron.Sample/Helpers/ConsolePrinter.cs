using Validtron.Results;

namespace Validtron.Sample.Helpers;

public static class ConsolePrinter
{
    public static void PrintResult(string title, ValidationResult result)
    {
        Console.WriteLine();

        Console.WriteLine($"--- {title} ---");

        Console.WriteLine(
            $"IsValid: {result.IsValid}");

        Console.WriteLine(
            $"Error Count: {result.Errors.Count}");

        Console.WriteLine();

        Console.WriteLine("Flat Errors:");

        foreach (var error in result.Errors)
        {
            Console.WriteLine(
                $"  {error.PropertyName} -> {error.ErrorMessage}");
        }

        Console.WriteLine();

        Console.WriteLine("ErrorsByProperty:");

        foreach (var pair in result.ErrorsByProperty)
        {
            Console.WriteLine(
                $"  {pair.Key}");

            foreach (var message in pair.Value)
            {
                Console.WriteLine(
                    $"    - {message}");
            }
        }
    }
}
