using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	public class UploadQuestionsValidator : BaseUploadValidator
	{
		public IEnumerable<CellValidationError> ValidateFile( Stream stream, IAdwRepository adw )
		{
			LoadStandardHeadings();

			var validator = new CellValidator( adw );

			//  by default the CsvReader will skip empty lines
			using ( var csv = new CsvReader( new StreamReader( stream ), true ) )
			{
				var columns = csv.GetFieldHeaders(); //  these are the standard headings

				//  Check for missing columns
				validator.Errors.AddRange( MissingColumns( columns.ToList() ) );

				//  Check that the Headings are correct
				validator.Errors.AddRange( HeadingErrors( columns.ToList() ) );

				//  if we have heading errors dont try to go any further
				if ( validator.Errors.Count > 0 )
					goto EndOfValidation;

				var r = 1;
				while ( csv.ReadNextRecord() )
				{
					if ( CsvHelper.EmptyLine( csv, columns.Length ) ) continue;

					//  process the single row
					r++;
					//var questionType = string.Empty;
					//var questionText = string.Empty;
					//var answerColumn = string.Empty;

					//for each column of the row
					for ( var c = 0; c < columns.Length; c++ )
					{
						var val = csv[ c ];

						var patColumn = SelectStandardColumn( columns[ c ] );

						//if ( patColumn.ColumnName.Equals( CommonConstants.QuestionColumn_QuestionType ) )
						//	questionType = val;
						//if ( patColumn.ColumnName.Equals( CommonConstants.QuestionColumn_QuestionText ) )
						//	questionText = val;
						//if ( patColumn.ColumnName.Equals( CommonConstants.QuestionColumn_AnswerColumn ) )
						//	answerColumn = val;

						if ( validator.DataIsValid( column: patColumn, value: val, rowNumber: r ) ) continue;

						//  test the number of errors against an arbitary limit
						if (!validator.Errors.Count.Equals( UploadValidationErrorLimit )) continue;
						validator.Errors.Add( new CellValidationError
							{
								OptionalMessage = ErrorLimitMessage()
							} );
						goto EndOfValidation;
					}
				}
			}
		EndOfValidation:

			return validator.Errors;
		}

		private void LoadStandardHeadings()
		{
			StandardColumns = new List<PatColumn>
				{
					new PatColumn
						{
							ColumnName = CommonConstants.QuestionColumn_QuestionType.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.QuestionColumn_QuestionText.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.QuestionColumn_AnswerColumn.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						}
				};
		}
	}
}