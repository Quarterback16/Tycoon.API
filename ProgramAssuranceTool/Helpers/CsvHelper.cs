using LumenWorks.Framework.IO.Csv;

namespace ProgramAssuranceTool.Helpers
{
	public static class CsvHelper
	{
		public static bool EmptyLine( CsvReader cols, int nCols )
		{
			var isEmptyLine = true;
			for ( var i = 0; i < nCols; i++ )
			{
				if ( string.IsNullOrEmpty( cols[ i ] ) ) continue;
				isEmptyLine = false;
				break;
			}
			return isEmptyLine;
		}
	}
}