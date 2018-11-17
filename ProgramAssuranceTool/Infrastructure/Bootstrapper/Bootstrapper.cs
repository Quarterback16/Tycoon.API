using System.Diagnostics;
using System.Linq;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Extensions;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Infrastructure.Bootstrapper
{
	/// <summary>
	/// Bootstrapper that automates Application configuration in Global.asax.
	/// </summary>
	public abstract class Bootstrapper : IBootstrapper
	{
		private readonly object syncLock = new object();
		private static IContainerProvider container;

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>The container.</value>
		public IContainerProvider Container
		{
			[DebuggerStepThrough]
			get
			{
				if ( container == null )
				{
					lock ( syncLock )
					{
						if ( container == null )
						{
							container = CreateContainer();

							DependencyResolver.SetResolver( container );
						}
					}
				}

				return container;
			}
		}

		/// <summary>
		/// Creates the container provider.
		/// </summary>
		/// <returns>The container provider.</returns>
		public abstract IContainerProvider CreateContainer();

		/// <summary>
		/// Bootstrapper start process to be called in Application_Start().
		/// </summary>
		public void Start()
		{
			// Resolve all Registrations, based on Order attributes position in sequence value, grouped so first in sequence are first
			var registrationGroups = Container.GetServices<IRegistration>().OrderBy( r => r.Order() ).GroupBy( r => r.Group() ).ToList();

			foreach ( var registrationGroup in registrationGroups.OrderBy( r => r.Key ) )
			{
				// Perform each registration, eg Routes, Bundles
				registrationGroup.ForEach( r => r.Register() );
			}
		}

		/// <summary>
		/// Bootstrapper end process to be called in Application_End().
		/// </summary>
		public void End()
		{

		}

		/// <summary>
		/// Dispose of Bootstrapper.
		/// </summary>
		public void Dispose()
		{
			if (Container != null)
				Container.Dispose();
		}
	}
}