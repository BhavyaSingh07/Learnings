using Entities;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDProject.StartUpExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddScoped<IcountriesService, CountriesService>();
            services.AddScoped<IPersonsService, PersonsService>();
            services.AddScoped<ICountriesRepository, CountryRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();


            return services;
            //builder.Services.AddHttpLogging(logging =>
            //{
            //    logging.LoggingFields("Application")
            //});
            //builder.Services.AddHttpLogging();
            //builder.Services.AddDbContext<PersonsDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //    //builder.Configuration.GetConnectionString("DefaultConnection")
            //    //builder.Configuration["ConnectionStrings:DefaultConnectionStrings"]
            //});

           

        }

    }
}
