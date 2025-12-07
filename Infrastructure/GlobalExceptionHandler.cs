using Microsoft.AspNetCore.Diagnostics; // For IExceptionHandler
using Microsoft.AspNetCore.Mvc; // For ProblemDetails

namespace Restaurant.Api.Infrastructure;

// Global exception handler middleware
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // Handles unhandled exceptions globally
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, // Current HTTP context
        Exception exception, // Exception that occurred
        CancellationToken cancellationToken) // Cancellation token
    {
        // Logs the exception details
        logger.LogError(exception, "An unhandled exception occurred.");

        // Creates ProblemDetails for the error response
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError, // 500 status code
            Title = "Server Error", // Title of error
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1", // Type URI
            Detail = "An unexpected error occurred. Please try again later.", // Detail message
        };

        // Sets the response status code and writes the ProblemDetails as JSON
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Indicate that the exception has been handled
        return true;
    }
}
