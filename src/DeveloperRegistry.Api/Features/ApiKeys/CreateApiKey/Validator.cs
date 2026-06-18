using FluentValidation;

namespace DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty().Length(26);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
