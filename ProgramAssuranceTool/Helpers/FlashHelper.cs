using System;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace ProgramAssuranceTool.Helpers
{
	public static class FlashHelper
	{
		/// <summary>
		///   Temp data hangs around for the request plus one
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="message"></param>
		public static void FlashInfo( this Controller controller, string message )
		{
			controller.TempData[ "info" ] = message;
		}

		public static void FlashWarning( this Controller controller, string message )
		{
			controller.TempData[ "warning" ] = message;
		}

		public static void FlashError( this Controller controller, string message )
		{
			controller.TempData[ "error" ] = message;
		}

		public static MvcHtmlString Flash( this HtmlHelper helper )
		{
			var message = "";
			var className = "";

			if ( helper.ViewContext.TempData[ "info" ] != null )
			{
				message = helper.ViewContext.TempData[ "info" ].ToString();
				className = "info";

				// clear others
				helper.ViewContext.TempData["warning"] = null;
				helper.ViewContext.TempData["error"] = null;
			}
			else if ( helper.ViewContext.TempData[ "warning" ] != null )
			{
				message = helper.ViewContext.TempData[ "warning" ].ToString();
				className = "warning";

				// clear others
				helper.ViewContext.TempData["info"] = null;
				helper.ViewContext.TempData["error"] = null;
			}
			else if ( helper.ViewContext.TempData[ "error" ] != null )
			{
				message = helper.ViewContext.TempData[ "error" ].ToString();
				className = "error";

				// clear others
				helper.ViewContext.TempData["info"] = null;
				helper.ViewContext.TempData["warning"] = null;
			}

			var sb = new StringBuilder();
			if ( !String.IsNullOrEmpty( message ) )
			{
				sb.AppendLine("<script>");
				sb.AppendLine("$(document).ready(function() {");
				var showFlash = string.Format("pat.ShowFlash('{0}', '{1}')", HttpUtility.JavaScriptStringEncode(message), className);
				sb.AppendLine(showFlash);
				sb.AppendLine("});");
				sb.AppendLine("</script>");
			}
			return MvcHtmlString.Create( sb.ToString() );
		}
	}
}

