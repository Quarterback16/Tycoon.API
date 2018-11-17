using System;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	public class CreateSampleViewModel : BaseViewModel
	{
		public string SampleMessage { get; set; }

		public string SessionKey { get; set; }

		public bool ContractMonitoringOrContractSiteVisitProject { get; set; }

		public bool IsAdministrator { get; set; }

		public SampleCriteria Criteria { get; set; }

		[Display( Name = "Sample Start Date (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		public DateTime SampleStartDate { get; set; }

		[Display( Name = "Sample Due Date (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		public DateTime SampleDueDate { get; set; }

		public void UseDummyValues()
		{
			if ( Criteria == null ) Criteria = new SampleCriteria();
			Criteria.ClaimTypeDescription = "EPTC - Steves Claim Type";
			Criteria.ClaimType = "EPTC";
			Criteria.OrgCode = "WRKW";
			Criteria.Organisation = "WRKW - Steves Testing Org";
			Criteria.EsaCode = "4LSC";
			Criteria.Esa = "4LSC - Steves testing ESA";
			Criteria.SiteCode = "";
			Criteria.FromClaimDate = new DateTime( 1, 1, 1 );
			Criteria.ToClaimDate = new DateTime( 1, 1, 1 );
			Criteria.MaxSampleSize = 20;
		}
	}
}