using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	public class UploadEditViewModel : BaseViewModel
	{
		[DisplayName("Sample Name")]
		[Required]
		[StringLength( 50 )]
		[HtmlProperties( MaxLength = 50 )]
		public string SampleName { get; set; }

		[DisplayName( "Sample Due Date (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		[Required]
		public DateTime SampleDueDate { get; set; }

		[DisplayName( "Is Additional" )]
		public bool IsAdditional { get; set; }

		[DisplayName( "Is in Scope" )]
		public bool IsInScope { get; set; }

		[DisplayName( "Is Accepted" )]
		public bool IsAccepted { get; set; }

		[DisplayName( "Is Random" )]
		public bool IsRandom { get; set; }

		[DisplayName( "National" )]
		public bool IsNational { get; set; }

		[DisplayName( "Sample Start Date" )]
		public DateTime? SampleStartDate { get; set; }

		public bool IsAdministrator { get; set; }

		public bool IsProjectCoordinator { get; set; }

		public int UploadId { get; set; }

		public int Reviews { get; set; }
		public int CompletedReviews { get; set; }

		public string OriginalName { get; set; }

		public bool ProjectIsContractMonitoringOrContractSiteVisit { get; set; }

	}
}