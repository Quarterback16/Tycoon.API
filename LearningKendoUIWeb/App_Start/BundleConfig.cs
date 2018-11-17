using System.Web;
using System.Web.Optimization;

namespace LearningKendoUIWeb
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles( BundleCollection bundles )
      {
         // Enable CDN
         bundles.UseCdn = true;

         // CDN paths for kendo stylesheet files
         var kendoCommonCssPath = "http://cdn.kendostatic.com/2013.1.319/styles/kendo.common.min.css";
         var kendoDefaultCssPath = "http://cdn.kendostatic.com/2013.1.319/styles/kendo.default.min.css";
         // CDN paths for kendo javascript files
         var kendoWebJsPath = "http://cdn.kendostatic.com/2012.2.710/js/kendo.web.min.js";

         bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
                     "~/Scripts/jquery-{version}.js" ) );

         bundles.Add( new ScriptBundle( "~/bundles/jqueryui" ).Include(
                     "~/Scripts/jquery-ui-{version}.js" ) );

         bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
                     "~/Scripts/jquery.unobtrusive*",
                     "~/Scripts/jquery.validate*" ) );

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
                     "~/Scripts/modernizr-*" ) );

         bundles.Add( new StyleBundle( "~/Content/css" ).Include( "~/Content/site.css" ) );

         bundles.Add( new StyleBundle( "~/Content/themes/base/css" ).Include(
                     "~/Content/themes/base/jquery.ui.core.css",
                     "~/Content/themes/base/jquery.ui.resizable.css",
                     "~/Content/themes/base/jquery.ui.selectable.css",
                     "~/Content/themes/base/jquery.ui.accordion.css",
                     "~/Content/themes/base/jquery.ui.autocomplete.css",
                     "~/Content/themes/base/jquery.ui.button.css",
                     "~/Content/themes/base/jquery.ui.dialog.css",
                     "~/Content/themes/base/jquery.ui.slider.css",
                     "~/Content/themes/base/jquery.ui.tabs.css",
                     "~/Content/themes/base/jquery.ui.datepicker.css",
                     "~/Content/themes/base/jquery.ui.progressbar.css",
                     "~/Content/themes/base/jquery.ui.theme.css" ) );

         // Create the CDN bundles for kendo javascript files
         bundles.Add( new ScriptBundle("~/bundles/kendo/web/js", kendoWebJsPath )
            .Include("~/Scripts/kendo/kendo.web.js"));
         // The ASP.NET MVC script file is not available from the Kendo Static CDN,
         // so we will include the bundle reference without the CDN path.
         bundles.Add(new ScriptBundle("~/bundles/kendo/mvc/js")
            .Include("~/Scripts/kendo/kendo.aspnetmvc.js"));
         // Create the CDN bundles for the kendo styleshseet files
         bundles.Add( new StyleBundle("~/bundles/kendo/common/css", kendoCommonCssPath )
            .Include("~/Content/kendo/kendo.common.css"));
         bundles.Add(new StyleBundle("~/bundles/kendo/default/css", kendoDefaultCssPath )
            .Include("~/Content/kendo/kendo.default.css"));

      }
   }
}