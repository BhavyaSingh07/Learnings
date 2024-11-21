using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomMiddleware>();
// builder.Services.AddTransient<HelloCustomMiddle>();
var app = builder.Build();

app.Use(async (HttpContext context, RequestDelegate next) => {
    await context.Response.WriteAsync("Middleware 1 executes\n");
    await next(context);
});

//app.UseMiddleware<MyCustomMiddleware>();
//app.UseMyCustomMiddleware();
app.UseHelloCustomMiddle();
 
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Middleware 3 executes\n");
});

app.Run();
