using FluentValidation;

namespace DeveloperRegistry.Api.Features.Owners.RemoveOwner;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty().Length(26);
        RuleFor(x => x.OwnerId).NotEmpty().Length(26);
    }
}
