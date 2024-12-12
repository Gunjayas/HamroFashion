using Asp.Versioning;
using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Middleware;
using HamroFashion.Api.V1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace HamroFashion.Api.V1.Endpoints
{
    /// <summary>
    /// Extensions that register minimal api mapping for our Admin service
    /// </summary>
    public static class AdminEndpoint
    {
        /// <summary>
        /// Adds required services for endpoints in our admin group to our DI container
        /// </summary>
        /// <param name="services">The service collection to add required services to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection AddAdminEndpointV1(this IServiceCollection services)
        {
            services.AddScoped<AdminService>();
            return services;
        }

        /// <summary>
        /// Configures our WebApplication to route HTTP/HTTPS traffic to our service methods
        /// </summary>
        /// <param name="app">The web application to configure</param>
        /// <returns>The web application (allows builder pattern)</returns>
        public static WebApplication UseAdminEndpointV1(this WebApplication app)
        {
            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/admin")
                .WithApiVersionSet(versionSet)
                .AddEndpointFilter<ExceptionFilter>();

            group
                .MapPost("/auth/credentials", FromCredentials)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Authenticate from credentials", Description = "Authenticate a user identity using credentials" });

            group
                .MapPost("/tag/create", CreateTagAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Tag", Description = "Attempts to create tag either category, collection or label" });

            group
                .MapPost("/tag/{tagId}remove", DeleteTagAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Delete Tag", Description = "Attempts to delete tag either category, collection or label" });

            group
                .MapGet("/tag/{tagType}", GetTagTypeAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get Tags by Type", Description = "Attempts to get tags either category, collection or label" });

            group
                .MapPut("/ban/{userId}", BanUserAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Ban User By Id", Description = "Attempts to ban user" });

            group
                .MapGet("/orders/all", GetOrdersAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get Orders", Description = "Attempts to get orders list of all user" });

            group
                .MapGet("/order/{orderId}", GetOrderByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get Order by Id", Description = "Attempts to get order by Id" });

            group
                .MapPut("/order/update", UpdateOrderAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Update Order", Description = "Attempts to update order" });

            return app;
        }

        public static async Task<IResult> UpdateOrderAsync([FromBody] UpdateOrder command, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            var res = await service.UpdateOrderAsync(command, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> GetOrderByIdAsync([FromRoute] Guid orderId, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            var res = await service.GetOrderById(orderId, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> GetOrdersAsync([FromServices] AdminService service, CancellationToken cancellationToken)
        {
            var res = await service.GetOrders(cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> BanUserAsync([FromRoute] Guid userId, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            await service.BanUser(userId, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> GetTagTypeAsync([FromRoute] string tagType, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            var res = await service.GetTagsByType(tagType, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> DeleteTagAsync([FromRoute] Guid tagId, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            await service.DeleteTagAsync(tagId, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> CreateTagAsync([FromBody] CreateTag command, [FromServices] AdminService service, CancellationToken cancellationToken)
        {
            await service.CreateTagAsync(command, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> FromCredentials([FromBody] GetUser command, [FromServices] TokenService service, CancellationToken cancellationToken)
        {
            var success = await service.FromCredentials(command, cancellationToken);
            return Results.Ok(success);
        }
    }
}
