using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using ProgramAssuranceTool.Infrastructure.Extensions;

namespace ProgramAssuranceTool.Infrastructure.Helpers
{
	/// <summary>
	/// Represents a helper that is used to create constructor delegates. This approach performs much better than using <see cref="Activator.CreateInstance(Type, object[])" />.
	/// </summary>
	internal static class DelegateHelper
	{
		/// <summary>
		/// Delegate of the constructor of an object.
		/// </summary>
		/// <returns></returns>
		internal delegate object ConstructorDelegate( params object[] parameters );

		/// <summary>
		/// Cache of contructor delegates.
		/// </summary>
		private static readonly ConcurrentDictionary<string, ConstructorDelegate> constructorDelegateCache = new ConcurrentDictionary<string, ConstructorDelegate>();

		/// <summary>
		/// Creates an instance of a type using the constructor.
		/// </summary>
		/// <typeparam name="T">Type of class.</typeparam>
		/// <param name="type">Type of class.</param>
		/// <returns>Created object instance.</returns>
		internal static T Create<T>( Type type ) where T : class
		{
			ConstructorDelegate creator = CreateConstructorDelegate( type );

			return creator() as T;
		}

		/// <summary>
		/// Creates a constructor delegate for the object type.
		/// </summary>
		/// <param name="type">Type of class.</param>
		/// <returns>Created object instance.</returns>
		internal static ConstructorDelegate CreateConstructorDelegate( Type type )
		{
			return CreateConstructorDelegate( type, null );
		}

		/// <summary>
		/// Creates a constructor delegate for the object type.
		/// </summary>
		/// <param name="type">Type of class to construct.</param>
		/// <param name="parameterTypes">Parameter types of constructor signature.</param>
		/// <returns>Created object instance.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="type" /> does not contain a constructor with a signature matching <paramref name="parameterTypes" />.</exception>
		internal static ConstructorDelegate CreateConstructorDelegate( Type type, Type[] parameterTypes )
		{
			string cacheKey = type.FullName;

			if ( parameterTypes != null )
			{
				parameterTypes.ForEach( t => cacheKey = string.Format( "{0}_{1}", cacheKey, t.Name ) );
			}

			return constructorDelegateCache.GetOrAdd( cacheKey, delegate( string key )
			{
				Expression<ConstructorDelegate> expression;
				var constructorDelegateParameter = Expression.Parameter( typeof( object[] ), "parameters" );

				// Use constructor with parameters if parameters were supplied
				if ( parameterTypes != null && parameterTypes.Length > 0 )
				{
					// Get constructor for parameters
					var constructorInfo = type.GetConstructor( BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null );

					if ( constructorInfo == null )
					{
						throw new ArgumentException( "Cannot find constructor with matching signature." );
					}

					// Prepare parameters as expressions
					var parameterInfos = constructorInfo.GetParameters();
					var parameterExpressions = new Expression[ parameterInfos.Length ];

					for ( int i = 0; i < parameterInfos.Length; i++ )
					{
						var index = Expression.Constant( i );
						var parameterType = parameterInfos[ i ].ParameterType;

						parameterExpressions[ i ] = Expression.Convert( Expression.ArrayIndex( constructorDelegateParameter, index ), parameterType );
					}

					// Use constructor with parameters
					expression = Expression.Lambda<ConstructorDelegate>( Expression.New( constructorInfo, parameterExpressions ), constructorDelegateParameter );
				}
				else
				{
					// Default to use parameterless constructor
					expression = Expression.Lambda<ConstructorDelegate>( Expression.Convert( Expression.New( type ), typeof( object ) ), constructorDelegateParameter );
				}

				return expression.Compile();
			}
			);
		}
	}
}