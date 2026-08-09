using System.Net;
using System.Text.Json;
using Touchliga.Contracts.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Api.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    // Sin esto, JsonSerializer.Serialize usa PascalCase por default
    // ("Message", "Errors") — Flutter siempre busca minúscula
    // ("message"), así que sin esta opción el mensaje real del
    // backend nunca llegaba a mostrarse, solo el genérico de
    // repuesto ("Error del servidor").
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Success = false,
            Message = exception.Message
        };

        context.Response.StatusCode = exception switch
        {
            ValidationException validation =>
                WriteValidation(response, validation, context),

            EntityNotFoundException =>
                (int)HttpStatusCode.NotFound,

            UnauthorizedAccessException =>
                (int)HttpStatusCode.Unauthorized,

            BusinessException =>
                (int)HttpStatusCode.Conflict,

            _ =>
                (int)HttpStatusCode.InternalServerError
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, _jsonOptions));
    }

    private static int WriteValidation(
        ErrorResponse response,
        ValidationException validation,
        HttpContext context)
    {
        response.Errors = validation.Errors;

        return (int)HttpStatusCode.BadRequest;
    }
}
