using System.Configuration;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   Base Repository class
	/// </summary>
	public abstract class PatRepository
	{
		protected string DbConnection;

		protected PatRepository()
		{
			DbConnection = ConfigurationManager.ConnectionStrings[ "ProgramAssuranceConnectionString" ].ConnectionString;
		}

		/// <summary>
		///  The connection string
		/// </summary>
		/// <returns></returns>
		public string ConnectionString()
		{
			return DbConnection;
		}
	}
}