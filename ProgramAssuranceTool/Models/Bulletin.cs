using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Models
{
	public class Bulletin : AuditEntity, IValidatableObject
	{
		private readonly IAdwRepository _adwRepository;

		public Bulletin(IAdwRepository adwRepository)
		{
			_adwRepository = adwRepository;
		}

		public Bulletin()
			: this(new AdwRepository())
		{
		}

		[Key]
		[Display(Name = "ID")]
		public int BulletinId { get; set; }

		private int _projectID;

		[Display(Name = "Project ID")]
		public int ProjectId
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(ProjectField))
				{
					var projects = ProjectField.Split('-');
					if (projects.Any())
					{
						int.TryParse(projects[0], out _projectID);
					}
				}
				return _projectID;
			}
			set { _projectID = value; }
		}

		[Display(Name = "Related Project")]
		public string ProjectField { get; set; }

		[Display(Name = "Title")]
		[Required(ErrorMessage = "Title is mandatory and cannot be blank")]
		[StringLength(200)]
		[HtmlProperties(MaxLength = 200)]
		[DataType(DataType.MultilineText)]
		public string BulletinTitle { get; set; }

		[Display(Name = "Description")]
		[Required(ErrorMessage = "Description is mandatory and cannot be blank")]
		[StringLength(5000)]
		[HtmlProperties(MaxLength = 5000)]
		[DataType(DataType.MultilineText)]
		[AllowHtml]
		public string Description { get; set; }

		[Display(Name = "Start Date  (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "Start Date is mandatory and cannot be blank")]
		public DateTime StartDate { get; set; }

		[Display(Name = "End Date (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "End Date is mandatory and cannot be blank")]
		public DateTime EndDate { get; set; }

		[Display(Name = "Bulletin Type")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForBulletinTypes, true, false)]
		[Required(ErrorMessage = "Bulletin Type is mandatory and cannot be blank")]
		public string BulletinType { get; set; }

		public bool IsBulletin
		{
			get
			{
				var isBulletin = false;
				if (!string.IsNullOrWhiteSpace(BulletinType))
				{
					isBulletin = BulletinType.Equals(DataConstants.StandardBulletinType, StringComparison.OrdinalIgnoreCase);
				}
				return isBulletin;
			}
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var result = new List<ValidationResult>();

			if (string.IsNullOrWhiteSpace(_adwRepository.GetDescription(DataConstants.AdwListCodeForBulletinTypes, BulletinType)))
			{
				var error = string.Format("Bulletin Type: {0} not found in ADW", BulletinType);
				result.Add(new ValidationResult(error, new[] { "BulletinType" }));
			}

			var startDateIsEmpty = false;
			if ((StartDate == new DateTime(1, 1, 1)))
			{
				startDateIsEmpty = true;
				result.Add(new ValidationResult("Start Date is mandatory and cannot be blank", new[] { "StartDate" }));
			}

			var endDateIsEmpty = false;
			if ((EndDate == new DateTime(1, 1, 1)))
			{
				endDateIsEmpty = true;
				result.Add(new ValidationResult("End Date is mandatory and cannot be blank", new[] { "EndDate" }));
			}

			if (!startDateIsEmpty)
			{
				if (BulletinId < 1 && !(StartDate >= DateTime.Today))
				{
					// DR01039364 = Start Date must be equal to or greater than the current date.
					result.Add(new ValidationResult("Start Date must be equal to or greater than the current date", new[] {"StartDate"}));
				}

				if (!endDateIsEmpty)
				{
					if (!(StartDate < EndDate))
					{
						// DR01039363 || DR01039376 =  Start Date must be less than the End Date.
						result.Add(new ValidationResult("Start Date must be less than the End Date", new[] {"StartDate"}));
					}
				}

			}

			return result;
		}
	}
}