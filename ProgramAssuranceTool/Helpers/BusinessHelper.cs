using System.IO;

namespace ProgramAssuranceTool.Helpers
{
	public class BusinessHelper
	{
		internal static bool DeleteServerFile( string serverFileName )
		{
			var didDelete = false;
			if ( File.Exists( serverFileName ) )
			{
				DeleteFile( serverFileName );
				didDelete = true;
			}
			return didDelete;
		}

		internal static void DeleteFile( string fileName )
		{
			if ( File.Exists( fileName ) )
				File.Delete( fileName );
		}

		public static bool TryToDelete( string f )
		{
			try
			{
				// A.
				// Try to delete the file.
				File.Delete( f );
				return true;
			}
			catch ( IOException )
			{
				// B.
				// We could not delete the file.
				return false;
			}
		}

	}

}