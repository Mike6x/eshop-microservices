using Basket.API.Features.v1.Basket;

namespace Basket.API;

public static class BasketModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("basket") { }
        
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var basketGroup = app.MapGroup("baskets").WithTags("Basket's API Group");
            basketGroup.MapCheckoutBasketEndpoint();
            basketGroup.MapGetBasketEndpoint();
            basketGroup.MapStoreBasketEndpoint();
            basketGroup.MapDeleteBasketEndpoint();
    
            // RouteGroupBuilder brandGroup = app.MapGroup("brands").WithTags("brands");
            // brandGroup.MapCreateBrandEndpoint();
            // brandGroup.MapGetBrandEndpoint();
            // brandGroup.MapGetBrandsEndpoint();
            // brandGroup.MapSearchBrandsEndpoint();
            // brandGroup.MapUpdateBrandEndpoint();
            // brandGroup.MapDeleteBrandEndpoint();
            // brandGroup.MapExportBrandsEndpoint();
            // brandGroup.MapImportBrandsEndpoint();
        }
    }
    // public static WebApplicationBuilder RegisterCatalogServices(this WebApplicationBuilder builder)
    // {
    //     ArgumentNullException.ThrowIfNull(builder);
    //     // builder.Services.BindDbContext<CatalogDbContext>();
    //     // builder.Services.AddScoped<IDbInitializer, CatalogDbInitializer>();
    //     //
    //     // builder.Services.AddKeyedScoped<IRepository<Product>, CatalogRepository<Product>>("catalog:products");
    //     // builder.Services.AddKeyedScoped<IReadRepository<Product>, CatalogRepository<Product>>("catalog:products");
    //
    //     return builder;
    // }
    //
    // public static WebApplication UseCatalogModule(this WebApplication app)
    // {
    //     return app;
    // }
}