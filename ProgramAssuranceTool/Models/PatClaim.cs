using System;

namespace ProgramAssuranceTool.Models
{
	public class PatClaim
	{
		public string ClaimTypeDescription { get; set; }
		public string ContractTypeDescription { get; set; }
		public string SiteDescription { get; set; }
		public string EsaDescription { get; set; }
		public string OrgDescription { get; set; }
		public long ClaimId { get; set; }
		public int ClaimSequenceNumber { get; set; }
		public string ClaimType { get; set; }
		public decimal ClaimAmount { get; set; }
		public string SiteCode { get; set; }
		public string SupervisingSiteCode { get; set; }
		public string OrgCode { get; set; }
		public long ActivityId { get; set; }
		public DateTime ClaimCreationDate { get; set; }
		public string StateCode { get; set; }
		public string ManagedBy { get; set; }
		public string ContractId { get; set; }
		public string ContractType { get; set; }
		public string ContractTitle { get; set; }
		public string EsaCode { get; set; }
		public long JobseekerId { get; set; }
		public string GivenName { get; set; }
		public string Surname { get; set; }
		public bool AutoSpecialClaimFlag { get; set; }
		public bool ManSpecialClaimFlag { get; set; }
		public string StatusCode { get; set; }
		public string StatusCodeDescription { get; set; }
	}
}