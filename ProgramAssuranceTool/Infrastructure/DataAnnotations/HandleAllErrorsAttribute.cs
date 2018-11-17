using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Extensions;

namespace ProgramAssuranceTool.Infrastructure.DataAnnotations
{
	/// <summary>
	/// Represents an attribute that is used to handle all errors.
	/// </summary>
	[ExcludeFromCodeCoverage]
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
	public class HandleAllErrorsAttribute : ActionFilterAttribute, IExceptionFilter
	{
		/// <summary>
		/// Called after the action method executes to handle a result with an error status code.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filterContext" /> is <c>null</c>.</exception>
		public override void OnActionExecuted( ActionExecutedContext filterContext )
		{
			if ( filterContext == null )
			{
				throw new ArgumentNullException( "filterContext" );
			}

			var result = filterContext.Result as HttpStatusCodeResult;

			if ( result != null )
			{
				filterContext.ExceptionHandled = filterContext.HttpContext.HandleErrorStatusCode( result.StatusCode, result.StatusDescription );
			}
		}

		/// <summary>
		/// Called when an unhandled exception occurs within an action method.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filterContext" /> is <c>null</c>.</exception>
		public void OnException( ExceptionContext filterContext )
		{
			if ( filterContext == null )
			{
				throw new ArgumentNullException( "filterContext" );
			}

			// Ignore if:
			// - is a child action
			// - exception was already handled
			// - not a 500 internal server error
			if ( filterContext.IsChildAction || filterContext.ExceptionHandled || new HttpException( null, filterContext.Exception ).GetHttpCode() != 500 )
			{
				return;
			}

			// Log 500 internal server error
			Elmah.ErrorSignal.FromCurrentContext().Raise( filterContext.Exception );

			// Handle error status code
			filterContext.ExceptionHandled = filterContext.HttpContext.HandleErrorStatusCode( 500 );
		}
	}
}