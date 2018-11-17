using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Types;

namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines the methods and properties that are required for an Inversion of Control container.
	/// </summary>
	public interface IContainerProvider : IDependencyResolver, IDisposable, IFluent
	{
		/// <summary>
		/// Register type mappings with container by configuration.
		/// </summary>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterByConfiguration();

		/// <summary>
		/// Register the type mapping with the container, with the default <see cref="LifetimeType" />.
		/// </summary>
		/// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
		/// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterType( Type serviceType, Type implementationType );

		/// <summary>
		/// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
		/// </summary>
		/// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
		/// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
		/// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterType( Type serviceType, Type implementationType, LifetimeType lifetime );

		/// <summary>
		/// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
		/// </summary>
		/// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
		/// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterType<TServiceType, TImplementationType>() where TImplementationType : TServiceType;

		/// <summary>
		/// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
		/// </summary>
		/// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
		/// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
		/// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterType<TServiceType, TImplementationType>( LifetimeType lifetime ) where TImplementationType : TServiceType;

		/// <summary>
		/// Register an instance with the container.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Instance registration is much like registering a type with <see cref="LifetimeType.Singleton" />,
		/// except that instead of the container creating the instance the first time it is requested, the
		/// instance is already created outside the container and that instance is added to the container.
		/// </para>
		/// </remarks>
		/// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
		/// <param name="instance">The instance.</param>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterInstance( Type serviceType, object instance );

		/// <summary>
		/// Register an instance with the container.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Instance registration is much like registering a type with <see cref="LifetimeType.Singleton" />,
		/// except that instead of the container creating the instance the first time it is requested, the
		/// instance is already created outside the container and that instance is added to the container.
		/// </para>
		/// </remarks>
		/// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
		/// <param name="instance">The instance.</param>
		/// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
		IContainerProvider RegisterInstance<TServiceType>( object instance );

		/// <summary>Resolves a single registered service.</summary>
		/// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
		/// <returns>The requested service.</returns>
		TServiceType GetService<TServiceType>();

		/// <summary>Resolves multiple registered services.</summary>
		/// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
		/// <returns>The requested services.</returns>
		IEnumerable<TServiceType> GetServices<TServiceType>();

		/// <summary>
		/// Injects the matching dependences.
		/// </summary>
		/// <param name="instance">The instance.</param>
		void Inject( object instance );
	}
}
