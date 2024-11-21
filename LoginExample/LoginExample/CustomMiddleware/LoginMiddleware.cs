using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace LoginExample.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path=="/" && context.Request.Method == "POST")
            {
                StreamReader reader = new StreamReader(context.Request.Body);
                string body =await reader.ReadToEndAsync();
                Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
                string? email = null, password = null;
                if (queryDict.ContainsKey("email"))
                {
                    email = Convert.ToString(queryDict["email"][0]);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid input for email\n");
                }
                if (queryDict.ContainsKey("password"))
                {
                    password = Convert.ToString(queryDict["password"][0]);
                }
                else
                {
                    if(context.Response.StatusCode==200)
                    {context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid input for password\n");
                    }
                    
                }

               if(string.IsNullOrEmpty(email)==false && string.IsNullOrEmpty(password)==false) 
                {
                    string validEmail = "admin@example.com"; string validPassword = "admin1234";
                    bool isValid;
                if(email==validEmail && password == validPassword)
                {
                    isValid = true;
                }
                else
                {
                    isValid=false;
                }

               if (isValid)
                    {
                        await context.Response.WriteAsync("Successful Login");
                    }
               else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid Login");
                    }
                }
            }
            else { await _next(context); }
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
