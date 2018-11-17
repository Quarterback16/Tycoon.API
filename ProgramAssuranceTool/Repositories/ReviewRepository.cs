using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   The class for interfacing with the Review Data store
	/// </summary>
	public class ReviewRepository : PatRepository, IReviewRepository
	{
		#region  Commands

		/// <summary>
		/// Updates a set of reviews with the same outcomes.
		/// </summary>
		/// <param name="reviews">The reviews.</param>
		/// <param name="outcome">The outcome.</param>
		/// <param name="userId">The user identifier.</param>
		/// <exception cref="System.ApplicationException">Bulk Outcomes failed to update</exception>
		public void BulkOutcome( IEnumerable<int> reviews, Review outcome, string userId )
		{
			var dtReviews = SqlHelper.BuildIdTable();
			SqlHelper.PopulateIdsTable( reviews, dtReviews );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaReviewBulkOutcome", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddTablePara( dtReviews, "@ReviewIds", "IntTableType", sqlParams );
					SqlHelper.AddVarcharPara( outcome.AssessmentCode, "@AssessmentCode", sqlParams );
					SqlHelper.AddVarcharPara( outcome.AssessmentAction, "@AssessmentAction", sqlParams );
					SqlHelper.AddVarcharPara( outcome.RecoveryReason, "@RecoveryReason", sqlParams );
					SqlHelper.AddVarcharPara( outcome.OutcomeCode, "@OutcomeCode", sqlParams );
					SqlHelper.AddVarcharPara( outcome.Comments, "@Comments", sqlParams );
					SqlHelper.AddNullableDateParameter( outcome.AssessmentDate, "@AssessmentDate", sqlParams );
					SqlHelper.AddNullableDateParameter( outcome.AssessmentActionDate, "@AssessmentActionDate", sqlParams );
					SqlHelper.AddNullableDateParameter( outcome.RecoveryReasonDate, "@RecoveryReasonDate", sqlParams );
					SqlHelper.AddNullableDateParameter( outcome.FinalOutcomeDate, "@FinalOutcomeDate", sqlParams );
					SqlHelper.AddVarcharPara( userId, "@UpdatedBy", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );

					connection.Open();

					command.ExecuteNonQuery();

					if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) == -1 )
						throw new ApplicationException( "Bulk Outcomes failed to update" );
				}
			}
		}

		private static Review CreateReview( Upload upload )
		{
			var review = new Review
			{
				UploadId = upload.UploadId,
				ProjectId = upload.ProjectId,
				IsAdditionalReview = upload.AdditionalReview,
				IsOutOfScope = ! upload.InScope,
				CreatedBy = upload.CreatedBy,
				CreatedOn = DateTime.Now
			};
			return review;
		}

		/// <summary>
		/// Creates reviews from a CSV stream
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>
		/// the number of reviews
		/// </returns>
		public int StoreReviewData( Upload upload, Stream stream )
		{
			var nReviewsStored = 0;

			//  assuming the data has been validated

			using ( var csv = new CsvReader( new StreamReader( stream ), true ) ) 
			{
					//  load an array with the column headers
					var columns = csv.GetFieldHeaders();

					//  for each line of the spreadsheet
					while ( csv.ReadNextRecord() )
					{
						if ( !CsvHelper.EmptyLine( csv, columns.Length ) )
						{
							//  process the review (single row)
							var review = CreateReview( upload );

							//for each column of the row
							for ( var c = 0; c < columns.Length; c++ )
							{
								var colName = columns[ c ];

								var val = csv[ c ];

								if (string.IsNullOrEmpty( val )) continue;

								switch (colName.ToUpper())
								{
									case CommonConstants.UploadColumn_ActivityId:
										review.ActivityId = Int64.Parse( val );
										break;

									case CommonConstants.UploadColumn_AutoSpecialCLaimFlag:
										review.AutoSpecialClaim = val.ToUpper().Equals( "Y" );
										break;

									case CommonConstants.UploadColumn_ClaimAmount:
										review.ClaimAmount = Decimal.Parse( val );
										break;

									case CommonConstants.UploadColumn_ClaimCreationDate:
										review.ClaimCreationDate = DateTime.Parse( val );
										break;

									case CommonConstants.UploadColumn_ClaimId:
										review.ClaimId = Int64.Parse( val );
										break;

									case CommonConstants.UploadColumn_ClaimSequenceNumber:
										review.ClaimSequenceNumber = Int32.Parse( val );
										break;

									case CommonConstants.UploadColumn_ClaimType:
										review.ClaimType = val;
										break;

									case CommonConstants.UploadColumn_ContractType:
										review.ContractType = val;
										break;

									case CommonConstants.UploadColumn_EmploymentServiceAreaCode:
										review.ESACode = val;
										break;

									case CommonConstants.UploadColumn_JobseekerId:
										review.JobseekerId = Int64.Parse( val );
										break;

									case CommonConstants.UploadColumn_JobseekerGivenName:
										review.JobSeekerGivenName = val;
										break;

									case CommonConstants.UploadColumn_JobseekerSurname:
										review.JobSeekerSurname = val;
										break;

									case CommonConstants.UploadColumn_ManagedBy:
										review.ManagedBy = val;
										break;

									case CommonConstants.UploadColumn_ManualSpecialClaimFlag:
										review.ManualSpecialClaim = val.ToUpper().Equals( "Y" );
										break;

									case CommonConstants.UploadColumn_OrgName:
										review.OrgName = val;
										break;

									case CommonConstants.UploadColumn_OrgCode:
										review.OrgCode = val;
										break;

									case CommonConstants.UploadColumn_SiteCode:
										review.SiteCode = val;
										break;

									case CommonConstants.UploadColumn_StateCode:
										review.StateCode = val;
										break;

									case CommonConstants.UploadColumn_AssessmentCode:
										review.AssessmentCode = val;
										break;

									case CommonConstants.UploadColumn_RecoveryReasonCode:
										review.RecoveryReason = val;
										break;

									case CommonConstants.UploadColumn_AssessmentActionCode:
										review.AssessmentAction = val;
										break;

									case CommonConstants.UploadColumn_OutcomeCode:
										review.OutcomeCode = val;
										break;

									case CommonConstants.UploadColumn_ScopeFlag:
										review.IsOutOfScope = val.ToUpper().Equals( "N" );
										break;

									case CommonConstants.UploadColumn_AdditionalReviewFlag:
										review.IsAdditionalReview = val.ToUpper().Equals( "Y" );
										break;

									case CommonConstants.UploadColumn_Comments:
										review.Comments = val;
										break;

									case CommonConstants.UploadColumn_ClaimRecoveryAmount:
										review.ClaimRecoveryAmount = Decimal.Parse( val );
										break;

								}
							}
							review.ReviewId = Add( review );

							nReviewsStored++;
						}
				}
			}
			return nReviewsStored;
		}

		/// <summary>
		/// Adds the specified review.
		/// </summary>
		/// <param name="review">The review.</param>
		/// <returns>
		/// id of the new review
		/// </returns>
		public int Add( Review review )
		{
			var reviewId = review.ReviewId;

			if ( reviewId == 0 )
			{
				using ( var connection = new SqlConnection( DbConnection ) )
				{
					using ( var command = new SqlCommand( "PaReviewInsert", connection ) )
					{
						var sqlParams = new List<SqlParameter>();

						var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
						sqlParams.Add( paramReturnValue );

						var paramId = new SqlParameter( "@ReviewId", SqlDbType.Int )
						{
							Direction = ParameterDirection.InputOutput,
							Value = 0
						};
						sqlParams.Add( paramId );

						SqlHelper.AddIntPara( review.ProjectId, "@ProjectId", sqlParams );
						SqlHelper.AddIntPara( review.UploadId, "@UploadId", sqlParams );
						SqlHelper.AddIntPara( review.ClaimSequenceNumber, "@ClaimSequenceNumber", sqlParams );
						SqlHelper.AddNullableBigIntPara( review.JobseekerId, "@JobseekerId", sqlParams );
						SqlHelper.AddNullableBigIntPara( review.ClaimId, "@ClaimId", sqlParams );
						SqlHelper.AddNullableBigIntPara( review.ActivityId, "@ActivityId", sqlParams );
						SqlHelper.AddVarcharPara( review.JobSeekerGivenName, "@JobSeekerGivenName", sqlParams );
						SqlHelper.AddVarcharPara( review.JobSeekerSurname, "@JobSeekerSurname", sqlParams );
						SqlHelper.AddVarcharPara( review.SiteCode, "@SiteCode", sqlParams );
						SqlHelper.AddVarcharPara( review.SiteName, "@SiteName", sqlParams );
						SqlHelper.AddVarcharPara( review.OrgName, "@OrgName", sqlParams );
						SqlHelper.AddVarcharPara( review.OrgCode, "@OrgCode", sqlParams );
						SqlHelper.AddVarcharPara( review.ESACode, "@ESACode", sqlParams );
						SqlHelper.AddVarcharPara( review.StateCode, "@StateCode", sqlParams );
						SqlHelper.AddVarcharPara( review.ManagedBy, "@ManagedBy", sqlParams );
						SqlHelper.AddVarcharPara( review.ContractType, "@ContractType", sqlParams );
						SqlHelper.AddVarcharPara( review.ClaimType, "@ClaimType", sqlParams );
						SqlHelper.AddVarcharPara( review.Comments, "@Comments", sqlParams );
						SqlHelper.AddVarcharPara( review.AssessmentCode, "@AssessmentCode", sqlParams );
						SqlHelper.AddVarcharPara( review.RecoveryReason, "@RecoveryReason", sqlParams );
						SqlHelper.AddVarcharPara( review.AssessmentAction, "@AssessmentAction", sqlParams );
						SqlHelper.AddVarcharPara( review.OutcomeCode, "@OutcomeCode", sqlParams );
						SqlHelper.AddBitPara( review.IsAdditionalReview, "@IsAdditionalReview", sqlParams );
						SqlHelper.AddBitPara( review.IsOutOfScope, "@IsOutOfScope", sqlParams );
						SqlHelper.AddBitPara( review.AutoSpecialClaim, "@AutoSpecialClaim", sqlParams );
						SqlHelper.AddBitPara( review.ManualSpecialClaim, "@ManualSpecialClaim", sqlParams );
						SqlHelper.AddNullableDateParameter( review.ClaimCreationDate, "@ClaimCreationDate", sqlParams );
						SqlHelper.AddMoneyPara( review.ClaimAmount, "@ClaimAmount", sqlParams );
						SqlHelper.AddMoneyPara( review.ClaimRecoveryAmount, "@ClaimRecoveryAmount", sqlParams );
						SqlHelper.AddVarcharPara( review.CreatedBy, "@CreatedBy", sqlParams );

						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.AddRange( sqlParams.ToArray() );
						connection.Open();
						command.ExecuteNonQuery();
						if ( ( (Int32) command.Parameters[ "@return_value" ].Value ) == 0 )
							reviewId = (int) paramId.Value;
					}
				}
			}
			return reviewId;
		}

		/// <summary>
		/// Updates the specified review.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Update( Review entity )
		{
			var review = entity;
			Debug.Assert( review != null, "review != null" );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaReviewUpdate", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( review.ReviewId, "@ReviewId", sqlParams );
					SqlHelper.AddIntPara( review.ProjectId, "@ProjectId", sqlParams );
					SqlHelper.AddIntPara( review.UploadId, "@UploadId", sqlParams );
					SqlHelper.AddIntPara( review.ClaimSequenceNumber, "@ClaimSequenceNumber", sqlParams );

					SqlHelper.AddNullableBigIntPara( review.JobseekerId, "@JobseekerId", sqlParams );
					SqlHelper.AddNullableBigIntPara( review.ClaimId, "@ClaimId", sqlParams );
					SqlHelper.AddNullableBigIntPara( review.ActivityId, "@ActivityId", sqlParams );

					SqlHelper.AddVarcharPara( review.SiteCode, "@SiteCode", sqlParams );
					SqlHelper.AddVarcharPara( review.SiteName, "@SiteName", sqlParams );
					SqlHelper.AddVarcharPara( review.OrgName, "@OrgName", sqlParams );
					SqlHelper.AddVarcharPara( review.OrgCode, "@OrgCode", sqlParams );
					SqlHelper.AddVarcharPara( review.ESACode, "@ESACode", sqlParams );
					SqlHelper.AddVarcharPara( review.StateCode, "@StateCode", sqlParams );
					SqlHelper.AddVarcharPara( review.ManagedBy, "@ManagedBy", sqlParams );

					SqlHelper.AddNullableVarcharPara( review.OutcomeCode, "@OutcomeCode", sqlParams );
					SqlHelper.AddNullableVarcharPara( review.AssessmentCode, "@AssessmentCode", sqlParams );
					SqlHelper.AddNullableVarcharPara( review.RecoveryReason, "@RecoveryReason", sqlParams );
					SqlHelper.AddNullableVarcharPara( review.AssessmentAction, "@AssessmentAction", sqlParams );

					SqlHelper.AddVarcharPara( review.JobSeekerGivenName, "@JobSeekerGivenName", sqlParams );
					SqlHelper.AddVarcharPara( review.JobSeekerSurname, "@JobSeekerSurname", sqlParams );
					SqlHelper.AddVarcharPara( review.ClaimType, "@ClaimType", sqlParams );
					SqlHelper.AddVarcharPara( review.Comments, "@Comments", sqlParams );

					SqlHelper.AddBitPara( review.IsAdditionalReview, "@IsAdditionalReview", sqlParams );
					SqlHelper.AddBitPara( review.IsOutOfScope, "@IsOutOfScope", sqlParams );
					SqlHelper.AddBitPara( review.AutoSpecialClaim, "@AutoSpecialClaim", sqlParams );
					SqlHelper.AddBitPara( review.ManualSpecialClaim, "@ManualSpecialClaim", sqlParams );

					SqlHelper.AddMoneyPara( review.ClaimAmount, "@ClaimAmount", sqlParams );
					SqlHelper.AddMoneyPara( review.ClaimRecoveryAmount, "@ClaimRecoveryAmount", sqlParams );


					SqlHelper.AddNullableDateParameter( review.AssessmentDate, "@AssessmentDate", sqlParams );
					SqlHelper.AddNullableDateParameter( review.AssessmentActionDate, "@AssessmentActionDate", sqlParams );
					SqlHelper.AddNullableDateParameter( review.RecoveryReasonDate, "@RecoveryReasonDate", sqlParams );
					SqlHelper.AddNullableDateParameter( review.FinalOutcomeDate, "@FinalOutcomeDate", sqlParams );
					SqlHelper.AddNullableDateParameter( review.ClaimCreationDate, "@ClaimCreationDate", sqlParams );

					SqlHelper.AddVarcharPara( review.UpdatedBy, "@UpdatedBy", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Deletes the specified Review.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="userId">The user identifier.</param>
		public void Delete( int id, string userId )
		{
			DeleteReviews( new List<int> {id}, userId );
		}

		/// <summary>
		/// Deletes the reviews.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <param name="userName">Name of the user.</param>
		public void DeleteReviews( List<int> reviewIds, String userName )
		{
			var dtReviews = BuildReviewTable();
			PopulateReviewTable( reviewIds, dtReviews );

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaReviewDelete", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddTablePara( dtReviews, "@ReviewIds", "IntTableType", sqlParams );
					SqlHelper.AddVarcharPara( userName, "@UserId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}

		private static DataTable BuildReviewTable()
		{
			var dtReviews = new DataTable();
			dtReviews.Columns.Add( "Id", typeof( Int32 ) );
			return dtReviews;
		}

		private static void PopulateReviewTable( IEnumerable<int> reviewIds, DataTable dtReviews )
		{
			foreach ( var reviewId in reviewIds )
			{
				var dr = dtReviews.NewRow();
				dr[ "Id" ] = reviewId;
				dtReviews.Rows.Add( dr );
			}
		}

		#endregion

		#region   Queries

		/// <summary>
		/// Gets all the reviews that match the grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns>
		/// list of review objects
		/// </returns>
		public List<Review> GetAll( GridSettings gridSettings )
		{
			//  Not required
			var list = new List<Review>();
			return list;
		}

		/// <summary>
		/// Gets the Review by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// review object
		/// </returns>
		public Review GetById( int id )
		{
			Review review = null;
			var reviewList = new List<Review>();

			const string sprocName = "PaReviewGetById";

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( id, "@ReviewId", sqlParams );

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( reviewList, command, sprocName );

					if ( reviewList.Count > 0 )
						review = reviewList[ 0 ];
				}
			}
			return review;
		}

		/// <summary>
		/// Gets all by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectStatus">The project status.</param>
		/// <returns></returns>
		public List<Review> GetAllByUploadId( int uploadId, Models.GridSettings gridSettings, string projectStatus )
		{
			if (string.IsNullOrEmpty( gridSettings.SortColumn ))
				gridSettings.SortColumn = "ReviewId";

			const string sprocName = "PaReviewsGetAllByUpload";

			var reviewList = new List<Review>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( uploadId, "@UploadId", sqlParams );

					SqlHelper.AddVarcharPara( gridSettings.SortColumn, "@sortColumnName", sqlParams );
					SqlHelper.AddVarcharPara( gridSettings.SortOrder, "@sortOrder", sqlParams );
					if (gridSettings.PageSize > 0)
						SqlHelper.AddIntPara( gridSettings.PageSize, "@pageSize", sqlParams );
					if (gridSettings.PageIndex > 0)
						SqlHelper.AddIntPara( gridSettings.PageIndex, "@pageIndex", sqlParams );

					if (gridSettings.IsSearch && gridSettings.Where != null)
					{
						foreach ( var rule in gridSettings.Where.rules )
						{
							if (!rule.field.Equals( "ReviewStatus" ))
							{
								//  convert rule into a parameter
								if (rule.field.IndexOf( "Date", StringComparison.Ordinal ) > -1)
									SqlHelper.AddDatePara( DateTime.Parse( rule.data ), "@" + rule.field, sqlParams );
								else
								{
									if (( rule.field.IndexOf( "Id", StringComparison.Ordinal ) > -1 ) ||
									    ( rule.field.IndexOf( "Number", StringComparison.Ordinal ) > -1 ))
										SqlHelper.AddBigIntPara( Int64.Parse( rule.data ), "@" + rule.field, sqlParams );
									else
									{
										switch (rule.field)
										{
											case "ReferredToFraud":
											case "IsAdditionalReview":
											case "IsOutOfScope":
												SqlHelper.AddBitPara( ConvertStringToBool( rule.data ), "@" + rule.field, sqlParams );
												break;
											default:
												SqlHelper.AddVarcharPara( rule.data, "@" + rule.field, sqlParams );
												if (!rule.field.Equals( "AssignedTo" ))
												{
													var opValue = rule.op.Equals( "eq" ) ? 1 : 0;
													SqlHelper.AddIntPara( opValue, "@" + rule.field + "Op", sqlParams );
												}
												break;
										}

										//  add the Op parameter
									}
								}
							}
						}
					}

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( reviewList, command, sprocName );

				}
			}
			return reviewList;
		}

		private static bool ConvertStringToBool( String value )
		{
			return String.Compare( value, "Y", StringComparison.OrdinalIgnoreCase ) == 0;
		}

		private static void LoadList( ICollection<Review> reviewList, SqlCommand command, string sprocName )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while (rdr.Read())
					AddReviewToList( reviewList, rdr, sprocName );
			}
			finally
			{
				if (rdr != null)
					rdr.Close();
			}
		}

		private static void AddReviewToList( ICollection<Review> reviewList, IDataRecord rdr, string sprocName )
		{
			var review = new Review();
			if (SqlHelper.HasColumn( rdr, "UpdatedOn" ))
				review.UpdatedOn = rdr[ "UpdatedOn" ] as DateTime? ?? default( DateTime );

			if ( SqlHelper.HasColumn( rdr, "UpdatedBy" ) )
				review.UpdatedBy = rdr[ "UpdatedBy" ] as string;

			review.CreatedOn = rdr[ "CreatedOn" ] as DateTime? ?? default( DateTime );
			review.CreatedBy = rdr[ "CreatedBy" ] as string;
			review.IsOutOfScope = rdr[ "IsOutOfScope" ] as bool? ?? default( bool );
			review.IsAdditionalReview = rdr[ "IsAdditionalReview" ] as bool? ?? default( bool );
			review.ClaimRecoveryAmount = rdr[ "ClaimRecoveryAmount" ] as decimal? ?? default( decimal );
			review.ClaimAmount = rdr[ "ClaimAmount" ] as decimal? ?? default( decimal );
			review.ManagedBy = rdr[ "ManagedBy" ] as string;
			review.OrgCode = rdr[ "OrgCode" ] as string;
			review.ESACode = rdr[ "ESACode" ] as string;
			review.SiteCode = rdr[ "SiteCode" ] as string;
			review.StateCode = rdr[ "StateCode" ] as string;
			review.Comments = rdr[ "Comments" ] as string;
			review.ClaimSequenceNumber = rdr[ "ClaimSequenceNumber" ] as int? ?? default( int );
			review.ClaimId = rdr[ "ClaimId" ] as long? ?? default( long );
			review.ActivityId = rdr[ "ActivityId" ] as long? ?? default( long );
			review.JobseekerId = rdr[ "JobseekerId" ] as long? ?? default( long );
			if ( sprocName.Equals( "PaReviewsGetAllByProject" ) )
			{
				review.UploadId = Int32.Parse( rdr[ "UploadId" ].ToString() );
				review.ReviewId = Int32.Parse( rdr[ "ReviewId" ].ToString() );
			}
			else
			{
				review.UploadId = (int) rdr[ "UploadId" ];
				review.ReviewId = (int) rdr[ "ReviewId" ];
			}
			review.ProjectId = (int) rdr[ "ProjectId" ];

			//  New fields
			if (SqlHelper.HasColumn( rdr, "OutcomeCode" ))
			{
				review.OutcomeCode = rdr[ "OutcomeCode" ] as string;
				review.AssessmentCode = rdr[ "AssessmentCode" ] as string;
				review.RecoveryReason = rdr[ "RecoveryReason" ] as string;
				review.AssessmentAction = rdr[ "AssessmentAction" ] as string;
				review.JobSeekerGivenName = rdr[ "JobSeekerGivenName" ] as string;
				review.JobSeekerSurname = rdr[ "JobSeekerSurname" ] as string;
				review.ContractType = rdr[ "ContractType" ] as string;
				review.ClaimType = rdr[ "ClaimType" ] as string;
				review.AssessmentDate = rdr[ "AssessmentDate" ] as DateTime? ?? default( DateTime );
				review.AssessmentActionDate = rdr[ "AssessmentAction" ] as DateTime? ?? default( DateTime );
				review.RecoveryReasonDate = rdr[ "RecoveryReasonDate" ] as DateTime? ?? default( DateTime );
				review.ClaimCreationDate = rdr[ "ClaimCreationDate" ] as DateTime? ?? default( DateTime );
				review.FinalOutcomeDate = rdr[ "FinalOutcomeDate" ] as DateTime? ?? default( DateTime );
				review.AutoSpecialClaim = rdr[ "AutoSpecialClaim" ] as bool? ?? default( bool );
				review.ManualSpecialClaim = rdr[ "ManualSpecialClaim" ] as bool? ?? default( bool );
			}

			if (SqlHelper.HasColumn( rdr, "PartialRecovery" ))
				review.PartialRecovery = rdr[ "PartialRecovery" ] as bool? ?? default( bool );

						//  New fields
			if (SqlHelper.HasColumn(rdr, "IsCheckListCompleted"))
			{
				review.IsCheckListCompleted = rdr["IsCheckListCompleted"] as bool? ?? default(bool);
			}

			reviewList.Add( review );
		}

		/// <summary>
		/// Gets all the reviews for a particular upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// list of review objects
		/// </returns>
		public List<Review> GetAllByUploadId( int uploadId )
		{
			var gs = new GridSettings {SortColumn = "ReviewId", SortOrder = "ASC", IsSearch = false};

			return GetAllByUploadIdPaged( uploadId, gs );
		}

		/// <summary>
		/// Gets all by upload identifier paged.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public List<Review> GetAllByUploadIdPaged( int uploadId, GridSettings gridSettings )
		{
			if ( string.IsNullOrEmpty( gridSettings.SortColumn ) )
				gridSettings.SortColumn = "ReviewId";

			const string sprocName = "PaReviewsGetAllByUpload";

			var reviewList = new List<Review>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( uploadId, "@UploadId", sqlParams );

					SqlHelper.AddVarcharPara( gridSettings.SortColumn, "@sortColumnName", sqlParams );
					SqlHelper.AddVarcharPara( gridSettings.SortOrder, "@sortOrder", sqlParams );
					if ( gridSettings.PageSize > 0 )
						SqlHelper.AddIntPara( gridSettings.PageSize, "@pageSize", sqlParams );
					if ( gridSettings.PageIndex > 0 )
						SqlHelper.AddIntPara( gridSettings.PageIndex, "@pageIndex", sqlParams );

					if (gridSettings.IsSearch && gridSettings.Where != null)
					{
						foreach ( var rule in gridSettings.Where.rules )
						{
							//  convert rule into a parameter
							if ( rule.field.IndexOf( "Date", StringComparison.Ordinal ) > -1 )
								SqlHelper.AddDatePara( DateTime.Parse( rule.data ), "@" + rule.field, sqlParams );
							else
							{
								if ( ( rule.field.IndexOf( "Id", StringComparison.Ordinal ) > -1 )
										|| ( rule.field.IndexOf( "Number", StringComparison.Ordinal ) > -1 ) )
									SqlHelper.AddBigIntPara( Int64.Parse( rule.data ), "@" + rule.field, sqlParams );
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

					LoadList( reviewList, command, sprocName );

				}
			}
			return reviewList;
		}

		/// <summary>
		/// Gets all by project identifier paged.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public List<Review> GetAllByProjectIdPaged( int projectId, GridSettings gridSettings )
		{
			if ( string.IsNullOrEmpty( gridSettings.SortColumn ) )
				gridSettings.SortColumn = "ReviewId";

			const string sprocName = "PaReviewsGetAllByProject";

			var reviewList = new List<Review>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
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
					if ( gridSettings.PageSize > 0 )
						SqlHelper.AddIntPara( gridSettings.PageSize, "@pageSize", sqlParams );
					if ( gridSettings.PageIndex > 0 )
						SqlHelper.AddIntPara( gridSettings.PageIndex, "@pageIndex", sqlParams );

					if (gridSettings.IsSearch && gridSettings.Where != null)
					{
						foreach ( var rule in gridSettings.Where.rules )
						{
							//  convert rule into a parameter
							if ( rule.field.IndexOf( "Date", StringComparison.Ordinal ) > -1 )
								SqlHelper.AddDatePara( DateTime.Parse( rule.data ), "@" + rule.field, sqlParams );
							else
								SqlHelper.AddVarcharPara( rule.data, "@" + rule.field, sqlParams );
						}
					}

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					LoadList( reviewList, command, sprocName );
				}
			}
			return reviewList;
		}

		/// <summary>
		/// Gets all the Reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// list of review objects
		/// </returns>
		public IEnumerable<Review> GetAllByProjectId( int projectId )
		{
			const string sprocName = "PaReviewGetByProjectId";

			var reviewList = new List<Review>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
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

					LoadList( reviewList, command, sprocName );
				}
			}
			return reviewList;
		}

		/// <summary>
		/// Gets all the finished Reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// list of review objects
		/// </returns>
		public IEnumerable<Review> GetFinishedReviewsByProjectId( int projectId )
		{
			const string sprocName = "PaReviewGetFinishedByProjectId";

			var reviewList = new List<Review>();
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( sprocName, connection ) )
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

					LoadList( reviewList, command, sprocName );
				}
			}
			return reviewList;
		}

		/// <summary>
		/// Counts the reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// the number of reviews
		/// </returns>
		public int CountReviews( int projectId )
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectReviewCountById", connection ) )
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

					if (( (Int32) command.Parameters[ "@return_value" ].Value ) == 0)
						theCount = (int) paramCount.Value;
				}
			}
			return theCount;
		}


		/// <summary>
		/// Counts the finished reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// the number of reviews
		/// </returns>
		public int CountFinishedReviews( int projectId )
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectReviewFinishCountById", connection ) )
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

		/// <summary>
		/// Counts the number of reviews in an upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// the number of reviews
		/// </returns>
		public int CountReviewsByUploadId( int uploadId )
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectReviewCountByUploadId", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( uploadId, "@UploadId", sqlParams );

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

					if (( (Int32) command.Parameters[ "@return_value" ].Value ) == 0)
						theCount = (int) paramCount.Value;
				}
			}
			return theCount;
		}

		/// <summary>
		/// Counts the completed reviewsin a certain upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// the count
		/// </returns>
		public int CountCompletedReviewsByUploadId( int uploadId )
		{
			var theCount = 0;
			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaProjectCompletedReviewCountByUploadId", connection ) )
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara( uploadId, "@UploadId", sqlParams );

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

		#endregion
	}
}