using System.Text.Json;
using LostAndFoundAPI.Common;

namespace LostAndFoundAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                context.Response.ContentType = "application/json";
                if( ex is KeyNotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else if(ex is UnauthorizedAccessException)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }

                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            }
            }
        }
    }
