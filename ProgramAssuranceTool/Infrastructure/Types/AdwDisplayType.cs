namespace ProgramAssuranceTool.Infrastructure.Types
{
	/// <summary>
	/// Represents an enum which defines the adw display type.
	/// </summary>
	public enum AdwDisplayType
	{
		/// <summary>
		/// Displays using the Adw long description.
		/// </summary>
		Description,

		/// <summary>
		/// Displays using the Adw short description.
		/// </summary>
		ShortDescription,

		/// <summary>
		/// Displays using the Adw code.
		/// </summary>
		Code,

		/// <summary>
		/// Displays using the Adw code and long description.
		/// </summary>
		CodeAndDescription,

		/// <summary>
		/// Displays using the Adw code and short description.
		/// </summary>
		CodeAndShortDescription,

		/// <summary>
		/// Default display type is Description.
		/// </summary>
		Default = Description
	}
}