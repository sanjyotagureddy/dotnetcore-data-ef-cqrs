using FluentValidation;

namespace DeveloperRegistry.Api.Features.Applications.UpdateApplication;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(26);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
    }
}
