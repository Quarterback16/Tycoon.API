using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using TrelloApi.Models;

namespace TrelloApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //  dependency injection registration
            services.AddSingleton<IBoardRepository, BoardRepository>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
         // Add MVC to the request pipeline.
            app.UseMvc();
         //app.UseMvc(routes =>
         //   {
         //      routes.MapRoute(
         //          name: "default",
         //          template: "{controller}/{action}/{id?}",
         //          defaults: new { controller = "Home", action = "Index" });
         //   });
      }
    }
}
