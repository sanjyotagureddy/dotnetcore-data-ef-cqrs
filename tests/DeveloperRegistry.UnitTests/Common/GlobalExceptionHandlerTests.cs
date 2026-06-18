using System.Text.Json;
using DeveloperRegistry.Api.Common.Exceptions;
using DeveloperRegistry.Api.Common.ProblemDetails;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace DeveloperRegistry.UnitTests.Common;

public sealed class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ShouldReturnValidationProblem_ForValidationException()
    {
        var sut = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var context = CreateHttpContext();
        var exception = new ValidationException(
            [
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Description", "Description is required"),
            ]);

        var handled = await sut.TryHandleAsync(context, exception, CancellationToken.None);
        var problem = await DeserializeProblemDetailsAsync(context);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        problem.Title.Should().Be("Validation failed");
        problem.Detail.Should().Contain("Name is required").And.Contain("Description is required");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturnNotFoundProblem_ForNotFoundException()
    {
        var sut = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var context = CreateHttpContext();
        var exception = new NotFoundException("owner not found");

        var handled = await sut.TryHandleAsync(context, exception, CancellationToken.None);
        var problem = await DeserializeProblemDetailsAsync(context);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be("Resource not found");
        problem.Detail.Should().Be("owner not found");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturnGenericProblem_ForUnknownException()
    {
        var sut = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("boom");

        var handled = await sut.TryHandleAsync(context, exception, CancellationToken.None);
        var problem = await DeserializeProblemDetailsAsync(context);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        problem.Title.Should().Be("Unexpected error");
        problem.Detail.Should().Be("An unexpected error occurred.");
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<ProblemDetails> DeserializeProblemDetailsAsync(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        var problem = await JsonSerializer.DeserializeAsync<ProblemDetails>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        problem.Should().NotBeNull();
        return problem!;
    }
}
