using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	public class SqlHelper
	{
		#region  Stored Procs Helpers

		public static void AddNullableBigIntPara( long? longValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramLong = new SqlParameter( paraName, SqlDbType.BigInt )
			{
				Value = longValue
			};
			sqlParams.Add( paramLong );
		}

		public static void AddBigIntPara( long longValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramLong = new SqlParameter( paraName, SqlDbType.BigInt )
			{
				Value = longValue
			};
			sqlParams.Add( paramLong );
		}

		public static void AddIntPara( int intValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramInt = new SqlParameter( paraName, SqlDbType.Int )
			{
				Value = intValue
			};
			sqlParams.Add( paramInt );
		}

		public static void AddIntPara( int? intValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramInt = new SqlParameter( paraName, SqlDbType.Int )
			{
				Value = intValue ?? 0
			};
			sqlParams.Add( paramInt );
		}

		public static void AddDecimalPara( decimal decimalValue, byte decimalPlaces, string paraName, List<SqlParameter> sqlParams )
		{
			var paramDecimal = new SqlParameter( paraName, SqlDbType.Decimal )
			{
				Value = decimalValue,
				Precision = 18,
				Scale = decimalPlaces
			};
			sqlParams.Add( paramDecimal );
		}

		public static void AddMoneyPara( decimal? decimalValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramMoney = new SqlParameter( paraName, SqlDbType.Money )
			{
				Value = decimalValue
			};
			sqlParams.Add( paramMoney );
		}

		public static void AddBitPara( bool boolValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramBool = new SqlParameter( paraName, SqlDbType.Bit )
			{
				Value = boolValue
			};
			sqlParams.Add( paramBool );
		}

		public static void AddBitPara( bool? boolValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramBool = new SqlParameter( paraName, SqlDbType.Bit )
			{
				Value = boolValue ?? false
			};
			sqlParams.Add( paramBool );
		}

		public static void AddDatePara( DateTime? dateValue, string paraName, List<SqlParameter> sqlParams )
		{
			var paramDate = new SqlParameter( paraName, SqlDbType.DateTime )
			{
				Value = dateValue
			};
			sqlParams.Add( paramDate );
		}

		public static void AddNullDatePara( string paraName, List<SqlParameter> sqlParams )
		{
			var paramDate = new SqlParameter( paraName, SqlDbType.DateTime )
			{
				Value = DBNull.Value
			};
			sqlParams.Add( paramDate );
		}

		public static void AddNullableDateParameter( DateTime? theDate, string paraName, List<SqlParameter> sqlParams )
		{
			if ( theDate == new DateTime( 1, 1, 1 ) )
				AddNullDatePara( paraName, sqlParams );
			else
				AddDatePara( theDate, paraName, sqlParams );
		}

		public static void AddVarcharPara( string value, string paraName, List<SqlParameter> sqlParams )
		{
			var paramVarchar = new SqlParameter( paraName, SqlDbType.VarChar )
			{
				Value = value
			};
			sqlParams.Add( paramVarchar );
		}

		public static void AddNullableVarcharPara( string value, string paraName, List<SqlParameter> sqlParams )
		{
			var paramVarchar = new SqlParameter( paraName, SqlDbType.VarChar )
			{
				Value = value ?? string.Empty
			};
			sqlParams.Add( paramVarchar );
		}

		public static void AddCharPara( string value, string paraName, List<SqlParameter> sqlParams )
		{
			var paramChar = new SqlParameter( paraName, SqlDbType.Char )
			{
				Value = value
			};
			sqlParams.Add( paramChar );
		}

		public static void AddVarbinaryPara( byte[] file, string paraName, List<SqlParameter> sqlParams )
		{
			var paramVarbinary = new SqlParameter( paraName, SqlDbType.VarBinary )
			{
				Value = file
			};
			sqlParams.Add( paramVarbinary );
		}

		public static void AddReturnPara( string paraName, List<SqlParameter> sqlParams )
		{
			var paramInt = new SqlParameter( paraName, SqlDbType.Int )
			{
				Direction = ParameterDirection.ReturnValue
			};
			sqlParams.Add( paramInt );
		}

		public static bool HasColumn( IDataRecord dr, string columnName )
		{
			for ( var i = 0; i < dr.FieldCount; i++ )
			{
				if ( dr.GetName( i ).Equals( columnName, StringComparison.InvariantCultureIgnoreCase ) )
					return true;
			}
			return false;
		}

		#endregion

		#region  Table Parameters

		internal static DataTable BuildIdTable()
		{
			var dtIds = new DataTable();
			dtIds.Columns.Add( "Id", typeof( Int32 ) );
			return dtIds;
		}

		internal static void PopulateIdsTable( IEnumerable<int> ids, DataTable dtIds )
		{
			foreach ( var i in ids )
			{
				var dr = dtIds.NewRow();
				dr[ "Id" ] = i;
				dtIds.Rows.Add( dr );
			}
		}

		public static DataTable BuildVarcharTable( string columnName )
		{
			var dtStrings = new DataTable();
			dtStrings.Columns.Add( columnName, typeof( String ) );
			return dtStrings;
		}

		public static DataTable BuildSettingTable()
		{
			var dt = new DataTable();
			dt.Columns.Add( "UserId", typeof( String ) );
			dt.Columns.Add( "Name", typeof( String ) );
			dt.Columns.Add( "SerialiseAs", typeof( String ) );
			dt.Columns.Add( "Value", typeof( String ) );
			dt.Columns.Add( "CreatedBy", typeof( String ) );
			return dt;
		}

		public static DataTable BuildQuestionTable()
		{
			var dt = new DataTable();
			dt.Columns.Add( "ProjectId", typeof( int ) );
			dt.Columns.Add( "QuestionType", typeof( String ) );
			dt.Columns.Add( "QuestionText", typeof( String ) );
			dt.Columns.Add( "AnswerColumn", typeof( String ) );
			dt.Columns.Add( "CreatedBy", typeof( String ) );
			return dt;
		}

		internal static void PopulateQuestionTable( List<PatQuestion> questions, DataTable dt )
		{
			foreach ( var q in questions )
			{
				var dr = dt.NewRow();
				dr[ "ProjectId" ] = q.ProjectId;
				dr[ "QuestionType" ] = q.Type;
				dr[ "QuestionText" ] = q.Text;
				dr[ "AnswerColumn" ] = q.AnswerColumn;
				dr[ "CreatedBy" ] = q.CreatedBy;
				dt.Rows.Add( dr );
			}
		}

		internal static void PopulateSettingsTable( List<UserSetting> settings, DataTable dt )
		{
			foreach ( var i in settings )
			{
				var dr = dt.NewRow();
				dr[ "UserId" ] = i.UserId;
				dr[ "Name" ] = i.Name;
				dr[ "SerialiseAs" ] = i.SerialiseAs;
				dr[ "Value" ] = i.Value;
				dr[ "CreatedBy" ] = i.UserId;
				dt.Rows.Add( dr );
			}
		}

		internal static void PopulateVarcharTable( string columnName, List<ProjectContract> strings, DataTable dtStrings )
		{
			foreach ( var i in strings )
			{
				var dr = dtStrings.NewRow();
				dr[ columnName ] = i.ContractType;
				dtStrings.Rows.Add( dr );
			}
		}

		internal static void AddTablePara( DataTable dt, string paramName, string tableTypeName, List<SqlParameter> sqlParams )
		{
			var paramTable = new SqlParameter( paramName, SqlDbType.Structured )
			{
				Direction = ParameterDirection.Input,
				TypeName = tableTypeName,
				Value = dt
			};
			sqlParams.Add( paramTable );
		}

		#endregion

	}
}
