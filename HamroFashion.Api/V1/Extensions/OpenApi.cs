using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// OpenApi helper extensions
    /// </summary>
    public static class OpenApi
    {

        /// <summary>
        /// Registers open API with our DI container
        /// </summary>
        /// <param name="services">The service collection to add open api to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    const string jwtAuthName = "JWT Access Token";

                    opt.AddSecurityDefinition(jwtAuthName, new OpenApiSecurityScheme
                    {
                        Description = $"{JwtBearerDefaults.AuthenticationScheme} in {HeaderNames.Authorization} header",
                        Name = HeaderNames.Authorization,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = jwtAuthName
                                }
                            },
                            new List<string>()
                        }
                    });
                });

            return services;
        }

        /// <summary>
        /// Configures our WebApplication to serve OpenAPI content
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseOpenApi(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) return app;

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
