using JG.FinTechTest.Commands;
using JG.FinTechTest.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace JG.FinTechTest.Tests.Controllers
{
    public class TestStartup : Startup
    {
        protected override void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IAddDonationCommand, FakeAddDonationCommand>();
        }
    }
}