using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Security;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.ApiKeys.CreateApiKey;

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

        var plainKey = ApiKeyGenerator.GeneratePlainTextKey();
        var apiKey = ApiKey.Create(
            NUlid.Ulid.NewUlid().ToString(),
            command.ApplicationId,
            command.Name,
            ApiKeyGenerator.Hash(plainKey),
            clock.UtcNow,
            command.ExpiresAtUtc);

        dbContext.ApiKeys.Add(apiKey);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(apiKey.Id, apiKey.ApplicationId, apiKey.Name, plainKey, apiKey.CreatedAtUtc, apiKey.ExpiresAtUtc);
    }
}
