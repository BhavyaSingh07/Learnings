using System.Globalization;
using RoutingParam.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("monthcheck", typeof(MonthsCustomConstraint));
});


var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    app.Map("files/{filename=samp}.{extension=txt}", async context =>
    {
        string? fnme = Convert.ToString(context.Request.RouteValues["filename"]);
        string? ext = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In File - {fnme} and extension - {ext}");    
    });

    app.Map("products/details/{id:int:range(1,100)?}", async context =>
    {
        if(context.Request.RouteValues.ContainsKey("id"))
        {string obj = Convert.ToString(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"The id of the product is - {obj}");
        }
        else
        {
            await context.Response.WriteAsync("Please provide the id");
        }
    });

    endpoints.Map("daily-report/{reportDate:datetime}", async context =>
    {
        DateTime dt = Convert.ToDateTime(context.Request.RouteValues["reportDate"]);
        await context.Response.WriteAsync($"The date is {dt.ToShortDateString()}");

    }
        );

    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityid = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"The id of the city is - {cityid.ToString()}");
    });

    endpoints.Map("employee/profile/{empname:length(3,8)=aman}", async context =>
    {
        string? employeename = Convert.ToString(context.Request.RouteValues["empname"]);
        await context.Response.WriteAsync($"The name of employee is {employeename}");
    });

    endpoints.Map("sales-reports/{year:int:min(1900)}/{month:monthcheck}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"The year is {year} and month is {month}");
    });

    endpoints.Map("sales-reports/2024/oct", async context =>
    {
        await context.Response.WriteAsync("Sales report is given");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Received at {context.Request.Path}");
});

app.Run();
