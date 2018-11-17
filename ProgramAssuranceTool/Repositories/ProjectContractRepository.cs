using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProgramAssuranceTool.Repositories
{
	public class ProjectContractRepository : PatRepository, IProjectContractRepository
	{
		#region  IRepository Members

		public void Add( ProjectContract entity )
		{
			var projectContract = entity;
			Debug.Assert( projectContract != null, "projectContract != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectContractInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					SqlHelper.AddIntPara( projectContract.ProjectId, "@ProjectId", sqlParams );
					SqlHelper.AddVarcharPara( projectContract.ContractType, "@ContractType", sqlParams );

					SqlHelper.AddVarcharPara( projectContract.CreatedBy, "@CreatedBy", sqlParams );

					//  Output parameters
					var paramId = new SqlParameter( "@Id", SqlDbType.Int )
					{
						Direction = ParameterDirection.InputOutput,
						Value = 0
					};
					sqlParams.Add( paramId );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) != 0 ) return;

					projectContract.Id = (int) paramId.Value;
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets all the ProjectContracts for a specified projectId
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// list of Project Contracts
		/// </returns>
		public List<ProjectContract> GetAllByProjectId( int projectId )
		{
			var gs = new GridSettings
			{
				IsSearch = false,
				SortColumn = "ProjectId",
				SortOrder = "ASC",
				PageIndex = 1,
				PageSize = 999999
			};
			return GetAllByProjectId( projectId, gs );
		}

		/// <summary>
		/// Gets all by project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public List<ProjectContract> GetAllByProjectId( int projectId, GridSettings gridSettings )
		{
			var list = new List<ProjectContract>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectContractGetAll", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( projectId, "@ProjectId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( list, command );
				}
			}

			return list;
		}

		private static void LoadList( ICollection<ProjectContract> list, SqlCommand command )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddItemToList( list, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddItemToList( ICollection<ProjectContract> list, IDataRecord rdr )
		{
			var entity = new ProjectContract
				{
					Id = (int) rdr[ "Id" ],
					ProjectId = (int) rdr[ "ProjectId" ],
					ContractType = rdr[ "ContractType" ] as string,
					CreatedBy = rdr[ "CreatedBy" ] as string,
					CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime ),
					UpdatedBy = rdr[ "UpdatedBy" ] as string,
					UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime )
				};

			list.Add( entity );
		}

		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveProjectContracts( int projectId, List<ProjectContract> list, string userId )
		{
			const string fieldName = "ContractType";
			var dtFields = SqlHelper.BuildVarcharTable(fieldName);
			SqlHelper.PopulateVarcharTable( fieldName, list, dtFields );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectContractSave", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddIntPara( projectId, "@ProjectId", sqlParams );
					SqlHelper.AddTablePara( dtFields, "@ContractTypeTable", "ContractTypeTableType", sqlParams );
					SqlHelper.AddVarcharPara( userId, "@CreatedBy", sqlParams );


					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}
	}
}