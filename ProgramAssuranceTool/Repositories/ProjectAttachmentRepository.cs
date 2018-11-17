using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	/// Project Attachment Repository
	/// </summary>
	public class ProjectAttachmentRepository : PatRepository, IProjectAttachmentRepository
	{
		/// <summary>
		/// Stores the attachment.
		/// </summary>
		/// <param name="fileData">The file data.</param>
		/// <param name="attachmentId">The attachment identifier.</param>
		public void StoreAttachment( byte[] fileData, int attachmentId )
		{

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectAttachmentStore", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};

					#region  Set param AttachmentId

					sqlParams.Add( paramReturnValue );
					var paramProjectId = new SqlParameter( "@Id", SqlDbType.Int )
					{
						Direction = ParameterDirection.Input,
						Value = attachmentId
					};
					sqlParams.Add( paramProjectId );

					#endregion

					SqlHelper.AddVarbinaryPara( fileData, "@Attachment", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

				}
			}
		}

		/// <summary>
		/// Retrieves the attachment.
		/// </summary>
		/// <param name="attachmentId">The attachment identifier.</param>
		/// <returns></returns>
		public byte[] RetrieveAttachment( int attachmentId )
		{
			var filedata = new byte[1024];
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectAttachmentRetrieve", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};

					#region  Set param AttachmentId

					sqlParams.Add( paramReturnValue );
					var paramProjectId = new SqlParameter( "@Id", SqlDbType.Int )
					{
						Direction = ParameterDirection.Input,
						Value = attachmentId
					};
					sqlParams.Add( paramProjectId );

					#endregion

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					var dst = new DataSet();
					var dad = new SqlDataAdapter( command );
					dad.Fill( dst );
					if ( dst.Tables[ 0 ].Rows.Count > 0 )
					{
						var item = dst.Tables[ 0 ].Rows[ 0 ];

						//document.Id = (int) item[ "Id" ];
						filedata = item[ "Attachment" ] as byte[];
					}
				}
			}
			return filedata;
		}

		/// <summary>
		/// Gets all project attachments by its project id
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
        public IQueryable<ProjectAttachment> GetAll(int projectId)
        {
            var gridSettings = new MvcJqGrid.GridSettings
            {
                IsSearch = false,
                PageSize = 99999999,
                PageIndex = 1,
                SortColumn = "Id",
                SortOrder = DataConstants.Descending
            };
            var results = GetAll(gridSettings, projectId);
            return results.AsQueryable();
        }

		  /// <summary>
		  /// Gets all project attachment by its grid setting and project id
		  /// </summary>
		  /// <param name="gridSettings">The grid settings.</param>
		  /// <param name="projectId">The project identifier.</param>
		  /// <returns></returns>
	    public List<ProjectAttachment> GetAll(GridSettings gridSettings, int projectId)
	    {
            if (string.IsNullOrEmpty(gridSettings.SortColumn))
            {
                gridSettings.SortColumn = "Id";
                gridSettings.SortOrder = DataConstants.Descending;
            }

            if (string.IsNullOrEmpty(gridSettings.SortOrder))
                gridSettings.SortOrder = DataConstants.Descending;

            var projectAttachmentList = new List<ProjectAttachment>();

            using (var connection = new SqlConnection(DbConnection))
            {
                using (var command = new SqlCommand("PaProjectAttachmentGetAllByProject", connection))
                {
                    var sqlParams = new List<SqlParameter>();
                    var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(paramReturnValue);

                    //also get the count back
                    var totalRecord = new SqlParameter("@TotalRecs", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = 0
                    };

                    sqlParams.Add(totalRecord);

                    SqlHelper.AddIntPara(projectId, "@ProjectId", sqlParams);
                    SqlHelper.AddVarcharPara(gridSettings.SortColumn, "@sortColumnName", sqlParams);
                    SqlHelper.AddVarcharPara(gridSettings.SortOrder, "@sortOrder", sqlParams);
                    SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
                    SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);

						  if (gridSettings.IsSearch && gridSettings.Where != null)
                    {
                        foreach (var rule in gridSettings.Where.rules)
                        {
                            //  convert rule into a parameter
                            SqlHelper.AddVarcharPara(rule.data, "@" + rule.field, sqlParams);
                        }
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParams.ToArray());
                    connection.Open();

                    LoadProjectAttachmentList(projectAttachmentList, command);

						 /*
                    if (((Int32)command.Parameters["@return_value"].Value) == 0)
                    {
                        var totalCount = (int)totalRecord.Value;
                    }
						  */ 
                }
            }
            return projectAttachmentList;

	    }

		 /// <summary>
		 /// Loads the project attachment list.
		 /// </summary>
		 /// <param name="projectAttachmentList">The project attachment list.</param>
		 /// <param name="command">The command.</param>
        private static void LoadProjectAttachmentList(ICollection<ProjectAttachment> projectAttachmentList, SqlCommand command)
        {
            SqlDataReader rdr = null;
            try
            {
                rdr = command.ExecuteReader();

                while (rdr.Read())
                    AddProjectAttachmentToList(projectAttachmentList, rdr);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
            }
        }

		  /// <summary>
		  /// Adds the project attachment to list.
		  /// </summary>
		  /// <param name="projectAttachmentList">The project attachment list.</param>
		  /// <param name="rdr">The RDR.</param>
        private static void AddProjectAttachmentToList(ICollection<ProjectAttachment> projectAttachmentList, IDataRecord rdr)
        {
            var projectAttachment = new ProjectAttachment
	            {
						Id = AppHelper.ToInt(rdr["Id"]), 
						ProjectId = AppHelper.ToInt(rdr["ProjectId"]), 
						DocumentName = string.Format("{0}", rdr["DocumentName"]),
						Description = string.Format("{0}", rdr["Description"]),
						Url = string.Format("{0}", rdr["Url"]),
						CreatedOn = rdr["CreatedOn"] as DateTime? ?? default(DateTime), 
						CreatedBy = string.Format("{0}", rdr["CreatedBy"]),
						UpdatedOn = rdr["UpdatedOn"] as DateTime? ?? default(DateTime)
	            };

			  projectAttachment.CreatedBy = rdr["CreatedBy"] as string;

            projectAttachmentList.Add(projectAttachment);
        }

		  /// <summary>
		  /// Inserts the specified entity.
		  /// </summary>
		  /// <param name="entity">The entity.</param>
		  /// <param name="fileData">The file data.</param>
		  /// <exception cref="System.ArgumentNullException">fileData</exception>
        public void Insert(ProjectAttachment entity, byte[] fileData)
        {
            if (fileData == null) throw new ArgumentNullException("fileData");

            using (var connection = new SqlConnection(DbConnection))
            {
                using (var command = new SqlCommand("PaProjectAttachementInsert", connection))
                {
                    var sqlParams = new List<SqlParameter>();

                    var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(paramReturnValue);

                    var documentId = new SqlParameter("@Id", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = 0
                        };

                    sqlParams.Add(documentId);

                    SqlHelper.AddIntPara(entity.ProjectId, "@ProjectId", sqlParams);
                    SqlHelper.AddVarcharPara(entity.DocumentName, "@DocumentName", sqlParams);
                    SqlHelper.AddVarcharPara(entity.Description, "@Description", sqlParams);
                    SqlHelper.AddVarcharPara(entity.Url, "@Url", sqlParams);
                    SqlHelper.AddVarbinaryPara(fileData, "@Attachment", sqlParams);
                    SqlHelper.AddVarcharPara(entity.CreatedBy, "@CreatedBy", sqlParams);
                    SqlHelper.AddDatePara(DateTime.Now, "@CreatedOn", sqlParams);
                    SqlHelper.AddVarcharPara(entity.UpdatedBy, "@UpdatedBy", sqlParams);
                    SqlHelper.AddDatePara(DateTime.Now, "@UpdatedOn", sqlParams);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParams.ToArray());

                    connection.Open();

                    command.ExecuteNonQuery();

                    if (((Int32)command.Parameters["@return_value"].Value) != 0) return;

                    entity.Id = (int)documentId.Value;
                }
            }
        }

		  /// <summary>
		  /// Updates the specified entity.
		  /// </summary>
		  /// <param name="entity">The entity.</param>
	    public void Update(ProjectAttachment entity)
	    {
            var projectAttachment = entity;

            using (var connection = new SqlConnection(DbConnection))
            {
                using (var command = new SqlCommand("PaProjectAttachementUpdate", connection))
                {
                    var sqlParams = new List<SqlParameter>();

                    SqlHelper.AddReturnPara("@return_value", sqlParams);

                    SqlHelper.AddIntPara(projectAttachment.Id, "@Id", sqlParams);
                    SqlHelper.AddIntPara(projectAttachment.ProjectId, "@ProjectId", sqlParams);
                    SqlHelper.AddVarcharPara(projectAttachment.DocumentName, "@DocumentName", sqlParams);
                    SqlHelper.AddVarcharPara(projectAttachment.Description, "@Description", sqlParams);
                    SqlHelper.AddVarcharPara(projectAttachment.Url, "@Url", sqlParams);
                    SqlHelper.AddVarcharPara(projectAttachment.UpdatedBy, "@UpdatedBy", sqlParams);
                    SqlHelper.AddDatePara(DateTime.Now, "@UpdatedOn", sqlParams);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParams.ToArray());

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
	    }

		 /// <summary>
		 /// Gets Project attachment  by identifier.
		 /// </summary>
		 /// <param name="id">The project attachment Id.</param>
		 /// <returns></returns>
        public ProjectAttachment GetById(int id)
        {
            ProjectAttachment projectAttachment = null;
            var projectAttachmentList = new List<ProjectAttachment>();
            using (var connection = new SqlConnection(DbConnection))
            {
                using (var command = new SqlCommand("PaProjectAttachementGet", connection))
                {
                    var sqlParams = new List<SqlParameter>();

                    var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(paramReturnValue);

                    SqlHelper.AddIntPara(id, "@Id", sqlParams);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParams.ToArray());
                    connection.Open();

                    LoadProjectAttachmentList(projectAttachmentList, command);

                    if (projectAttachmentList.Count > 0)
                        projectAttachment = projectAttachmentList[0];
                }
            }
            return projectAttachment;// as T;
        }

		  /// <summary>
		  /// Counts the  project attachment by its project id.
		  /// </summary>
		  /// <param name="projectId">The project identifier.</param>
		  /// <returns></returns>
	    public int Count(int projectId)
	    {
            return GetAll(projectId).Count();
	    }

		 /// <summary>
		 /// Counts the project attachment by its project id and  grid settings.
		 /// </summary>
		 /// <param name="gridSettings">The grid settings.</param>
		 /// <param name="projectId">The project identifier.</param>
		 /// <returns></returns>
	    public int Count(GridSettings gridSettings, int projectId)
	    {
            return GetAll(gridSettings, projectId).Count();
        }

		 /// <summary>
		 /// Deletes the specified attachment.
		 /// </summary>
		 /// <param name="id">The project attachment identifier.</param>
	    public void Delete(int id)
	    {
            using (var connection = new SqlConnection(DbConnection))
            {
                using (var command = new SqlCommand("PaProjectAttachementDelete", connection))
                {
                    var sqlParams = new List<SqlParameter>();

                    var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(paramReturnValue);
                    SqlHelper.AddIntPara(id, "@Id", sqlParams);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParams.ToArray());

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
	    }

	}
}
