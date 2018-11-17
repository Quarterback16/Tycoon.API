using System;

using ProgramAssuranceTool.Interfaces;

namespace ProgramAssuranceTool.Helpers
{
	public class ConsoleLogger : ILog
	{
		public void Info( string message )
		{
			Console.WriteLine( message );
		}

		public void Warning( string message )
		{
			Console.WriteLine( message );
		}

		public void Error( string message )
		{
			Console.WriteLine( message );
		}

		public void Exception( Exception exception )
		{
			Console.WriteLine( exception.Message );
		}
	}
}