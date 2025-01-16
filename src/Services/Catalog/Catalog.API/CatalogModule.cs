using Catalog.API.Features.v1.Products;
using Catalog.API.Features.v2.Products;

namespace Catalog.API;

public static class CatalogModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("catalog") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("products").WithTags("Product's API Group");
            productGroup.MapCreateProductEndpoint();
            productGroup.MapGetProductByIdEndpoint();
            productGroup.MapGetProductsEndpoint();
            productGroup.MapGetProductByCategoryEndpoint();
            productGroup.MapUpdateProductEndpoint();
            productGroup.MapDeleteProductEndpoint();
            // productGroup.MapExportProductsEndpoint();
            // productGroup.MapImportProductsEndpoint();
            
            productGroup.MapGetProductsV2Endpoint();

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
    
    // public static WebApplication UseCatalogModule(this WebApplication app)
    // {
    //     return app;
    // }
}
