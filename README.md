<div align="center">

<img src="logo.png" alt="Validtron" width="140" />

# Validtron

### Modern, fluent and strongly-typed validation for .NET

Build expressive validation rules with a clean fluent API — with support for async validation, conditional rules, nested objects, collections, cascade modes and dependency injection.

[![NuGet](https://img.shields.io/nuget/v/Validtron.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/Validtron)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Validtron.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/Validtron)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/github/license/ferdikurnazdm/Validtron?style=flat-square)](LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/ferdikurnazdm/Validtron?style=flat-square&logo=github)](https://github.com/ferdikurnazdm/Validtron/stargazers)

[Documentation](https://ferdikurnazdm.github.io/Validtron/) ·
[Quick Start](https://ferdikurnazdm.github.io/Validtron/guide/quick-start.html) ·
[Examples](sample/Validtron.Sample) ·
[Report an Issue](https://github.com/ferdikurnazdm/Validtron/issues)

</div>

---

## What is Validtron?

**Validtron** is a lightweight, fluent and extensible validation library for modern .NET applications.

It provides a strongly-typed API for keeping validation logic expressive, reusable and separate from your domain models.

```csharp
public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18);
    }
}
```

No validation attributes scattered across your models.  
No manual chains of `if` statements.  
Just readable validation rules defined where they belong.

---

## Features

- **Strongly typed** — Define rules using compile-time-safe expressions.
- **Fluent API** — Build readable and expressive validation rules.
- **Sync & Async** — Use both synchronous and asynchronous validation.
- **Conditional Rules** — Execute rules only when specific conditions are met.
- **Cascade Modes** — Control whether validation continues after a failure.
- **Nested Validators** — Compose validators for complex object graphs.
- **Collection Validation** — Validate items inside collections.
- **Custom Rules** — Extend validation with your own business logic.
- **Custom Messages** — Configure validation messages to fit your application.
- **Dependency Injection** — Integrate validators with the .NET DI container.
- **Lightweight** — Focused validation without unnecessary complexity.

---

## Installation

Install Validtron from NuGet:

```bash
dotnet add package Validtron
```

Or through the NuGet Package Manager:

```powershell
Install-Package Validtron
```

---

## Quick Start

Consider a simple model:

```csharp
public sealed class Customer
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }
}
```

Create a validator by inheriting from `Validator<T>`:

```csharp
public sealed class CustomerValidator : Validator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18);
    }
}
```

Then validate your object:

```csharp
var customer = new Customer
{
    Name = "",
    Email = "invalid-email",
    Age = 16
};

var validator = new CustomerValidator();

var result = validator.Validate(customer);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.Message);
    }
}
```

The validation logic remains outside your model while still being strongly typed and easy to discover.

---

## Validation Results

Validtron returns a structured validation result that makes it easy to inspect failures.

```csharp
var result = validator.Validate(customer);

if (result.IsValid)
{
    // Continue...
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(
            $"{error.PropertyName}: {error.Message}"
        );
    }
}
```

Errors can also be accessed by property, making the result convenient for APIs, forms and application services.

```csharp
var errorsByProperty = result.ErrorsByProperty;
```

---

## Async Validation

Validation often depends on external resources such as databases or APIs.

Validtron supports asynchronous validation without forcing synchronous workarounds.

```csharp
var result = await validator.ValidateAsync(
    customer,
    cancellationToken
);
```

This makes asynchronous rules suitable for scenarios such as:

- checking whether a username already exists;
- validating an e-mail against an external service;
- verifying identifiers in a database;
- performing domain-specific asynchronous checks.

---

## Conditional Validation

Rules can be applied conditionally when validation depends on the state of the object.

This keeps business conditions close to the rules they control instead of spreading conditional logic throughout your application.

```csharp
RuleFor(x => x.CompanyName)
    .NotEmpty()
    .When(x => x.IsBusinessCustomer);
```

Conditional rules are useful when different validation requirements apply to different states of the same model.

---

## Nested Objects

Complex models can be composed from smaller validators.

```csharp
public sealed class AddressValidator : Validator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.PostalCode)
            .NotEmpty();
    }
}
```

The parent validator can then delegate validation of the nested object to the appropriate validator.

This allows validators to stay small, reusable and focused on a single responsibility.

---

## Collection Validation

Validtron can validate elements inside collections, allowing complex object graphs to remain strongly typed and composable.

This is useful for models such as:

```csharp
public sealed class Order
{
    public List<OrderItem> Items { get; set; } = [];
}
```

Each `OrderItem` can have its own validator rather than putting all validation logic inside the parent validator.

---

## Cascade Modes

Sometimes there is no reason to continue evaluating rules after an earlier rule has already failed.

Validtron supports configurable cascade behavior so validation can either:

- continue evaluating remaining rules; or
- stop after the first relevant failure.

This is especially useful when later validation depends on an earlier requirement being satisfied.

---

## Custom Validation

Built-in rules cover common scenarios, but real applications inevitably contain domain-specific requirements.

Validtron is designed to support custom validation logic while keeping it inside the same fluent validation pipeline.

This allows application-specific rules to remain reusable and testable instead of becoming scattered business logic.

---

## Dependency Injection

Validtron integrates with the standard .NET dependency injection ecosystem.

Validators can be registered in the application container and consumed by services that require them, keeping validation infrastructure consistent with the rest of your application architecture.

This is particularly useful in:

- ASP.NET Core APIs;
- application services;
- background workers;
- command/query handlers;
- modular applications.

---

## Why Validtron?

Validation code tends to start simple:

```csharp
if (string.IsNullOrWhiteSpace(user.Email))
{
    // ...
}
```

As an application grows, validation quickly becomes more complex:

```text
required fields
      ↓
format validation
      ↓
conditional rules
      ↓
nested objects
      ↓
collections
      ↓
database checks
      ↓
business rules
```

Validtron provides a dedicated validation layer for that complexity while keeping the calling code simple.

```csharp
var result = await validator.ValidateAsync(model);
```

The goal is straightforward:

> **Make validation expressive, composable and predictable without making it complicated.**

---

## Project Structure

```text
Validtron
│
├── src/
│   └── Validtron/
│       ├── Builders/
│       ├── Configurations/
│       ├── DependencyInjection/
│       ├── Exceptions/
│       ├── Extensions/
│       ├── Internal/
│       ├── Results/
│       ├── Rules/
│       └── Validator.cs
│
├── sample/
│   └── Validtron.Sample/
│
├── test/
│   └── Validtron.UnitTest/
│
├── docs/
├── CHANGELOG.md
├── CONTRIBUTING.md
├── SECURITY.md
└── LICENSE
```

---

## Documentation

Full documentation and examples are available at:

**https://ferdikurnazdm.github.io/Validtron/**

For a practical introduction, see the [Quick Start](https://ferdikurnazdm.github.io/Validtron/guide/quick-start.html).

You can also explore the [`Validtron.Sample`](sample/Validtron.Sample) project for working examples.

---

## Contributing

Contributions are welcome.

If you would like to fix a bug, improve documentation or propose a new feature, please read [CONTRIBUTING.md](CONTRIBUTING.md) before opening a pull request.

For larger changes, consider opening an issue first so the proposed approach can be discussed.

---

## Security

If you discover a security issue, please follow the process described in [SECURITY.md](SECURITY.md).

Please avoid reporting security vulnerabilities through public GitHub issues.

---

## Changelog

Release history and notable changes are documented in [CHANGELOG.md](CHANGELOG.md).

---

## License

Validtron is licensed under the [MIT License](LICENSE).

---

<div align="center">

### Built for clean and expressive .NET validation.

If Validtron is useful to you, consider giving the project a ⭐.

[Documentation](https://ferdikurnazdm.github.io/Validtron/) ·
[NuGet](https://www.nuget.org/packages/Validtron) ·
[GitHub](https://github.com/ferdikurnazdm/Validtron)

</div>