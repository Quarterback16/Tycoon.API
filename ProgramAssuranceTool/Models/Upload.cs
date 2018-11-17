using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class Upload : AuditEntity, IValidatableObject
	{

		[Key]
		[Display( Name = "Upload ID" )]
		public int UploadId { get; set; }

		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		[Display( Name = "Upload Name" )]
		public string Name { get; set; }

		[Display( Name = "Source File" )]
		public string SourceFile { get; set; }

		[Display( Name = "Date Uploaded" )]
		public DateTime DateUploaded { get; set; }

		[Display( Name = "Includes Outcomes" )]
		public bool IncludesOutcomes { get; set; }

		[Display( Name = "Additional Review" )]
		public bool AdditionalReview { get; set; }

		[Display( Name = "In Scope" )]
		public bool InScope { get; set; }

		[Display( Name = "Out of Scope" )]
		public bool OutOfScope { get; set; }

		[Display( Name = "Random or Targeted" )]
		public bool RandomFlag { get; set; }

		[Display( Name = "Accepted" )]
		public bool AcceptedFlag { get; set; }

		[Display( Name = "Accepted" )]
		public bool NationalFlag { get; set; }

		[ScaffoldColumn( true )]
		public string ServerFile { get; set; }

		[ScaffoldColumn( true )]
		public string OrgCode { get; set; }

		[ScaffoldColumn( true )]
		public string EsaCode { get; set; }

		[ScaffoldColumn( true )]
		public string SiteCode { get; set; }

		[ScaffoldColumn( true )]
		public DateTime DueDate { get; set; }

		[Display( Name = "Upload Type" )]
		public string RandomOrTargetted { get; set; }

		[ScaffoldColumn( true )]
		public int Rows { get; set; }

		[ScaffoldColumn( true )]
		public string Status { get; set; }

		[ScaffoldColumn( true )]
		public string UploadedBy { get; set; }

		public string SampleName()
		{
			return string.Format( "Upload {0} - {1} - {2} - {3}", UploadId , OrgCode ?? "<OrgCode>", EsaCode ?? "<EsaCode>", SiteCode ?? "<SiteCode>" );
		}

		public string IsOutOfScope()
		{
			return ( InScope ? "In Scope" : "Out of Scope" );
		}

		public string AdditionalOrNot()
		{
			return ( InScope ? "Additional" : "Not Additional" );
		}

		public bool IsLoaded()
		{
			var isLoaded = false;
			if (!string.IsNullOrEmpty( SourceFile ))
			{
				if (DateUploaded < DateTime.Now.Subtract( new TimeSpan( 2, 0, 0, 0 ) ))
					isLoaded = true;
			}
			return isLoaded;
		}

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			if ( DateUploaded.Equals( new DateTime(1,1,1) ) )
				yield return new ValidationResult( "There must be an upload date", new[] { "DateUploaded" } );
		}

		public bool HasSourceFile()
		{
			var hasSourceFile = ! string.IsNullOrEmpty( SourceFile );
			return hasSourceFile;
		}
	}
}