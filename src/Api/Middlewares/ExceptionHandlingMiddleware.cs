using FluentValidation;
using LifeCore.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LifeCore.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ValidationProblemDetails(
                exception.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray()))
            {
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (ConflictException exception)
        {
            await WriteProblemDetailsAsync(context, StatusCodes.Status409Conflict, "Conflict", exception.Message);
        }
        catch (UnauthorizedException exception)
        {
            await WriteProblemDetailsAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.ToString());
            await WriteProblemDetailsAsync(context, StatusCodes.Status500InternalServerError, "Server error", exception.Message);
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode
        };

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}