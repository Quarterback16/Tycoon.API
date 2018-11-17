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
	///   The interface into the Sample data store
	/// </summary>
	public class SampleRepository : PatRepository, ISampleRepository
	{
		/// <summary>
		/// Gets the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Sample record</returns>
		public Sample GetById( int id )
		{
			Sample sample = null;
			var sampleList = new List<Sample>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaSampleGet", connection ) )
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

					LoadList( sampleList, command );
					if ( sampleList.Count > 0 )
						sample = sampleList[ 0 ];
				}
			}
			return sample;// as T;
		}

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Add( Sample entity )
		{
			var sample = entity;
			Debug.Assert( sample != null, "sample != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaSampleInsert", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					SqlHelper.AddVarcharPara( sample.SessionKey, "@SessionKey", sqlParams );

					SqlHelper.AddVarcharPara( sample.ClaimTypeDescription, "@ClaimTypeDescription", sqlParams );
					SqlHelper.AddVarcharPara( sample.ContractTypeDescription, "@ContractTypeDescription", sqlParams );
					SqlHelper.AddVarcharPara( sample.SiteDescription, "@SiteDescription", sqlParams );
					SqlHelper.AddVarcharPara( sample.EsaDescription, "@EsaDescription", sqlParams );
					SqlHelper.AddVarcharPara( sample.OrgDescription, "@OrgDescription", sqlParams );

					SqlHelper.AddBigIntPara( sample.ClaimId, "@ClaimId", sqlParams );
					SqlHelper.AddIntPara( sample.ClaimSequenceNumber, "@ClaimSequenceNumber", sqlParams );
					SqlHelper.AddVarcharPara( sample.ClaimType, "@ClaimType", sqlParams );
					SqlHelper.AddMoneyPara( sample.ClaimAmount, "@ClaimMoney", sqlParams );

					SqlHelper.AddVarcharPara( sample.SiteCode, "@SiteCode", sqlParams );
					SqlHelper.AddVarcharPara( sample.SupervisingSiteCode, "@SupervisingSiteCode", sqlParams );
					SqlHelper.AddVarcharPara( sample.OrgCode, "@OrgCode", sqlParams );

					SqlHelper.AddBigIntPara( sample.ActivityId, "@ActivityId", sqlParams );
					SqlHelper.AddDatePara( sample.ClaimCreationDate, "@ClaimCreationDate", sqlParams );

					SqlHelper.AddVarcharPara( sample.StatusCode, "@ClaimStatusCode", sqlParams );
					SqlHelper.AddVarcharPara( sample.StatusCodeDescription, "@ClaimStatusDescription", sqlParams );

					SqlHelper.AddVarcharPara( sample.StateCode, "@StateCode", sqlParams );
					SqlHelper.AddVarcharPara( sample.ManagedBy, "@ManagedBy", sqlParams );
					SqlHelper.AddVarcharPara( sample.ContractId, "@ContractId", sqlParams );
					SqlHelper.AddVarcharPara( sample.ContractType, "@ContractType", sqlParams );
					SqlHelper.AddVarcharPara( sample.ContractTitle, "@ContractTitle", sqlParams );

					SqlHelper.AddVarcharPara( sample.EsaCode, "@EsaCode", sqlParams );
					SqlHelper.AddBigIntPara( sample.JobseekerId, "@JobseekerId", sqlParams );
					SqlHelper.AddVarcharPara( sample.GivenName, "@GivenName", sqlParams );
					SqlHelper.AddVarcharPara( sample.Surname, "@LastName", sqlParams );

					SqlHelper.AddCharPara( sample.AutoSpecialClaimFlag, "@AutoSpecialClaimFlag", sqlParams );
					SqlHelper.AddCharPara( sample.ManSpecialClaimFlag, "@ManualSpecialClaimFlag", sqlParams );

					SqlHelper.AddVarcharPara( sample.CreatedBy, "@CreatedBy", sqlParams );

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

					sample.Id = (int) paramId.Value;
				}
			}
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Update( Sample entity )
		{
			var sample = entity;
			Debug.Assert( sample != null, "sample != null" );
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaSampleUpdatebyId", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					SqlHelper.AddReturnPara( "@return_value", sqlParams );

					if ( sample.Id > 0 )
						SqlHelper.AddIntPara( sample.Id, "@Id", sqlParams );

					SqlHelper.AddVarcharPara( sample.UpdatedBy, "@UpdatedBy", sqlParams );
					SqlHelper.AddBitPara( sample.Selected, "@Selected", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets the sample.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>
		/// list of candidate sample claims
		/// </returns>
		public List<Sample> GetSample( string sessionKey )
		{
			var list = new List<Sample>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaSampleGetBySessionKey", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddVarcharPara( sessionKey, "@SessionKey", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( list, command );
				}
			}
			return list;
		}

		/// <summary>
		///   Save extracted list informetion into SAMPLE for selection
		/// </summary>
		/// <param name="sessionKey"></param>
		/// <param name="claims"></param>
		/// <param name="userId"></param>
		public void SaveSample( string sessionKey, List<PatClaim> claims, string userId )
		{
			// foreach claim
			foreach ( var sample in claims.Select( c => new Sample
				{
					SessionKey = sessionKey,
					ClaimId = c.ClaimId,
					Selected = true,
					ClaimTypeDescription = c.ClaimTypeDescription,
					ContractTypeDescription = c.ContractTypeDescription,
					SiteDescription = c.SiteDescription,
					EsaDescription = c.EsaDescription,
					OrgDescription = c.OrgDescription,
					ClaimSequenceNumber = c.ClaimSequenceNumber,
					ClaimType = c.ClaimType,
					ClaimAmount = c.ClaimAmount,
					SiteCode = c.SiteCode,
					SupervisingSiteCode = c.SupervisingSiteCode,
					OrgCode = c.OrgCode,
					ActivityId = c.ActivityId,
					ClaimCreationDate = c.ClaimCreationDate,
					StatusCode = c.StatusCode,
					StatusCodeDescription = c.StatusCodeDescription,
					StateCode = c.StateCode,
					ManagedBy = c.ManagedBy,
					ContractId = c.ContractId,
					ContractType = c.ContractType,
					ContractTitle = c.ContractTitle,
					EsaCode = c.EsaCode,
					JobseekerId = c.JobseekerId,
					GivenName = c.GivenName,
					Surname = c.Surname,
					AutoSpecialClaimFlag = c.AutoSpecialClaimFlag ? "Y" : "N",
					ManSpecialClaimFlag = c.ManSpecialClaimFlag ? "Y" : "N",
					CreatedBy = userId
				} ) )
				Add( sample );
		}

		/// <summary>
		/// Saves the sample de selections.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="list">The list.</param>
		/// <param name="selectedList">The selected list.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveSampleDeSelections( string sessionKey, List<Sample> list, List<Sample> selectedList, string userId )
		{
			foreach ( var sample in from sample in list let isSelected = selectedList.Contains( sample ) where ! isSelected select sample )
			{
				sample.Selected = false;
				Update( sample );
			}
		}

		private static void LoadList( ICollection<Sample> list, SqlCommand command )
		{
			SqlDataReader rdr = null;
			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddRecordToList( list, rdr );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddRecordToList( ICollection<Sample> list, IDataRecord rdr )
		{
			var item = new Sample { Id = (int) rdr[ "Id" ] };

			if ( !rdr.IsDBNull( 1 ) )
				item.Id = (int) rdr[ "Id" ];

			item.SessionKey = rdr[ "SessionKey" ] as string;

			item.ClaimTypeDescription = rdr[ "ClaimTypeDescription" ] as string;
			item.ContractTypeDescription = rdr[ "ContractTypeDescription" ] as string;
			item.SiteDescription = rdr[ "SiteDescription" ] as string;
			item.EsaDescription = rdr[ "EsaDescription" ] as string;
			item.OrgDescription = rdr[ "OrgDescription" ] as string;
			item.ContractType = rdr[ "ContractType" ] as string;
			item.SupervisingSiteCode = rdr[ "SupervisingSiteCode" ] as string;
			item.OrgCode = rdr[ "OrgCode" ] as string;
			item.ClaimType = rdr[ "ClaimType" ] as string;
			item.SiteCode = rdr[ "SiteCode" ] as string;
			item.StateCode = rdr[ "StateCode" ] as string;
			item.ManagedBy = rdr[ "ManagedBy" ] as string;
			item.ContractType = rdr[ "ContractType" ] as string;
			item.ContractTitle = rdr[ "ContractTitle" ] as string;
			item.EsaCode = rdr[ "EsaCode" ] as string;
			item.StateCode = rdr[ "StateCode" ] as string;
			item.GivenName = rdr[ "GivenName" ] as string;
			item.Surname = rdr[ "LastName" ] as string;
			item.AutoSpecialClaimFlag = rdr[ "AutoSpecialClaimFlag" ] as string;
			item.ManSpecialClaimFlag = rdr[ "ManualSpecialClaimFlag" ] as string;
			item.ContractId = rdr[ "ContractId" ] as string;

			item.JobseekerId = rdr[ "JobseekerId" ] as long? ?? default( long );
			item.ActivityId = rdr[ "ActivityId" ] as long? ?? default( long );

			item.ClaimId = rdr[ "ClaimId" ] as long? ?? default( long );
			item.ClaimSequenceNumber = (int) rdr[ "ClaimSequenceNumber" ];
			item.ClaimAmount = rdr[ "ClaimMoney" ] as decimal? ?? default( decimal );

			item.StatusCode = rdr[ "ClaimStatusCode" ] as string;
			item.StatusCodeDescription = rdr[ "ClaimStatusDescription" ] as string;

			item.ClaimCreationDate = rdr[ "ClaimCreationDate" ] as DateTime? ?? default( DateTime );

			item.Selected = rdr[ "Selected" ] as bool? ?? default( bool );

			item.UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime );
			item.UpdatedBy = rdr[ "UpdatedBy" ] as string;
			item.CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime );
			item.CreatedBy = rdr[ "CreatedBy" ] as string;

			list.Add( item );
		}

		/// <summary>
		/// Deletes the Sample claims by a session key.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		public void DeleteBySessionKey( string sessionKey )
		{
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaSampleDeleteBySessionKey", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );
					SqlHelper.AddVarcharPara( sessionKey, "@SessionKey", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Gets the distinct session keys.
		/// </summary>
		/// <returns>
		/// array of keys
		/// </returns>
		public string[] GetDistinctSessionKeys()
		{
			return GetDistinct( "PaSampleSessionKeysDistinct", "SessionKey" );
		}

		private string[] GetDistinct( string sprocName, string targetField )
		{
			var list = new List<Sample>();

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

					LoadSessionKeyList( list, command, targetField );
				}
			}
			return list.Select( x => x.SessionKey ).ToArray();
		}

		private static void LoadSessionKeyList( ICollection<Sample> list, SqlCommand command, string targetField )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while ( rdr.Read() )
					AddSessionKeyToList( list, rdr, targetField );
			}
			finally
			{
				if ( rdr != null )
					rdr.Close();
			}
		}

		private static void AddSessionKeyToList( ICollection<Sample> list, IDataRecord rdr, string targetField )
		{
			var sample = new Sample
			{
				SessionKey = rdr[ targetField ] as string
			};
			list.Add( sample );
		}

	}
}