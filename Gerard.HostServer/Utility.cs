using System.Configuration;

namespace Gerard.HostServer
{
	public static class Utility
	{
		private static DataLibrarian _tflWs;

		public static DataLibrarian TflWs
		{
			get
			{
				return _tflWs
					?? (_tflWs = new DataLibrarian(
										NflConnectionString(),
										TflConnectionString(),
										CtlConnectionString(),
										new NLogAdaptor()));
			}
			set { _tflWs = value; }
		}
		public static string NflConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			var connStr = connections["NflConnectionString"].ConnectionString;
			return connStr;
		}

		public static string TflConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			return connections["TflConnectionString"].ConnectionString;
		}

		public static string CtlConnectionString()
		{
			var connections = ConfigurationManager.ConnectionStrings;
			return connections["CtlConnectionString"].ConnectionString;
		}
	}
}
