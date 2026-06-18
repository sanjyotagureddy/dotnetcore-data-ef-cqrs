using DeveloperRegistry.Api.Common;
using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public sealed class Handler(RegistryDbContext dbContext, IClock clock, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.Id == command.ApplicationId, cancellationToken)
            ?? throw new NotFoundException($"Application '{command.ApplicationId}' was not found.");

        var webhook = application.RegisterWebhook(IdGenerator.NewId(), command.EventName, command.Url, clock.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(webhook.Id, webhook.ApplicationId, webhook.EventName, webhook.Url, webhook.Enabled);
    }
}
