using System;
using System.Collections.Generic;
using ProgramAssuranceTool.Infrastructure.Exceptions;
using ProgramAssuranceTool.Infrastructure.Models;
using ProgramAssuranceTool.Infrastructure.Types;

namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines the methods and properties that are required for a Cache Service.
	/// </summary>
	public interface ICacheService
	{
		/// <summary>
		/// Set an item in the cache under a unique key.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="model">Key model.</param>
		/// <param name="o">Item to be stored in cache.</param>
		/// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
		void Set<T>( KeyModel model, T o );


		/// <summary>
		/// Set an item in the cache under a unique key.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="model">Key model.</param>
		/// <param name="o">Item to be stored in cache.</param>
		/// <param name="timeToLive">time to live in the cache</param>
		/// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
		void Set<T>( KeyModel model, T o, TimeSpan timeToLive );

		/// <summary>
		/// Remove an item from the cache.
		/// </summary>
		/// <param name="model">Key model.</param>
		void Remove( KeyModel model );

		/// <summary>
		/// Remove all variations of an item from the cache based on a key name.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="CacheType.Default" />.
		/// </remarks>
		/// <param name="key">Key used in key model.</param>
		void Remove( string key );

		/// <summary>
		/// Remove all variations of an item from the cache based on a key name.
		/// </summary>
		/// <param name="cacheType">Cache type to use.</param>
		/// <param name="key">Key used in key model.</param>
		void Remove( CacheType cacheType, string key );

		/// <summary>
		/// Checks if the item is stored in the cache.
		/// </summary>
		/// <param name="model">Key model.</param>
		/// <returns><c>true</c> if the item is stored in the cache; otherwise, <c>false</c>.</returns>
		bool Contains( KeyModel model );

		/// <summary>
		/// Try to get a stored object.
		/// </summary>
		/// <typeparam name="T">Type of stored item.</typeparam>
		/// <param name="model">Key model.</param>
		/// <param name="value">Stored value.</param>
		/// <returns>Stored item as type.</returns>
		bool TryGet<T>( KeyModel model, out T value );

		/// <summary>
		/// Try to get all stored objects that belong to they specified key.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="CacheType.Default" />.
		/// </remarks>
		/// <typeparam name="T">Type of stored item.</typeparam>
		/// <param name="key">Key used in key model.</param>
		/// <param name="value">Stored values.</param>
		/// <returns>Stored items as type.</returns>
		bool TryGet<T>( string key, out IEnumerable<T> value );

		/// <summary>
		/// Try to get all stored objects that belong to they specified key.
		/// </summary>
		/// <param name="cacheType">Cache type to use.</param>
		/// <typeparam name="T">Type of stored item.</typeparam>
		/// <param name="key">Key used in key model.</param>
		/// <param name="value">Stored values.</param>
		/// <returns>Stored items as type.</returns>
		bool TryGet<T>( CacheType cacheType, string key, out IEnumerable<T> value );
	}
}