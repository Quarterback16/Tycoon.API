using System.Configuration;

namespace TFLLib.IntegrationTests
{
	public class IntegrationTestsBase
	{
		protected DataLibrarian Sut;

		public void Initialise()
		{
			//  Integrate with a real database
			Sut = new DataLibrarian(
			   NflConnectionString(),
			   TflConnectionString(),
			   CtlConnectionString(),
			   new TestLogger());
		}

		private static string NflConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			var connStr = connections["NflConnectionString"].ConnectionString;
			return connStr;
		}

		private static string TflConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			return connections["TflConnectionString"].ConnectionString;
		}

		private static string CtlConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			return connections["CtlConnectionString"].ConnectionString;
		}
	}

}
