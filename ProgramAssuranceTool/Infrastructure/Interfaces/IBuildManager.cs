using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines the methods and properties that are required for a Build Manager.
	/// </summary>
	public interface IBuildManager
	{
		/// <summary>
		/// Gets the available Mvc Application assemblies.
		/// </summary>
		/// <value>The Mvc Application assemblies.</value>
		IEnumerable<Assembly> Assemblies
		{
			get;
		}

		/// <summary>
		/// Gets the available public types of <see cref="Assemblies"/>.
		/// </summary>
		/// <value>The concrete types.</value>
		IEnumerable<Type> PublicTypes
		{
			get;
		}

		/// <summary>
		/// Gets the available concrete types of <see cref="Assemblies"/>.
		/// </summary>
		/// <value>The concrete types.</value>
		IEnumerable<Type> ConcreteTypes
		{
			get;
		}

		/// <summary>
		/// Resolves the type of the given type name.
		/// </summary>
		/// <param name="typeName">A namespaced type and assembly name.</param>
		/// <example>
		/// <code>
		/// var type = BuildManagerWrapper.Current.ResolveType("System.Web.Mvc.IController, System.Web.Mvc");
		/// </code>
		/// </example>
		/// <returns>The <see cref="Type" /> for the given type name.</returns>
		Type ResolveType( string typeName );
	}
}