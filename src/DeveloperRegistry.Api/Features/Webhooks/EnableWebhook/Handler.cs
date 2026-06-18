using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Webhooks.EnableWebhook;

public sealed class Handler(RegistryDbContext dbContext, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var webhook = await dbContext.Webhooks.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException($"Webhook '{command.Id}' was not found.");

        webhook.Enable();
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(webhook.Id, webhook.Enabled);
    }
}
