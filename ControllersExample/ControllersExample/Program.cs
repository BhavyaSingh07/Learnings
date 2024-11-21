using ControllersExample.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//reading reuqests -> validation -> invoking models -> preparing response


app.UseStaticFiles();
app.MapControllers();




app.Run();
