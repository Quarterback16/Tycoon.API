using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO.Csv;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels;
using ProgramAssuranceTool.ViewModels.Project;
using Filter = MvcJqGrid.Filter;
using GridSettings = MvcJqGrid.GridSettings;
using Rule = MvcJqGrid.Rule;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   The class which accesses the Project data store
	/// </summary>
	public class ProjectRepository : PatRepository, IProjectRepository
	{
		#region IRepository Members

		/// <summary>
		/// Gets all the projects.
		/// </summary>
		/// <returns>
		/// a list of projects
		/// </returns>
		public IQueryable<Project> GetAll( DateTime uploadFrom, DateTime uploadTo )
		{
			var gridSettings = new GridSettings
				{
					IsSearch = false,
					PageSize = 99999999,
					PageIndex = 1,
					SortColumn = "ProjectId",
					SortOrder = "asc"
				};
			var results = GetAll( gridSettings, uploadFrom, uploadTo );
			return results.AsQueryable();
		}

		/// <summary>
		/// Gets all the projects mathing the grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadFrom"></param>
		/// <param name="uploadTo"></param>
		/// <returns>
		/// a list of projects
		/// </returns>
		public List<Project> GetAll( GridSettings gridSettings, DateTime uploadFrom, DateTime uploadTo )
		{
			if ( string.IsNullOrEmpty( gridSettings.SortColumn ) )
				gridSettings.SortColumn = "ProjectId";
			if ( string.IsNullOrEmpty( gridSettings.SortOrder ) )
				gridSettings.SortOrder = "ASC";

			var projectList = new List<Project>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectGetAll", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddVarcharPara( gridSettings.SortColumn, "@sortColumnName", sqlParams );
					SqlHelper.AddVarcharPara( gridSettings.SortOrder, "@sortOrder", sqlParams );
					SqlHelper.AddIntPara( gridSettings.PageIndex, "@pageIndex", sqlParams );
					SqlHelper.AddIntPara( gridSettings.PageSize, "@pageSize", sqlParams );

					if ( uploadFrom != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( uploadFrom, "@UploadDateFrom", sqlParams );

					if ( uploadTo != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( uploadTo, "@UploadDateTo", sqlParams );

					if (gridSettings.IsSearch && gridSettings.Where != null)
					{
						foreach ( var rule in gridSettings.Where.rules )
						{
							//  convert rule into a parameter

							if ( rule.field.IndexOf( "Date", StringComparison.Ordinal ) > -1 )
							{
								DateTime theDate;
								var isValid = AppHelper.ToDbDateTime(rule.data, out theDate);
								if (isValid)
								{
									SqlHelper.AddDatePara(theDate, "@" + rule.field, sqlParams);
								}
								else
								{
									return projectList;
								}
							}
							else
							{
								if (rule.field.Equals( "ProjectId" ))
								{
									var id = Regex.Replace( rule.data, @"[^\d]", "" );  //  make sure that user hasnt accidentally typed a non numeric
									int projectId;
									int.TryParse(id, out projectId);
									SqlHelper.AddIntPara(projectId, "@" + rule.field, sqlParams);
								}
								else
								{
									SqlHelper.AddVarcharPara( rule.data, "@" + rule.field, sqlParams );
									var opValue = rule.op.Equals( "eq" ) ? 1 : 0;
									SqlHelper.AddIntPara( opValue, "@" + rule.field + "Op", sqlParams );
								}
							}
						}
					}

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadProjectList( projectList, command );
				}
			}
			return projectList;
		}

		/// <summary>
		/// Gets the Project by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// Project object
		/// </returns>
		public Project GetById( int id )
		{
			Project project = null;
			var projectList = new List<Project>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectGet", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( id, "@ProjectId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadProjectList( projectList, command );
					if ( projectList.Count > 0 )
						project = projectList[ 0 ];
				}
			}
			return project; // as T;
		}

		private static void LoadProjectList( ICollection<Project> projectList, SqlCommand command )
		{
			SqlDataReader rdr = null;
			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddProjectToList( projectList, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddProjectToList( ICollection<Project> projectList, IDataRecord rdr )
		{
			var project = new Project { ProjectId = (int) rdr[ "ProjectId" ] };

			if ( !rdr.IsDBNull( 1 ) )
				project.ProjectId = (int) rdr[ "ProjectId" ];

			project.ProjectType = rdr[ "ProjectType" ] as string;
			project.ProjectName = rdr[ "ProjectName" ] as string;
			project.Coordinator = rdr[ "Coordinator" ] as string;
			project.Comments = rdr[ "Comments" ] as string;

			if ( SqlHelper.HasColumn( rdr, "OrgCode" ) )
				project.OrgCode = rdr[ "OrgCode" ] as string;
			if ( SqlHelper.HasColumn( rdr, "NSWandACT" ) )
				project.Resource_NSW_ACT = rdr[ "NSWandACT" ] as bool? ?? default( bool );

			project.Resource_NO = rdr[ "Resource_NO" ] as bool? ?? default( bool );
			project.Resource_QLD = rdr[ "Resource_QLD" ] as bool? ?? default( bool );
			project.Resource_NT = rdr[ "Resource_NT" ] as bool? ?? default( bool );
			project.Resource_WA = rdr[ "Resource_WA" ] as bool? ?? default( bool );
			project.Resource_SA = rdr[ "Resource_SA" ] as bool? ?? default( bool );
			project.Resource_TAS = rdr[ "Resource_TAS" ] as bool? ?? default( bool );
			project.Resource_VIC = rdr[ "Resource_VIC" ] as bool? ?? default( bool );

			project.UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime );
			project.UpdatedBy = rdr[ "UpdatedBy" ] as string;
			project.CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime );
			project.CreatedBy = rdr[ "CreatedBy" ] as string;

			projectList.Add( project );
		}

		/// <summary>
		/// Adds the specified Project.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Add( Project entity )
		{
			var project = entity;
			Debug.Assert( project != null, "project != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					if ( project.ProjectId > 0 )
						SqlHelper.AddIntPara( project.ProjectId, "@ProjectId", sqlParams );
					SqlHelper.AddVarcharPara( project.ProjectType, "@ProjectType", sqlParams );
					SqlHelper.AddVarcharPara( project.OrgCode, "@OrgCode", sqlParams );
					SqlHelper.AddVarcharPara( project.ProjectName, "@ProjectName", sqlParams );
					SqlHelper.AddVarcharPara( project.Coordinator, "@Coordinator", sqlParams );
					SqlHelper.AddVarcharPara( project.Comments, "@Comments", sqlParams );

					SqlHelper.AddDatePara( DateTime.Now, "@CreatedOn", sqlParams );
					SqlHelper.AddVarcharPara( project.CreatedBy, "@CreatedBy", sqlParams );

					SqlHelper.AddBitPara( project.Resource_NO, "@Resource_NO", sqlParams );
					SqlHelper.AddBitPara( project.Resource_NSW_ACT, "@Resource_NSW_ACT", sqlParams );
					SqlHelper.AddBitPara( project.Resource_QLD, "@Resource_QLD", sqlParams );
					SqlHelper.AddBitPara( project.Resource_NT, "@Resource_NT", sqlParams );
					SqlHelper.AddBitPara( project.Resource_WA, "@Resource_WA", sqlParams );
					SqlHelper.AddBitPara( project.Resource_SA, "@Resource_SA", sqlParams );
					SqlHelper.AddBitPara( project.Resource_TAS, "@Resource_TAS", sqlParams );
					SqlHelper.AddBitPara( project.Resource_VIC, "@Resource_VIC", sqlParams );

					//  Output parameters
					var paramId = new SqlParameter( "@ProjectId", SqlDbType.Int )
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

					project.ProjectId = (int) paramId.Value;
				}
			}
		}

		/// <summary>
		/// Updates the specified Project.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Update( Project entity )
		{
			var project = entity;
			Debug.Assert( project != null, "project != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					if ( project.ProjectId > 0 )
						SqlHelper.AddIntPara( project.ProjectId, "@ProjectId", sqlParams );

					SqlHelper.AddVarcharPara( project.ProjectType, "@ProjectType", sqlParams );
					SqlHelper.AddVarcharPara( project.ProjectName, "@ProjectName", sqlParams );
					SqlHelper.AddVarcharPara( project.OrgCode, "@OrgCode", sqlParams );
					SqlHelper.AddVarcharPara( project.Coordinator, "@Coordinator", sqlParams );
					SqlHelper.AddVarcharPara( project.Comments, "@Comments", sqlParams );

					SqlHelper.AddBitPara( project.Resource_NO, "@Resource_NO", sqlParams );
					SqlHelper.AddBitPara( project.Resource_NSW_ACT, "@Resource_NSW_ACT", sqlParams );
					SqlHelper.AddBitPara( project.Resource_QLD, "@Resource_QLD", sqlParams );
					SqlHelper.AddBitPara( project.Resource_NT, "@Resource_NT", sqlParams );
					SqlHelper.AddBitPara( project.Resource_WA, "@Resource_WA", sqlParams );
					SqlHelper.AddBitPara( project.Resource_SA, "@Resource_SA", sqlParams );
					SqlHelper.AddBitPara( project.Resource_TAS, "@Resource_TAS", sqlParams );
					SqlHelper.AddBitPara( project.Resource_VIC, "@Resource_VIC", sqlParams );

					SqlHelper.AddVarcharPara( project.UpdatedBy, "@UpdatedBy", sqlParams );
					SqlHelper.AddDatePara( DateTime.Now, "@UpdatedOn", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Deletes the specified Project.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete( int id )
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectDelete", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddIntPara( id, "@ProjectId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}
		}

		#endregion IRepository Members

		#region Extras

		/// <summary>
		/// Gets all project types.
		/// </summary>
		/// <returns>array of Project Types</returns>
		public string[] GetAllProjectTypes()
		{
			return GetDistinct( "PaProjectProjectTypesDistinct", "ProjectType" );
		}

		private string[] GetDistinct( string sprocName, string targetField )
		{
			var projectList = new List<Project>();

			using ( var sqlConnection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, sqlConnection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					sqlConnection.Open();

					LoadCoordinatorList( projectList, command, targetField );
				}
			}
			return projectList.Select( x => x.Coordinator ).ToArray();
		}

		private static void LoadCoordinatorList( ICollection<Project> projectList, SqlCommand command, string targetField )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddCoordinatorToList( projectList, rdr, targetField );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddCoordinatorToList( ICollection<Project> projectList, IDataRecord rdr, string targetField )
		{
			var project = new Project
				{
					Coordinator = rdr[ targetField ] as string
				};
			projectList.Add( project );
		}

		/// <summary>
		/// Counts the number of projects.
		/// </summary>
		/// <returns>
		/// the number of projects
		/// </returns>
		public int CountProjects()
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectCount", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

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

		/// <summary>
		/// Determines whether [is unique project name] [the specified project name].
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public bool IsUniqueProjectName( string projectName, int projectId )
		{
			return !IsProjectNameUsed( projectName, projectId );
		}

		/// <summary>
		/// Determines whether the project name is used.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// used or not
		/// </returns>
		public bool IsProjectNameUsed( string projectName, int projectId )
		{
			var projectList = FindProjectsByName( projectName );
			var isUsed = projectList.Any( project => project.ProjectId != projectId );
			return isUsed;
		}

		#region Project Questions

		private static PatQuestion CreateQuestion( BaseViewModel vm )
		{
			var quest = new PatQuestion
				{
					CreatedOn = DateTime.Now,
					ProjectId = vm.ProjectId
				};
			return quest;
		}

		/// <summary>
		/// Stores the question data.
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		public int StoreQuestionData( UploadQuestionsViewModel vm, Stream stream )
		{
			var list = new List<PatQuestion>();

			using ( var csv = new CsvReader( new StreamReader( stream ), true ) )
			{
				//  load an array with the column headers
				var columns = csv.GetFieldHeaders();

				//  for each line of the spreadsheet
				while ( csv.ReadNextRecord() )
				{
					if ( CsvHelper.EmptyLine( csv, columns.Length ) ) continue;

					//  process the single row
					var question = CreateQuestion( vm );

					//for each column of the row
					for ( var c = 0; c < columns.Length; c++ )
					{
						var colName = columns[ c ];

						var val = csv[ c ];

						if ( string.IsNullOrEmpty( val ) ) continue;

						switch ( colName.ToUpper() )
						{
							case CommonConstants.QuestionColumn_QuestionType:
								question.Type = val;
								break;

							case CommonConstants.QuestionColumn_QuestionText:
								question.Text = val;
								break;

							case CommonConstants.QuestionColumn_AnswerColumn:
								question.AnswerColumn = val;
								break;
						}
					}
					list.Add( question );
				}

				InsertAll( list );
				return list.Count;
			}
		}

		/// <summary>
		/// Inserts all from a collection of Questions.
		/// </summary>
		/// <param name="list">The list.</param>
		public void InsertAll( List<PatQuestion> list )
		{
			var dtFields = SqlHelper.BuildQuestionTable();
			SqlHelper.PopulateQuestionTable( list, dtFields );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectQuestionsUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddTablePara( dtFields, "@ProjectQuestions", "ProjectQuestionsTableType", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets the project questions for the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public List<PatQuestion> GetProjectQuestions( int projectId )
		{
			var list = new List<PatQuestion>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectQuestionGetByProject", connection ) )
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

					LoadQuestionList( list, command );
				}
			}
			return list;
		}

		private static void LoadQuestionList( ICollection<PatQuestion> list, SqlCommand command )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddQuestionToList( list, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddQuestionToList( ICollection<PatQuestion> list, IDataRecord rdr )
		{
			var entity = new PatQuestion
			{
				ProjectQuestionId = (int) rdr[ "ProjectQuestionId" ],
				ProjectId = (int) rdr[ "ProjectId" ],
				Type = rdr[ "QuestionType" ] as string,
				Text = rdr[ "QuestionText" ] as string,
				AnswerColumn = rdr[ "AnswerColumn" ] as string,
				CreatedBy = rdr[ "CreatedBy" ] as string,
				CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime ),
				UpdatedBy = rdr[ "UpdatedBy" ] as string,
				UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime )
			};

			list.Add( entity );
		}

		#endregion Project Questions

		private IEnumerable<Project> FindProjectsByName( string projectName )
		{
			var myGs = new GridSettings
				{
					Where = ProjectNameRule( projectName ),
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};

			var projectList = GetAll( myGs, new DateTime(1,1,1), new DateTime(1,1,1)  );
			return projectList;
		}

		private static
			Filter ProjectNameRule
			( string
				  projectName )
		{
			var whereFilter = new Filter { groupOp = "AND" };
			var rules = new List<Rule> { new Rule { field = "ProjectName", data = projectName, op = "ET" } };
			whereFilter.rules = rules.ToArray();
			return whereFilter;
		}

		#endregion Extras
	}
}