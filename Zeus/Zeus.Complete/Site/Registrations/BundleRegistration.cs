using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using Employment.Web.Mvc.Infrastructure.Interfaces;


namespace Employment.Web.Mvc.Zeus.Registrations
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

                return (containerProvider != null) ? containerProvider.GetService<IBuildManager>() : null;
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

                return (containerProvider != null) ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        /// <summary>
        /// Register bundle.
        /// </summary>
        public void Register()
        {
            bool enable;
            if (bool.TryParse(ConfigurationManager.AppSettings.Get("EnableBundleOptimizations"), out enable))
            {
                BundleTable.EnableOptimizations = enable;
            }

            // Add styles
            BundleTable.Bundles.Add(new StyleBundle("~/content/1")
                .Include("~/Content/fonts.css",
                         "~/Content/Themes/base/jquery-ui.css",
                         "~/Content/bootstrap.css",          
                         "~/Content/font-awesome.css",
                         "~/Content/animate.css",
                         "~/Content/style.css",
                         "~/Content/style-responsive.css",
                         "~/Content/theme/default.css"
                )
                 
            //);

            //BundleTable.Bundles.Add(new StyleBundle("~/content/2")
                .Include("~/Content/Site.css")

                .Include("~/Content/bootstrap-datepicker.css",
                         "~/Content/bootstrap-datepicker3.css",
                         "~/Content/bootstrap-datepaginator.css",
                         //"~/Content/bootstrapper-datetimepicker.css",
                         //"~/Content/bootstrap-timepicker.css",
                         "~/Content/jquery.timepicker.css",
                         "~/Content/jquery-jvectormap-1.2.2.css",
                         "~/Content/jquery.gritter.css")

                .Include("~/Content/select2.css",
                         "~/Content/select2-bootstrap.css")
                         
                         //"~/Content/jquery-timepicker.css")

                .Include("~/Content/foo/footable.core.css",
                         "~/Content/foo/footable.standalone.css")
                         

                //.Include("~/Content/print.css")
                //.Include("~/Content/screen.css")
                //.Include("~/Content/edge.css")
                //.Include("~/Content/CustomSmartAutocomplete.css")// smartAutocomplete
                
                );

            BundleTable.Bundles.Add(new StyleBundle("~/content/calendar").Include("~/Content/fullcalendar.css"));

            //BundleTable.Bundles.Add(new StyleBundle("~/content/ie8")
            //    .Include("~/Content/excanvas.min.css")
            //    );

            BundleTable.Bundles.Add(new StyleBundle("~/content/highcontrast")
               // .Include("~/Content/highcontrast.css")
               );


            // Add scripts
            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/1")
                //.Include("~/Scripts/jquery-1.11.1.min.js")
                //.Include("~/Scripts/jquery-ui-1.11.1.js")
                //.Include("~/Scripts/jquery-1.10.2.js")
                //.Include("~/Scripts/jquery-1.7.2.js",
                .Include("~/Scripts/jquery-1.8.2.js",
                    "~/Scripts/jquery-ui.min.js",
                    "~/Scripts/bootstrap.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/calendar").Include("~/Scripts/fullcalendar.js"));


            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/2")
                .Include("~/Scripts/html5shiv.js",
                         "~/Scripts/respond.min.js",
                         "~/Scripts/excanvas.min.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/scripts/3")
                .Include("~/Scripts/jquery.validate.js",
                         "~/Scripts/jquery.validate.unobtrusive.js",
                         "~/Scripts/jquery-auto-numeric-1.7.5.js",
                         "~/Scripts/jquery-textrange.js",
                         "~/Scripts/equalize.js",
                         "~/Scripts/jquery-blockUI.js")


                
                .Include("~/Scripts/date.js",
                         "~/Scripts/zeus-*",
                         "~/Scripts/zeus.validate-*",
                         "~/Scripts/zeus.validate.unobtrusive-*",
                         "~/Scripts/interface.js",
                         "~/Scripts/sortTable.js",
                         //"~/Scripts/select2-3.4.0.js")
                         "~/Scripts/select2-3.5.1.js")
                
                // ColorAdmin theme
                .Include(
                         "~/Scripts/jquery.slimscroll.js",
                         "~/Scripts/jquery.gritter.js",
                         "~/Scripts/jquery.flot.js",
                         "~/Scripts/jquery.flot.pie.js",
                         "~/Scripts/jquery.flot.resize.js",
                         "~/Scripts/jquery.flot.time.js",
                         "~/Scripts/jquery.sparkline.js",
                         "~/Scripts/jquery-jvectormap-1.2.2.min.js")
                //.Include("~/Scripts/jquery-jvectormap-world-mill-en.js")

                // Extra plugin for side by side bar graphs
                .Include("~/Scripts/jquery.flot.orderBars.js")

                //.Include("~/Scripts/moment.js")
                .Include("~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-maxlength.js",
                //"~/Scripts/bootstrap-timepicker.js",
                         "~/Scripts/jquery.timepicker.js",
                         "~/Scripts/moment.js",
                         "~/Scripts/bootstrap-datepaginator.js",
                         "~/Scripts/foo/footable.js",
                         "~/Scripts/foo/footable.sort.js",
                         "~/Scripts/knockout.min.js",
                         "~/Scripts/knockout.mapping.js",
                         "~/Scripts/apps.js")
                
                
                // smartAutocomplete
                //.Include("~/Scripts/smartautcompleteSelect.js")

                );
            
            // Include area styles and scripts
            var areaStyleBundle = new StyleBundle("~/content/areas");
            var areaScriptBundle = new ScriptBundle("~/scripts/areas");
            const string areaNamespace = "Employment.Web.Mvc.Area.";
            IEnumerable<string> areas = BuildManager.Assemblies.Where(a => a.FullName.StartsWith(areaNamespace,StringComparison.Ordinal)).Select(a => a.FullName.Substring(areaNamespace.Length, a.FullName.IndexOf(',', areaNamespace.Length) - areaNamespace.Length));

            foreach (string area in areas)
            {
                // Include area styles
                var stylesVirtualPath = string.Format("~/Areas/{0}/Content", area);

                if (BundleTable.VirtualPathProvider.DirectoryExists(stylesVirtualPath))
                {
                    areaStyleBundle.IncludeDirectory(stylesVirtualPath, "*.css");
                }

                // Include area scripts
                var scriptsVirtualPath = string.Format("~/Areas/{0}/Scripts", area);

                if (BundleTable.VirtualPathProvider.DirectoryExists(scriptsVirtualPath))
                {
                    areaScriptBundle.IncludeDirectory(scriptsVirtualPath, "*.js");
                }
            }

            BundleTable.Bundles.Add(areaStyleBundle);
            BundleTable.Bundles.Add(areaScriptBundle);
        }
    }
}