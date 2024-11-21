using LoginExample.CustomMiddleware;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHelloMiddleware();


app.Run(async context =>
{
    await context.Response.WriteAsync("No response");
});

app.Run();
