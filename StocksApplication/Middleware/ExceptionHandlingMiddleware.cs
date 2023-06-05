using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StocksApiBasics.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try //any possible error generated in following middlewares will be catched by catch block
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType}.{ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType}.{ExceptionMessage}", ex.GetType().ToString(), ex.Message);

                }
                //httpContext.Response.StatusCode = 500;
                //await httpContext.Response.WriteAsync("Error Occured");

                throw; //rethrow


            }


        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
