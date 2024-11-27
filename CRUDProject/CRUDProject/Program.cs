using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IcountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
//builder.Services.AddDbContext<PersonsDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//    //builder.Configuration.GetConnectionString("DefaultConnection")
//    //builder.Configuration["ConnectionStrings:DefaultConnectionStrings"]
//});

builder.AddSqlServerDbContext<PersonsDbContext>(
            "DefaultConnection", configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseSqlServer();
                dbContextOptionsBuilder.ConfigureWarnings(
                    warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});


var app = builder.Build();

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath:"Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();






