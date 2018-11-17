using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	/// CheckList Repository
	/// </summary>
	public class CheckListRepository : PatRepository, ICheckListRepository
	{
		/// <summary>
		/// Gets the checklist by its Review Id
		/// </summary>
		/// <param name="id">The check list review Id.</param>
		/// <returns></returns>
		public CheckList GetById( int id )
		{
			var reviewID = id;
			CheckList checkList = null;
			var checkLists = new List<CheckList>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaCheckListGetById", connection ) )
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara(reviewID, "@ReviewId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

                    LoadList(checkLists, command);

                    if (checkLists.Count > 0)
                        checkList = checkLists[0];
				}
			}
            return checkList;
		}

		/// <summary>
		/// Save the specified checklist.
		/// If the review id exists then update otherwise insert
		/// </summary>
		/// <param name="entity">The entity.</param>
	    public void Save(CheckList entity)
	    {
			 using (var connection = new SqlConnection(DbConnection))
			 {
				 using (var command = new SqlCommand("PaCheckListSave", connection))
				 {
					 var sqlParams = new List<SqlParameter>();

					 var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					 {
						 Direction = ParameterDirection.ReturnValue
					 };
					 sqlParams.Add(paramReturnValue);

					 var checkListId = new SqlParameter("@CheckListId", SqlDbType.Int)
					 {
						 Direction = ParameterDirection.InputOutput,
						 Value = 0,
					 };

					 sqlParams.Add(checkListId);

					 SqlHelper.AddIntPara(entity.ReviewID, "@ReviewId", sqlParams);

					 // need to allow nullable bit
					 var paraIsClaimDuplicateOverlapping = new SqlParameter("@IsClaimDuplicateOverlapping", SqlDbType.Bit) { Value = AppHelper.ToNullableBool(entity.IsClaimDuplicateOverlapping), IsNullable = true, Direction = ParameterDirection.Input };
					 var paraIsClaimIncludedInDeedNonPayableOutcomeList = new SqlParameter("@IsClaimIncludedInDeedNonPayableOutcomeList", SqlDbType.Bit) { Value = AppHelper.ToNullableBool(entity.IsClaimIncludedInDeedNonPayableOutcomeList), IsNullable = true, Direction = ParameterDirection.Input };
					 var paraDoesDocEvidenceMeetGuidelineRequirement = new SqlParameter("@DoesDocEvidenceMeetGuidelineRequirement", SqlDbType.Bit) { Value = AppHelper.ToNullableBool(entity.DoesDocEvidenceMeetGuidelineRequirement), IsNullable = true, Direction = ParameterDirection.Input };
					 var paraIsDocEvidenceConsistentWithESS = new SqlParameter("@IsDocEvidenceConsistentWithESS", SqlDbType.Bit) { Value = AppHelper.ToNullableBool(entity.IsDocEvidenceConsistentWithESS), IsNullable = true, Direction = ParameterDirection.Input };
					 var paraIsDocEvidenceSufficientToSupportPaymentType = new SqlParameter("@IsDocEvidenceSufficientToSupportPaymentType", SqlDbType.Bit) { Value = AppHelper.ToNullableBool(entity.IsDocEvidenceSufficientToSupportPaymentType), IsNullable = true, Direction = ParameterDirection.Input };

					 sqlParams.Add(paraIsClaimDuplicateOverlapping);
					 sqlParams.Add(paraIsClaimIncludedInDeedNonPayableOutcomeList);
					 sqlParams.Add(paraDoesDocEvidenceMeetGuidelineRequirement);
					 sqlParams.Add(paraIsDocEvidenceConsistentWithESS);
					 sqlParams.Add(paraIsDocEvidenceSufficientToSupportPaymentType);

					 SqlHelper.AddVarcharPara(entity.Comment, "@Comment", sqlParams);
					 SqlHelper.AddVarcharPara(entity.CreatedBy, "@CreatedBy", sqlParams);
					 SqlHelper.AddVarcharPara(entity.UpdatedBy, "@UpdatedBy", sqlParams);

					 command.CommandType = CommandType.StoredProcedure;
					 command.Parameters.AddRange(sqlParams.ToArray());

					 connection.Open();

					 command.ExecuteNonQuery();

					 if (((Int32)command.Parameters["@return_value"].Value) != 0) return;

					 entity.CheckListID = (int)checkListId.Value;
				 }
			 }
	    }

		 /// <summary>
		 /// Loads the list.
		 /// </summary>
		 /// <param name="checkLists">The check lists.</param>
		 /// <param name="command">The command.</param>
		private static void LoadList( ICollection<CheckList> checkLists, SqlCommand command )
		{
			SqlDataReader rdr = null;

			try
			{
				rdr = command.ExecuteReader();

				while (rdr.Read())
					AddCheckListToList(checkLists, rdr);
			}
			finally
			{
				if (rdr != null)
					rdr.Close();
			}
		}

		/// <summary>
		/// Adds the check list to list.
		/// </summary>
		/// <param name="checkLists">The check lists.</param>
		/// <param name="rdr">The RDR.</param>
		private static void AddCheckListToList( ICollection<CheckList> checkLists, IDataRecord rdr )
		{
			var checkList = new CheckList
				{
					CheckListID = AppHelper.ToInt(rdr["CheckListId"]),
					ReviewID = AppHelper.ToInt(rdr["ReviewId"]),
					UploadID = AppHelper.ToInt(rdr["UploadId"]),
					ProjectID = AppHelper.ToInt(rdr["ProjectId"]),
					ProjectName = string.Format("{0}", rdr["ProjectName"]),
					JobSeekerID = string.Format("{0}", rdr["JobseekerId"]),
					JobSeekerName = string.Format("{0} {1}", rdr["JobseekerGivenName"], rdr["JobseekerSurname"]),
					ClaimID = AppHelper.ToLong(rdr["ClaimId"]),
					ClaimSequenceNumber = AppHelper.ToInt(rdr["ClaimSequenceNumber"]),
					ClaimType = string.Format("{0}", rdr["ClaimType"]),
					ClaimTypeDescription = string.Format("{0}", rdr["ClaimTypeDescription"]),
					ClaimAmount = AppHelper.ToDecimal(rdr["ClaimAmount"]),
					ClaimIs = GetClaimIs(rdr["AutoSpecialClaim"] as bool? ?? default(bool), rdr["ManualSpecialClaim"] as bool? ?? default(bool)),

					// display nullable bit
					IsClaimDuplicateOverlapping = AppHelper.ToAdwBoolean(rdr["IsClaimDuplicateOverlapping"]),
					IsClaimIncludedInDeedNonPayableOutcomeList = AppHelper.ToAdwBoolean(rdr["IsClaimIncludedInDeedNonPayableOutcomeList"]),
					DoesDocEvidenceMeetGuidelineRequirement = AppHelper.ToAdwBoolean(rdr["DoesDocEvidenceMeetGuidelineRequirement"]),
					IsDocEvidenceConsistentWithESS = AppHelper.ToAdwBoolean(rdr["IsDocEvidenceConsistentWithESS"]),
					IsDocEvidenceSufficientToSupportPaymentType = AppHelper.ToAdwBoolean(rdr["IsDocEvidenceSufficientToSupportPaymentType"]),

					Comment = string.Format("{0}", rdr["Comment"]),
					IsCheckListCompleted = rdr["IsCheckListCompleted"] as bool? ?? default(bool),

					CreatedBy = string.Format("{0}", rdr["CreatedBy"]),
					CreatedOn = rdr["CreatedOn"] as DateTime? ?? default(DateTime),
					UpdatedBy = string.Format("{0}", rdr["UpdatedBy"]),
					UpdatedOn = rdr["UpdatedOn"] as DateTime? ?? default(DateTime)
				};

            checkLists.Add(checkList);
		}

		/// <summary>
		/// Gets the claim is.
		/// </summary>
		/// <param name="isAutoClaim">if set to <c>true</c> [is automatic claim].</param>
		/// <param name="isManualClaim">if set to <c>true</c> [is manual claim].</param>
		/// <returns></returns>
		private static string GetClaimIs(bool isAutoClaim, bool isManualClaim)
		{
			var result = string.Empty;

			if (isAutoClaim)
			{
				const string space = "   ";
				result += "The Claim is an Auto Special Claim." + space;
			}

			if (isManualClaim)
			{
					result += "The Claim is a Manual Special Claim.";
			}

			return result;
		}
	}
}