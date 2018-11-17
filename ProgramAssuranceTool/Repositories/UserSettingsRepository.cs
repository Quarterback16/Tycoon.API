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
	///   The class responsible for talking to the User Settings data store
	/// </summary>
	public class UserSettingsRepository : PatRepository, IUserSettingsRepository
	{
		/// <summary>
		/// Adds the specified User Setting.
		/// </summary>
		/// <param name="entity">The user setting.</param>
		public void Add( UserSetting entity )
		{
			var userSetting = entity;
			Debug.Assert( userSetting != null, "userSetting != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserSettingsInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					SqlHelper.AddVarcharPara( userSetting.UserId, "@UserId", sqlParams );
					SqlHelper.AddVarcharPara( userSetting.Name, "@Name", sqlParams );
					SqlHelper.AddVarcharPara( userSetting.SerialiseAs, "@SerialiseAs", sqlParams );
					SqlHelper.AddVarcharPara( userSetting.Value, "@Value", sqlParams );
					SqlHelper.AddVarcharPara( userSetting.CreatedBy, "@CreatedBy", sqlParams );

					//  Output parameters
					var paramUserSettingsId = new SqlParameter( "@Id", SqlDbType.Int )
						{
							Direction = ParameterDirection.InputOutput,
							Value = 0
						};
					sqlParams.Add( paramUserSettingsId );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) != 0 ) return;

					userSetting.Id = (int) paramUserSettingsId.Value;
				}
			}
		}

		/// <summary>
		/// Inserts all.
		/// </summary>
		/// <param name="list">The list.</param>
		public void InsertAll( List<UserSetting> list )
		{
			var dtFields = SqlHelper.BuildSettingTable();
			SqlHelper.PopulateSettingsTable( list, dtFields );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserSettingsInsertAll", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddTablePara( dtFields, "@UserSettings", "UserSettingsTableType", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets all the settings for a particular user.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns>
		/// list of settings
		/// </returns>
		public IEnumerable<UserSetting> GetAllByUserId( string userId )
		{
			var list = new List<UserSetting>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserSettingsGetAll", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddVarcharPara( userId, "@userId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadUserSettingsList( list, command );
				}
			}
			return list;
		}

		private static void LoadUserSettingsList( ICollection<UserSetting> list, SqlCommand command )
		{
			SqlDataReader rdr = null;
			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddUserSettingToList( list, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddUserSettingToList( ICollection<UserSetting> list, IDataRecord rdr )
		{
			var userSetting = new UserSetting
				{
					Id = (int) rdr["Id"], 
					UserId = rdr["UserId"] as string, 
					Name = rdr["Name"] as string, 
					SerialiseAs = rdr["SerialiseAs"] as string, 
					Value = rdr["Value"] as string, 
					UpdatedOn = rdr["UpdatedOn"] as DateTime? ?? default(DateTime), 
					UpdatedBy = rdr["UpdatedBy"] as string, 
					CreatedOn = rdr["CreatedOn"] as DateTime? ?? default(DateTime), 
					CreatedBy = rdr["CreatedBy"] as string
				};

			//for ( var i = 0; i < rdr.FieldCount; i++ )
			//	Debug.WriteLine( "{0:0#} {1}={2} {3} DBNull={4}",
			//		i, rdr.GetName( i ).PadRight( 20 ), rdr[ i ], rdr.GetFieldType( i ), rdr.IsDBNull( i ) );

			list.Add( userSetting );
		}

		/// <summary>
		/// Deletes all the settings for a user.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		public void DeleteAll( string userId )
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserSettingsDeleteAll", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddVarcharPara( userId, "@UserId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}
	}
}