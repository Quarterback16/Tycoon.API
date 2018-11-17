using System;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Models
{
	public class SampleCriteria
	{
		public const string AuditUser = "AUDIT";

		[Display( Name = "Org Code" )]
		[StringLength( 4 )]
		public string OrgCode { get; set; }

		[Display( Name = "Organisation" )]
		[StringLength( 250 )]
		[Required]
		public string Organisation { get; set; }

		[Display( Name = "ESA Code" )]
		[StringLength( 4 )]
		public string EsaCode { get; set; }

		[Display( Name = "ESA" )]
		[StringLength( 250 )]
		[Required]
		public string Esa { get; set; }

		[Display( Name = "Site Code" )]
		[StringLength( 4 )]
		public string SiteCode { get; set; }

		[Display( Name = "Site" )]
		[StringLength( 250 )]
		public string Site { get; set; }

		[Display( Name = "Claim Type" )]
		[StringLength( 4 )]
		public string ClaimType { get; set; }

		[Display( Name = "Claim Type Description" )]
		[StringLength( 250 )]
		public string ClaimTypeDescription { get; set; }

		[Display( Name = "Claim From Date (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		public DateTime? FromClaimDate { get; set; }

		[Display( Name = "Claim To Date (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		public DateTime? ToClaimDate { get; set; }

		[Display( Name = "Number of Claims to Extract (maximum 50)" )]
		public int? MaxSampleSize { get; set; }

		[Display( Name = "Include Auto and Manual Special Claims" )]
		public bool IncludeSpecialClaims { get; set; }

		public string RequestingUser { get; set; }

		public void Audit( IAuditService auditService )
		{
			auditService.AuditActivity( string.Format( "{0}:{1}", "OrgCode", OrgCode ), AuditUser );
			auditService.AuditActivity( string.Format( "{0}:{1}", "ESA", EsaCode ), AuditUser );
			auditService.AuditActivity( string.Format( "{0}:{1}", "ClaimType", ClaimType ), AuditUser );
			auditService.AuditActivity( string.Format( "{0}:{1}", "Site", SiteCode ), AuditUser );
			auditService.AuditActivity( string.Format( "{0}:{1}", "From", FromClaimDate ), AuditUser );
			auditService.AuditActivity( string.Format( "{0}:{1}", "To", ToClaimDate ), AuditUser );
		}
	}
}