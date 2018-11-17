using System;
using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	/// Questionnaire Repository
	/// </summary>
	public interface IQuestionnaireRepository
	{
		/// <summary>
		/// Gets the review questionnaire by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		ReviewQuestionnaire GetReviewQuestionnaireByReviewId(int reviewId);

		/// <summary>
		/// Gets the question answers by review identifier and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		List<QuestionAnswer> GetQuestionAnswersByReviewId(MvcJqGrid.GridSettings gridSettings, int reviewId);

		/// <summary>
		/// Counts the question answers by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		int CountQuestionAnswersByReviewId(int reviewId);

		/// <summary>
		/// Gets the question answers by upload identifier and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		List<QuestionAnswer> GetQuestionAnswersByUploadId(MvcJqGrid.GridSettings gridSettings, int uploadId);

		/// <summary>
		/// Gets the review questionnaire data by upload id and grid settings
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		List<ReviewQuestionnaire> GetReviewQuestionnaireData(MvcJqGrid.GridSettings gridSettings, int uploadId);

		/// <summary>
		/// Counts the review questionnaires by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		int CountReviewQuestionnairesByUploadId(int uploadId);

		/// <summary>
		/// Gets the project questions by project id
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		List<String> GetProjectQuestions( int projectId );
	}
}