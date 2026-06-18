using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Webhooks.RegisterWebhook;

public sealed class Handler(RegistryDbContext dbContext, IClock clock, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var exists = await dbContext.Applications.AnyAsync(x => x.Id == command.ApplicationId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException($"Application '{command.ApplicationId}' was not found.");
        }

        var webhook = Webhook.Create(NUlid.Ulid.NewUlid().ToString(), command.ApplicationId, command.EventName, command.Url, clock.UtcNow);
        dbContext.Webhooks.Add(webhook);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(webhook.Id, webhook.ApplicationId, webhook.EventName, webhook.Url, webhook.Enabled);
    }
}
