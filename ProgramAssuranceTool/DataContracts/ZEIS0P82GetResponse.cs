using System;
using System.Runtime.Serialization;
using Employment.Esc.Shared.Contracts;

namespace ProgramAssuranceTool.DataContracts
{
	[DataContract(Namespace = "http://employment.esc.contracts/2011/03", Name = "ZEIS0P82")]
	public class ZEIS0P82GetResponse : IExtensibleDataObject
	{
		[DataMember(Order = 1), FieldAlias("outClaimId")]
		public long ClaimId { get; set; }

		[DataMember(Order = 2), FieldAlias("outClaimSeqNumber")]
		public int ClaimSeqNumber { get; set; }

		[DataMember(Order = 3), FieldAlias("outClaimStatusCode")]
		public string ClaimStatusCode{ get; set; }

		[DataMember(Order = 4), FieldAlias("outClaimStartDate")]
		public DateTime ClaimStartDate { get; set; }

		[DataMember(Order = 5), FieldAlias("outClaimEndDate")]
		public DateTime ClaimEndDate { get; set; }

		[DataMember(Order = 6), FieldAlias("outStream")]
		public string Stream { get; set; }

		[DataMember(Order = 7), FieldAlias("outPlacementDate")]
		public DateTime PlacementDate { get; set; }

		[DataMember(Order = 8), FieldAlias("outRefPlacementComDate")]
		public DateTime RefPlacementComDate { get; set; }

		[DataMember(Order = 9), FieldAlias("outRefPlacementEndDate")]
		public DateTime RefPlacementEndDate { get; set; }

		[DataMember(Order = 10), FieldAlias("outReferralDate")]
		public DateTime ReferralDate { get; set; }

		[DataMember(Order = 11), FieldAlias("outRefOutcomeDesc")]
		public string RefOutcomeCode { get; set; }

		[DataMember(Order = 12), FieldAlias("outSiteSequenceNumber")]
		public int SiteSequenceNumber { get; set; }

		[DataMember(Order = 13), FieldAlias("outSiteCode")]
		public string SiteCode { get; set; }

		[DataMember(Order = 14), FieldAlias("outSiteSpecialistType")]
		public string SiteSpecialistType { get; set; }

		[DataMember(Order = 15), FieldAlias("outEmployerName")]
		public string EmployerName { get; set; }

		[DataMember(Order = 16), FieldAlias("outJobseekerId")]
		public long JobseekerId { get; set; }

		[DataMember(Order = 17), FieldAlias("outLMRCode")]
		public string LMRCode { get; set; }

		[DataMember(Order = 18), FieldAlias("outRegStatusCode")]
		public string RegStatusCode { get; set; }

		[DataMember(Order = 19), FieldAlias("outJobId")]
		public long JobId { get; set; }

		[DataMember(Order = 20), FieldAlias("outJobTitleText")]
		public string JobTitleText { get; set; }

		[DataMember(Order = 21), FieldAlias("outJobSiteCd")]
		public string JobSiteCd { get; set; }

		[DataMember(Order = 22), FieldAlias("outJobCreationDate")]
		public DateTime JobCreationDate { get; set; }

		[DataMember(Order = 23), FieldAlias("outJobHoursTxt")]
		public string JobHoursTxt { get; set; }

		[DataMember(Order = 24), FieldAlias("outJobDescription")]
		public string JobDescription { get; set; }

		[DataMember(Order = 25), FieldAlias("outJobTenureCd")]
		public string JobTenureCd { get; set; }

		[DataMember(Order = 26), FieldAlias("outRefOutcomeDescription")]
		public string RefOutcomeDescription { get; set; }

		[DataMember(Order = 27), FieldAlias("outJobTenureDescription")]
		public string JobTenureDescription { get; set; }
		
		public ExtensionDataObject ExtensionData
		{
			get;
			set;
		}

	}
}