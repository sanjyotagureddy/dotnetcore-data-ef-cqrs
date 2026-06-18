using FluentValidation;

namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty().Length(26);
        RuleFor(x => x.EventName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(1000).Must(x => Uri.TryCreate(x, UriKind.Absolute, out _));
    }
}
