﻿using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Tycoon.Api.Utils;

namespace Tycoon.Api
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddSingleton<MessageBus>();
			services.AddHandlers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//else
			//	app.UseExceptionHandler();
			//app.UseMiddleware<ExceptionHandler>();  // ur own

			app.UseStatusCodePages();  // optional, will return a status code message
			app.UseMvc();

			//  to use convention-based routing NOT ATDVISED: use Attribute-Based Routing
			//app.UseMvc(
			//	config =>
			//	{
			//		config.MapRoute(
			//			name: "Default",
			//			template: "{controller}/{action}/{id?}",
			//			defaults: new
			//			{
			//				controller = "Home",
			//				action = "Index"
			//			});
			//	});

			app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Hello World! from Tycoon.API");
			});
		}
	}
}
