using Asp.Versioning;
using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Middleware;
using HamroFashion.Api.V1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HamroFashion.Api.V1.Endpoints
{
    /// <summary>
    /// Extensions that register minimal api mapping for our Products route
    /// </summary>
    public static class ProductEndpoint
    {
        /// <summary>
        /// Adds required services for endpoints in our products group to our DI container
        /// </summary>
        /// <param name="services">The service collection to add required services to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection AddProductEndpointV1(this IServiceCollection services)
        {
            services.AddScoped<ProductService>();
            return services;
        }

        public static WebApplication UseProductEndpointV1 (this WebApplication app)
        {
            var version = new ApiVersion(1, 0);

            var versionSet = app.NewApiVersionSet()
              .HasApiVersion(version)
              .ReportApiVersions()
              .Build();

            var group = app.MapGroup("/products")
                .WithApiVersionSet(versionSet)
                .AddEndpointFilter<ExceptionFilter>();

            group
                .MapPost("/create", CreateAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to create a new product" });

            group
                .MapPut("/update", UpdateAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to update a product" });

            group
                .MapPost("/{productId}/delete", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to delete a product" });

            group
                .MapPost("/{productId}", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to get a product by id" });

            group
                .MapPost("/", ListAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to get a filtered list of products" });

            return app;
        }

        public static async Task<IResult> CreateAsync([FromBody] CreateProduct command, [FromServices] ProductService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.CreateAsync(command, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> UpdateAsync([FromBody] UpdateProduct command, [FromServices] ProductService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.UpdateAsync(command, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> DeleteAsync([FromRoute] Guid productId, [FromServices] ProductService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            await service.DeleteAsync(productId, CurrentUser, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> GetByIdAsync([FromRoute] Guid productId, [FromServices] ProductService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.GetByIdAsync(productId, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }
        public static async Task<IResult> ListAsync([FromBody] PagedQuery<ListProducts> query, [FromServices] ProductService service, CancellationToken cancellationToken)
        {
            var res = await service.ListAsync(query, cancellationToken);
            return Results.Ok(res);
        }
    }
}
