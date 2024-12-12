using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Options;
using HamroFashion.Api.V1.Providers;
using HamroFashion.Api.V1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamroFashion.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private AsyncServiceScope scope;
        private ServiceProvider provider;
        private UserService _userService;
        private TokenService _tokenService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Test.json")
                .Build();

            services
                .AddSingleton<IConfiguration>(p => configuration)
                .Configure<JwtOptions>(configuration.GetSection("Jwt"));

            var hamrofashionDbName = configuration.GetConnectionString("HamroFashionContext") ??
                throw new ArgumentNullException("Missing HamroFashionContext app setting");

            services
                .AddDbContext<HamroFashionContext>(x =>
                    x.UseInMemoryDatabase(hamrofashionDbName)
                );

            //Register service
            services.AddScoped<UserService>();
            services.AddScoped<TokenService>();

            provider = services.BuildServiceProvider();
            scope = provider.CreateAsyncScope();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await scope.DisposeAsync();
            await provider.DisposeAsync();
        }

        [Test]
        public async Task Can_AuthFromCredentials()
        {
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();

            var createdUser = await userService.CreateAsync(new CreateUser
            {
                EmailAddress = "test@user.com",
                Password = "Password123",
                UserName = "Test"
            }, default);

            var authResult = await tokenService.FromCredentials(new GetUser
            {
                EmailAddress = "test@user.com",
                Password = "Password123"
            }, default);

            
        }
    }
}
