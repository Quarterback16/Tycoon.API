using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.System;

namespace ProgramAssuranceTool.Helpers
{
	public class BuroProcess
	{
		public const string BuroUser = "BURO";

		public bool IsDirty { get; set; }
		/// <summary>
		///   BURO = Batch Update Review Outcomes
		/// </summary>
		/// <returns></returns>
		public BuroViewModel Execute( BuroViewModel vm
			, IUploadRepository uploadRepository
			, IQuestionnaireRepository questionnaireRepository
			, IAuditService auditService
			, IReviewRepository reviewRepository )
		{
			var uploadList = uploadRepository.GetAll();
			foreach ( var upload in uploadList )
			{
				if (upload.AcceptedFlag) continue;  //  once upload is accepted its over

				var uploadId = upload.UploadId;
				var reviewList = reviewRepository.GetAllByUploadId( uploadId );
				foreach ( var review in reviewList )
				{
					vm.ReviewsRead++;
					IsDirty = false;
					//  get the questionaire data
					var q = questionnaireRepository.GetReviewQuestionnaireByReviewId( review.ReviewId );

					if (q == null) continue;

					var oldVals = review;
					CheckForQuestionaireAssessmentOutcome( q, review );
					CheckForQuestionaireRecoveryReason( q, review );
					CheckForQuestionaireAssessmentAction( q, review );
					CheckForQuestionaireFinalOutcome( q, review );

					if (!IsDirty) continue;

					var errors = new List<IntegrityError>();
					ApplyReviewModelValidations( review, errors );
					if (errors.Count > 0)
					{
						AuditErrors( auditService, errors );
						vm.ValidationErrors += errors.Count;
					}
					else
					{
						review.UpdatedBy = BuroUser;
						review.UpdatedOn = DateTime.Now;
						reviewRepository.Update( review );
						auditService.AuditActivity( UpdateMessage( review, oldVals ), BuroUser );
						vm.ReviewsUpdated++;
					}
				}
			}
			return vm;
		}

		private static string UpdateMessage( Review review, Review oldVal )
		{
			return string.Format( 
				"Review {0} updated 1:{1}>{2} 2:{3}>{4} 3:{5}>{6} 4:{7}>{8}",
				review.ReviewId,
				oldVal.AssessmentCode,
				review.AssessmentCode,
				oldVal.RecoveryReason,
            review.RecoveryReason,
				oldVal.AssessmentAction,
			   review.AssessmentAction,
				oldVal.OutcomeCode,
			   review.OutcomeCode );

		}

		private static void AuditErrors( IAuditService auditService, IEnumerable<IntegrityError> errors )
		{
			foreach ( var integrityError in errors )
			{
				var ex = new ApplicationException( integrityError.ToString() );
				Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
				auditService.AuditActivity( integrityError.ToString(), BuroUser );
			}
		}

		private static void ApplyReviewModelValidations( Review review, ICollection<IntegrityError> errorList )
		{
			var context = new ValidationContext( review, null, null );
			var results = new List<ValidationResult>();
			Validator.TryValidateObject( review, context, results, true );
			foreach ( var validationResult in results )
			{
				AddIntegrityError( errorList, validationResult.MemberNames.FirstOrDefault(), 
					"Review", review.ReviewId, validationResult.ErrorMessage, "Validation" );
			}
		}

		private static void AddIntegrityError( ICollection<IntegrityError> errorList, string column, string table,
			int key, string message, string errorCategory )
		{
			var error = new IntegrityError
			{
				Column = column,
				Table = table,
				Key = key,
				Message = message,
				ErrorCategory = errorCategory,
				Detected = DateTime.Now
			};
			errorList.Add( error );
		}

		private void CheckForQuestionaireAssessmentOutcome( ReviewQuestionnaire q, Review review )
		{
			if (q.AssessmentOutcomeCode == review.AssessmentCode) return;
			if (q.Date <= review.AssessmentDate) return;
			review.AssessmentCode = q.AssessmentOutcomeCode;
			review.AssessmentDate = q.Date;
			IsDirty = true;
		}

		private void CheckForQuestionaireRecoveryReason( ReviewQuestionnaire q, Review review )
		{
			if ( q.RecoveryReasonCode == review.RecoveryReason ) return;
			if ( q.Date <= review.RecoveryReasonDate ) return;
			review.RecoveryReason = q.RecoveryReasonCode;
			review.RecoveryReasonDate = q.Date;
			IsDirty = true;
		}

		private void CheckForQuestionaireAssessmentAction( ReviewQuestionnaire q, Review review )
		{
			if ( q.RecoveryActionCode == review.AssessmentAction ) return;
			if ( q.Date <= review.AssessmentActionDate ) return;
			review.AssessmentAction = q.RecoveryActionCode;
			review.AssessmentActionDate = q.Date;
			IsDirty = true;
		}

		private void CheckForQuestionaireFinalOutcome( ReviewQuestionnaire q, Review review )
		{
			if ( q.FinalOutcomeCode == review.OutcomeCode ) return;
			if ( q.Date <= review.FinalOutcomeDate ) return;
			review.OutcomeCode = q.FinalOutcomeCode;
			review.FinalOutcomeDate = q.Date;
			IsDirty = true;
		}
	}
}