using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endpoint = context.GetEndpoint();
    if(endpoint != null)
    {
        await context.Response.WriteAsync($"The endpoint is {endpoint.DisplayName}\n\n");
    }
    await next(context);
});

app.UseRouting();

app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        await context.Response.WriteAsync($"The endpoint is {endpoint.DisplayName}\n\n");
    }
    await next(context);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("map1", async context =>
    {
        await context.Response.WriteAsync("Hello map 1");
    });

    endpoints.MapPost("/map2", async context =>
    {
        await context.Response.WriteAsync("Hello map 2");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Received at {context.Request.Path}");
});
app.Run();
