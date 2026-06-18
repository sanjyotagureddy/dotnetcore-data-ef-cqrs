using FluentValidation;

namespace DeveloperRegistry.Api.Common.Validation;

public static class ValidationExtensions
{
    public static async Task ValidateRequestAsync<T>(this IValidator<T> validator, T request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}
