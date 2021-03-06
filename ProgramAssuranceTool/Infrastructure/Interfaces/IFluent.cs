﻿using System;
using System.ComponentModel;

namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
	/// <summary>
	/// Hides the common object members in development environment.
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public interface IFluent
	{
		/// <summary>
		/// Determines whether two <see cref="Object"/> instances are equal.
		/// </summary>
		/// <param name="value">The <see cref="Object"/> to compare with the current <see cref="Object"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Object"/> is equal to the current <see cref="Object"/>; otherwise, <c>false</c>.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		bool Equals( object value );

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="Object"/>.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		int GetHashCode();

		/// <summary>
		/// Gets the <see cref="Type"/> of the current instance.
		/// </summary>
		/// <returns>The <see cref="Type"/> instance that represents the exact runtime type of the current instance.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		Type GetType();

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> that represents the current <see cref="Object"/>.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		string ToString();
	}
}
