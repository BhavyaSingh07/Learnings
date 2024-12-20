using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repositories;
using RepositoryContracts;
using CRUDProject.StartUpExtensions;
using CRUDProject.Filters.ActionFilters;
using CRUDProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
    //options.Filters.Add<ResponseHeaderActionFilter>();

    options.Filters.Add(new ResponseHeaderActionFilter(logger, "my-key-fromglobal", "my-value-fromglobal", 2));
});


builder.AddSqlServerDbContext<ApplicationDbContext>(
                       "DefaultConnection", configureDbContextOptions: dbContextOptionsBuilder =>
                       {
                           dbContextOptionsBuilder.UseSqlServer();
                           dbContextOptionsBuilder.ConfigureWarnings(
                               warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
                       });

builder.Services.ConfigureServices();


builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
});

var app = builder.Build();

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("info-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddDebug();
//});


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseHttpLogging();
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath:"Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();






