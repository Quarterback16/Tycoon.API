using System.Collections.Generic;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class ReviewQuestionnaire : AuditEntity
	{
		private readonly IAdwRepository _adwRepository;

		public ReviewQuestionnaire(IAdwRepository adwRepository)
		{
			_adwRepository = adwRepository;
		}

		public ReviewQuestionnaire()
			: this(new AdwRepository())
		{
			QuestionAnswers = new List<QuestionAnswer>();
		}

		[Display(Name = "Questionnaire ID")]
		public int QuestionnaireID { get; set; }

		[Display(Name = "Review ID")]
		public int ReviewID { get; set; }

		[Display(Name = "Reference ID")]
		public long ReferenceID { get; set; }

		[Display(Name = "Upload ID")]
		public int UploadID { get; set; }

		[Display(Name = "Upload Name")]
		public string UploadName { get; set; }

		[Display(Name = "Project ID")]
		public int ProjectID { get; set; }

		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		[Display(Name = "User ID")]
		public string UserID { get; set; }

		[Display(Name = "Questionnaire Code")]
		public string QuestionnaireCode { get; set; }

		[Display(Name = "Assessment Outcome Code")]
		public string AssessmentOutcomeCode { get; set; }

		[Display(Name = "Assessment Outcome Description")]
		public string AssessmentOutcomeDescription { get; set; }

		[Display(Name = "Recovery Reason Code")]
		public string RecoveryReasonCode { get; set; }

		[Display(Name = "Recovery Reason Description")]
		public string RecoveryReasonDescription { get; set; }

		[Display(Name = "Recovery Action Code")]
		public string RecoveryActionCode { get; set; }

		[Display(Name = "Recovery Action Description")]
		public string RecoveryActionDescription { get; set; }

		[Display(Name = "Final Outcome Code")]
		public string FinalOutcomeCode { get; set; }

		[Display(Name = "Final Outcome Description")]
		public string FinalOutcomeDescription { get; set; }

		[Display(Name = "Date")]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		public List<QuestionAnswer> QuestionAnswers { get; set; }

		public string PreviousButtonEnabled { get; set; }

		public string NextButtonEnabled { get; set; }

	}

	public class QuestionAnswer
	{
		public int QuestionId { get; set; }

		public string QuestionCode { get; set; }

		public string QuestionHeadingCode { get; set; }

		public string QuestionText { get; set; }

		public string AnswerText { get; set; }

	}

}