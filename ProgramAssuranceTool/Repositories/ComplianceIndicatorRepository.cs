using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	public class ComplianceIndicatorRepository : PatRepository, IComplianceIndicatorRepository
	{

		/// <summary>
		/// Updates the Compliance indicator.
		/// </summary>
		/// <param name="entity">The CI data.</param>
		public void Update( ComplianceIndicator entity )
		{
			var complianceIndicator = entity;
			Debug.Assert( complianceIndicator != null, "complianceIndicator != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaComplianceIndicatorUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( complianceIndicator.ComplianceIndicatorId, "@Id", sqlParams );

					SqlHelper.AddVarcharPara( complianceIndicator.Programme, "@Programme", sqlParams );
					SqlHelper.AddVarcharPara( complianceIndicator.SubjectTypeCode, "@SubjectTypeCode", sqlParams );
					SqlHelper.AddVarcharPara( complianceIndicator.Subject, "@Subject", sqlParams );
					SqlHelper.AddVarcharPara( complianceIndicator.EsaCode, "@EsaCode", sqlParams );
					SqlHelper.AddVarcharPara( complianceIndicator.Quarter, "@Quarter", sqlParams );

					SqlHelper.AddDecimalPara( complianceIndicator.Value, 3, "@ComplianceIndicator", sqlParams );

					SqlHelper.AddVarcharPara( complianceIndicator.UpdatedBy, "@UpdatedBy", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		///   Removes all the previous Compliance Indicators
		/// </summary>
		public void Zap()
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaComplianceIndicatorZap", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
			
		}
	}
}