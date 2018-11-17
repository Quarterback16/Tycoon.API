using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.Models
{
	public class ProjectAttachment : AuditEntity, IValidatableObject
	{
		[Key]
		[Display( Name = "Document ID" )]
		public int Id { get; set; }

		public int ProjectId { get; set; }

		public string ProjectName { get; set; }

		[Display( Name = "Document Name" )]
		[Required(ErrorMessage = "Please provide a Document Name")]
		[StringLength( 250 )]
		[HtmlProperties( MaxLength = 250 )]
		public string DocumentName { get; set; }

		[Display( Name = "Description" )]
		[StringLength( 250 )]
		[HtmlProperties( MaxLength = 250 )]
		[DataType( DataType.MultilineText )]
		[AllowHtml]
		public string Description { get; set; }

		[Display( Name = "File Name" )]
		[StringLength( 200 )]
		public string Url { get; set; }

		public HttpPostedFileBase Attachment { get; set; }

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			var result = new List<ValidationResult>();

			if (string.IsNullOrWhiteSpace( Url ))
			{
				if (Attachment == null)
				{
					result.Add( new ValidationResult( "Please specify the File Name  to upload", new[] {"Attachment"} ) );
				}
				else if (Attachment.ContentLength < 1)
				{
					result.Add(new ValidationResult("The document does not have a valid content length. Please specify a valid document to upload", new[] { "Attachment" }));
				}
			}

			return result;
		}
	}
}