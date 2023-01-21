using System.Text.Json;
using Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static void Seed(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var ctx = services.GetRequiredService<StoreContext>();
                ctx.Database.Migrate();
                
                if (!ctx.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands != null)
                        foreach (var item in brands)
                        {
                            ctx.ProductBrands.Add(item);
                        }
                    ctx.SaveChanges();
                }
            
                if (!ctx.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types != null)
                        foreach (var item in types)
                        {
                            ctx.ProductTypes.Add(item);
                        }
                    ctx.SaveChanges();
                }
                
                if (!ctx.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products != null)
                        foreach (var item in products)
                        {
                            ctx.Products.Add(item);
                        }

                    ctx.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                var logger = loggerFactory.CreateLogger<StoreContext>();
                logger.LogError(exception, "An error occured during migration");
            }
        }
    }
}