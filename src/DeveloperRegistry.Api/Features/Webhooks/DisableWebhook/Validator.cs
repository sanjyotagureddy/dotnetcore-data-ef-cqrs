using FluentValidation;

namespace DeveloperRegistry.Api.Features.Webhooks.DisableWebhook;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(26);
    }
}
