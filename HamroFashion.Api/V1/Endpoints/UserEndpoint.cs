using Asp.Versioning;
using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Extensions;
using HamroFashion.Api.V1.Middleware;
using HamroFashion.Api.V1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HamroFashion.Api.V1.Endpoints
{
    /// <summary>
    /// Extensions that register minimal api mapping for our User route
    /// </summary>
    public static class UserEndpoint
    {
        /// <summary>
        /// Adds required services for endpoints in our users group to our DI container
        /// </summary>
        /// <param name="services">The service collection to add required services to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection AddUserEndpointV1(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            return services;
        }

        public static WebApplication UseUserEndpointV1(this WebApplication app)
        {
            var version = new ApiVersion(1, 0);

            var versionSet = app.NewApiVersionSet()
              .HasApiVersion(version)
              .ReportApiVersions()
              .Build();

            var group = app.MapGroup("/user")
                .WithApiVersionSet(versionSet)
                .AddEndpointFilter<ExceptionFilter>();

            group
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Product", Description = "Attempts to create a new product" });

            group
                .MapPut("/changepassword", ChangePasswordAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Change Password", Description = "Attempts to change password" });

            group
                .MapPost("/{userId}/userdetail", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get User By Id", Description = "Attempts to get a user by id" });

            group
                .MapPost("/wishlist/{productId}/add", AddWishListProduct)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Add wishlist product", Description = "Attempts to add product to wishlist" });

            group
                .MapPost("/wishlist/{productId}/remove", RemoveWishListProduct)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Remove wishlist product", Description = "Attempts to remove product from wishlist" });
            group
                .MapGet("/cart", GetCart)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get Cart of the Current logged in user", Description = "Attempts to get a cart of currently logged in user" });

            group
                .MapPost("/cart/{productId}/add", AddToCart)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Add cart product", Description = "Attempts to add product to cart" });

            group
                .MapPost("/cart/{productId}/decreaseitem", DecreaseCartQuantity)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "decrease cart item quantity", Description = "Attempts to decrease cart item quantity" });

            group
                .MapPost("/cart/{productId}/remove", RemoveFromCart)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Remove cart product", Description = "Attempts to remove product from cart" });

            group
                .MapPost("/order/create", CreateOrder)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Create Order" , Description = "Attempts to create order" });

            group
                .MapGet("/order/{orderId}", GetOrderById)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get Order By Id", Description = "Attempts to get specific order" });

            group
                .MapGet("/order/all", GetAllOrders)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Get All Order", Description = "Attempts to get all user's orders" });

            return app;
        }

        public static async Task<IResult> GetAllOrders([FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.GetOrders(CurrentUser, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> GetOrderById([FromRoute] Guid orderId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.GetOrderDetails(orderId, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> CreateOrder([FromBody] PlaceOrder command, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.PlaceOrder(command, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }
        public static async Task<IResult> CreateAsync([FromBody] CreateUser command, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(res);
        }

        /*public static async Task<IResult> UpdateAsync([FromBody] UpdateProduct command, [FromServices] ProductService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.UpdateAsync(command, CurrentUser, cancellationToken);
            return Results.Ok(res);
        }*/

        public static async Task<IResult> ChangePasswordAsync([FromBody] ChangePassword command, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            await service.ChangePasswordAsync(command, CurrentUser, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> GetByIdAsync([FromRoute] Guid userId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var res = await service.GetByIdAsync(userId, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> AddWishListProduct([FromRoute] Guid productId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            await service.AddWishListProduct(productId, CurrentUser, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> RemoveWishListProduct([FromRoute] Guid productId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            await service.RemoveWishListProduct(productId, CurrentUser, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> GetCart([FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();
            var res = await service.GetCart(userId, cancellationToken);
            return Results.Ok(res);
        }

        public static async Task<IResult> AddToCart([FromRoute] Guid productId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();
            await service.AddToCart(productId, userId, cancellationToken);
            return Results.Ok();
        }

        public static async Task<IResult> RemoveFromCart([FromRoute] Guid productId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();
            await service.RemoveFromCart(productId, userId, cancellationToken);
            return Results.Ok();
        }
        public static async Task<IResult> DecreaseCartQuantity([FromRoute] Guid productId, [FromServices] UserService service, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            await service.DecreaseCartQuantity(productId, CurrentUser, cancellationToken);
            return Results.Ok();
        }
    }
}
