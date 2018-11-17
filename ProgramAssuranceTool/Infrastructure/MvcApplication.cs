using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Diagnostics;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Extensions;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using Elmah;
using System.Web.Compilation;  // BuildManager


namespace ProgramAssuranceTool.Infrustructure
{
	/// <summary>
	/// Defines a base class to manage application life cycle.
	/// </summary>
	/// <remarks>
	/// Multithreaded Singleton
	/// http://msdn.microsoft.com/en-us/library/ff650316.aspx
	/// </remarks>
	/// 
	public abstract class MvcApplication : HttpApplication
	{
		private static readonly object syncLock = new object();
		private static volatile IBootstrapper bootstrapper;

		/// <summary>
		/// Gets the bootstrapper.
		/// </summary>
		public IBootstrapper Bootstrapper
		{
			[DebuggerStepThrough]
			get
			{
				if ( bootstrapper == null )
				{
					lock ( syncLock )
					{
						if ( bootstrapper == null )
						{
							bootstrapper = CreateBootstrapper();
						}
					}
				}

				return bootstrapper;
			}
		}

		/// <summary>
		/// Gets the container.
		/// </summary>
		public IContainerProvider Container
		{
			[DebuggerStepThrough]
			get
			{
				return Bootstrapper.Container;
			}
		}

		/// <summary>
		/// Creates the bootstrapper.
		/// </summary>
		protected abstract IBootstrapper CreateBootstrapper();

		private static bool init;

		/// <summary>
		/// Executes before Appliation_Start() via [PreApplicationStartMethod] attribute in AssemblyInfo.
		/// </summary>
		public static void PreStart()
		{
			// Ensure this is only run once
			if ( init )
			{
				return;
			}

			init = true;

			// Add reference to Area assemblies based on a naming convention
			AppDomain.CurrentDomain.GetAssemblies().Where( a => !a.GlobalAssemblyCache && a.FullName.StartsWith( "Employment.Web.Mvc.Area." ) ).ToList().ForEach( BuildManager.AddReferencedAssembly );
		}

		/// <summary>
		/// Executes when the application starts.
		/// </summary>
		protected void Application_Start()
		{
			Bootstrapper.Start();
			AddTask("PatBatch", AppHelper.BatchJobFrequencyInSeconds);
			OnStart();
		}

		private void AddTask( string name, int seconds )
		{
			var onCacheRemove = new CacheItemRemovedCallback( CacheItemRemoved );
			HttpRuntime.Cache.Insert( name, seconds, null,
				 DateTime.Now.AddSeconds( seconds ), Cache.NoSlidingExpiration,
				 CacheItemPriority.NotRemovable, onCacheRemove );
		}

		public void CacheItemRemoved( string taskName, object v, CacheItemRemovedReason r )
		{
			// do stuff here if it matches our taskname, like WebRequest

			if ( taskName.Equals( "PatBatch" ) && !AppHelper.IsLocalWorkstation )
				BatchJobs.DoJob( taskName, "AUTO" );

			// re-add our task so it recurs
			AddTask( taskName, Convert.ToInt32( v ) );
		}

		/// <summary>
		/// Executes when the application ends.
		/// </summary>
		protected void Application_End()
		{
			OnEnd();
			Bootstrapper.End();
		}

		/// <summary>
		/// Executes when the application starts. Override if additional start tasks are required.
		/// </summary>
		protected virtual void OnStart()
		{
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add( new RazorViewEngine() );
		}

		/// <summary>
		/// Executes when the application ends. Override if additional end tasks are required.
		/// </summary>
		protected virtual void OnEnd()
		{

		}

		/// <summary>
		/// Executes at the beginning of each request.
		/// </summary>
		protected void Application_BeginRequest( object sender, EventArgs e )
		{
			// Prevent the "ID3206: A SignInResponse message may only redirect within the current web application" error by ensuring the request contains a trailing slash
			if ( string.Compare( Request.Path, Request.ApplicationPath, StringComparison.InvariantCultureIgnoreCase ) == 0 && !( Request.Path.EndsWith( "/" ) ) )
			{
				Response.Redirect( Request.Path + "/" );
			}
		}


		/// <summary>
		/// Handle application error events that occur outside of the MVC process.
		/// </summary>
		/// <remarks>
		/// Due to the way MVC works, application errors outside of the MVC process must be handled here.
		/// For example, if a URL does not match any MVC route then MVC will not handle it so that is taken care of here.
		/// </remarks>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		protected void Application_Error( object sender, EventArgs e )
		{
			var error = Server.GetLastError();

			// Default to status code of 500 (Server Error)
			var statusCode = ( error is HttpException ) ? ( error as HttpException ).GetHttpCode() : 500;

			// Log 500 internal server errors
			if (statusCode == 500)
				ErrorSignal.FromCurrentContext().Raise( error );

			// Handle error status code
			Context.HandleErrorStatusCode( statusCode );
		}

	}
}