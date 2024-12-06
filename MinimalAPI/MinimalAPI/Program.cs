using System.Text.Json;
using MinimalAPI.Models;
using MinimalAPI.RouteGroups;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var group = app.MapGroup("/products").ProductsAPI();


app.Run();
