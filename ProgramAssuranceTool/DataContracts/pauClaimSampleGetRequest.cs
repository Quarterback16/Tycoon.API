using System;
using System.Runtime.Serialization;
using Employment.Esc.Shared.Contracts;

namespace ProgramAssuranceTool.DataContracts
{
	/// <summary>
	///  The Sample request object 
	/// </summary>
	[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "pauClaimSampleGetRequest" )]
	public class pauClaimSampleGetRequest : IExtensibleDataObject
	{
		[DataMember( Order = 1, IsRequired = true )]
		[FieldAlias( "inGroupClaim" )]
		public pauClaimSampleItem[] Claims { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	/// <summary>
	///   The data structure for a claim
	/// </summary>
	[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "pauClaimSampleGetRequest" )]
	public class pauClaimSampleItem
	{
		[FieldAlias( "outClaimTypeDesc" )]
		public string ClaimTypeDescription { get; set; }

		[FieldAlias( "outContractTypeDesc" )]
		public string ContractTypeDescription { get; set; }

		[FieldAlias( "outSiteDesc" )]
		public string SiteDescription { get; set; }

		[FieldAlias( "outESADesc" )]
		public string EsaDescription { get; set; }

		[FieldAlias( "outOrgDesc" )]
		public string OrgDescription { get; set; }

		[FieldAlias( "outClaimStatusDesc" )]
		public string ClaimStatusDescription { get; set; }

		[FieldAlias( "outClaimId" )]
		public long ClaimId { get; set; }

		[FieldAlias( "outClaimSeqNumber" )]
		public int ClaimSequenceNumber { get; set; }

		[FieldAlias( "outClaimStatusCode" )]
		public string ClaimStatusCode { get; set; }

		[FieldAlias( "outClaimType" )]
		public string ClaimType { get; set; }

		[FieldAlias( "outAmount" )]
		public decimal ClaimAmount { get; set; }

		[FieldAlias( "outSiteCode" )]
		public string SiteCode { get; set; }

		[FieldAlias( "outSupervisingSiteCd" )]
		public string SupervisingSiteCode { get; set; }

		[FieldAlias( "outOrganisationCd" )]
		public string OrgCode { get; set; }

		[FieldAlias( "outActivityId" )]
		public long ActivityId { get; set; }

		[FieldAlias( "outClaimCreationDate" )]
		public DateTime ClaimCreationDate { get; set; }

		[FieldAlias( "outStateCode" )]
		public string StateCode { get; set; }

		[FieldAlias( "outManagedBy" )]
		public string ManagedBy { get; set; }

		[FieldAlias( "outContractId" )]
		public string ContractId { get; set; }

		[FieldAlias( "outContractType" )]
		public string ContractType { get; set; }

		[FieldAlias( "outContractTitle" )]
		public string ContractTitle { get; set; }

		[FieldAlias( "outESACd" )]
		public string EsaCode { get; set; }

		[FieldAlias( "outJobseekerId" )]
		public long JobseekerId { get; set; }

		[FieldAlias( "outFirstGivenName" )]
		public string GivenName { get; set; }

		[FieldAlias( "outSurname" )]
		public string Surname { get; set; }

		[FieldAlias( "outAutoSpecialClaimFlag" )]
		public string AutoSpecialClaimFlag { get; set; }

		[FieldAlias( "outManSpecialClaimFlag" )]
		public string ManSpecialClaimFlag { get; set; }
	}
}