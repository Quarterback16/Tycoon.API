using System;
using System.Linq;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.ViewEngines
{
    /// <summary>
    /// Represents a view engine that is used to render a Web page that uses the C# ASP.NET Razor syntax.
    /// </summary>
    public class CsRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.ViewEngines.CsRazorViewEngine" /> class.
        /// </summary>
        public CsRazorViewEngine()
        {
            const string extension = "cshtml";

            // Only include the cshtml type in location formats and file extensions
            AreaViewLocationFormats = AreaViewLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            AreaMasterLocationFormats = AreaMasterLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            AreaPartialViewLocationFormats = AreaPartialViewLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            ViewLocationFormats = ViewLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            MasterLocationFormats = MasterLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            PartialViewLocationFormats = PartialViewLocationFormats.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
            FileExtensions = FileExtensions.Where(s => s.EndsWith(extension,StringComparison.Ordinal)).ToArray();
        }
    }
}
