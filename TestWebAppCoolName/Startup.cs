using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestWebAppCoolName.Startup))]
namespace TestWebAppCoolName
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
