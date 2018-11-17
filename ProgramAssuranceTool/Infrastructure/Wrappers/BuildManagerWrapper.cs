using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Extensions;

namespace ProgramAssuranceTool.Infrastructure.Wrappers
{
	/// <summary>
	/// Represents a wrapper for the <see cref="System.Web.Compilation.BuildManager" /> to make it Unit Testable.
	/// </summary>
	public class BuildManagerWrapper : IBuildManager
	{
		private static readonly IBuildManager instance = new BuildManagerWrapper();

		private IEnumerable<Assembly> allReferencedAssemblies;
		private IEnumerable<Assembly> referencedAssemblies;
		private IEnumerable<Type> publicTypes;
		private IEnumerable<Type> concreteTypes;

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The current.</value>
		public static IBuildManager Current
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// Gets the available Mvc Application assemblies.
		/// </summary>
		/// <value>The Mvc Application assemblies.</value>
		public virtual IEnumerable<Assembly> Assemblies
		{
			[DebuggerStepThrough]
			get
			{
				return referencedAssemblies ?? ( referencedAssemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where( assembly => !assembly.GlobalAssemblyCache && assembly.FullName.StartsWith( "Employment.Web.Mvc" ) ).ToList() );
			}
		}

		/// <summary>
		/// Gets the available public types of <see cref="Assemblies"/>.
		/// </summary>
		/// <value>The concrete types.</value>
		public IEnumerable<Type> PublicTypes
		{
			get
			{
				return publicTypes ?? ( publicTypes = Assemblies.PublicTypes().ToList() );
			}
		}

		/// <summary>
		/// Gets the available concrete types of <see cref="Assemblies"/>.
		/// </summary>
		/// <value>The concrete types.</value>
		public IEnumerable<Type> ConcreteTypes
		{
			get
			{
				return concreteTypes ?? ( concreteTypes = Assemblies.ConcreteTypes().ToList() );
			}
		}


		/// <summary>
		/// Resolves the type of the given type name.
		/// </summary>
		/// <param name="fullTypeName">A namespaced type with the assembly name.</param>
		/// <example>
		/// <code>
		/// var type = BuildManagerWrapper.Current.ResolveType("System.Web.Mvc.IController, System.Web.Mvc");
		/// </code>
		/// </example>
		/// <returns>The <see cref="Type" /> for the given type name.</returns>
		public Type ResolveType( string fullTypeName )
		{
			Type type = Type.GetType( fullTypeName );

			if ( type != null )
			{
				return type;
			}

			const string splitter = ", ";

			if ( !string.IsNullOrEmpty( fullTypeName ) && fullTypeName.Contains( splitter ) )
			{
				var splitResult = fullTypeName.Split( new[] { splitter }, StringSplitOptions.RemoveEmptyEntries );

				var typeName = splitResult.First();
				var assemblyName = splitResult.Last();

				allReferencedAssemblies = allReferencedAssemblies ?? BuildManager.GetReferencedAssemblies().Cast<Assembly>();

				var assemblyWithType = allReferencedAssemblies.FirstOrDefault( a => a.GetName().Name == assemblyName );

				if ( assemblyWithType != null )
				{
					return assemblyWithType.GetType( typeName, true );
				}
			}

			throw new InvalidOperationException( string.Format( "Could not resolve type: {0}.", fullTypeName ) );
		}
	}
}