namespace ProgramAssuranceTool.Helpers
{
	public class DataConstants
	{
		//  ADW lists
		public const string AdwListCodeForBulletinTypes = "PBT";

		public const string AdwListCodeForProjectTypes = "PPR";
		public const string AdwListCodeForProjectContracts = "PAC";
		public const string AdwListCodeForContractTypes = "CTY";
		public const string AdwListCodeForOrgCodes = "ORG";
		public const string AdwListCodeForSiteCodes = "DIS";
		public const string AdwListCodeForEsaCodes = "ESA";
		public const string AdwListCodeForClaimTypes = "CMY";
		public const string AdwListCodeForStateCodes = "STT";

		public const string AdwListCodeForLmrCodes = "LMR";

		public const string AdwRelatedListCodeForPatClaimTypes = "ESFC";

		public const string AdwListCodeForAssessmentCodes = "RAS";
		public const string AdwListCodeForAssessmentActionCodes = "RAF";
		public const string AdwListCodeForOutcomeCodes = "RFO";
		public const string AdwListCodeForRecoveryReasonCodes = "RFD";

		public const string AdwListCodeForComplianceRiskReportSortOrder = "COI";
		public const string AdwListCodeForSiteVisitReportSortOrder = "SVR";
		public const string AdwListCodeForProgressReportSortOrder = "PGR";
		public const string AdwListCodeForProviderSummaryReportSortOrder = "SMR";

		//  ADW "special" values
//		public const string AdwLCodeForProjectType_Contract_Monitoring = "CMO";  

		//  ADW State Codes
		public static string StateQueensland = "QLD";
		public static string StateNewSouthWales = "NSW";
		public static string StateVictoria = "VIC";
		public static string StateAustralianCapitalTerritory = "ACT";
		public static string StateNorthernTerritory = "NT";
		public static string StateSouthAustralia = "SA";
		public static string StateTasmania = "TAS";
		public static string StateWesternAustralia = "WA";

		public static string ConversionUserId = "CONVERSION";
		public static string BatchUserId = "BATCH";

		//  SQL parameters
		public static string SqlOperationContains = "0";
		public static string SqlOperationEquals = "1";

		//  Compliance Indicators
		public static string ComplianceIndicatorSubject_Organisations = "ORG";
		public static string ComplianceIndicatorSubject_ESAs = "ESA";
		public static string ComplianceIndicatorSubject_Sites = "SITE";

		public static int StandardFieldIdForJobseekerGivenName = 11;
		public static int StandardFieldIdForJobseekerSurname = 5;
		public static int StandardFieldIdForClaimCreationDate = 143;
		public static int StandardFieldIdForClaimType_Prod = 98;
		public static int StandardFieldIdForClaimType_Dev = 130;

		public static string Ascending = "ASC";
		public static string Descending = "DESC";

		// Project Type Report
		public const string ProjectByOrg = "ORG";
		public const string ProjectByType = "TYPE";
		public const string ProjectByESA = "ESA";
		public const string ProjectByState = "STATE";
		public const string ProjectByNational = "NATIONAL";

		// Finding Summary Report
		public const string FindingSummaryInScope = "INSCOPE";
		public const string FindingSummaryOutScope = "OUTSCOPE";
		public const string FindingSummaryRecovery = "RECOVERY";

		// Bulletin
		public const string StandardBulletinType = "STD";
		public const string FaqBulletinType = "FAQ";

		//  special Outcome codes
		public const string AssessmentValid = "VLD";
		public const string AssessmentValidwithQualification = "VLQ";
		public const string FinalOutcomeValid_NFA = "VAN";

		//  NEW Outcome Codes

		public static string PatReviewOutcome_InProgress = "";
		public static string PatReviewOutcome_Valid_NFA = "VAN";
		public static string PatReviewOutcome_Valid_AdminDeficiency = "VAD";
		public static string PatReviewOutcome_Invalid_Recovery = "INR";
		public static string PatReviewOutcome_Invalid_No_Recovery = "INN";
		public static string PatReviewOutcome_Invalid_AdminDeficiency = "IAD";

		// Project Types
		public const string ProjectType_Contract_Monitoring = "CMO";
		public const string ProjectType_Site_Monitoring = "SMO";
		public const string ProjectType_Program_Assurance_National = "PAN";
		public const string ProjectType_Program_Assurance_State = "PAS";
		public const string ProjectType_Special_Claims_Quality_Assurance = "SCQ";

		// Yes/ No Indicator
		public const string AdwListCodeForYesNo = "IND";
		public const string YES = "Y";
		public const string NO = "N";

	}
}