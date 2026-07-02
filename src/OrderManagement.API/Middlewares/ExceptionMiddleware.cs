using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Exceptions;
using System.Text.Json;

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
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteProblemDetails(
                context,
                StatusCodes.Status409Conflict,
                "Business rule violation",
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                "Resource not found",
                ex.Message);
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

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}