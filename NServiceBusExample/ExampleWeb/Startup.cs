using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExampleWeb.Startup))]
namespace ExampleWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
