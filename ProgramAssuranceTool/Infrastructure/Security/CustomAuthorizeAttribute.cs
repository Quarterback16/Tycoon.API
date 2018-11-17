using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Infrastructure.Security
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
	public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		/// <summary>
		/// Role type depending on Environment
		/// </summary>
		public RoleType RoleTypes { get; set; }

		/// <summary>
		/// When overidden provides an entry point for custom authorization check.
		/// </summary>
		/// <param name="httpContext">Http Context.</param>
		/// <returns>Calls base method.</returns>
		protected override bool AuthorizeCore( HttpContextBase httpContext )
		{
			Roles = ConfigurationManager.AppSettings[ "Roles" ];
			//if (string.IsNullOrEmpty( Roles ))
			//	Roles =
			//		@"NATION\APP_PAAM, NATION\APP_PAAM_ADMIN, NATION\APP_PAAM_NO, NATION\APP_PAAM_QLD, NATION\APP_PAAM_NSW, NATION\APP_PAAM_NT, NATION\APP_PAAM_SA, NATION\APP_PAAM_TAS, NATION\APP_PAAM_VIC, NATION\APP_PAAM_WA";

			return base.AuthorizeCore( httpContext );
		}

	}

	/// <summary>
	/// Enum for Role Types.
	/// </summary>
	public enum RoleType
	{
		TestRoles,

		DevRoles
	}


}