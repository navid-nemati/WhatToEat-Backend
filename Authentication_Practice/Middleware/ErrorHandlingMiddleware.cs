using Authentication_Practice.Models;

namespace Authentication_Practice.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
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
            catch (AppException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                await WriteError(context, ex.StatusCode, ex.Message, ex.Errors);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                //await WriteError(
                //    context,
                //    StatusCodes.Status500InternalServerError,
                //    "خطای غیر منتظره سمت سرور"
                //);

                await WriteError(
                    context,
                    StatusCodes.Status500InternalServerError,
                    ex.Message
                );
            }
        }

        private async Task WriteError(
            HttpContext ctx,
            int StatusCode,
            string message,
            object? errors = null)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = StatusCode;

            var result = new ErrorResponse
            {
                Status = StatusCode,
                Message = message,
                Errors = errors
            };  

            await ctx.Response.WriteAsJsonAsync(result);
        }
    }
}
