using Asp.Versioning;
using CitiesManager.WebAPI.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//builder.Services.AddApiVersioning(config =>
//{
//    config.ApiVersionReader = new UrlSegmentApiVersionReader();
//    //config.ApiVersionReader = new QueryStringApiVersionReader();
//    config.DefaultApiVersion = new ApiVersion(1);
//    config.AssumeDefaultVersionWhenUnspecified = true;
//});





builder.Services.AddEndpointsApiExplorer(); //generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
    
}); //generates OpenApi specification

builder.Services.AddApiVersioning(op =>
{
    op.AssumeDefaultVersionWhenUnspecified = true;
    op.DefaultApiVersion = ApiVersion.Default;
    op.ReportApiVersions = true;
}).AddApiExplorer(op =>
{
    op.GroupNameFormat = "'v'V";
    op.SubstituteApiVersionInUrl = true;
});



var app = builder.Build();


app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger(); //create endpoints for swagger.json
app.UseSwaggerUI(); //create swagger ui for testing endpoints


app.UseAuthorization();
app.MapControllers();

app.Run();
