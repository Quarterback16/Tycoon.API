namespace ProgramAssuranceTool.Infrastructure.Types
{
	/// <summary>
	/// Represents an enum which defines the cache type.
	/// </summary>
	public enum CacheType
	{
		/// <summary>
		/// Cache at the global level.
		/// </summary>
		Global,

		/// <summary>
		/// Cache at the organisation level.
		/// </summary>
		Organisation,

		/// <summary>
		/// Cache at the site level.
		/// </summary>
		Site,

		/// <summary>
		/// Cache at the user level.
		/// </summary>
		User,

		/// <summary>
		/// Default is at the user level.
		/// </summary>
		Default = Global
	}
}