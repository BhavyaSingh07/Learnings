using ServiceContracts;
using Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Add(new ServiceDescriptor(
        typeof(ICitiesService), typeof(CitiesService), ServiceLifetime.Scoped
        //Transient - per injection, scoped - browser req, singleton - same for entire lifetime of app
));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();

//Global state in services , use concurrent dictionary for singleton services, use distributed cache

// dont use scoped services to share data among classes 

//Service Locator pattern

//captive dependencies - dont inject transient services in singleton service
