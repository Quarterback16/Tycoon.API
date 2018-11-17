using System;
using System.Collections.Generic;
using System.Data;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProgramAssuranceTool.Repositories
{
	public class ControlRepository : PatRepository, IControlRepository
	{
		/// <summary>
		/// Updates the Control record
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Update( PatControl entity )
		{
			var patControl = entity;
			Debug.Assert( patControl != null, "control != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaControlUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					if ( patControl.ControlId > 0 )
						SqlHelper.AddIntPara( patControl.ControlId, "@Id", sqlParams );

					SqlHelper.AddIntPara( patControl.ProjectCount, "@ProjectCount", sqlParams );
					SqlHelper.AddIntPara( patControl.SampleCount, "@SampleCount", sqlParams );
					SqlHelper.AddIntPara( patControl.ReviewCount, "@ReviewCount", sqlParams );

					SqlHelper.AddDecimalPara( patControl.ProjectCompletion, 3, "@ProjectCompletion", sqlParams );
					SqlHelper.AddDecimalPara( patControl.TotalComplianceIndicator, 3, "@TotalComplianceIndicator", sqlParams );

					SqlHelper.AddBitPara( patControl.SystemAvailable, "@SystemAvailable", sqlParams );

					if ( patControl.LastBatchRun != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( patControl.LastBatchRun, "@LastBatchRun", sqlParams );
					if ( patControl.LastComplianceRun != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( patControl.LastComplianceRun, "@LastComplianceRun", sqlParams );

					SqlHelper.AddVarcharPara( patControl.UpdatedBy, "@UpdatedBy", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets the Contract record
		/// </summary>
		/// <returns></returns>
		public List<PatControl> GetAll()
		{
			var list = new List<PatControl>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaControlGet", connection ) )
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

					SqlDataReader rdr = null;

					try
					{
						rdr = command.ExecuteReader();

						while ( rdr.Read() )
							AddControlToList( list, rdr );
					}
					finally
					{
						if ( rdr != null )
							rdr.Close();
					}
				}
			}
			return list;
		}

		private static void AddControlToList( ICollection<PatControl> list, IDataRecord rdr )
		{
			var record = new PatControl
				{
					ControlId = (int) rdr["ControlId"], 
					SystemAvailable = rdr["SystemAvailable"] as bool? ?? default(bool), 
					ProjectCount = (int) rdr["ProjectCount"], 
					SampleCount = (int) rdr["SampleCount"], 
					ReviewCount = (int) rdr["ReviewCount"], 
					ProjectCompletion = rdr["ProjectCompletion"] as decimal? ?? default(decimal), 
					TotalComplianceIndicator = rdr["TotalComplianceIndicator"] as decimal? ?? default(decimal), 
					LastBatchRun = rdr["LastBatchRun"] as DateTime? ?? default(DateTime), 
					LastComplianceRun = rdr["LastComplianceRun"] as DateTime? ?? default(DateTime)
				};

			list.Add( record );
		}

		/// <summary>
		/// Adds the Control record.
		/// </summary>
		public void Add()
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaControlInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

				}
			}
		}
	}
}
