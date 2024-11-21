using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MiddlewareExample.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HelloCustomMiddle
    {
        private readonly RequestDelegate _next;

        public HelloCustomMiddle(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

           if( httpContext.Request.Query.ContainsKey("firstName") && httpContext.Request.Query.ContainsKey("secondName"))
            {
               string fullName = httpContext.Request.Query["firstName"] + " " + httpContext.Request.Query["secondName"];
                await httpContext.Response.WriteAsync(fullName);
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HelloCustomModdleExtensions
    {
        public static IApplicationBuilder UseHelloCustomMiddle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloCustomMiddle>();
        }
    }
}
