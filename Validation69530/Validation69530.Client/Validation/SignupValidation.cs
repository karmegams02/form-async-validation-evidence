using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Validation;

namespace Validation69530.Client.Validation;

public enum AvailabilityOutcome
{
    Available,
    Taken,
    Failure
}

public sealed class UsernameAvailabilityService
{
    public AvailabilityOutcome Outcome { get; set; } =
        AvailabilityOutcome.Available;

    public async Task<bool> IsAvailableAsync(
        string username,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        return Outcome switch
        {
            AvailabilityOutcome.Available => true,
            AvailabilityOutcome.Taken => false,
            AvailabilityOutcome.Failure => throw new InvalidOperationException(
                "The simulated username service failed."),
            _ => throw new InvalidOperationException(
                $"Unsupported outcome: {Outcome}.")
        };
    }
}

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Parameter,
    AllowMultiple = false)]
public sealed class UsernameAvailableAttribute : AsyncValidationAttribute
{
    public UsernameAvailableAttribute()
    {
        ErrorMessage = "Username is already taken.";
    }

    protected override async Task<ValidationResult?> IsValidAsync(
        object? value,
        ValidationContext validationContext,
        CancellationToken cancellationToken)
    {
        if (value is not string username ||
            string.IsNullOrWhiteSpace(username))
        {
            return ValidationResult.Success;
        }

        var service =
            validationContext.GetRequiredService<UsernameAvailabilityService>();

        var available = await service.IsAvailableAsync(
            username,
            cancellationToken);

        return available
            ? ValidationResult.Success
            : new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                [validationContext.MemberName!]);
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        throw new InvalidOperationException(
            "Synchronous username validation isn't supported.");
    }
}

[ValidatableType]
public sealed class SignupModel
{
    [Required(ErrorMessage = "Username is required.")]
    [UsernameAvailable]
    public string Username { get; set; } = string.Empty;
}