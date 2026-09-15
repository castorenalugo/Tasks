using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.Api.Exceptions;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken ct)
    {
        var problemDetails = new ProblemDetails()
        {
            Title = "An error ocurred when processing your request.",
            Detail = ex.Message,
            Status = StatusCodes.Status500InternalServerError
        };

        if(ex is NotFoundEx notFoundEx)
        {
            problemDetails.Title = "Resource not found";
            if(notFoundEx.ReasonCode != null)
                problemDetails.Extensions["ReasonCode"] = notFoundEx.ReasonCode;
            problemDetails.Status = StatusCodes.Status404NotFound;
        }

        if(ex is ValidationEx validationEx)
        {
            problemDetails.Title = "Resource not found";
            if(validationEx.ReasonCode != null)
                problemDetails.Extensions["ReasonCode"] = validationEx.ReasonCode;
            problemDetails.Status = StatusCodes.Status400BadRequest;
        }

        if(ex is NotAllowedEx notAllowedEx)
        {
            problemDetails.Title = "Resource not found";
            if(notAllowedEx.ReasonCode != null)
                problemDetails.Extensions["ReasonCode"] = notAllowedEx.ReasonCode;
            problemDetails.Status = StatusCodes.Status403Forbidden;
        }

        context.Response.StatusCode = problemDetails.Status.Value;

        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}