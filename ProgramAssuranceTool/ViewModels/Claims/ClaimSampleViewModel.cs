using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.ViewModels.Claims
{
	public class ClaimSampleViewModel
	{
		public List<PatClaim> Claims { get; set; }

		public string ErrorMessage { get; set; }
		public string ExitStatusNumber { get; set; }
		public string ErrorCode { get; set; }

		public string ErrorDiagnostic( string userId )
		{
			return string.Format( "{0}-{1}-{2}-{3}", 
				ErrorCode, ErrorMessage, ExitStatusNumber, userId );
		}
	}
}