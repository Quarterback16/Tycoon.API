using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Helpers
{
	/// <summary>
	///   Class responsible for exporting Review Data to CSV (Excel) file
	/// </summary>
	public class ReviewExporter
	{
		public string ExportReviews( List<Review> reviewList, IPatService patService )
		{
			var sw = new StringWriter();

			//  First row is the Column Headers, now standardised
			WriteColumnHeaders( sw );

			foreach ( var review in reviewList )
			{
				var sb = new StringBuilder();

				WriteCell( sb, review.ReviewId.ToString( CultureInfo.InvariantCulture ) );

				WriteCell( sb, review.UploadId.ToString( CultureInfo.InvariantCulture ) );
				WriteCell( sb, review.ProjectId.ToString( CultureInfo.InvariantCulture ) );

				WriteCell( sb, review.OrgCode );
				WriteCell( sb, patService.GetOrgName( review.OrgCode ) );

				WriteCell( sb, review.ESACode );
				WriteCell( sb, patService.GetEsaDescription( review.ESACode ) );

				WriteCell( sb, review.SiteCode );
				WriteCell( sb, patService.GetSiteDescription( review.SiteCode ) );

				WriteCell( sb, review.StateCode );
				WriteCell( sb, review.ManagedBy );

				WriteCell( sb, review.ClaimId.ToString( CultureInfo.InvariantCulture ) );
				WriteCell( sb, review.ClaimType );
				WriteCell( sb, patService.GetClaimTypeDescription( review.ClaimType ) );

				WriteCell( sb, AppHelper.ShortDate(review.ClaimCreationDate) );

				WriteCell( sb, review.ContractType );
				WriteCell( sb, patService.GetContractTypeDescription( review.ContractType ) );

				WriteCell( sb, AppHelper.NullableDollarAmount( review.ClaimAmount) );
				WriteCell( sb, AppHelper.FlagOut( review.AutoSpecialClaim ) );
				WriteCell( sb, AppHelper.FlagOut( review.ManualSpecialClaim ) );

				WriteCell( sb, review.JobseekerId.ToString( CultureInfo.InvariantCulture ) );
				WriteCell( sb, review.JobSeekerGivenName );
				WriteCell( sb, review.JobSeekerSurname );

				WriteCell( sb, review.ActivityId.ToString( CultureInfo.InvariantCulture ) );

				WriteCell( sb, review.AssessmentCode );
				WriteCell( sb, patService.GetAssessmentDescription(review.AssessmentCode) );

				WriteCell( sb, review.RecoveryReason );
				WriteCell( sb, patService.GetRecoveryReasonDescription( review.RecoveryReason ) );

				WriteCell( sb, review.AssessmentAction );
				WriteCell( sb, patService.GetAssessmentActionDescription( review.AssessmentAction ) );

				WriteCell( sb, review.OutcomeCode );
				WriteCell( sb, patService.GetFinalOutcomeDescription( review.OutcomeCode ) );

				WriteCell( sb, AppHelper.FlagOut( review.IsOutOfScope ) );
				WriteCell( sb, AppHelper.FlagOut( review.IsAdditionalReview ) );

				WriteCell( sb, review.Comments );

				sw.WriteLine( sb.ToString() );
			}
			return sw.ToString();
		}

		private static void WriteCell( StringBuilder sb, string value )
		{
			sb.Append( String.Format( "\"{0}\",", value ) );
		}

		private static void WriteColumnHeaders( TextWriter sw )
		{
			var sb = new StringBuilder();

			AddColumn( sb, CommonConstants.ReviewColumnReviewId );
			AddColumn( sb, CommonConstants.ReviewColumnUploadId );
			AddColumn( sb, CommonConstants.ReviewColumnProjectId );

			AddColumn( sb, CommonConstants.ReviewColumnOrgCode );
			AddColumn( sb, CommonConstants.ReviewColumnOrgName );
			AddColumn( sb, CommonConstants.ReviewColumnEsaCode );
			AddColumn( sb, CommonConstants.ReviewColumnEsaName );

			AddColumn( sb, CommonConstants.ReviewColumnSiteCode );
			AddColumn( sb, CommonConstants.ReviewColumnSiteName );

			AddColumn( sb, CommonConstants.ReviewColumnStateCode );
			AddColumn( sb, CommonConstants.ReviewColumnManagedBy );

			AddColumn( sb, CommonConstants.ReviewColumnClaimId );
			AddColumn( sb, CommonConstants.ReviewColumnClaimType );
			AddColumn( sb, CommonConstants.ReviewColumnClaimTypeDescription );
			AddColumn( sb, CommonConstants.ReviewColumnClaimCreationDate );
			AddColumn( sb, CommonConstants.ReviewColumnContractType );
			AddColumn( sb, CommonConstants.ReviewColumnContractTypeDescription );
			AddColumn( sb, CommonConstants.ReviewColumnClaimAmount );
			AddColumn( sb, CommonConstants.ReviewColumnAutoSpecialClaim );
			AddColumn( sb, CommonConstants.ReviewColumnManualSpecialClaim );

			AddColumn( sb, CommonConstants.ReviewColumnJobseekerId );
			AddColumn( sb, CommonConstants.ReviewColumnJobseekerGivenName );
			AddColumn( sb, CommonConstants.ReviewColumnJobseekerSurname );

			AddColumn( sb, CommonConstants.ReviewColumnActivityId );

			AddColumn( sb, "Assessment Outcome Code" );
			AddColumn( sb, CommonConstants.ReviewColumnAssessmentCodeDescription );

			AddColumn( sb, "Recovery Reason Code" );
			AddColumn( sb, CommonConstants.ReviewColumnRecoveryReasonDescription );

			AddColumn( sb, "Assessment Action Code" );
			AddColumn( sb, CommonConstants.ReviewColumnAssessmentActionDescription ); 

			AddColumn( sb, "Final Outcome Code" );
			AddColumn( sb, "Final Outcome Desc" );

			AddColumn( sb, CommonConstants.ReviewColumnIsOutOfScope );
			AddColumn( sb, CommonConstants.ReviewColumnIsAdditional );

			AddColumn( sb, CommonConstants.ReviewColumnComments );

			sw.WriteLine( sb.ToString() );
		}

		private static void AddColumn( StringBuilder sb, string columnHeader )
		{
			sb.Append( string.Format( "\"{0}\",", columnHeader ) );
		}
	}
}