using System.Web.Mvc;
using System.Web.Optimization;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Registrations
{
	/// <summary>
	/// Represents a registration that is used to register bundle.
	/// </summary>
	public class BundleRegistration : IRegistration
	{
		/// <summary>
		/// Build manager.
		/// </summary>
		protected IBuildManager BuildManager
		{
			get
			{
				var containerProvider = DependencyResolver.Current as IContainerProvider;

				return ( containerProvider != null ) ? containerProvider.GetService<IBuildManager>() : null;
			}
		}

		/// <summary>
		/// Configuration manager.
		/// </summary>
		protected IConfigurationManager ConfigurationManager
		{
			get
			{
				var containerProvider = DependencyResolver.Current as IContainerProvider;

				return ( containerProvider != null ) ? containerProvider.GetService<IConfigurationManager>() : null;
			}
		}

		/// <summary>
		/// Register bundle.
		/// </summary>
		public void Register()
		{
			bool enable;
			if (bool.TryParse( ConfigurationManager.AppSettings.Get( "EnableBundleOptimizations" ), out enable ))
				BundleTable.EnableOptimizations = enable;

			#region CSS

			//  main site.css
		    BundleTable.Bundles.Add(new StyleBundle("~/Content/css")
					  .Include( "~/Content/CSS/font-awesome.css" )  //  Font Awesome
					  .Include( "~/Content/CSS/screen.css" )  //  Blueprint
					  .Include( "~/Content/CSS/print.css" )  //  Used for some fonts
					  .Include( "~/Content/CSS/plugins/buttons/screen.css" )
                 //.Include("~/Content/liquid.css")
                 //.Include("~/Content/CSS/jquery-ui-1.10.0.custom.min.css")
                 .Include("~/Content/CSS/ui.jqgrid.css")
				     .Include("~/Content/site.css")
					  );


			 // Add styles - jquery ui
			 BundleTable.Bundles.Add( new StyleBundle( "~/Content/themes/base/css" )
					.Include( "~/Content/themes/base/jquery.ui.core.css" )
					.Include( "~/Content/themes/base/jquery.ui.resizable.css" )
					.Include( "~/Content/themes/base/jquery.ui.selectable.css" )
					.Include( "~/Content/themes/base/jquery.ui.accordion.css" )
					.Include( "~/Content/themes/base/jquery.ui.autocomplete.css" )
					.Include( "~/Content/themes/base/jquery.ui.button.css" )
					.Include( "~/Content/themes/base/jquery.ui.dialog.css" )
//					.Include( "~/Content/themes/base/jquery.ui.slider.css" )   //  Not used as of yet
					.Include( "~/Content/themes/base/jquery.ui.tabs.css" )
					.Include( "~/Content/themes/base/jquery.ui.datepicker.css" )
//					.Include( "~/Content/themes/base/jquery.ui.progressbar.css" )  //  Not used as of yet
					.Include( "~/Content/themes/base/jquery.ui.theme.css" )
					);

			 // Add styles - bootstrap 
			 BundleTable.Bundles.Add( new StyleBundle( "~/Content/css/bootstrap" )
				.Include( "~/Content/CSS/bootstrap.css" )
				);

			#endregion

			#region  Javascript

			// Add scripts
			BundleTable.Bundles.Add( new ScriptBundle( "~/bundles/jquery" )
				 .Include( "~/Scripts/jquery-{version}.js" ) );

			BundleTable.Bundles.Add( new ScriptBundle( "~/bundles/jqueryui" )
				 .Include( "~/Scripts/jquery-ui-{version}.js" ) );

            BundleTable.Bundles.Add( new ScriptBundle( "~/bundles/jqueryval" )
                 .Include( "~/Scripts/jquery.unobtrusive*" )
                 .Include( "~/Scripts/jquery.validate*" ) );

			BundleTable.Bundles.Add( new ScriptBundle( "~/bundles/modernizr" )
				 .Include( "~/Scripts/modernizr-*" ) );

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jqueryGrid")
                 .Include("~/Scripts/grid.locale-en.js")
                 .Include("~/Scripts/jquery.jqGrid.min.js")
                 .Include("~/Scripts/jquery-ui-1.10.0.custom.min.js")
                 .Include("~/Scripts/json2.js")
                 );

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/PAT")
					 .Include( "~/Scripts/PAT.js" )
                );

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap*"));

		    #endregion

		}
	}
}