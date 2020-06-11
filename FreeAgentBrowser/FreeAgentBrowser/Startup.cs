using FreeAgentBrowser.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FreeAgentBrowser
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. 
		// Use this method to add services to the container.
		// For more information on how to configure your application, 
		// visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(
			IServiceCollection services)
		{
			//  built in dependency injection here
			services.AddDbContext<AppDbContext>(
				options => options.UseSqlServer(
					Configuration.GetConnectionString(
						"DefaultConnection")));
			//  Framework services
			services.AddControllersWithViews();
			//  Custom services
			services.AddScoped<
				IPlayerRepository, 
				PlayerRepository>();
			services.AddScoped<
				IPositionRepository, 
				PositionRepository>();
		}

		// This method gets called by the runtime. Use this method to configure 
		// the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app, 
			IWebHostEnvironment env)
		{
			//  add middleware components here, order is important

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//app.UseHttpsRedirection();  //  to use HTTPS
			//app.UseStatusCodePages();  //  support for text only headers
			app.UseStaticFiles();      //  to serve static files from wwwroot
			app.UseRouting();

			app.UseEndpoints(
				endpoints =>
				{
					endpoints.MapControllerRoute(
						name: "default", 
						pattern: "{controller=Home}/{action=Index}/{id?}");
				});
		}
	}
}
