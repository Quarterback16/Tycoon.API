using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Succinctly.Startup))]
namespace Succinctly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
