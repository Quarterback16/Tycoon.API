using System;
using System.Collections.Generic;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	public class CellValidator
	{
		private readonly IAdwRepository _adwRepository;

		public string RowType { get; set; }

		public List<PatColumn> Columns { get; set; }

		public List<CellValidationError> Errors { get; set; }

		public CellValidator( IAdwRepository adw )
		{
			Errors = new List<CellValidationError>();
			RowType = "row";
			_adwRepository = adw;
		}

		/// <summary>
		///    works out if the value is a valid value for this column
		/// </summary>
		/// <param name="column">One of the standard columns</param>
		/// <param name="value">the value uploaded</param>
		/// <param name="rowNumber">the sequential row number</param>
		/// <returns></returns>

		public bool DataIsValid( PatColumn column, string value, int rowNumber )
		{
			var isValid = true;
			if (column == null) return true;

			var optionalMessage = string.Empty;

			if ( value.Trim().Length > 0 )
			{
				// test known columns, actually they are all known columns
				if ( ! string.IsNullOrEmpty( column.ListCode ) )

					isValid = CheckAdw( column, value, ref optionalMessage );
				else
				{
					// check the data type
					switch ( column.DataType )
					{
						case CommonConstants.DataTypeText:
							if ( value.Trim().Length > CommonConstants.DataTypeTextMaximumLength )
							{
								isValid = false;
								optionalMessage = "Max. length exceeded";
							}
							break;

						case CommonConstants.DataTypeNumber:
							Int64 number;
							isValid = Int64.TryParse( value, out number );
							if ( !isValid ) optionalMessage = string.Format( "{0} is not a valid number value", value );
							break;

						case CommonConstants.DataTypeDate:
							DateTime theDate;
							isValid = DateTime.TryParse( value, out theDate );
							if ( !isValid ) optionalMessage = string.Format( "{0} is not a valid date value", value );

							break;

						case CommonConstants.DataTypeFlag:
							isValid = value.ToUpper().Equals( "Y" ) || value.ToUpper().Equals( "N" );
							if (!isValid) optionalMessage = string.Format( "{0} is not a valid flag value", value );
							break;

						case CommonConstants.DataTypeCurrency:
							decimal amount;
							isValid = Decimal.TryParse( value, out amount );
							if (!isValid) optionalMessage = string.Format( "{0} is not a valid currency amount", value );
							break;

						default:
							throw new ApplicationException( string.Format( "CellValidator: Invalid DataType- {0}", column.DataType ) );
					}
				}
			}
			else
			{
				//  value is empty, which is fine UNLESS its the managed by field
				if (column.ColumnName.Equals( CommonConstants.UploadColumn_ManagedBy) )
				{
					isValid = false;
					optionalMessage = "Missing Managed By value";
				}
			}


			if ( !isValid )
			{
				var error = new CellValidationError
				{
					DataType = column.DataType,
					ErrorValue = value,
					ErrorColumn = column,
					RowType = RowType,
					RowNumber = rowNumber,
					OptionalMessage = optionalMessage
				};
				Errors.Add( error );
			}

			return isValid;
		}

		public bool CheckAdw( PatColumn column, string value, ref string optionalMessage )
		{
			var isValid = true;
			if (string.IsNullOrEmpty( _adwRepository.GetDescription( column.ListCode, value ) ))
			{
				isValid = false;
				optionalMessage = string.Format( "{0} is not a valid {1}", value, column.ColumnName );
			}
			return isValid;
		}

	}
}