using JG.FinTechTest.Commands;
using JG.FinTechTest.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace JG.FinTechTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);

            services
                .AddMvc()
                .AddApplicationPart(typeof(GiftAidController).Assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        protected virtual void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IAddDonationCommand>(new AddDonationCommand());
        }
    }
}
