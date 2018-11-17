using System;
using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	[Serializable]
	public class ClaimListViewModel : BaseViewModel
	{
		public string SessionKey { get; set; }

		public List<PatClaim> Claims { get; set; }

		public bool Additional { get; set; }
		public bool OutOfScope { get; set; }

		public string OrgCode { get; set; }
		public string EsaCode { get; set; }
		public string SiteCode { get; set; }

		public DateTime DueDate { get; set; }
	}
}