using Asp.Versioning;
using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Middleware;
using HamroFashion.Api.V1.Services;
using Microsoft.AspNetCore.Mvc;

namespace HamroFashion.Api.V1.Endpoints
{
    /// <summary>
    /// Extensions that register minimal api mapping for our Token service
    /// </summary>
    public static class TokenEndpoint
    {
        /// <summary>
        /// Adds required services for endpoints in our token group to our DI container
        /// </summary>
        /// <param name="services">The service collection to add required services to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection AddTokenEndpointV1(this IServiceCollection services)
        {
            services.AddScoped<TokenService>();
            return services;
        }

        /// <summary>
        /// Configures our WebApplication to route HTTP/HTTPS traffic to our service methods
        /// </summary>
        /// <param name="app">The web application to configure</param>
        /// <returns>The web application (allows builder pattern)</returns>
        public static WebApplication UseTokenEndpointV1(this WebApplication app)
        {
            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/token")
                .WithApiVersionSet(versionSet)
                .AddEndpointFilter<ExceptionFilter>();

            group
                .MapPost("/auth/credentials", FromCredentials)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { OperationId = "Authenticate from credentials", Description = "Authenticate a user identity using credentials" });

            return app;
        }

        public static async Task<IResult> FromCredentials([FromBody] GetUser command, [FromServices] TokenService service, CancellationToken cancellationToken)
        {
            var success = await service.FromCredentials(command, cancellationToken);
            return Results.Ok(success);
        }
    }
}
