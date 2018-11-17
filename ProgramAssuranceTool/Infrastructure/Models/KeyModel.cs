using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Description;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Types;

namespace ProgramAssuranceTool.Infrastructure.Models
{
	/// <summary>
	/// Key model.
	/// </summary>
	public class KeyModel : IFluent
	{
		/// <summary>
		/// Caching level to use.
		/// </summary>
		public CacheType CacheType { get; private set; }

		/// <summary>
		/// Namespace of the <see cref="Service" /> using the current <see cref="ICacheService" /> instance.
		/// </summary>
		internal string Namespace;

		/// <summary>
		/// Key.
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// Values that are used as part of the key.
		/// </summary>
		public IList<object> Values { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyModel" /> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="key" /> is <c>null</c>.</exception>
		public KeyModel( string key ) : this( CacheType.Default, key ) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyModel" /> class.
		/// </summary>
		/// <param name="cacheType">Type of cache level to use.</param>
		/// <param name="key">The key.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="key" /> is <c>null</c>.</exception>
		public KeyModel( CacheType cacheType, string key )
		{
			if ( string.IsNullOrEmpty( key ) )
			{
				throw new ArgumentNullException( "key" );
			}

			CacheType = cacheType;
			Key = key;
			Values = new List<object>();
			Namespace = "ProgramAssuranceTool";
		}

		/// <summary>
		/// Get key.
		/// </summary>
		/// <returns>The namespaced key made up of the object values.</returns>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="Namespace"/> is <c>null</c>.</exception>
		public string GetKey()
		{
			if ( string.IsNullOrEmpty( Namespace ) )
			{
				throw new InvalidOperationException( "Namespace cannot be null." );
			}

			var key = new StringBuilder();

			key.Append( GetMainKey() );

			var values = GetValuesKey();

			if ( !string.IsNullOrEmpty( values ) )
			{
				key.AppendFormat( ".{0}", values );
			}

			return key.ToString();
		}

		/// <summary>
		/// Gets the main part of the key.
		/// </summary>
		/// <returns>The main part of the key.</returns>
		internal string GetMainKey()
		{
			var key = new StringBuilder();

			key.AppendFormat( "[{0}.{1}]", Namespace, Key );

			return key.ToString();
		}

		/// <summary>
		/// Gets values as string for use in key.
		/// </summary>
		/// <returns>Values as string for use in key.</returns>
		internal string GetValuesKey()
		{
			var key = new StringBuilder();

			foreach ( var value in Values )
			{
				if ( value == null )
				{
					continue;
				}

				if ( key.Length > 0 )
				{
					key.Append( "." );
				}

				var values = value as IEnumerable<object>;

				if ( values != null )
				{
					key.AppendFormat( "[{0}]", string.Join( ".", values ) );
				}
				else
				{
					key.Append( value );
				}
			}

			return key.ToString();
		}

		/// <summary>
		/// Add a value.
		/// </summary>
		/// <param name="value">Value to add.</param>
		/// <returns>The current instance of this object.</returns>
		public KeyModel Add( object value )
		{
			if ( value != null && ( !( value is string ) || value as string != string.Empty ) )
			{
				Values.Add( value );
			}
			return this;
		}

		/// <summary>
		/// Clear values.
		/// </summary>
		/// <returns>The current instance of this object.</returns>
		public KeyModel Clear()
		{
			Values.Clear();

			return this;
		}
	}
}