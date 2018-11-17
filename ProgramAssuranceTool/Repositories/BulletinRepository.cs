using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	/// Bulletin Repository
	/// </summary>
	public class BulletinRepository : PatRepository, IBulletinRepository
	{
		#region  IRepository Members

		/// <summary>
		/// To get all bulletin data based on its type and admin privilage.
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>
		/// a list of bulletin
		/// </returns>
        public IQueryable<Bulletin> GetAll(string bulletinType, bool isAdmin)
        {
            var gridSettings = new MvcJqGrid.GridSettings
            {
                IsSearch = false,
                PageSize = 99999999,
                PageIndex = 1,
                SortColumn = "EndDate",
                SortOrder = DataConstants.Descending
            };
            var results = GetAll(gridSettings, bulletinType, isAdmin);
            return results.AsQueryable();
        }

		  /// <summary>
		  /// To get all bulletin data based on its type and admin privilage and grid setting
		  /// </summary>
		  /// <param name="gridSettings">The grid settings.</param>
		  /// <param name="bulletinType">Type of the bulletin.</param>
		  /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		  /// <returns>
		  /// a list of bulletin
		  /// </returns>
        public List<Bulletin> GetAll(MvcJqGrid.GridSettings gridSettings, string bulletinType, bool isAdmin)
	    {
	        if (string.IsNullOrEmpty(gridSettings.SortColumn))
	        {
	            gridSettings.SortColumn = "EndDate";
                gridSettings.SortOrder = DataConstants.Descending;
	        }

	        if (string.IsNullOrEmpty(gridSettings.SortOrder))
	            gridSettings.SortOrder = DataConstants.Descending;

			var bulletinList = new List<Bulletin>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaBulletinGetAll", connection ) )
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
               SqlHelper.AddVarcharPara(bulletinType, "@bulletinType ", sqlParams);
               SqlHelper.AddBitPara(isAdmin, "@isAdmin", sqlParams);

					if (gridSettings.IsSearch && gridSettings.Where != null)
					{
						foreach ( var rule in gridSettings.Where.rules )
						{
							//  convert rule into a parameter

							if ( rule.field.IndexOf("Date", StringComparison.Ordinal) > -1 )
							{
								DateTime theDate;
								var isValid = AppHelper.ToDbDateTime(rule.data, out theDate);
								if (isValid)
								{
									SqlHelper.AddDatePara(theDate, "@" + rule.field, sqlParams);
								}
								else
								{
									return bulletinList;
								}
							}
							else
								SqlHelper.AddVarcharPara( rule.data, "@" + rule.field, sqlParams );
						}
					}

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadBulletinList( bulletinList, command );
				}
			}
			return bulletinList;
		}

		  /// <summary>
		  /// Gets the bulletin by its id.
		  /// </summary>
		  /// <param name="id">The bulletin id.</param>
		  /// <returns>
		  /// a bulletin
		  /// </returns>
		public  Bulletin GetById( int id )
		{
			Bulletin bulletin = null;
			var bulletinList = new List<Bulletin>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaBulletinGetById", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

               SqlHelper.AddIntPara(id, "@BulletinId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadBulletinList( bulletinList, command );
					if ( bulletinList.Count > 0 )
						bulletin = bulletinList[ 0 ];
				}
			}
			return bulletin;// as T;
		}

		/// <summary>
		/// Loads the bulletin list.
		/// </summary>
		/// <param name="bulletinList">The bulletin list.</param>
		/// <param name="command">The command.</param>
		private static void LoadBulletinList( ICollection<Bulletin> bulletinList, SqlCommand command )
		{
			SqlDataReader rdr = null;
			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddBulletinToList( bulletinList, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		/// <summary>
		/// Adds the bulletin to list.
		/// </summary>
		/// <param name="bulletinList">The bulletin list.</param>
		/// <param name="rdr">The RDR.</param>
		private static void AddBulletinToList( ICollection<Bulletin> bulletinList, IDataRecord rdr )
		{
			var bulletin = new Bulletin {BulletinId = (int) rdr["BulletinId"]};

			if ( ! rdr.IsDBNull( 1 ) )
				bulletin.ProjectId = (int) rdr[ "ProjectId" ];

			for ( int i = 0; i < rdr.FieldCount; i++ )
				Debug.WriteLine( "{0:0#} ={1} {2} DBNull={3}",
					i, rdr[ i ], rdr.GetFieldType( i ), rdr.IsDBNull( i ) );

			bulletin.BulletinTitle = string.Format("{0}", rdr["Title"]);
			bulletin.BulletinType = string.Format("{0}", rdr["BulletinType"]);
			bulletin.Description = string.Format("{0}", rdr["Description"]);

			if ( rdr[ "StartDate" ] != null )
				bulletin.StartDate = rdr[ "StartDate" ] as DateTime? ?? default( DateTime );

			if ( rdr[ "EndDate" ] != null )
				bulletin.EndDate   = rdr[ "EndDate" ] as DateTime? ?? default( DateTime );

			bulletin.UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime );
			bulletin.UpdatedBy = string.Format("{0}", rdr["UpdatedBy"]);
			bulletin.CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime );
			bulletin.CreatedBy = string.Format("{0}", rdr["CreatedBy"]);

			bulletinList.Add( bulletin );
		}

		/// <summary>
		/// Adds the specified bulletin.
		/// </summary>
		/// <param name="entity">The bulletin.</param>
		public void Add( Bulletin entity )
		{
			var bulletin = entity;
			Debug.Assert( bulletin != null, "bulletin != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaBulletinInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					if ( bulletin.ProjectId > 0  )
						SqlHelper.AddIntPara( bulletin.ProjectId, "@ProjectId", sqlParams );

					SqlHelper.AddVarcharPara( bulletin.BulletinTitle, "@Title", sqlParams );
					SqlHelper.AddVarcharPara( bulletin.Description, "@Description", sqlParams );
					SqlHelper.AddVarcharPara( bulletin.BulletinType, "@BulletinType", sqlParams );

					if ( bulletin.StartDate != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( bulletin.StartDate, "@StartDate", sqlParams );

					if ( bulletin.EndDate != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( bulletin.EndDate, "@EndDate", sqlParams );

					SqlHelper.AddDatePara( DateTime.Now, "@CreatedOn", sqlParams );
					SqlHelper.AddVarcharPara( bulletin.CreatedBy, "@CreatedBy", sqlParams );

					//  Output parameters
					var paramBulletinId = new SqlParameter( "@BulletinId", SqlDbType.Int )
					{
						Direction = ParameterDirection.InputOutput,
						Value = 0
					};
					sqlParams.Add( paramBulletinId );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) != 0 ) return;

					bulletin.BulletinId = (int) paramBulletinId.Value;
				}
			}
		}

		/// <summary>
		/// Updates the specified bulletin.
		/// </summary>
		/// <param name="entity">The bulletin.</param>
		public void Update( Bulletin entity )
		{
			var bulletin = entity;
			Debug.Assert( bulletin != null, "Bulletin != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaBulletinUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					SqlHelper.AddIntPara( bulletin.BulletinId, "@BulletinId", sqlParams );

					if ( bulletin.ProjectId > 0 )
						SqlHelper.AddIntPara( bulletin.ProjectId, "@ProjectId", sqlParams );

					SqlHelper.AddVarcharPara( bulletin.BulletinTitle, "@Title", sqlParams );
					SqlHelper.AddVarcharPara( bulletin.Description, "@Description", sqlParams );
					SqlHelper.AddVarcharPara( bulletin.BulletinType, "@BulletinType", sqlParams );

					if ( bulletin.StartDate != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( bulletin.StartDate, "@StartDate", sqlParams );
					if ( bulletin.EndDate != new DateTime( 1, 1, 1 ) )
						SqlHelper.AddDatePara( bulletin.EndDate, "@EndDate", sqlParams );

					//SqlHelper.AddVarcharPara( bulletin.CreatedBy, "@CreatedBy", sqlParams );
					//SqlHelper.AddDatePara( bulletin.CreatedOn, "@CreatedOn", sqlParams );

					SqlHelper.AddVarcharPara( bulletin.UpdatedBy, "@UpdatedBy", sqlParams );
					SqlHelper.AddDatePara( DateTime.Now, "@UpdatedOn", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}

		}

		/// <summary>
		/// Deletes the specified bulletin.
		/// </summary>
		/// <param name="id">The bulletin id.</param>
		public void Delete( int id )
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaBulletinDelete", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddIntPara( id, "@BulletinId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Counts the bulletins by its type and admin privilage.
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>
		/// number of bulletin matching those criterias
		/// </returns>
        public int Count(string bulletinType, bool isAdmin)
	    {
            return GetAll(bulletinType, isAdmin).Count();
	    }

		  /// <summary>
		  /// Counts the bulletin by its grid settings, bulletin type and admin privilage
		  /// </summary>
		  /// <param name="gridSettings">The grid settings.</param>
		  /// <param name="bulletinType">Type of the bulletin.</param>
		  /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		  /// <returns>
		  /// number of bulletin matching those criterias
		  /// </returns>
        public int Count(MvcJqGrid.GridSettings gridSettings, string bulletinType, bool isAdmin)
        {
            return GetAll(gridSettings, bulletinType, isAdmin).Count();
        }

        #endregion

	}
}
