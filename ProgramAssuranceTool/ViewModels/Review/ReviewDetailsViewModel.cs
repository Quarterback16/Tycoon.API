using System;

namespace ProgramAssuranceTool.ViewModels.Review
{
	[Serializable]
	public class ReviewDetailsViewModel : BaseViewModel
	{
		public string UploadName { get; set; }
		public string OutOfScope { get; set; }
		public bool OutOfScopeFlag { get; set; }
		public string Additional { get; set; }
		public bool AdditionalFlag { get; set; }
		public Models.Review Review { get; set; }
		public string NumberOfSelections { get; set; }
		public string Nav { get; set; }
		public string OldAssessmentOutcome { get; set; }
		public string OldRecoveryReason { get; set; }
		public string OldOutcomeCode { get; set; }
		public string OldAssessmentAction { get; set; }
		public string DeleteMessage { get; set; }
		public bool CanDelete { get; set; }
		public bool CanEdit { get; set; }
		public string ChangesMade { get; set; }
	}
}