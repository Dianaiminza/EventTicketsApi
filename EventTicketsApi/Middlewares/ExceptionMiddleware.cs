using Infrastructure.shared.CustomExceptions;
using Infrastructure.shared.CustomExceptions.Models;
using Infrastructure.shared.Serializers;
using Infrastructure.shared.Wrapper;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace EventTicketsApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debugger.Break();

            var errorMessage = ex.GetFullMessage();
            IResult<IEnumerable<ValidationError>> errorResponse;
            var logDebug = true;

            switch (ex)
            {
                case ApiException apiException:
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      errorMessage,
                      apiException.StatusCode,
                      apiException.Errors)
                    .ConfigureAwait(false);
                    break;

                case ArgumentNullException:
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      errorMessage,
                      HttpStatusCode.BadRequest,
                      Enumerable.Empty<ValidationError>())
                    .ConfigureAwait(false);
                    break;

                case ArgumentException:
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      errorMessage,
                      HttpStatusCode.BadRequest,
                      Enumerable.Empty<ValidationError>())
                    .ConfigureAwait(false);
                    break;

                case KeyNotFoundException:
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      errorMessage,
                      HttpStatusCode.BadRequest,
                      Enumerable.Empty<ValidationError>())
                    .ConfigureAwait(false);
                    break;

                case UnauthorizedAccessException:
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      ex.Message.Trim(),
                      HttpStatusCode.Unauthorized,
                      Enumerable.Empty<ValidationError>())
                    .ConfigureAwait(false);
                    break;

                default:
                    // unhandled error
                    logDebug = false;
                    _logger.LogError("Unhandled exception: ", ex.GetFullMessage());
                    errorResponse = await Result<IEnumerable<ValidationError>>.FailAsync(
                      "An unhandled error occurred",
                      HttpStatusCode.InternalServerError,
                      Enumerable.Empty<ValidationError>())
                    .ConfigureAwait(false);
                    break;
            }

            if (logDebug)
            {
                _logger.LogDebug(ex.GetFullMessage());
            }

            var response = context.Response;
            response.ContentType = "application/problem+json";
            response.StatusCode = (int)errorResponse.StatusCode;
            await response
              .WriteAsync(
                JsonConvert.SerializeObject(errorResponse, SerializerSettingsHelper.CamelCase()))
              .ConfigureAwait(false);
        }
    }
}
