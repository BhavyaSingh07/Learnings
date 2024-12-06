using Asp.Versioning;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddTransient<IJwtService, JwtService>();


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

builder.Services.AddCors(t =>
{
    t.AddDefaultPolicy(b =>
    {
        b.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>());
        b.WithHeaders("Authorization", "origin", "accept", "content-type")
        .WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(op =>
{
    op.Password.RequiredLength = 5;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireUppercase = false;
    op.Password.RequireLowercase = true;
    op.Password.RequireDigit = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(op =>
    {
        op.IncludeErrorDetails = true;
        op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
});

builder.Services.AddAuthorization();

var app = builder.Build();


app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger(); //create endpoints for swagger.json
app.UseSwaggerUI(); //create swagger ui for testing endpoints

app.UseRouting();
app.UseCors();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
