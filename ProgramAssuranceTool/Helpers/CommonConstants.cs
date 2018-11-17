namespace ProgramAssuranceTool.Helpers
{
	public class CommonConstants
	{
		//  Grids
		public const int GridStandardNoOfRows = 20;
		public const int GridStandardDoubleRows = 20;
		public const int GridStandardHeight = 440;  // was 240;
		public const int GridStandardWidth = 1070;
		public const int GridDefaultPageSize = 20;
		public const int GridMaxRecordToDownload = 100000;

		public const string ReviewListColumn = "ReviewList-Col";

		//  Connection strings
		public static string ConnectionString = "ProgramAssuranceConnectionString";

		// Session variables
		public const string SessionProjectId = "SESSION_PROJECTID";
		public const string SessionProjectName = "SESSION_PROJECTNAME";
		public const string SessionUploadId = "SESSION_UPLOADID";
		public const string SessionUploadName = "SESSION_UPLOADNAME";
		public const string SessionApplicationId = "SESSION_APPLICATIONID";
		public const string SessionStatusCode = "SESSION_STATUSCODE";

		public const string SessionQueryUserId = "SESSION_QUERYUSERID";
		public const string SessionQueryFromDate = "SESSION_QUERYFROMDATE";
		public const string SessionQueryToDate = "SESSION_QUERYTODATE";

		public const string SessionReviewDetails = "SESSION_REVIEWDETAILS";
		public const string SessionRelatedData = "SESSION_RELATEDDATA";

		public const string SessionSampleData = "SESSION_SAMPLEDATA";

		public const string SessionAdminStatus = "SESSION_ADMINSTATUS";

		public const string SessionUploadedFrom = "SESSION_UPLOADEDFROM";
		public const string SessionUploadedTo = "SESSION_UPLOADEDTO";
		public const string SessionProjectGrid = "SESSION_PROJECTGRID";

		public const string SessionSiteCodeCriteria = "SESSION_SITECODE";

		public const string SessionCustomisations = "SESSION_CUSTOMISATIONS";
		public const string SessionBulkOutcomes = "SESSION_BULKOUTCOMES";

		public const string SessionProjectListFilters = "ProjectGridSettings";

		public const string SessionCheckListsOriginal = "SESSION_CHECKLISTS_ORIGINAL";
		public const string SessionCheckListsUserModified = "SESSION_CHECKLISTS_USERMODIFIED";
		public const string SessionCheckListUserModified = "SESSION_CHECKLIST_USERMODIFIED";

		//  flash notification message types
		public const string FlashMessageTypeInfo = "info";
		public const string FlashMessageTypeWarning = "warning";
		public const string FlashMessageTypeError = "error";

		// used to identify easily whether or not the element is enabled or disabled
		public const string EnabledElement  = "enabled=true";
		public const string DisabledElement = "enabled=false disabled=disabled";

		//  Provider Search Type
		public static string ProviderSearchByAbn = "ABN";

		//  dummy values
		public static string TemporaryAbn = "99999999999";

		//  Access control groups
		public static string Domain = "NATION\\";

		public static string PaamGroupAdministrators = "APP_PAAM_ADMIN";

		public static string PaamGroupGeneral = "APP_PAAM";
		public static string PaamGroupNo = "APP_PAAM_NO";
		public static string PaamGroupAct = "APP_PAAM_ACT"; //  merging this one into NSW
		public static string PaamGroupNsw = "APP_PAAM_NSW";
		public static string PaamGroupNt = "APP_PAAM_NT";
		public static string PaamGroupQld = "APP_PAAM_QLD";
		public static string PaamGroupSa = "APP_PAAM_SA";
		public static string PaamGroupTas = "APP_PAAM_TAS";
		public static string PaamGroupVic = "APP_PAAM_VIC";
		public static string PaamGroupWa = "APP_PAAM_WA";

		//  App Settings
		public static string LdapPath = "LdapPath";
		public static string LdapGroupQueryPath = "LdapGroupQueryPath";
		public static string LdapUserQueryPath = "LdapUserQueryPath";

		//  Findings Code Types
		public const int NormalFindingsCode = 0;
		public const int ExtendedFindingsCode = 1;

		#region  Validation data types

		public const string DataTypeText = "Text";
		public const string DataTypeNumber = "Number";
		public const string DataTypeDate = "Date";
		public const string DataTypeCurrency = "Currency";
		public const string DataTypeFlag = "Flag";

		public const int DataTypeTextMaximumLength = 100;

		#endregion

		#region  Upload Outcome Columns

		public static string OutcomeColumnAssessmentCode = "Assessment Code";
		public static string OutcomeColumnAssessmentActionCode = "Assessment Action";
		public static string OutcomeColumnRecoveryReasonCode = "Recovery Reason Code";
		public static string OutcomeColumnOutcomeCode = "Outcome Code";
		public static string OutcomeColumnComments = "Outcome Comments";
		public static string OutcomeColumnScopeFlag = "Out of Scope";
		public static string OutcomeColumnAdditionalReviewFlag = "Additional";
		public static string OutcomeColumnClaimRecoveryAmount = "Claim Recovery Amount";
		public static string OutcomeColumnClaimPartialRecovery = "Partial Recovery";

		#endregion

		#region  Standard Upload columns

		public const string UploadColumn_JobseekerId = "JOB SEEKER ID";
		public const string UploadColumn_ClaimId = "CLAIM ID";
		public const string UploadColumn_ClaimSequenceNumber = "CLAIM SEQUENCE NUMBER";
		public const string UploadColumn_JobseekerGivenName = "JOB SEEKER GIVEN NAME";
		public const string UploadColumn_JobseekerSurname = "JOB SEEKER SURNAME";
		public const string UploadColumn_ClaimAmount = "CLAIM AMOUNT";
		public const string UploadColumn_ClaimType = "CLAIM TYPE";
		public const string UploadColumn_ClaimTypeDescription = "CLAIM TYPE DESCRIPTION";
		public const string UploadColumn_EmploymentServiceAreaCode = "EMPLOYMENT SERVICE AREA CODE";
		public const string UploadColumn_EmploymentServiceAreaName = "EMPLOYMENT SERVICE AREA NAME";
		public const string UploadColumn_OrgCode = "ORG CODE";
		public const string UploadColumn_OrgName = "ORG NAME";
		public const string UploadColumn_SiteCode = "SITE CODE";
		public const string UploadColumn_SiteName = "SITE NAME";
		public const string UploadColumn_StateCode = "STATE CODE";
		public const string UploadColumn_ManagedBy = "MANAGED BY";
		public const string UploadColumn_ContractType = "CONTRACT TYPE";
		public const string UploadColumn_ContractTypeDescription = "CONTRACT TYPE DESCRIPTION";
		public const string UploadColumn_ClaimCreationDate = "CLAIM CREATION DATE";
		public const string UploadColumn_ActivityId = "ACTIVITY ID";
		public const string UploadColumn_AutoSpecialCLaimFlag = "AUTO SPECIAL CLAIM FLAG";
		public const string UploadColumn_ManualSpecialClaimFlag = "MANUAL SPECIAL CLAIM FLAG";

		public const string UploadColumn_AssessmentCode = "ASSESSMENT CODE";
		public const string UploadColumn_AssessmentActionCode = "ASSESSMENT ACTION";
		public const string UploadColumn_RecoveryReasonCode = "RECOVERY REASON CODE";
		public const string UploadColumn_OutcomeCode = "OUTCOME CODE";

		public const string UploadColumn_ScopeFlag = "OUT OF SCOPE";
		public const string UploadColumn_AdditionalReviewFlag = "ADDITIONAL";

		public const string UploadColumn_ClaimRecoveryAmount = "CLAIM RECOVERY AMOUNT";
		public const string UploadColumn_Comments = "OUTCOME COMMENTS";

		public const int NumberOfStandardColumns = 22;

		#endregion

		#region  Questions Upload Columns

		public const string QuestionColumn_QuestionType = "TYPE";
		public const string QuestionColumn_QuestionText = "QUESTION TEXT";
		public const string QuestionColumn_AnswerColumn = "ANSWER COLUMN";

		#endregion

		#region New Outcome Codes

		public static string PatReviewOutcome_InProgress = "";
		public static string PatReviewOutcome_Valid_NFA = "Valid - NFA";
		public static string PatReviewOutcome_Valid_AdminDeficiency = "Valid (Admin Deficiency – Provider education)";
		public static string PatReviewOutcome_Invalid_Recovery = "Invalid (recovery)";
		public static string PatReviewOutcome_Invalid_No_Recovery = "Invalid (no recovery)";
		public static string PatReviewOutcome_Invalid_AdminDeficiency = "Invalid (Admin deficiency - provider education)";

		#endregion

		#region  Standard Field Names

		public const string JobSeekerGivenName = "Job Seeker Given Name";
		public const string JobSeekerSurname = "Job Seeker Surname";
		public const string ClaimCreationDate = "Claim Creation Date";
		public const string ClaimType = "Claim Type";
		public const string ContractType = "Contract Type";

		#endregion

		#region Report

		public const string Pdf = "PDF";
		public const string Word = "WORD";
		public const string Csv = "CSV";

		#endregion

		#region  Project Tabs

		public const int ProjectTab_Details = 1;
		public const int ProjectTab_Samples = 2;
		public const int ProjectTab_Reviews = 3;
		public const int ProjectTab_Contracts = 4;
		public const int ProjectTab_Documents = 5;
		public const int ProjectTab_Questions = 6;

		#endregion

		#region Button definition

		  public const string ButtonUndefined = "UNDEFINED";
		 public const string ButtonSave = "SAVE";
		  public const string ButtonDelete = "DELETE";
		  public const string ButtonPrevious = "PREVIOUS";
		  public const string ButtonNext = "NEXT";
		  public const string ButtonCancel = "CANCEL";
		  public const string ButtonDownload = "DOWNLOAD";
		 
		#endregion

		#region  Resource Codes

		public static string ResourceCode_NO = "NO";
		public static string ResourceCode_NSWACT = "NSWACT";
		public static string ResourceCode_QLD = "QLD";
		public static string ResourceCode_VIC = "VIC";
		public static string ResourceCode_SA = "SA";
		public static string ResourceCode_WA = "WA";
		public static string ResourceCode_TAS = "TAS";
		public static string ResourceCode_NT = "NT";

		 #endregion

		#region  Column headings for Review Export

		public static string ReviewColumnReviewId = "Review ID";
		public static string ReviewColumnUploadId = "Upload ID";
		public static string ReviewColumnProjectId = "Project ID";
		public static string ReviewColumnComments = "Comments";
		public static string ReviewColumnSiteCode = "Site Code";
		public static string ReviewColumnOrgCode = "Org Code";
		public static string ReviewColumnStateCode = "State Code";
		public static string ReviewColumnClaimAmount = "Claim Amount";
		public static string ReviewColumnClaimId = "Claim Id";
		public static string ReviewColumnClaimSeqNumber = "Claim Seq No";
		public static string ReviewColumnClaimRecoveryAmount = "Recovery Amount";
		public static string ReviewColumnClaimType = "Claim Type";
		public static string ReviewColumnClaimTypeDescription = "Claim Type Description";
		public static string ReviewColumnContractType = "Contract Type";
		public static string ReviewColumnContractTypeDescription = "Contract Type Description";
		public static string ReviewColumnAutoSpecialClaim = "Auto Special Claim";
		public static string ReviewColumnManualSpecialClaim = "Manual Special Claim";
		public static string ReviewColumnJobseekerId = "Jobseeker ID";
		public static string ReviewColumnJobseekerGivenName = "Jobseeker Given Name";
		public static string ReviewColumnJobseekerSurname = "Jobseeker Surname";
		public static string ReviewColumnActivityId = "Activity ID";
		public static string ReviewColumnIsAdditional = "Additional";
		public static string ReviewColumnIsOutOfScope = "Out of Scope";
		public static string ReviewColumnManagedBy = "Managed By";
		public static string ReviewColumnEsaCode = "ESA Code";
		public static string ReviewColumnEsaName = "ESA Name";
		public static string ReviewColumnOrgName = "Org Name";
		public static string ReviewColumnSiteName = "Site Name";

		public static string ReviewColumnAssessmentCode = "Assessment Code";
		public static string ReviewColumnOutcomeCode = "Outcome Code"; 
		public static string ReviewColumnRecoveryReason = "Recovery Reason";
		public static string ReviewColumnAssessmentAction = "Assessment Action";

		public static string ReviewColumnAssessmentCodeDescription = "Assessment Outcome Desc";
		public static string ReviewColumnOutcomeCodeDescription = "Outcome Desc";
		public static string ReviewColumnRecoveryReasonDescription = "Recovery Reason Desc";
		public static string ReviewColumnAssessmentActionDescription = "Assessment Action Desc";

		public static string ReviewColumnClaimCreationDate = "Claim Creation Date";

		#endregion

		#region  Column Headings for Review List

		public const string ReviewListHeadingAutoSpecialClaim = "Auto Special Claim";
		public const string ReviewListFieldAutoSpecialClaim = "AutoSpecialClaim";
		public const string ReviewListHeadingManualSpecialClaim = "Manual Special Claim";
		public const string ReviewListFieldManualSpecialClaim = "ManualSpecialClaim";
		public const string ReviewListHeadingJobSeekerId = "Job Seeker ID";
		public const string ReviewListFieldJobSeekerId = "JobseekerId";
		public const string ReviewListHeadingClaimId = "Claim ID";
		public const string ReviewListFieldClaimId = "ClaimId";
		public const string ReviewListHeadingJobSeekerGivenName = "Job Seeker Given Name";
		public const string ReviewListFieldJobSeekerGivenName = "JobSeekerGivenName";
		public const string ReviewListHeadingJobSeekerSurname = "Job Seeker Surname";
		public const string ReviewListFieldJobSeekerSurname = "JobSeekerSurname";
		public const string ReviewListHeadingClaimAmount = "Claim Amount";
		public const string ReviewListFieldClaimAmount = "ClaimAmount";
		public const string ReviewListHeadingClaimType = "Claim Type";
		public const string ReviewListFieldClaimType = "ClaimType";
		public const string ReviewListHeadingClaimDescription = "Claim Type Description";
		public const string ReviewListFieldClaimDescription = "ClaimTypeDescription";
		public const string ReviewListHeadingEmploymentServiceAreaCode = "Employment Service Area Code";
		public const string ReviewListFieldEmploymentServiceAreaCode = "ESACode";
		public const string ReviewListHeadingEmploymentServiceAreaName = "Employment Service Area Name";
		public const string ReviewListFieldEmploymentServiceAreaName = "ESAName";
		public const string ReviewListHeadingOrgCode = "Org Code";
		public const string ReviewListFieldOrgCode = "OrgCode";
		public const string ReviewListHeadingOrgName = "Org Name";
		public const string ReviewListFieldOrgName = "OrgName";
		public const string ReviewListHeadingSiteCode = "Site Code";
		public const string ReviewListFieldSiteCode = "SiteCode";
		public const string ReviewListHeadingSiteName = "Site Name";
		public const string ReviewListFieldSiteName = "SiteName";
		public const string ReviewListHeadingStateCode = "State Code";
		public const string ReviewListFieldStateCode = "StateCode";
		public const string ReviewListHeadingManagedBy = "Managed By";
		public const string ReviewListFieldManagedBy = "ManagedBy";
		public const string ReviewListHeadingContractType = "Contract Type";
		public const string ReviewListFieldContractType = "ContractType";
		public const string ReviewListHeadingContractTypeDescription = "Contract Type Description";
		public const string ReviewListFieldContractTypeDescription = "ContractTypeDescription";
		public const string ReviewListHeadingClaimCreationDate = "Claim Creation Date";
		public const string ReviewListFieldClaimCreationDate = "ClaimCreationDate";
		public const string ReviewListHeadingActivityId = "Activity ID";
		public const string ReviewListFieldActivityId = "ActivityId";
		public const string ReviewListHeadingAssessmentOutcome = "Assessment Outcome";
		public const string ReviewListFieldAssessmentOutcome = "AssessmentCode";
		public const string ReviewListHeadingRecoveryReason = "Recovery Reason";
		public const string ReviewListFieldRecoveryReason = "RecoveryReason";
		public const string ReviewListHeadingAssessmentAction = "Assessment Action";
		public const string ReviewListFieldAssessmentAction = "AssessmentAction";
		public const string ReviewListHeadingFinalOutcome = "Final Outcome";
		public const string ReviewListFieldFinalOutcome = "OutcomeCode";

		#endregion

		public const string InvalidUploadFileOrFileIsOpenMessage = "Program Assurance .csv template must be used";
		public const string DuplicateProjectNameMessage = "Project already exists. Please enter a different Organisation or project type.";
	}
}