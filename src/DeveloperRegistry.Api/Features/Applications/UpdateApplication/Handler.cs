using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.Time;
using DeveloperRegistry.Api.Common.Validation;
using DeveloperRegistry.Api.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperRegistry.Api.Features.Applications.UpdateApplication;

public sealed class Handler(RegistryDbContext dbContext, IClock clock, IValidator<Command> validator)
{
    public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        await validator.ValidateRequestAsync(command, cancellationToken);

        var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException($"Application '{command.Id}' was not found.");

        application.Update(command.Name, command.Description, clock.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(application.Id, application.Name, application.Description, application.UpdatedAtUtc);
    }
}
