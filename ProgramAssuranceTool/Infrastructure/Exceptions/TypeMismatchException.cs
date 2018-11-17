using System;

namespace ProgramAssuranceTool.Infrastructure.Exceptions
{
	/// <summary>
	/// The exception that is thrown when an attempt is made to set an element of the wrong type.
	/// </summary>
	[Serializable]
	public class TypeMismatchException : Exception
	{
		/// <summary>
		/// Expected type.
		/// </summary>
		public Type ExpectedType { get; set; }

		/// <summary>
		/// Actual type.
		/// </summary>
		public Type ActualType { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMismatchException" /> class.
		/// </summary>
		/// <param name="expectedType">The expected type.</param>
		/// <param name="actualType">The actual type.</param>
		public TypeMismatchException( Type expectedType, Type actualType )
			: base( string.Format( "Type mismatch between the expected type ({0}) and the actual type ({1}).", expectedType, actualType ) )
		{
			ExpectedType = expectedType;
			ActualType = actualType;
		}
	}
}