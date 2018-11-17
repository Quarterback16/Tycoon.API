using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	public class UploadValidator : BaseUploadValidator
	{
		public IEnumerable<CellValidationError> ValidateFile( Stream stream, IAdwRepository adw, bool includesOutcomes )
		{
			LoadStandardHeadings( includesOutcomes );

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

					//  process the review (single row)
					r++;

					//for each column of the row
					for ( var c = 0; c < columns.Length; c++ )
					{
						var val = csv[ c ];

						var patColumn = SelectStandardColumn( columns[ c ] );

						if ( validator.DataIsValid( column: patColumn, value: val, rowNumber: r ) ) continue;

						//  test the number of errors against an arbitary limit
						if ( !validator.Errors.Count.Equals( UploadValidationErrorLimit ) ) continue;
						validator.Errors.Add( new CellValidationError
						{
							OptionalMessage = ErrorLimitMessage()
						} );
						goto EndOfValidation;
					}

					//  If there is outcome data check the outcome rules still are valid
					if ( includesOutcomes )
						CheckOutcomeData( csv, columns, r, validator );
				}
			}
		EndOfValidation:

			return validator.Errors;
		}


		public IEnumerable<CellValidationError> ValidateFile( string fileName, IAdwRepository adw, bool includesOutcomes )
		{
			LoadStandardHeadings( includesOutcomes );

			var validator = new CellValidator( adw );

			//  by default the CsvReader will skip empty lines
			using ( var csv = new CsvReader( new StreamReader( fileName ), true ) )
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
				while (csv.ReadNextRecord())
				{
					if (CsvHelper.EmptyLine( csv, columns.Length )) continue;

					//  process the review (single row)
					r++;

					//for each column of the row
					for ( var c = 0; c < columns.Length; c++ )
					{
						var val = csv[ c ];

						var patColumn = SelectStandardColumn( columns[ c ] );

						if (validator.DataIsValid( column: patColumn, value: val, rowNumber: r )) continue;

						//  test the number of errors against an arbitary limit
						if (!validator.Errors.Count.Equals( UploadValidationErrorLimit )) continue;
						validator.Errors.Add( new CellValidationError{ 
							OptionalMessage = ErrorLimitMessage()
						} );
						goto EndOfValidation;
					}

					//  If there is outcome data check the outcome rules still are valid
					if (includesOutcomes)
						CheckOutcomeData( csv, columns, r, validator );
				}
			}
			EndOfValidation:

			return validator.Errors;
		}

		private void LoadStandardHeadings( bool includesOutcomes )
		{
			StandardColumns = new List<PatColumn>
				{
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_JobseekerId.ToUpper(),
							DataType = CommonConstants.DataTypeNumber,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimId.ToUpper(),
							DataType = CommonConstants.DataTypeNumber,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimSequenceNumber.ToUpper(),
							DataType = CommonConstants.DataTypeNumber,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_JobseekerGivenName.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_JobseekerSurname.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimAmount.ToUpper(),
							DataType = CommonConstants.DataTypeCurrency,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimType.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForClaimTypes
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimTypeDescription.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_EmploymentServiceAreaCode.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_EmploymentServiceAreaName.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_OrgCode.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForOrgCodes
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_OrgName.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_SiteCode.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForSiteCodes
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_SiteName.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_StateCode.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForStateCodes
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ManagedBy.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ContractType.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForContractTypes
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ContractTypeDescription.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ClaimCreationDate.ToUpper(),
							DataType = CommonConstants.DataTypeDate,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ActivityId.ToUpper(),
							DataType = CommonConstants.DataTypeNumber,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_AutoSpecialCLaimFlag.ToUpper(),
							DataType = CommonConstants.DataTypeFlag,
							ListCode = string.Empty
						},
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_ManualSpecialClaimFlag.ToUpper(),
							DataType = CommonConstants.DataTypeFlag,
							ListCode = string.Empty
						}
				};
			if (includesOutcomes)
			{
				StandardColumns.Add( 
					new PatColumn
						{
							ColumnName = CommonConstants.UploadColumn_AssessmentCode.ToUpper(),
							DataType = CommonConstants.DataTypeText,
							ListCode = DataConstants.AdwListCodeForAssessmentCodes
						}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_RecoveryReasonCode.ToUpper(),
						DataType = CommonConstants.DataTypeText,
						ListCode = DataConstants.AdwListCodeForRecoveryReasonCodes
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_AssessmentActionCode.ToUpper(),
						DataType = CommonConstants.DataTypeText,
						ListCode = DataConstants.AdwListCodeForAssessmentActionCodes
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_OutcomeCode.ToUpper(),
						DataType = CommonConstants.DataTypeText,
						ListCode = DataConstants.AdwListCodeForOutcomeCodes
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_ScopeFlag.ToUpper(),
						DataType = CommonConstants.DataTypeFlag,
						ListCode = string.Empty
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_AdditionalReviewFlag.ToUpper(),
						DataType = CommonConstants.DataTypeFlag,
						ListCode = string.Empty
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_ClaimRecoveryAmount.ToUpper(),
						DataType = CommonConstants.DataTypeCurrency,
						ListCode = string.Empty
					}
					);
				StandardColumns.Add(
					new PatColumn
					{
						ColumnName = CommonConstants.UploadColumn_Comments.ToUpper(),
						DataType = CommonConstants.DataTypeText,
						ListCode = string.Empty
					}
					);
			}
		}

		private static void CheckOutcomeData( CsvReader csv, IList<string> columns, int row, CellValidator validator )
		{
			//  first extract the 4 outcome fields
			var assessmentCode = string.Empty;
			var assessmentAction = string.Empty;
			var recoveryReason = string.Empty;
			var outcomeCode = string.Empty;

			for ( var i = 0; i < columns.Count; i++ )
			{
				if (columns[ i ].Equals( CommonConstants.OutcomeColumnAssessmentCode ))
					assessmentCode = csv[ i ];
				else if (columns[ i ].Equals( CommonConstants.OutcomeColumnRecoveryReasonCode ))
					recoveryReason = csv[ i ];
				else if (columns[ i ].Equals( CommonConstants.OutcomeColumnAssessmentActionCode ))
					assessmentAction = csv[ i ];
				else if (columns[ i ].Equals( CommonConstants.OutcomeColumnOutcomeCode ))
					outcomeCode = csv[ i ];
			}

			//  now we have 4 upload outcome values for a potential review

			CheckAdw( CommonConstants.OutcomeColumnOutcomeCode, DataConstants.AdwListCodeForOutcomeCodes, row, validator, outcomeCode );
			CheckAdw( CommonConstants.OutcomeColumnAssessmentActionCode, DataConstants.AdwListCodeForAssessmentActionCodes, row, validator, assessmentAction );
			CheckAdw( CommonConstants.OutcomeColumnRecoveryReasonCode, DataConstants.AdwListCodeForRecoveryReasonCodes, row, validator, recoveryReason );
			CheckAdw( CommonConstants.OutcomeColumnAssessmentCode, DataConstants.AdwListCodeForAssessmentCodes, row, validator, assessmentCode );

			//  BR-PAUPL-0025
			if (! string.IsNullOrEmpty( recoveryReason ))
			{
				if (string.IsNullOrEmpty( assessmentCode ))
					AddOutcomeError( row, validator.Errors, "Recovery reason cannot exist without a Assessment code" );
			}
			if ( !string.IsNullOrEmpty( assessmentAction ) )
			{
				if (string.IsNullOrEmpty( recoveryReason ))
					AddOutcomeError( row, validator.Errors, "Assessment Action cannot exist without Recovery Reason" );
			}
			if (string.IsNullOrEmpty( outcomeCode )) return;
			if (string.IsNullOrEmpty( assessmentCode ))
				AddOutcomeError( row, validator.Errors, "Final outcome cannot exist without Assessment code" );
		}

		private static void CheckAdw( string columnName, string listCode, int row, CellValidator validator, string codeValue )
		{
			if (string.IsNullOrEmpty( codeValue )) return;

			var codeColumn = new PatColumn
				{
					ColumnName = columnName,
					DataType = CommonConstants.DataTypeText,
					ListCode = listCode
				};
			var errorMessage = string.Empty;
			if (!validator.CheckAdw( codeColumn, codeValue, ref errorMessage ))
				AddOutcomeError( row, validator.Errors, errorMessage );
		}

		private static void AddOutcomeError( int row, ICollection<CellValidationError> errors, string msg )
		{
			var err = new CellValidationError
				{
					OptionalMessage = msg,
					RowNumber = row,
					RowType = "ReviewOutcome"
				};
			errors.Add( err );
		}


	}


}