using Asp.Versioning;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static HamroFashion.Api.V1.Data.HamroFashionContext;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using HamroFashion.Api.V1.Extensions;
using HamroFashion.Api.V1.Endpoints;
using HamroFashion.Api.V1.Utils;
using HamroFashion.Api.V1.Services;
namespace HamroFashion.Api
{
    /// <summary>
    /// Our primary application object
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Our AppSettings and environment variable configuration values
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var builder = CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            ConfigureApplication(app, builder.Environment);

            app.Run();
        }

        protected static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Configuration = builder.Configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            Mailer.Configure(builder.Configuration);

            builder.Host.UseSerilog();
            return builder;
        }

        protected static async void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            //Versioning
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine
                (
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                );
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, HamroFashionJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireClaim("userRoles", "Admin"));

                options.AddPolicy("AtleastUser",
                    policy => policy.RequireClaim("userRoles", "User"));

                options.AddPolicy("CanPost",
                    policy => policy.Requirements.Add(new CanPostPermissionRequirement()));

                options.AddPolicy("CanOrder",
                    policy => policy.Requirements.Add(new CanOrderPermissionRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, CanPostPermissionRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, CanOrderPermissionRequirementHandler>();

            var hamroFashionConnectionString = config.GetConnectionString(nameof(HamroFashionContext));
            var dbOptions = new DbContextOptionsBuilder<HamroFashionContext>();
            dbOptions.UseSqlServer(hamroFashionConnectionString);

            //Pipelines and globals
            services
                .AddDbContext<HamroFashionContext>(options => options.UseSqlServer(hamroFashionConnectionString))
                .AddTransient(x => new TransientHamroFashionContext(dbOptions.Options))
                .DiscoverValidators()
                .AddEndpointsApiExplorer()
                .AddOpenApi();

            services.AddHostedService<MailerBackgroundService>();

            //Endpoints and related services
            services
                .Configure<JsonOptions>(options =>
                {
                    options.SerializerOptions.PropertyNameCaseInsensitive = true;
                    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
                .AddProductEndpointV1()
                .AddAdminEndpointV1()
                .AddUserEndpointV1()
                .AddTokenEndpointV1();
            
            var provider = services.BuildServiceProvider();
            var db = provider.GetRequiredService<HamroFashionContext>();
            try
            {
                db.Database.EnsureCreated();
                await db.SeedDatabaseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Configure our application
        /// </summary>
        /// <param name="app">The application to configure</param>
        /// <param name="environment">The hosting environment</param>
        protected static void ConfigureApplication(WebApplication app, IWebHostEnvironment environment)
        {
            app
                .UseCorsFilter()
                .UseTokenEndpointV1()
                .UseUserEndpointV1()
                .UseProductEndpointV1()
                .UseAdminEndpointV1()
                .UseOpenApi();
        }
    }
}
