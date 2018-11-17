using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgramAssuranceTool.Infrastructure.Types
{
	/// <summary>
	/// Represents an enum which defines the lifetime of the registered service.
	/// </summary>
	public enum LifetimeType
	{
		/// <summary>
		/// The object will be created every time it is requested (default).
		/// </summary>
		Transient,

		/// <summary>
		/// This object will be created only once and the same object will be returned each time it is requested.
		/// </summary>
		Singleton,

		/// <summary>
		/// The default lifetime type is Transient.
		/// </summary>
		Default = Transient
	}
}