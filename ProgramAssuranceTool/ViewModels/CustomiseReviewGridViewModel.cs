using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.ViewModels
{
	[Serializable]
	public class CustomiseReviewGridViewModel
	{
		public int ProjectId { get; set; }

		public int UploadId { get; set; }

		public string UserId { get; set; }

		[Display( Name = "Column 1" )]
		public string Col01 { get; set; }

		[Display( Name = "Column 2" )]
		public string Col02 { get; set; }

		[Display( Name = "Column 3" )]
		public string Col03 { get; set; }

		[Display( Name = "Column 4" )]
		public string Col04 { get; set; }

		[Display( Name = "Column 5" )]
		public string Col05 { get; set; }

		[Display( Name = "Column 6" )]
		public string Col06 { get; set; }

		[Display( Name = "Column 7" )]
		public string Col07 { get; set; }

		[Display( Name = "Column 8" )]
		public string Col08 { get; set; }

		[Display( Name = "Column 9" )]
		public string Col09 { get; set; }

		[Display( Name = "Column 10" )]
		public string Col10 { get; set; }

		[Display( Name = "Column 11" )]
		public string Col11 { get; set; }

		[Display( Name = "Column 12" )]
		public string Col12 { get; set; }

		[Display( Name = "Column 13" )]
		public string Col13 { get; set; }

		[Display( Name = "Column 14" )]
		public string Col14 { get; set; }

		[Display( Name = "Column 15" )]
		public string Col15 { get; set; }

		[Display( Name = "Column 16" )]
		public string Col16 { get; set; }

		[Display( Name = "Column 17" )]
		public string Col17 { get; set; }

		[Display( Name = "Column 18" )]
		public string Col18 { get; set; }

		[Display( Name = "Column 19" )]
		public string Col19 { get; set; }

		[Display( Name = "Column 20" )]
		public string Col20 { get; set; }

		[Display( Name = "Column 21" )]
		public string Col21 { get; set; }

		[Display( Name = "Column 22" )]
		public string Col22 { get; set; }

		[Display( Name = "Column 23" )]
		public string Col23 { get; set; }

		[Display( Name = "Column 24" )]
		public string Col24 { get; set; }

		[Display( Name = "Column 25" )]
		public string Col25 { get; set; }

		[Display( Name = "Grid Width" )]
		[Range( 740, 2000 )]
		[Required]
		public int GridWidth { get; set; }

		[Display( Name = "Margin" )]
		[Range( 0, 40 )]
		[Required]
		public int Margin { get; set; }

		[Display( Name = "Depth" )]
		[Range( 240, 2400 )]
		[Required]
		public int Depth { get; set; }

		[Display( Name = "Page Size" )]
		[Required]
		public string PageSize { get; set; }

		[Display( Name = "Sort Order" )]
		[Required]
		public string SortOrder { get; set; }  // column

		[Display( Name = "Ascending or Descending" )]
		[Required]
		public string AscOrDescending { get; set; }

		public IEnumerable<string> ColumnNames()
		{
			return new[]
				{
					"   ",
					CommonConstants.ReviewListHeadingActivityId,
					CommonConstants.ReviewListHeadingAssessmentAction,
					CommonConstants.ReviewListHeadingAssessmentOutcome,
					CommonConstants.ReviewListHeadingAutoSpecialClaim, 
					CommonConstants.ReviewListHeadingClaimAmount,
					CommonConstants.ReviewListHeadingClaimCreationDate,
					CommonConstants.ReviewListHeadingClaimId,
					CommonConstants.ReviewListHeadingClaimType,
					CommonConstants.ReviewListHeadingClaimDescription,
					CommonConstants.ReviewListHeadingContractType,
					CommonConstants.ReviewListHeadingContractTypeDescription,
					CommonConstants.ReviewListHeadingEmploymentServiceAreaCode,
					CommonConstants.ReviewListHeadingEmploymentServiceAreaName,
					CommonConstants.ReviewListHeadingFinalOutcome,
					CommonConstants.ReviewListHeadingJobSeekerGivenName,
					CommonConstants.ReviewListHeadingJobSeekerId, 
					CommonConstants.ReviewListHeadingJobSeekerSurname,
					CommonConstants.ReviewListHeadingManagedBy,
					CommonConstants.ReviewListHeadingManualSpecialClaim, 
					CommonConstants.ReviewListHeadingOrgCode,
					CommonConstants.ReviewListHeadingOrgName,
					CommonConstants.ReviewListHeadingRecoveryReason,
					CommonConstants.ReviewListHeadingSiteCode,
					CommonConstants.ReviewListHeadingSiteName,
					CommonConstants.ReviewListHeadingStateCode
				};
		}

		public IEnumerable<string> SortDirections()
		{
			return new[]
				{
					"Ascending", 
					"Descending"
				};
		}

		public IEnumerable<string> RowsPerPage()
		{
			return new[]
				{
					"10", 
					"15",
					"20",
					"50",
					"100",
					"200"
				};
		}

		public int ColumnCount()
		{
			var ccount = 0;
			if ( !string.IsNullOrEmpty( Col01 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col02 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col03 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col04 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col05 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col06 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col07 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col08 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col09 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col10 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col11 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col12 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col13 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col14 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col15 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col16 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col17 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col18 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col19 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col20 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col21 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col22 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col23 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col24 ) ) ccount++;
			if ( !string.IsNullOrEmpty( Col25 ) ) ccount++;
			return ccount;
		}

		public bool ColumnsContain( string columnName )
		{
			var columns = GetColumns();
			return columns.Contains( columnName );
		}

		public List<string> GetColumns()
		{
			var list = new List<string>();
			if ( !string.IsNullOrEmpty( Col01 ) ) list.Add( Col01 );
			if ( !string.IsNullOrEmpty( Col02 ) ) list.Add( Col02 );
			if ( !string.IsNullOrEmpty( Col03 ) ) list.Add( Col03 );
			if ( !string.IsNullOrEmpty( Col04 ) ) list.Add( Col04 );
			if ( !string.IsNullOrEmpty( Col05 ) ) list.Add( Col05 );
			if ( !string.IsNullOrEmpty( Col06 ) ) list.Add( Col06 );
			if ( !string.IsNullOrEmpty( Col07 ) ) list.Add( Col07 );
			if ( !string.IsNullOrEmpty( Col08 ) ) list.Add( Col08 );
			if ( !string.IsNullOrEmpty( Col09 ) ) list.Add( Col09 );
			if ( !string.IsNullOrEmpty( Col10 ) ) list.Add( Col10 );
			if ( !string.IsNullOrEmpty( Col11 ) ) list.Add( Col11 );
			if ( !string.IsNullOrEmpty( Col12 ) ) list.Add( Col12 );
			if ( !string.IsNullOrEmpty( Col13 ) ) list.Add( Col13 );
			if ( !string.IsNullOrEmpty( Col14 ) ) list.Add( Col14 );
			if ( !string.IsNullOrEmpty( Col15 ) ) list.Add( Col15 );
			if ( !string.IsNullOrEmpty( Col16 ) ) list.Add( Col16 );
			if ( !string.IsNullOrEmpty( Col17 ) ) list.Add( Col17 );
			if ( !string.IsNullOrEmpty( Col18 ) ) list.Add( Col18 );
			if ( !string.IsNullOrEmpty( Col19 ) ) list.Add( Col19 );
			if ( !string.IsNullOrEmpty( Col20 ) ) list.Add( Col20 );
			if ( !string.IsNullOrEmpty( Col21 ) ) list.Add( Col21 );
			if ( !string.IsNullOrEmpty( Col22 ) ) list.Add( Col22 );
			if ( !string.IsNullOrEmpty( Col23 ) ) list.Add( Col23 );
			if ( !string.IsNullOrEmpty( Col24 ) ) list.Add( Col24 );
			if ( !string.IsNullOrEmpty( Col25 ) ) list.Add( Col25 );
			return list;
		}

		public bool ChangeGridWidth { get; set; }
		public bool ChangeDepth { get; set; }
		public bool ChangeMargin { get; set; }
	}
}