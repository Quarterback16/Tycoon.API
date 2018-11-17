using System;
using Microsoft.Practices.Unity.Configuration;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Wrappers;

namespace ProgramAssuranceTool.Infrastructure.Ioc.Unity
{
	/// <summary>
	/// Bootstrapper that automates Application configuration using Unity.
	/// </summary>
	public class UnityBootstrapper : Bootstrapper.Bootstrapper
	{
		/// <summary>
		/// Creates the Unity container provider.
		/// </summary>
		public override IContainerProvider CreateContainer()
		{
			var container = new UnityContainerProvider();

			var configuration = ConfigurationManagerWrapper.Current.GetSection<UnityConfigurationSection>( "unity" );

			if ( configuration == null )
			{
				throw new InvalidOperationException( "Could not find Unity configuration section in Web.config" );
			}

			return container.RegisterByConfiguration();
		}
	}
}