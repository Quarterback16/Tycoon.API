using System.Collections.Generic;
using System.IO;
using ProgramAssuranceTool.Models;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	///   The Interface into the Review data store
	/// </summary>
	public interface IReviewRepository
	{
		/// <summary>
		/// Gets all the reviews that match the grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns>list of review objects</returns>
		List<Review> GetAll( GridSettings gridSettings );
		/// <summary>
		/// Gets the Review by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>review object</returns>
		Review GetById( int id );
		/// <summary>
		/// Adds the specified review.
		/// </summary>
		/// <param name="review">The review.</param>
		/// <returns>id of the new review</returns>
		int Add( Review review );
		/// <summary>
		/// Updates the specified review.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Update( Review entity );
		/// <summary>
		/// Deletes the specified Review.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="userId">The user identifier.</param>
		void Delete( int id, string userId );
		/// <summary>
		/// Counts the reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>the number of reviews</returns>
		int CountReviews( int projectId );
		/// <summary>
		/// Counts the finished reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>the number of reviews</returns>
		int CountFinishedReviews( int projectId );
		/// <summary>
		/// Counts the number of reviews in an upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>the number of reviews</returns>
		int CountReviewsByUploadId( int uploadId );
		/// <summary>
		/// Gets all the Reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>list of review objects</returns>
		IEnumerable<Review> GetAllByProjectId( int projectId );
		/// <summary>
		/// Gets all the finished Reviews for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>list of review objects</returns>
		IEnumerable<Review> GetFinishedReviewsByProjectId( int projectId );
		/// <summary>
		/// Gets all reviews by project paged.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The gs.</param>
		/// <returns>list of review objects</returns>
		List<Review> GetAllByProjectIdPaged( int projectId, GridSettings gs );
		/// <summary>
		/// Gets all the reviews for a particular upload and grid criteria.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="gs">The grid settings.</param>
		/// <returns>list of review objects</returns>
		List<Review> GetAllByUploadIdPaged( int uploadId, GridSettings gs );
		/// <summary>
		/// Gets all the reviews for a particular upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>list of review objects</returns>
		List<Review> GetAllByUploadId( int uploadId );
		/// <summary>
		/// Deletes a set of reviews.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <param name="userId">The user identifier.</param>
		void DeleteReviews( List<int> reviewIds, string userId );
		/// <summary>
		///  Updates a set of reviews with the same outcomes.
		/// </summary>
		/// <param name="reviews">The reviews.</param>
		/// <param name="outcome">The outcome.</param>
		/// <param name="userId">The user identifier.</param>
		void BulkOutcome( IEnumerable<int> reviews, Review outcome, string userId );
		/// <summary>
		///   Creates reviews from a CSV stream
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>the number of reviews</returns>
		int StoreReviewData( Upload upload, Stream stream );
		/// <summary>
		/// Counts the completed reviewsin a certain upload.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>the count</returns>
		int CountCompletedReviewsByUploadId( int uploadId );
	}
}