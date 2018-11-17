using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   A data repository for the AUdit trail
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UserActivityRepository<T> : PatRepository where T : class
	{
		/// <summary>
		///   Adds an audit object.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Add( T entity )
		{
			var userActivity = entity as UserActivity;
			Debug.Assert( userActivity != null, "userActivity != null" );
			if ( userActivity.UserId == null ) return;

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserActivityInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					var paramEventId = new SqlParameter( "@ActivityId", SqlDbType.Int )
					{
						Direction = ParameterDirection.InputOutput,
						Value = 0
					};
					sqlParams.Add( paramEventId );

					if ( userActivity.UserId.Length > 10 )
						userActivity.UserId = userActivity.UserId.Substring( 0, 10 );
					if ( userActivity.CreatedBy == null )
						userActivity.CreatedBy = userActivity.UserId.Length > 10
															 ? userActivity.UserId.Substring( 0, 10 )
															 : userActivity.UserId;
					else
					{
						if ( userActivity.CreatedBy.Length > 10 )
							userActivity.CreatedBy = userActivity.CreatedBy.Substring( 0, 10 );
					}
					if ( userActivity.Activity.Length > 200 )
						userActivity.Activity = userActivity.Activity.Substring( 0, 200 );

					SqlHelper.AddVarcharPara( userActivity.UserId, "@UserId", sqlParams );
					SqlHelper.AddVarcharPara( userActivity.Activity, "@Activity", sqlParams );
					SqlHelper.AddVarcharPara( userActivity.CreatedBy, "@CreatedBy", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) == 0 )
						userActivity.ActivityId = (int) paramEventId.Value;
				}
			}
		}

		/// <summary>
		///   Method for cleaning out the old records in the audit trail
		/// </summary>
		/// <param name="keepDate">The keep date.</param>
		public void DeletePriorTo( DateTime keepDate )
		{
			if ( keepDate.Equals( new DateTime( 1, 1, 1 ) ) )
				keepDate = DateTime.Now.Subtract( new TimeSpan( 30, 0, 0, 0 ) );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserActivityDelete", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddDatePara( keepDate, "@keepFrom", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets all the audit records based on the selection criteria.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="from">From date time.</param>
		/// <param name="to">To date time.</param>
		/// <returns>List of audit records</returns>
		public List<UserActivity> GetAll( string userId, DateTime from, DateTime to )
		{
			if ( string.IsNullOrEmpty( userId ) ) userId = String.Empty;
			if ( from.Equals( new DateTime( 1, 1, 1 ) ) ) from = new DateTime( 2011, 1, 1 );
			if ( to.Equals( new DateTime( 1, 1, 1 ) ) ) to = DateTime.Now;

			var activityList = new List<UserActivity>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaUserActivityGet", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					if ( !string.IsNullOrEmpty( userId ) )
						SqlHelper.AddVarcharPara( userId, "@UserId", sqlParams );
					SqlHelper.AddDatePara( from, "@CreatedFrom", sqlParams );
					SqlHelper.AddDatePara( to, "@CreatedTo", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					SqlDataReader rdr = null;

					try
					{
						rdr = command.ExecuteReader();

						while ( rdr.Read() )
							AddActivityToList( activityList, rdr );
					}
					finally
					{
						if ( rdr != null )
							rdr.Close();
					}
				}
			}
			return activityList;
		}

		private static void AddActivityToList( ICollection<UserActivity> activityList, IDataRecord rdr )
		{
			var activity = new UserActivity
			{
				ActivityId = (int) rdr[ "ActivityId" ],
				UserId = rdr[ "UserId" ] as string,
				Activity = rdr[ "Activity" ] as string,
				CreatedOn = (DateTime) rdr[ "CreatedOn" ]
			};
			activityList.Add( activity );
		}
	}
}