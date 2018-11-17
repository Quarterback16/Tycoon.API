using System;
using System.Runtime.Serialization;
using Employment.Esc.Shared.Contracts;

namespace ProgramAssuranceTool.DataContracts
{
	/// <summary>
	///   The request object for the Program Assurance Tool Claim Sample Service
	/// </summary>
	[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "clmSampleGetRequest" )]
	public class clmClaimSampleGetRequest : IExtensibleDataObject
	{
		[DataMember( Order = 1, IsRequired = true )]
		[FieldAlias( "inDisSiteCode" )]
		public string ClaimSiteCode { get; set; }

		[DataMember( Order = 2, IsRequired = true )]
		[FieldAlias( "inOrgOrganisationCd" )]
		public string ClaimOrgCode { get; set; }

		[DataMember( Order = 3, IsRequired = true )]
		[FieldAlias( "inESACd" )]
		public string ClaimEsaCode { get; set; }

		[DataMember( Order = 4, IsRequired = true )]
		[FieldAlias( "inAutoSpecClaimCd" )]
		public string AutoSpecialClaimFlag { get; set; }

		[DataMember( Order = 5, IsRequired = true )]
		[FieldAlias( "inFromCreationDate" )]
		public DateTime? ClaimFromDate { get; set; }

		[DataMember( Order = 6, IsRequired = true )]
		[FieldAlias( "inManuSpecClaimCd" )]
		public string ManualSpecialClaimFlag { get; set; }

		[DataMember( Order = 7, IsRequired = true )]
		[FieldAlias( "inMaxSampleSize" )]
		public int MaximumSampleSize { get; set; }

		[DataMember( Order = 8, IsRequired = true )]
		[FieldAlias( "inToCreationDate" )]
		public DateTime? ClaimToDate { get; set; }

		[DataMember( Order = 9, IsRequired = true )]
		[FieldAlias( "inGroupRateType" )]
		public pauClaimType[] ClaimTypeArray { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}
}

[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "clmSampleGetRequest" )]
public class pauClaimType
{
	[FieldAlias( "inItemClaimType" )]
	public string ClaimType
	{
		get;
		set;
	}	    
}