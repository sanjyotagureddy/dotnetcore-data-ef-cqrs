using DeveloperRegistry.Api.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace DeveloperRegistry.Api.Common.ProblemDetails;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception while processing request");

        var problem = exception switch
        {
            ValidationException validationException => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = string.Join("; ", validationException.Errors.Select(x => x.ErrorMessage)),
                Type = "https://httpstatuses.com/400",
            },
            NotFoundException => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message,
                Type = "https://httpstatuses.com/404",
            },
            _ => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Unexpected error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred.",
                Type = "https://httpstatuses.com/500",
            },
        };

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
