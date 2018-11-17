using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   The class responsible for talking to the Upload Data Store
	/// </summary>
	public class UploadRepository : PatRepository, IUploadRepository
	{
		#region IRepository Members

		/// <summary>
		/// Gets all the uploads.
		/// </summary>
		/// <returns>
		/// a list of uploads
		/// </returns>
		public List<Upload> GetAll()
		{
			var list = new List<Upload>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadGetAll", connection ) )
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

					LoadList( list, command );
				}
			}
			return list;
		}

		/// <summary>
		/// Gets the Upload by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// upload record
		/// </returns>
		public Upload GetById( int id )
		{
			Upload upload = null;
			var uploadList = new List<Upload>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadGet", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( id, "@Id", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( uploadList, command );

					if ( uploadList.Count > 0 )
						upload = uploadList[ 0 ];
				}
			}
			return upload;
		}

		/// <summary>
		/// Updates the specified upload object.
		/// </summary>
		/// <param name="entity">The upload.</param>
		public void Update( Upload entity )
		{
			var upload = entity;
			Debug.Assert( upload != null, "upload != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( upload.UploadId, "@Id", sqlParams );
					SqlHelper.AddIntPara( upload.ProjectId, "@ProjectId", sqlParams );

					SqlHelper.AddDatePara( upload.DateUploaded, "@DateUploaded", sqlParams );

					SqlHelper.AddVarcharPara( upload.SourceFile, "@SourceFile", sqlParams );

					SqlHelper.AddIntPara( upload.Rows, "@Rows", sqlParams );
					SqlHelper.AddVarcharPara( upload.Status, "@Status", sqlParams );

					SqlHelper.AddVarcharPara( upload.Name, "@Name", sqlParams );
					SqlHelper.AddVarcharPara( upload.UploadedBy, "@UploadedBy", sqlParams );
					SqlHelper.AddBitPara( upload.IncludesOutcomes, "@IncludesOutcomes", sqlParams );
					SqlHelper.AddBitPara( upload.AdditionalReview, "@AdditionalReview", sqlParams );
					SqlHelper.AddBitPara( upload.InScope, "@InScope", sqlParams );
					SqlHelper.AddBitPara( upload.AcceptedFlag, "@AcceptedFlag", sqlParams );
					SqlHelper.AddBitPara( upload.RandomFlag, "@RandomFlag", sqlParams );
					SqlHelper.AddBitPara( upload.NationalFlag, "@NationalFlag", sqlParams );
					SqlHelper.AddVarcharPara( upload.ServerFile, "@ServerFile", sqlParams );

					SqlHelper.AddNullableDateParameter( upload.DueDate, "@DueDate", sqlParams );

					SqlHelper.AddVarcharPara( upload.UpdatedBy, "@UpdatedBy", sqlParams );
					SqlHelper.AddDatePara( DateTime.Now, "@UpdatedOn", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		public void Add( Upload upload )
		{
			Debug.Assert( upload != null, "upload != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					//also get the Id back
					var paramUploadId = new SqlParameter( "@UploadId", SqlDbType.Int )
						{
							Direction = ParameterDirection.InputOutput,
							Value = 0
						};
					sqlParams.Add( paramUploadId );

					SqlHelper.AddIntPara( upload.ProjectId, "@ProjectId", sqlParams );
					SqlHelper.AddDatePara( upload.DateUploaded, "@DateUploaded", sqlParams );

					SqlHelper.AddVarcharPara( upload.SourceFile, "@SourceFile", sqlParams );

					SqlHelper.AddVarcharPara( upload.Name, "@Name", sqlParams );

					SqlHelper.AddVarcharPara( upload.UploadedBy, "@UploadedBy", sqlParams );
					SqlHelper.AddBitPara( upload.IncludesOutcomes, "@IncludesOutcomes", sqlParams );
					SqlHelper.AddBitPara( upload.AdditionalReview, "@AdditionalReview", sqlParams );
					SqlHelper.AddBitPara( upload.InScope, "@InScope", sqlParams );

					SqlHelper.AddBitPara( upload.AcceptedFlag, "@AcceptedFlag", sqlParams );
					SqlHelper.AddBitPara( upload.RandomFlag, "@RandomFlag", sqlParams );
					SqlHelper.AddBitPara( upload.NationalFlag, "@NationalFlag", sqlParams );

					SqlHelper.AddNullableDateParameter( upload.DueDate, "@DueDate", sqlParams );

					SqlHelper.AddVarcharPara( upload.ServerFile, "@ServerFile", sqlParams );
					SqlHelper.AddVarcharPara( upload.CreatedBy, "@CreatedBy", sqlParams );
					SqlHelper.AddVarcharPara( upload.UpdatedBy, "@UpdatedBy", sqlParams );
					SqlHelper.AddDatePara( upload.CreatedOn, "@CreatedOn", sqlParams );
					SqlHelper.AddDatePara( upload.UpdatedOn, "@UpdatedOn", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) != 0 ) return;
					upload.UploadId = (int) paramUploadId.Value;
				}
			}
		}

		public void Delete( int id )
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadDelete", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddIntPara( id, "@Id", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		#endregion IRepository Members

		#region Queries

		/// <summary>
		/// Gets all by project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public List<Upload> GetAllByProjectId( int projectId )
		{
			var gs = new MvcJqGrid.GridSettings
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
		public List<Upload> GetAllByProjectId( int projectId, MvcJqGrid.GridSettings gridSettings )
		{
			if ( gridSettings.SortColumn == null )
				gridSettings.SortColumn = "DateUploaded";

			if ( gridSettings.SortColumn.Equals( "UploadDateShort" ) )
				gridSettings.SortColumn = "DateUploaded";

			var uploadList = new List<Upload>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUploadGetAllByProject", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( projectId, "@ProjectId", sqlParams );
					SqlHelper.AddVarcharPara( gridSettings.SortColumn, "@sortColumnName", sqlParams );
					SqlHelper.AddVarcharPara( gridSettings.SortOrder, "@sortOrder", sqlParams );
					SqlHelper.AddIntPara( gridSettings.PageSize, "@pageSize", sqlParams );
					SqlHelper.AddIntPara( gridSettings.PageIndex, "@startingAt", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( uploadList, command );
				}
			}

			return uploadList;
		}

		private static void LoadList( ICollection<Upload> uploadList, SqlCommand command )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddUploadToList( uploadList, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddUploadToList( ICollection<Upload> uploadList, IDataRecord rdr )
		{
			var upload = new Upload
				{
					UploadId = (int) rdr[ "UploadId" ],
					ProjectId = (int) rdr[ "ProjectId" ],
					Name = rdr[ "Name" ] as string
				};

			if ( SqlHelper.HasColumn( rdr, "RandomFlag" ) )
				upload.RandomFlag = (bool) rdr[ "RandomFlag" ];
			if ( SqlHelper.HasColumn( rdr, "AcceptedFlag" ) )
				upload.AcceptedFlag = (bool) rdr[ "AcceptedFlag" ];
			if (SqlHelper.HasColumn( rdr, "NationalFlag" ))
			{
				if (rdr[ "NationalFlag" ] == null)
					upload.NationalFlag = false;
				else
				   upload.NationalFlag = (bool) rdr[ "NationalFlag" ];
			}

			upload.DateUploaded = rdr[ "DateUploaded" ] as DateTime? ?? default( DateTime );
			upload.DueDate = rdr[ "DueDate" ] as DateTime? ?? default( DateTime );
			upload.SourceFile = rdr[ "SourceFile" ] as string;
			upload.Rows = rdr[ "Rows" ] as int? ?? default( int );
			upload.Status = rdr[ "Status" ] as string;
			upload.UploadedBy = rdr[ "UploadedBy" ] as string;
			upload.IncludesOutcomes = (bool) rdr[ "IncludesOutcomes" ];
			upload.AdditionalReview = (bool) rdr[ "AdditionalReview" ];
			upload.InScope = (bool) rdr[ "InScope" ];
			upload.ServerFile = rdr[ "ServerFile" ] as string;
			upload.CreatedBy = rdr[ "CreatedBy" ] as string;
			upload.CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime );
			upload.UpdatedBy = rdr[ "UpdatedBy" ] as string;
			upload.UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime );
			uploadList.Add( upload );
		}

		/// <summary>
		/// Counts the number of uploads for a project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// the count
		/// </returns>
		public int CountUploads( int projectId )
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectUploadCountById", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( projectId, "@ProjectId", sqlParams );

					//also get the count back
					var paramCount = new SqlParameter( "@Count", SqlDbType.Int )
						{
							Direction = ParameterDirection.InputOutput,
							Value = 0
						};
					sqlParams.Add( paramCount );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) == 0 )
						theCount = (int) paramCount.Value;
				}
			}
			return theCount;
		}

		#endregion Queries
	}
}