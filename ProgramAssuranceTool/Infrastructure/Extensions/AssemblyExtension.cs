using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProgramAssuranceTool.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions for <see cref="System.Reflection.Assembly"/>.
	/// </summary>
	public static class AssemblyExtension
	{
		/// <summary>
		/// Gets the public types that implement the specified type in the assembly.
		/// </summary>
		/// <typeparam name="T">The type to get the public implementations for.</typeparam>
		/// <param name="assembly">The assembly instances.</param>
		/// <returns>The public types implementing the specified type.</returns>
		public static IEnumerable<Type> GetPublicTypesImplementing<T>( this Assembly assembly )
		{
			return assembly.PublicTypes().Where( t => typeof( T ).IsAssignableFrom( t ) );
		}

		/// <summary>
		/// Gets the public types that implement the specified type in the assemblies.
		/// </summary>
		/// <typeparam name="T">The type to get the public implementations for.</typeparam>
		/// <param name="assemblies">The assembly instance.</param>
		/// <returns>The public types implementing the specified type.</returns>
		public static IEnumerable<Type> GetPublicTypesImplementing<T>( this IEnumerable<Assembly> assemblies )
		{
			return ( assemblies == null ) ? Enumerable.Empty<Type>() : assemblies.PublicTypes().Where( t => typeof( T ).IsAssignableFrom( t ) );
		}

		/// <summary>
		/// Gets the public types in the assembly.
		/// </summary>
		/// <param name="assembly">The assembly instance.</param>
		/// <returns>The public types.</returns>
		public static IEnumerable<Type> PublicTypes( this Assembly assembly )
		{
			return ( assembly == null ) ? Enumerable.Empty<Type>() : assembly.GetExportedTypes();
		}

		/// <summary>
		/// Gets the public types in the assemblies.
		/// </summary>
		/// <param name="assemblies">The assembly instances.</param>
		/// <returns>The public types.</returns>
		public static IEnumerable<Type> PublicTypes( this IEnumerable<Assembly> assemblies )
		{
			return ( assemblies == null ) ? Enumerable.Empty<Type>() : assemblies.SelectMany( assembly => assembly.PublicTypes() );
		}

		/// <summary>
		/// Gets the concrete types that implement the specified type in the assembly.
		/// </summary>
		/// <typeparam name="T">The type to get the concrete implementations for.</typeparam>
		/// <param name="assembly">The assembly instances.</param>
		/// <returns>The concrete types implementing the specified type.</returns>
		public static IEnumerable<Type> GetConcreteTypesImplementing<T>( this Assembly assembly )
		{
			return assembly.ConcreteTypes().Where( t => typeof( T ).IsAssignableFrom( t ) );
		}

		/// <summary>
		/// Gets the concrete types that implement the specified type in the assemblies.
		/// </summary>
		/// <typeparam name="T">The type to get the concrete implementations for.</typeparam>
		/// <param name="assemblies">The assembly instance.</param>
		/// <returns>The concrete types implementing the specified type.</returns>
		public static IEnumerable<Type> GetConcreteTypesImplementing<T>( this IEnumerable<Assembly> assemblies )
		{
			return ( assemblies == null ) ? Enumerable.Empty<Type>() : assemblies.ConcreteTypes().Where( t => typeof( T ).IsAssignableFrom( t ) );
		}

		/// <summary>
		/// Gets the concrete types in the assembly.
		/// </summary>
		/// <param name="assembly">The assembly instance.</param>
		/// <returns>The concrete types.</returns>
		public static IEnumerable<Type> ConcreteTypes( this Assembly assembly )
		{
			return ( assembly == null ) ? Enumerable.Empty<Type>() : assembly.PublicTypes().Where( type => ( type != null ) && type.IsClass && !type.IsAbstract && !type.IsInterface );
		}

		/// <summary>
		/// Gets the concrete types in the assemblies.
		/// </summary>
		/// <param name="assemblies">The assembly instances.</param>
		/// <returns>The concrete types.</returns>
		public static IEnumerable<Type> ConcreteTypes( this IEnumerable<Assembly> assemblies )
		{
			return ( assemblies == null ) ? Enumerable.Empty<Type>() : assemblies.SelectMany( assembly => assembly.ConcreteTypes() );
		}
	}
}