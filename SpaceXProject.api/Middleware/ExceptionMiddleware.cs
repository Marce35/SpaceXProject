using System.Net;
using System.Text.Json;
using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;

namespace SpaceXProject.api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IResultFactory _resultFactory;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IResultFactory resultFactory)
    {
        _next = next;
        _logger = logger;
        _resultFactory = resultFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)HttpStatusCode.OK;

        var result = _resultFactory.Exception(exception, "An internal server error occurred.");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(result, jsonOptions);
        await context.Response.WriteAsync(json);
    }
}
