using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Middleware
{
    public class ErrorHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            catch (NotFoundException e)
            {
                await CreateContext(context, e, 404);
            }
            catch (UnauthorizedAccessException e)
            {
                await CreateContext(context, e, 401);
            }
            catch (FormatException e)
            {
                await CreateContext(context, e, 400);
            }
            catch (Exception e)
            {
                await CreateContext(context, e, 500);
            }
        }

        private async Task CreateContext (HttpContext context, Exception e, int statusCode)
        {
            _logger.LogError(e, e.Message);

            var result = JsonConvert.SerializeObject(new
            {
                error = e.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}