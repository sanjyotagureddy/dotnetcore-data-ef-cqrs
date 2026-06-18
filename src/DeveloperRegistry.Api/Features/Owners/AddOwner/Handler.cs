using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Domain;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Owners.AddOwner;

public sealed class Handler(RegistryDbContext dbContext, IClock clock, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var applicationExists = await dbContext.Applications.AnyAsync(x => x.Id == command.ApplicationId, cancellationToken);
        if (!applicationExists)
        {
            throw new NotFoundException($"Application '{command.ApplicationId}' was not found.");
        }

        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var owner = await dbContext.Owners.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (owner is null)
        {
            owner = Owner.Create(NUlid.Ulid.NewUlid().ToString(), command.Name, normalizedEmail, clock.UtcNow);
            dbContext.Owners.Add(owner);
        }

        var exists = await dbContext.ApplicationOwners.AnyAsync(
            x => x.ApplicationId == command.ApplicationId && x.OwnerId == owner.Id,
            cancellationToken);

        if (!exists)
        {
            dbContext.ApplicationOwners.Add(ApplicationOwner.Create(command.ApplicationId, owner.Id, clock.UtcNow));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(owner.Id, command.ApplicationId, owner.Name, owner.Email);
    }
}
