using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using Elmah;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
#if DEBUG
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using Employment.Web.Mvc.Infrastructure.Profilers;
#endif

namespace Employment.Web.Mvc.Infrastructure
{
    /// <summary>
    /// Defines a base class to manage application life cycle.
    /// </summary>
    /// <remarks>
    /// Multithreaded Singleton
    /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </remarks>
    public abstract class MvcApplication : HttpApplication
    {
        private static object syncLock = new object();
        private static volatile IBootstrapper bootstrapper;

        private IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        private IConfigurationManager ConfigurationManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        /// <summary>
        /// Whether MiniProfiler is enabled.
        /// </summary>
        public bool MiniProfilerEnabled = false;

        /// <summary>
        /// The key name used for storing the logged error id's in the user session.
        /// </summary>
        public static readonly KeyModel LoggedErrorIDKey = new KeyModel("ElmahLoggedErrorIDKey");

        /// <summary>
        /// Gets the bootstrapper.
        /// </summary>
        public IBootstrapper Bootstrapper
        {
            [DebuggerStepThrough]
            get
            {
                if (bootstrapper == null)
                {
                    lock (syncLock)
                    {
                        if (bootstrapper == null)
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
        [ExcludeFromCodeCoverage]
        public static void PreStart()
        {
            // Ensure this is only run once
            if (init)
            {
                return;
            }

            init = true;

            // Add reference to Area assemblies based on a naming convention
            AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache && a.FullName.StartsWith("Employment.Web.Mvc.Area.",StringComparison.Ordinal)).ToList().ForEach(BuildManager.AddReferencedAssembly);
        }

        /// <summary>
        /// Executes when the application starts.
        /// </summary>
        protected void Application_Start()
        {
#if DEBUG
            SetupMiniProfiler();
#endif

            Bootstrapper.Start();
            OnStart();
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
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Prevent the "ID3206: A SignInResponse message may only redirect within the current web application" error by ensuring the request contains a trailing slash
            if (string.Compare(Request.Path, Request.ApplicationPath, StringComparison.OrdinalIgnoreCase) == 0 && !(Request.Path.EndsWith("/",StringComparison.Ordinal)))
            {
                Response.Redirect(string.Format("{0}/", Request.Path));
            }

#if DEBUG
            if (bool.TryParse(ConfigurationManager.AppSettings.Get("EnableMiniProfiler"), out MiniProfilerEnabled) && MiniProfilerEnabled)
            {
                // MiniProfiler
                MiniProfiler.Start();
            }
#endif
        }

        /// <summary>
        /// Executes when the user has been authenticated.
        /// </summary>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
#if DEBUG
            if (MiniProfilerEnabled)
            {
                // Stop profiler and discard the results if the user does not have the appropriate access
                if (!AuthorizedForMiniProfiler())
                {
                    MiniProfiler.Stop(true);
                }
            }
#endif
        }

        /// <summary>
        /// Executes at the end of each request.
        /// </summary>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
#if DEBUG
            if (MiniProfilerEnabled)
            {
                // MiniProfiler
                MiniProfiler.Stop();
            }
#endif
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
        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();

            // Default to status code of 500 (Server Error)
            var statusCode = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

            // Log 500 internal server errors
            if (statusCode == 500)
            {
                ErrorSignal.FromCurrentContext().Raise(error);
            }

            // Handle error status code
            Context.HandleErrorStatusCode(statusCode);
        }

        /// <summary>
        /// Stores the logged error ID's for the current request in session for use on the Application Error error page.
        /// </summary>
        /// <remarks>
        /// Only stores logged error ID's for the current request in session when custom errors is enabled.
        /// </remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The error logged event arguments.</param>
        public void ErrorLog_Logged(object sender, ErrorLoggedEventArgs args)
        {
            if (!Context.IsCustomErrorEnabled || UserService == null)
            {
                return;
            }

            List<string> errorIDs;

            var userService = UserService;
            // Get logged error ID's for current request
            if (!userService.Session.TryGet(LoggedErrorIDKey, out errorIDs))
            {
                errorIDs = new List<string>();
            }

            // Add logged error ID
            if (!errorIDs.Contains(args.Entry.Id))
            {
                errorIDs.Add(args.Entry.Id);
            }

            // Update list of logged error ID's for current request
            userService.Session.Set(LoggedErrorIDKey, errorIDs);
        }

        /// <summary>Provides an application-wide implementation of the <see cref="P:System.Web.UI.PartialCachingAttribute.VaryByCustom" /> property.</summary>
        /// <returns>If the value of the <paramref name="custom" /> parameter is "browser", the browser's <see cref="P:System.Web.Configuration.HttpCapabilitiesBase.Type" />; otherwise, null.</returns>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that contains information about the current Web request. </param>
        /// <param name="custom">The custom string that specifies which cached response is used to respond to the current request. </param>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var userService = UserService;
            if (!string.IsNullOrEmpty(custom) && custom.Equals("user", StringComparison.OrdinalIgnoreCase) && userService != null)
            {
                return string.Format("{0}.{1}.{2}", userService.Username, userService.OrganisationCode, userService.SiteCode);
            }

            return base.GetVaryByCustomString(context, custom);
        }

        private bool AuthorizedForMiniProfiler()
        {
            var users = new [] {"XX1234", "SS0920", "IH0093", "VV2520", "NH0221", "DF0262", "DG0356", "VC0151", "NS0280", "BS2557", "DM0874", "AS0547" };

            return !(UserService == null || string.IsNullOrEmpty(UserService.Username) || !users.Any(u => UserService.Username.StartsWith(u, StringComparison.OrdinalIgnoreCase)));
        }

#if DEBUG
        private void SetupMiniProfiler()
        {
            // Setup MiniProfiler settings
            var ignored = MiniProfiler.Settings.IgnoredPaths.ToList();

            ignored.Add("asset.axd");

            MiniProfiler.Settings.IgnoredPaths = ignored.ToArray();
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
            MiniProfiler.Settings.Results_Authorize = request => AuthorizedForMiniProfiler();
            MiniProfiler.Settings.Results_List_Authorize = request => AuthorizedForMiniProfiler();
            MiniProfiler.Settings.ShowControls = true;
            //MiniProfiler.Settings.UseExistingjQuery = true;
            MiniProfiler.Settings.TrivialDurationThresholdMilliseconds=0;
            MiniProfiler.Settings.PopupMaxTracesToShow = 6;
            MiniProfiler.Settings.PopupShowTimeWithChildren = true;
            MiniProfiler.Settings.PopupShowTrivial = false;


            // Store results based on username
            WebRequestProfilerProvider.Settings.UserProvider = new UsernameUserProfiler();

            // Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }
#endif
    }
}
