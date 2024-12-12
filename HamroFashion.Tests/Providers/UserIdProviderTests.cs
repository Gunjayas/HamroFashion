using HamroFashion.Api.V1.Providers;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamroFashion.Tests.Providers
{
    [TestFixture]
    public class UserIdProviderTests
    {
        private Guid userId = Guid.NewGuid();
        private IServiceProvider provider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IUserIdProvider, StaticUserIdProvider>(p => new StaticUserIdProvider(userId));

            provider = services.BuildServiceProvider();
        }

        [Test]
        public void CanGet_StaticUserId()
        {
            var userIdProvider = provider.GetRequiredService<IUserIdProvider>();

            Assert.That(userIdProvider.GetUserId(), Is.EqualTo(userId));
        }
    }
}
