using DeveloperRegistry.Api.Common;
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

        var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.Id == command.ApplicationId, cancellationToken)
            ?? throw new NotFoundException($"Application '{command.ApplicationId}' was not found.");

        var owner = await dbContext.Owners.FirstOrDefaultAsync(x => x.Email == command.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (owner is null)
        {
            owner = Owner.Create(IdGenerator.NewId(), command.Name, command.Email, clock.UtcNow);
            dbContext.Owners.Add(owner);
        }

        var exists = await dbContext.ApplicationOwners.AnyAsync(
            x => x.ApplicationId == command.ApplicationId && x.OwnerId == owner.Id,
            cancellationToken);

        if (!exists)
        {
            application.AddOwner(owner.Id, clock.UtcNow);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(owner.Id, command.ApplicationId, owner.Name, owner.Email);
    }
}
