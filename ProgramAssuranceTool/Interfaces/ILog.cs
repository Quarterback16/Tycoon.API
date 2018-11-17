using System;

namespace ProgramAssuranceTool.Interfaces
{
	public interface ILog
	{
		void Info( string message );

		void Warning( string message );

		void Error( string message );

		void Exception( Exception exception );
	}
}