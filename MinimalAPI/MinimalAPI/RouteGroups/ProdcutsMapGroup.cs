using System.Runtime.CompilerServices;
using MinimalAPI.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.RouteGroups
{
    public static class ProdcutsMapGroup
    {
        private static List<Product> products = new List<Product>()
{
                     new Product(){ Id = 1, ProductName = "Smart Phone"},
                     new Product() {Id = 2, ProductName = "Television"}
             };
        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {

            group.MapGet("/", async (context) => {
                //var content = string.Join('\n', products.Select(t => t.ToString()));
                await context.Response.WriteAsync(JsonSerializer.Serialize(products));
            });

            group.MapGet("/{id:int}", async (HttpContext context, int id) =>
            {
                Product? product = products.FirstOrDefault(t => t.Id == id);
                if (product == null)
                {
                    await context.Response.WriteAsync($"Product {id} not found");
                    return;
                    //await context.Response.WriteAsync(products.Where(t => t.Id == id).ToString());
                }
                await context.Response.WriteAsync(JsonSerializer.Serialize(product));
            });

            group.MapPost("/", async (HttpContext context, Product product) => {
                products.Add(product); ;
                await context.Response.WriteAsync("Product Added!!");
            }).AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
            {
                var product = context.Arguments.OfType<Product>().FirstOrDefault();
                if(product == null)
                {
                    return Results.BadRequest();
                }
                var validationContext = new ValidationContext(product);
                List<ValidationResult> errors = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(product, validationContext, errors, true);

                if (!isValid)
                {
                    return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
                }

                var result = await next(context);

                //After logic here


                return result;
            });

            group.MapPut("/{id}", async (HttpContext context,int id, [FromBody]Product product) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    context.Response.StatusCode = 400; //Bad Request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }

                productFromCollection.ProductName = product.ProductName;

                await context.Response.WriteAsync("Product Updated");
            });

            group.MapDelete("/{id}", async (HttpContext context, int id) => {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    //context.Response.StatusCode = 400; //Bad Request
                    //await context.Response.WriteAsync("Incorrect Product ID");
                    //return;
                    return Results.BadRequest();
                }

                products.Remove(productFromCollection);
                return Results.Ok("Product Deleted");

               // await context.Response.WriteAsync("Product Deleted");
            });


            return group;
        }
    }
}
