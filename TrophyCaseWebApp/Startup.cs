using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrophyCaseWebApp.Startup))]
namespace TrophyCaseWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
