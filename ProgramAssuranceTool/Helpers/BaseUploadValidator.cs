using System.Collections.Generic;
using System.Linq;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	public class BaseUploadValidator
	{
		#region  Validator Constants

		public const int UploadValidationErrorLimit = 50;

		#endregion

		public List<PatColumn> StandardColumns { get; set; }

		internal IEnumerable<CellValidationError> MissingColumns( IEnumerable<string> columns )
		{
			var upperCols = ToUpper( columns );
			var headingErrorList = new List<CellValidationError>();
			foreach (
				var standardColumn in StandardColumns.Where( standardColumn => !upperCols.Contains( standardColumn.ColumnName ) ) )
				AddHeadingError( 0, headingErrorList, string.Format( "{0} must be supplied.", standardColumn.ColumnName ) );

			return headingErrorList;
		}

		/// <summary>
		///   Check the headings for correctness
		/// </summary>
		/// <param name="columns">The headings in the CSV</param>
		/// <returns>a bunch or validation errors</returns>
		internal IEnumerable<CellValidationError> HeadingErrors( IEnumerable<string> columns )
		{
			var headingErrorList = new List<CellValidationError>();
			foreach (
				var column in columns.Where( column => !StandardColumns.Any( stdCol => stdCol.ColumnName.Equals( column.ToUpper() ) ) ) )
				AddHeadingError( 0, headingErrorList, string.Format( "{0} is not a standard heading.", column ) );

			return headingErrorList;
		}

		private static IEnumerable<string> ToUpper( IEnumerable<string> columns )
		{
			return columns.Select( column => column.ToUpper() ).ToList();
		}

		private static void AddHeadingError( int row, ICollection<CellValidationError> errors, string msg )
		{
			var err = new CellValidationError
			{
				OptionalMessage = msg,
				RowNumber = row,
				RowType = "Heading"
			};
			errors.Add( err );
		}

		internal PatColumn SelectStandardColumn( string column )
		{
			return StandardColumns.FirstOrDefault( col => col.ColumnName.Equals( column.ToUpper() ) );
		}

		internal string ErrorLimitMessage()
		{
			return string.Format( "Error limit {0} reached - please fix the spreadsheet and attempt another upload",
			               UploadValidationErrorLimit );
		}
	}
}