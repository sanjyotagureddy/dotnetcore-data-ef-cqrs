using FluentValidation;

namespace DeveloperRegistry.Api.Features.Applications.CreateApplication;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
    }
}
