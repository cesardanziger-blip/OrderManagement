using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Exceptions.Common;

namespace OrderManagement.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed.");

            await WriteValidationProblemDetails(context, ex);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteProblemDetails(
                context,
                StatusCodes.Status409Conflict,
                "Business rule violation",
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                "Resource not found",
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteProblemDetails(
                context,
                StatusCodes.Status409Conflict,
                "Invalid operation",
                ex.Message);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var dbValues = await entry.GetDatabaseValuesAsync();
                var currentValues = entry.CurrentValues;
                var originalValues = entry.OriginalValues;
            }

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            await WriteProblemDetails(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemDetails(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }

    private static async Task WriteValidationProblemDetails(
        HttpContext context,
        ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}