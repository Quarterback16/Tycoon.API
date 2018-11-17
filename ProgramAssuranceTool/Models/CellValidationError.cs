namespace ProgramAssuranceTool.Models
{
	public class CellValidationError
	{
		public PatColumn ErrorColumn { get; set; }
		public string RowType { get; set; }
		public int RowNumber { get; set; }
		public string ErrorValue { get; set; }
		public string DataType { get; set; }
		public string OptionalMessage { get; set; }

		public string CellErrorMessage()
		{
			if ( RowType != null && RowType.Equals( "ReviewOutcome" ) )
				return string.Format( "{0} - row {1}",
											OptionalMessage,
											RowNumber );

			if ( RowType != null && RowType.Equals( "DataTypeMismatch" ) )
				return string.Format( "{0}", OptionalMessage );


			var dataType = DataType ?? string.Empty;
			var errorValue = ErrorValue ?? string.Empty;
			string columnName;
			if (ErrorColumn == null)
				columnName = string.Empty;
			else
				columnName = string.IsNullOrEmpty( ErrorColumn.ColumnName ) ? string.Empty : ErrorColumn.ColumnName;

			if (DataType == null)
			{
				//  Its not a data type error, so what is it
				if (RowType != null)
					return RowType.Equals( "Review" ) ? string.Format( "{0} in row {1}", OptionalMessage, RowNumber ) : OptionalMessage;
				return OptionalMessage;
			}

			return string.Format( "Invalid {0} data {1} '{2}' in {3} column - row {4}",
										dataType,
										OptionalMessage,
										errorValue,
										columnName,
										RowNumber );
		}
	}
}