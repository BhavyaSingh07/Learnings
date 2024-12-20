using System.IO;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async(HttpContext context) => {
    //context.Response.StatusCode = 400;
    //context.Response.Headers["my keys"] = "my value";
    //context.Response.Headers["Content-type"] = "text/html";
    //string path = context.Request.Path;
    //string method = context.Request.Method;
    //if(context.Request.Method == "GET")
    //{
    //    context.Response.Headers["Content-type"] = "text/html";
    //    if (context.Request.Headers.ContainsKey("AuthorizationKey"))
    //    {
    //        string auth = context.Request.Headers["AuthorizationKey"];
    //        await context.Response.WriteAsync($"<p1>{auth}</p1>");
    //    }

    //}
    //  System.IO.StreamReader reader = new StreamReader(context.Request.Body);
    //  string body = await reader.ReadToEndAsync();
    //  Dictionary<string, StringValues> dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
    //if (dict.ContainsKey("firstName"))
    //{
    //    string fname = dict["firstName"][0];
    //    await context.Response.WriteAsync(fname);
    //}

    if (context.Request.Method == "GET" && context.Request.Path == "/")
    {
        int firstNumber = 0, secondNumber = 0;
        string? operation = null;
        long? result = null;

        
        if (context.Request.Query.ContainsKey("firstNumber"))
        {
            string firstNumberString = context.Request.Query["firstNumber"][0];
            if (!string.IsNullOrEmpty(firstNumberString))
            {
                firstNumber = Convert.ToInt32(firstNumberString);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
        }

        
        if (context.Request.Query.ContainsKey("secondNumber"))
        {
            string secondNumberString = context.Request.Query["secondNumber"][0];
            if (!string.IsNullOrEmpty(secondNumberString))
            {
                secondNumber = Convert.ToInt32(context.Request.Query["secondNumber"][0]);
            }
            else
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
        }

       
        if (context.Request.Query.ContainsKey("operation"))
        {
            operation = Convert.ToString(context.Request.Query["operation"][0]);

            
            switch (operation)
            {
                case "add": result = firstNumber + secondNumber; break;
                case "subtract": result = firstNumber - secondNumber; break;
                case "multiply": result = firstNumber * secondNumber; break;
                case "divide": result = (secondNumber != 0) ? firstNumber / secondNumber : 0; break; 
                case "mod": result = (secondNumber != 0) ? firstNumber % secondNumber : 0; break; 
            }

            
            if (result.HasValue)
            {
                await context.Response.WriteAsync(result.Value.ToString());
            }

            else
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'operation'\n");
            }

        } 

        
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'operation'\n");
        }
    }
});
   
app.Run();
