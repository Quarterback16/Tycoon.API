using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GridSettings = MvcJqGrid.GridSettings;
using System.Linq;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	/// Questionnaire Repository
	/// </summary>
	public class QuestionnaireRepository : PatRepository, IQuestionnaireRepository
	{
		/// <summary>
		/// Gets the project questions by project id
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public List<String> GetProjectQuestions( int projectId )
		{
			var list = new List<String>();

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using ( var command = new SqlCommand( "PaGetProjectQuestions", connection ) )
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

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while ( reader.Read() )
						{
							list.Add(  string.Format( "{0}", reader[ "QuestionText" ]  ) );
						}
					}
					finally
					{
						if ( reader != null ) { reader.Close(); }
					}
				}

			}
			return list;
		}

		/// <summary>
		/// Gets the review questionnaire by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public ReviewQuestionnaire GetReviewQuestionnaireByReviewId(int reviewId)
		{
			ReviewQuestionnaire reviewQuestionnaire = null;

			using ( var connection = new SqlConnection( DbConnection ) )
			{
				using (var command = new SqlCommand("PaReviewQuestionnaireGetBy", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter( "@return_value", SqlDbType.Int )
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add( paramReturnValue );

					SqlHelper.AddIntPara(reviewId, "@ReviewId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlParams.ToArray() );
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							reviewQuestionnaire = new ReviewQuestionnaire
								{
									ProjectID = AppHelper.ToInt(reader["ProjectId"]),
									ProjectName = string.Format("{0}", reader["ProjectName"]),
									UploadID = AppHelper.ToInt(reader["UploadId"]),
									UploadName = string.Format("{0}", reader["UploadName"]),
									QuestionnaireID = AppHelper.ToInt(reader["QuestionnaireId"]),
									ReviewID = AppHelper.ToInt(reader["ReviewId"]),
									ReferenceID = AppHelper.ToLong(reader["ReferenceId"]),
									UserID = string.Format("{0}", reader["UserId"]),
									QuestionnaireCode = string.Format("{0}", reader["QuestionnaireCode"]),
									AssessmentOutcomeCode = string.Format("{0}", reader["AssessmentOutcomeCode"]),
									AssessmentOutcomeDescription = string.Format("{0}", reader["AssessmentOutcomeDescription"]),
									RecoveryReasonCode = string.Format("{0}", reader["RecoveryReasonCode"]),
									RecoveryReasonDescription = string.Format("{0}", reader["RecoveryReasonDescription"]),
									RecoveryActionCode = string.Format("{0}", reader["RecoveryActionCode"]),
									RecoveryActionDescription = string.Format("{0}", reader["RecoveryActionDescription"]),
									FinalOutcomeCode = string.Format("{0}", reader["FinalOutcomeCode"]),
									FinalOutcomeDescription = string.Format("{0}", reader["FinalOutcomeDescription"]),
									Date = reader["Date"] as DateTime? ?? default(DateTime),
									CreatedBy = string.Format("{0}", reader["CreatedBy"]),
									CreatedOn = reader["CreatedOn"] as DateTime? ?? default(DateTime),
									UpdatedBy = string.Format("{0}", reader["UpdatedBy"]),
									UpdatedOn = reader["UpdatedOn"] as DateTime? ?? default(DateTime)
								};
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}

			}
			return reviewQuestionnaire;
		}

		/// <summary>
		/// Gets the question answers by review identifier and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public List<QuestionAnswer> GetQuestionAnswersByReviewId(GridSettings gridSettings, int reviewId)
		{
			var questionAnswers = new List<QuestionAnswer>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaQuestionAnswerGetBy", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddIntPara(reviewId, "@ReviewId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							questionAnswers.Add(new QuestionAnswer
								{
									QuestionId = (int) reader["QuestionId"],
									QuestionHeadingCode = string.Format("{0}", reader["QuestionHeadingCode"]),
									QuestionCode = string.Format("{0}", reader["QuestionCode"]),
									QuestionText = string.Format("{0}", reader["QuestionText"]),
									AnswerText = string.Format("{0}", reader["AnswerText"])
								});
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}

			}
			return questionAnswers;
		}

		/// <summary>
		/// Counts the question answers by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public int CountQuestionAnswersByReviewId(int reviewId)
		{
			var theCount = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaQuestionAnswerCountByReviewId", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
						{
							Direction = ParameterDirection.ReturnValue
						};
					sqlParams.Add(paramReturnValue);

					// also get the count back
					var paramCount = new SqlParameter("@Count", SqlDbType.Int)
						{
							Direction = ParameterDirection.InputOutput,
							Value = 0
						};

					sqlParams.Add(paramCount);

					SqlHelper.AddIntPara(reviewId, "@ReviewId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();
					command.ExecuteNonQuery();

					if (((int) command.Parameters["@return_value"].Value) == 0)
					{
						theCount = (int) paramCount.Value;
					}
				}
				return theCount;
			}
		}

		/// <summary>
		/// Gets the question answers by upload identifier and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public List<QuestionAnswer> GetQuestionAnswersByUploadId(GridSettings gridSettings, int uploadId)
		{
			var questionAnswers = new List<QuestionAnswer>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaQuestionAnswerGetBy", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddIntPara(uploadId, "@UploadId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							questionAnswers.Add(new QuestionAnswer
							{
								QuestionId = (int)reader["QuestionId"],
								QuestionHeadingCode = string.Format("{0}", reader["QuestionHeadingCode"]),
								QuestionCode = string.Format("{0}", reader["QuestionCode"]),
								QuestionText = string.Format("{0}", reader["QuestionText"]),
								AnswerText = string.Format("{0}", reader["AnswerText"])
							});
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}

			}
			return questionAnswers;
		}

		/// <summary>
		/// Gets the review questionnaire data by upload id and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public List<ReviewQuestionnaire> GetReviewQuestionnaireData(GridSettings gridSettings, int uploadId)
		{
			var reviewQuestions = new List<ReviewQuestionnaire>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReviewQuestionnaireGetBy", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddIntPara(uploadId, "@UploadId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.HasRows)
						{
							while (reader.Read())
							{
								if (reader.FieldCount > 10)
								{
									// 1st result set which is the review question
									reviewQuestions.Add(new ReviewQuestionnaire
										{
											ProjectID = (int) reader["ProjectId"],
											ProjectName = string.Format("{0}", reader["ProjectName"]),
											UploadID = (int) reader["UploadId"],
											UploadName = string.Format("{0}", reader["ProjectName"]),
											QuestionnaireID = (int) reader["QuestionnaireID"],
											ReviewID = (int) reader["ReviewId"],
											ReferenceID = (long) reader["ReferenceId"],
											UserID = string.Format("{0}", reader["UserId"]),
											QuestionnaireCode = string.Format("{0}", reader["QuestionnaireCode"]),
											AssessmentOutcomeCode = string.Format("{0}", reader["AssessmentOutcomeCode"]),
											AssessmentOutcomeDescription = string.Format("{0}", reader["AssessmentOutcomeDescription"]),
											RecoveryReasonCode = string.Format("{0}", reader["RecoveryReasonCode"]),
											RecoveryReasonDescription = string.Format("{0}", reader["RecoveryReasonDescription"]),
											RecoveryActionCode = string.Format("{0}", reader["RecoveryActionCode"]),
											RecoveryActionDescription = string.Format("{0}", reader["RecoveryActionDescription"]),
											FinalOutcomeCode = string.Format("{0}", reader["FinalOutcomeCode"]),
											FinalOutcomeDescription = string.Format("{0}", reader["FinalOutcomeDescription"]),
											Date = reader["Date"] as DateTime? ?? default(DateTime),
											CreatedBy = string.Format("{0}", reader["CreatedBy"]),
											CreatedOn = reader["CreatedOn"] as DateTime? ?? default(DateTime),
											UpdatedBy = string.Format("{0}", reader["UpdatedBy"]),
											UpdatedOn = reader["ProjectName"] as DateTime? ?? default(DateTime)
										});
								}
								else
								{
									// 2nd result set which is the extra columns of the row from 1st result set
									var questionnairId = (int) reader["QuestionnaireId"];
									var reviewQuestionsnaire = reviewQuestions.First(qa => qa.QuestionnaireID == questionnairId);

									if (reviewQuestionsnaire != null)
									{
										reviewQuestionsnaire.QuestionAnswers.Add(new QuestionAnswer
											{
												QuestionId = (int) reader["QuestionId"],
												QuestionCode = string.Format("{0}", reader["QuestionCode"]),
												AnswerText = string.Format("{0}", reader["AnswerText"]),
												QuestionText = string.Format("{0}", reader["QuestionText"])
											}
											);
									}
								}
							}

							reader.NextResult();
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}

			}
			return reviewQuestions;
		}

		/// <summary>
		/// Counts the review questionnaires by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public int CountReviewQuestionnairesByUploadId(int uploadId)
		{
			var theCount = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReviewQuestionnaireCountByUploadId", connection))
				{
					var sqlParams = new List<SqlParameter>();

					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};
					sqlParams.Add(paramReturnValue);

					// also get the count back
					var paramCount = new SqlParameter("@Count", SqlDbType.Int)
					{
						Direction = ParameterDirection.InputOutput,
						Value = 0
					};

					sqlParams.Add(paramCount);

					SqlHelper.AddIntPara(uploadId, "@UploadId", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();
					command.ExecuteNonQuery();

					if (((int)command.Parameters["@return_value"].Value) == 0)
					{
						theCount = (int)paramCount.Value;
					}
				}
				return theCount;
			}
		}
	}
}