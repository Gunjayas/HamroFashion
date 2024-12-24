using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Configurations;
using HamroFashion.Api.V1.Data.Entities;
using HamroFashion.Api.V1.Data.Options;
using HamroFashion.Api.V1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HamroFashion.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private AsyncServiceScope scope;
        private ServiceProvider provider;

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
            services.AddScoped<ProductService>();
            services.AddScoped<ImageService>();

            provider = services.BuildServiceProvider();
            scope = provider.CreateAsyncScope();

            // Seed roles
            var db = scope.ServiceProvider.GetRequiredService<HamroFashionContext>();
            db.Roles.AddAsync(new Role { Id = SeedData.UserRoleId, Name = "User" });
            db.SaveChangesAsync();
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
                Password = "stringst",
                UserName = "Test"
            }, default, skipEmail:true);

            var authResult = await tokenService.FromCredentials(new GetUser
            {
                EmailAddress = "test@user.com",
                Password = "stringst"
            }, default);

            await userService.DeleteByIdAsync(createdUser.Id, default);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(authResult, Is.Not.Null);
            Assert.That(authResult.User, Is.EqualTo(createdUser));
        }

        [Test]
        public async Task Can_Create()
        {
            var service = scope.ServiceProvider.GetRequiredService<UserService>();

            var createdUser = await service.CreateAsync(new CreateUser
            {
                EmailAddress = "test@user.com",
                Password = "Password123"
            }, default, skipEmail: true);

            await service.DeleteByIdAsync(createdUser.Id, default);

            Assert.That(createdUser, Is.Not.Null);
        }

        [Test]
        public async Task Can_GetById()
        {
            var service = scope.ServiceProvider.GetRequiredService<UserService>();

            var createdUser = await service.CreateAsync(new CreateUser
            {
                EmailAddress = "test@user.com",
                Password = "Password123"
            }, default, skipEmail: true);

            var gettedUser = await service.GetByIdAsync(createdUser.Id, default);

            await service.DeleteByIdAsync(createdUser.Id, default);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(gettedUser, Is.Not.Null);
            Assert.That(gettedUser, Is.EqualTo(createdUser));
        }

        [Test]
        public async Task Can_AddToCart()
        {
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var productService = scope.ServiceProvider.GetRequiredService<ProductService>();

            var createdProduct = await productService.CreateAsync(new CreateProduct
            {
                Name = "Product",
                Description = "this is product",
                Color = "blue",
                Size = "XL",
                Quantity = 3,
                Price = 600,
                Availability = true
            }, default);

            var createdUser = await userService.CreateAsync(new CreateUser
            {
                EmailAddress = "test@user.com",
                Password = "Password123"
            }, default, skipEmail:true);

            await userService.AddToCart(createdProduct.Id, createdUser.Id, default);

            var cart = await userService.GetCart(createdUser.Id, default);

            await userService.RemoveFromCart(createdProduct.Id, createdUser.Id, default);

            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdUser, Is.Not.Null);

            Assert.That(cart, Is.Not.Null);



            var cartItem = cart.CartItems.First();
            Assert.That(cart.TotalPrice, Is.EqualTo(createdProduct.Price * cartItem.Quantity));
            Assert.That(cartItem.ProductId, Is.EqualTo(createdProduct.Id));
            Assert.That(cartItem.Quantity, Is.EqualTo(3));
            Assert.That(cartItem.Price, Is.EqualTo(600));
        }
    }
}
