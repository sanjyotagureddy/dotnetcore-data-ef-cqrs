using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Owners.RemoveOwner;

public sealed class Handler(RegistryDbContext dbContext, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var relation = await dbContext.ApplicationOwners.FirstOrDefaultAsync(
            x => x.ApplicationId == command.ApplicationId && x.OwnerId == command.OwnerId,
            cancellationToken);

        if (relation is null)
        {
            return new Response(command.ApplicationId, command.OwnerId, false);
        }

        dbContext.ApplicationOwners.Remove(relation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(command.ApplicationId, command.OwnerId, true);
    }
}
