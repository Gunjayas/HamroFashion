using HamroFashion.Api.V1.Exceptions;
using Newtonsoft.Json;
using Serilog;

namespace HamroFashion.Api.V1.Middleware
{
    /// <summary>
    /// Our custom endpoint filter that is applied to the group handler. This filter will catch
    /// any exceptions thrown by filters or route handlers after it is added. It will log and
    /// convert exceptions to HTTP friendly responses.
    /// </summary>
    public class ExceptionFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                return await next(context);
            }
            catch (BadHttpRequestException err)
            {
                Log.Warning($"Bad Request: {err.Message}");
                return Results.ValidationProblem(new Dictionary<string, string[]>(), err.Message);
            }
            catch(ApiException err)
            {
                Log.Warning($"Bad Request: {JsonConvert.SerializeObject(err.FieldErrors)}");
                return Results.ValidationProblem(err.FieldErrors, err.Message);
            }
            catch (EntityNotFoundException err)
            {
                Log.Warning(err.Message);
                return Results.NotFound(err.Message);
            }
            catch (UnauthorizedAccessException err)
            {
                Log.Warning(err.Message);
                return TypedResults.Unauthorized();
            }
            catch (Exception err)
            {
                Log.Error(err.ToString());
                return Results.Problem(err.Message);
            }
        }
    }
}
