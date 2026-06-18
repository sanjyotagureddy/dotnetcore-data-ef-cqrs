using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Security;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.ApiKeys.RotateApiKey;

public sealed class Handler(RegistryDbContext dbContext, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var apiKey = await dbContext.ApiKeys.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException($"API key '{command.Id}' was not found.");

        var plain = ApiKeyGenerator.GeneratePlainTextKey();
        apiKey.Rotate(ApiKeyGenerator.Hash(plain), command.ExpiresAtUtc);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(apiKey.Id, plain, apiKey.Status.ToString(), apiKey.ExpiresAtUtc);
    }
}
