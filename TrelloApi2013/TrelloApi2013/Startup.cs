using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TrelloApi2013.Startup))]

namespace TrelloApi2013
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
