using DeveloperRegistry.Api.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace DeveloperRegistry.Api.Common.ProblemDetails;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            ValidationException validationException => HandleValidation(validationException),
            NotFoundException => HandleNotFound(exception),
            ConflictException => HandleConflict(exception),
            _ => HandleUnexpected(exception),
        };

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private Microsoft.AspNetCore.Mvc.ProblemDetails HandleValidation(ValidationException exception)
    {
        logger.LogWarning("Validation failed: {Errors}", string.Join("; ", exception.Errors.Select(x => x.ErrorMessage)));

        var errors = exception.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Detail = string.Join("; ", exception.Errors.Select(x => x.ErrorMessage)),
            Type = "https://httpstatuses.com/400",
            Extensions = { ["errors"] = errors },
        };
    }

    private Microsoft.AspNetCore.Mvc.ProblemDetails HandleNotFound(Exception exception)
    {
        logger.LogWarning(exception, "Resource not found");

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Resource not found",
            Status = StatusCodes.Status404NotFound,
            Detail = exception.Message,
            Type = "https://httpstatuses.com/404",
        };
    }

    private Microsoft.AspNetCore.Mvc.ProblemDetails HandleConflict(Exception exception)
    {
        logger.LogWarning(exception, "Conflict");

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Conflict",
            Status = StatusCodes.Status409Conflict,
            Detail = exception.Message,
            Type = "https://httpstatuses.com/409",
        };
    }

    private Microsoft.AspNetCore.Mvc.ProblemDetails HandleUnexpected(Exception exception)
    {
        logger.LogError(exception, "Unhandled exception while processing request");

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Unexpected error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An unexpected error occurred.",
            Type = "https://httpstatuses.com/500",
        };
    }
}
