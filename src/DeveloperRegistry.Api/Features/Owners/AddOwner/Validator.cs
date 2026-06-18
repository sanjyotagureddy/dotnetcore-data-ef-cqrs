using FluentValidation;

namespace DeveloperRegistry.Api.Features.Owners.AddOwner;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty().Length(26);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
    }
}
