using System;
using System.Web;
using ProgramAssuranceTool.ViewModels;
using ProgramAssuranceTool.ViewModels.Review;

namespace ProgramAssuranceTool.Helpers
{
	public class SessionHelper
	{
		public static void SetSessionProjectId( HttpSessionStateBase session, int projectId )
		{
			session[ CommonConstants.SessionProjectId ] = projectId;
		}

		public static int GetSessionProjectId( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionProjectId ] == null
						? 0
						: Int32.Parse( session[ CommonConstants.SessionProjectId ].ToString() );
		}

		public static void SetSessionReviewDetails( HttpSessionStateBase session, string html )
		{
			if ( String.IsNullOrEmpty( html ) ) html = String.Empty;
			session[ CommonConstants.SessionReviewDetails ] = html;
		}

		public static string GetSessionReviewDetails( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionReviewDetails ] == null
						? string.Empty
						: session[ CommonConstants.SessionReviewDetails ].ToString();
		}

		public static void SetSessionRelatedData( HttpSessionStateBase session, string html )
		{
			if ( String.IsNullOrEmpty( html ) ) html = String.Empty;
			session[ CommonConstants.SessionRelatedData ] = html;
		}

		public static string GetSessionRelatedData( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionRelatedData ] == null
						? string.Empty
						: session[ CommonConstants.SessionRelatedData ].ToString();
		}

		public static void SetSessionAdminStatus( HttpSessionStateBase session, string adminStatus )
		{
			session[ CommonConstants.SessionAdminStatus ] = adminStatus;
		}

		public static string GetSessionAdminStatus( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionAdminStatus ] == null
						? string.Empty
						: session[ CommonConstants.SessionAdminStatus ].ToString();
		}

		public static void SetSessionUploadedFrom( HttpSessionStateBase session, string uploadedFrom )
		{
			session[ CommonConstants.SessionUploadedFrom ] = uploadedFrom;
		}

		public static string GetSessionUploadedFrom( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionUploadedFrom ] == null
						? string.Empty
						: session[ CommonConstants.SessionUploadedFrom ].ToString();
		}

		public static void SetSessionUploadedTo( HttpSessionStateBase session, string uploadedTo )
		{
			session[ CommonConstants.SessionUploadedTo ] = uploadedTo;
		}

		public static string GetSessionUploadedTo( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionUploadedTo ] == null
						? string.Empty
						: session[ CommonConstants.SessionUploadedTo ].ToString();
		}

		public static void SetSessionProjectGrid( HttpSessionStateBase session, string html )
		{
			session[ CommonConstants.SessionProjectGrid ] = html;
		}

		public static string GetSessionProjectGrid( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionProjectGrid ] == null
						? string.Empty
						: session[ CommonConstants.SessionProjectGrid ].ToString();
		}

		public static void SetSessionSiteCodeCriteria( HttpSessionStateBase session, string criteria )
		{
			session[ CommonConstants.SessionSiteCodeCriteria ] = criteria;
		}

		public static string GetSessionSiteCodeCriteria( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionSiteCodeCriteria ] == null
						? string.Empty
						: session[ CommonConstants.SessionSiteCodeCriteria ].ToString();
		}

		public static void SetSessionReviewListCriteria( HttpSessionStateBase session, ReviewListCriteriaViewModel criteria )
		{
			session[ CommonConstants.SessionSiteCodeCriteria ] = criteria;
		}

		public static ReviewListCriteriaViewModel GetSessionReviewListCriteria( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionSiteCodeCriteria ] == null
						? new ReviewListCriteriaViewModel()
						: (ReviewListCriteriaViewModel) session[ CommonConstants.SessionSiteCodeCriteria ];
		}

		public static void SetSessionCustomisations( HttpSessionStateBase session, CustomiseReviewGridViewModel customisations )
		{
			session[ CommonConstants.SessionCustomisations ] = customisations;
		}

		public static CustomiseReviewGridViewModel GetSessionCustomisations( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionCustomisations ] == null
						? new CustomiseReviewGridViewModel()
						: (CustomiseReviewGridViewModel) session[ CommonConstants.SessionCustomisations ];
		}

		public static ReviewDetailsViewModel GetSessionBulkOutcomes( HttpSessionStateBase session )
		{
			return session[ CommonConstants.SessionBulkOutcomes ] == null
						? new ReviewDetailsViewModel()
						: (ReviewDetailsViewModel) session[ CommonConstants.SessionBulkOutcomes ];
		}

		public static void SetSessionBulkReviewOutcomes( HttpSessionStateBase session, ReviewDetailsViewModel vm )
		{
			session[ CommonConstants.SessionBulkOutcomes ] = vm;
		}

	}
}