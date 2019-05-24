using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using TestWebAppCoolName.Models;

[assembly: OwinStartupAttribute(typeof(TestWebAppCoolName.Startup))]
namespace TestWebAppCoolName
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

            var options = new DashboardOptions
            {
                AuthorizationFilters = new[]
                {
                    new AuthorizationFilter { Roles = Roles.Admin },
                    
                }
            };
            app.UseHangfireDashboard("/hangfire", options);
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate(() =>
                GetSite(), Cron.Hourly);
        }

        public async Task GetSite()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("http://www.vkurzu.cz");
                Console.WriteLine(result.StatusCode);
            }
        }
    }
}
