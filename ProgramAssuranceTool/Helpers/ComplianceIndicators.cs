using System;
using System.Collections.Generic;
using System.Linq;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool.ViewModels.System;

namespace ProgramAssuranceTool.Helpers
{
	public class ComplianceIndicators
	{
		private static readonly ComplianceIndicatorRepository ciRepository;
		private static readonly ProjectRepository projectRepository;
		private static readonly ProjectContractRepository projectContractRepository;
		private static readonly ReviewRepository reviewRepository;
		private static readonly UploadRepository uploadRepository;
		private static readonly UserActivityRepository<UserActivity> activityRepository;

		static ComplianceIndicators()
		{
			ciRepository = new ComplianceIndicatorRepository();
			projectRepository = new ProjectRepository();
			projectContractRepository = new ProjectContractRepository();
			reviewRepository = new ReviewRepository();
			uploadRepository = new UploadRepository();
			activityRepository = new UserActivityRepository<UserActivity>();
		}

		/// <summary>
		///    Generatea all the compliance Indicators
		/// </summary>
		public ComplianceIndicatorsViewModel Generate( ComplianceIndicatorsViewModel vm )
		{
			ClearOutOldComplianceIndicators();
			vm = GenerateProgrammeIndicators( vm );
			return vm;
		}

		private static void ClearOutOldComplianceIndicators()
		{
			ciRepository.Zap();
		}

		private ComplianceIndicatorsViewModel GenerateProgrammeIndicators( ComplianceIndicatorsViewModel vm )
		{
			vm = ClearTheView( vm );
			var countList = new Dictionary<string, CountStat>();

			var projectList = projectRepository.GetAll( new DateTime(1,1,1), new DateTime(1,1,1) );
			foreach ( var project in projectList )
			{
				var projectProgrammeList = projectContractRepository.GetAllByProjectId( project.ProjectId );
				vm.ProjectsRead++;
				var reviewList = reviewRepository.GetAllByProjectId( project.ProjectId );

				foreach ( var review in reviewList )
				{
					//if ( ( review.OrgCode != "BALT" ) || review.ESACode != "4TWE" || review.Quarter() != "20122" ) continue;  //TODO:  take this away its just to speed testing
					//if ( ( review.OrgCode != "LEON" ) || review.ESACode != "4ONB" || review.Quarter() != "20114" 
					//	|| (review.ProjectId != 255 && review.ProjectId != 333 ) ) continue;  //TODO:  take this away its just to speed testing
					ProcessReview( vm, review, countList, project, projectProgrammeList );
				}
			}
#if DEBUG
			//  for proof of calculation
			foreach ( var pair in countList )
			{
				var ciLine = string.Format( "Key:{0} - {1} - {2}", pair.Key, pair.Value, pair.Value.ReviewList() );
				var ua = new UserActivity { Activity = ciLine, UserId = "CI-GEN" };
				activityRepository.Add( ua );
			}
#endif
			StoreIndicators( countList, vm );

			return vm;
		}

		private static Upload lastUpload;

		/// <summary>
		///  get the Last upload
		/// </summary>
		/// <param name="uploadId"></param>
		/// <returns></returns>
		private static Upload GetUpload( int uploadId )
		{
			if ( lastUpload == null ) lastUpload = new Upload();
			if ( lastUpload.UploadId == uploadId )
				return lastUpload;

			lastUpload = uploadRepository.GetById( uploadId );
			return lastUpload;
		}

		private void ProcessReview( ComplianceIndicatorsViewModel vm, Review review,
											IDictionary<string, CountStat> countList, Project project,
			IEnumerable<ProjectContract> contractList )
		{
			var upload = GetUpload( review.UploadId );

			//  Only looking at accepted reviews
			if ( !upload.AcceptedFlag ) return;

			var reviewOutcome = ReviewOutcome( review );
			vm.ReviewsRead++;

			if ( !IsCompleted( review ) ) return;

			//  Only looking at completed reviews
			vm.ReviewsCompleted++;

			var orgCode = OrgCodeFor( review );

			var esaCode = EsaCodeFor( review );

			var orgEsaCode = OrgEsaCodeFor( orgCode, esaCode );

			var multiplier = MultiplierFor( project, upload.RandomFlag, upload.NationalFlag );

			//  we have a positive multiplier
			var compliancePoints = WeightedScore( multiplier, reviewOutcome );

			vm.TotalCompliancePoints += compliancePoints;

			var quarter = review.Quarter();

			foreach ( var programme in contractList )
			{
				IncrementCounts( programme.ContractType, multiplier, quarter, orgCode, orgEsaCode,
									  compliancePoints, countList, review.ReviewId   );
			}
		}

		private static string OrgEsaCodeFor( string orgCode, string esaCode )
		{
			var orgEsaCode = String.Empty;
			if ( !string.IsNullOrEmpty( orgCode ) && !string.IsNullOrEmpty( esaCode ) )
				orgEsaCode = string.Format( "{0}:{1}", orgCode, esaCode );
			return orgEsaCode;
		}

		private static void StoreIndicators( ICollection<KeyValuePair<string, CountStat>> countList, ComplianceIndicatorsViewModel vm )
		{
			foreach ( var ci in countList.Select( pair => new ComplianceIndicator
			{
				Programme = pair.Value.Programme,
				ComplianceIndicatorId = 0,
				CreatedBy = DataConstants.BatchUserId,
				UpdatedBy = DataConstants.BatchUserId,
				Quarter = pair.Value.Quarter,
				SubjectTypeCode = SubjectTypeOf( pair.Value.SubjectArea ),
				Subject = OrgOutOf( pair.Value.SubjectArea, pair.Value.Subject ),
				EsaCode = EsaOutOf( pair.Value.SubjectArea, pair.Value.Subject ),
				Value = pair.Value.ComplianceIndicator()
			} ) )
			{
				ciRepository.Update( ci );
			}
			vm.IndicatorsGenerated += countList.Count;

			countList.Clear(); 
		}

		private static string SubjectTypeOf( string subjectArea )
		{
			var subjectType = subjectArea;
			if ( subjectArea.Equals( "ORG:ESA" ) )
				subjectType = "ESA";
			if ( subjectArea.Equals( "ORG" ) )
				subjectType = "PRG";  //  requested by Karthi
			return subjectType;
		}

		private static string OrgOutOf( string subjectArea, string subject )
		{
			var orgCode = subject;
			if ( subjectArea.Equals( "ORG:ESA" ) )
			{
				var nPoint = subject.IndexOf( ":", StringComparison.Ordinal );
				orgCode = subject.Substring( 0, nPoint );
			}
			return orgCode;
		}

		private static string EsaOutOf( string subjectArea, string subject )
		{
			var esaCode = subject;
			if ( subjectArea.Equals( "ORG:ESA" ) )
			{
				var nPoint = subject.IndexOf( ":", StringComparison.Ordinal );
				esaCode = subject.Substring( nPoint + 1, subject.Length - nPoint - 1 );
			}
			else
			{
				if ( subjectArea.Equals( "ORG" ) )
					esaCode = string.Empty;
			}
			return esaCode;
		}

		private static void IncrementCounts( string programme,
			int multiplier, string quarter, string orgCode, string orgEsaCode,
			decimal compliancePoints, IDictionary<string, CountStat> countList, int reviewId  )
		{
			//  PAM ones
			IncrementCount( programme, multiplier, quarter, orgCode, compliancePoints, countList, "ORG", reviewId );
			IncrementCount( programme, multiplier, quarter, orgEsaCode, compliancePoints, countList, "ORG:ESA", reviewId );
		}

		private static void IncrementCount( string programme, int multiplier,
			string quarter, string code, decimal compliancePoints,
			IDictionary<string, CountStat> countList, string countType, int reviewId )
		{
			if ( string.IsNullOrEmpty( code ) ) return;
			var key = string.Format( "{0}-{1}-{2}-{3}", programme, quarter, countType, code );
			AddPoints( key, countList, multiplier, compliancePoints, quarter, countType, code, programme, reviewId );
		}

		//  add the count
		private static void AddPoints( string key, IDictionary<string, CountStat> countList,
			int multiplier, decimal compliancePoints, string quarter, string countType, string code, string programme, int reviewId )
		{
			if ( !countList.ContainsKey( key ) )
			{
				var countStat = new CountStat
				{
					Programme = programme,
					Quarter = quarter,
					SubjectArea = countType,
					Subject = code,
					Reviews = new List<int>()
				};
				countList.Add( key, countStat );
			}
			if ( multiplier == 4 )
			{
				countList[ key ].FourCount++;
				countList[ key ].FourPoints += compliancePoints;
				countList[ key ].Reviews.Add( reviewId );

			}
			else if ( multiplier == 2 )
			{
				countList[ key ].TwoCount++;
				countList[ key ].TwoPoints += compliancePoints;
				countList[ key ].Reviews.Add( reviewId );
			}
			else if ( multiplier == 1 )
			{
				countList[ key ].OneCount++;
				countList[ key ].OnePoints += compliancePoints;
				countList[ key ].Reviews.Add( reviewId );
			}
		}

		private static ComplianceIndicatorsViewModel ClearTheView( ComplianceIndicatorsViewModel vm )
		{
			vm.ReviewsRead = 0;
			vm.IndicatorsGenerated = 0;
			vm.ProjectsRead = 0;
			vm.CompletedProjects = 0;
			vm.SamplesRead = 0;
			vm.ReviewsCompleted = 0;
			vm.TotalCompliancePoints = 0;
			return vm;
		}

		private static string EsaCodeFor( Review review )
		{
			return review.ESACode;
		}

		private static string OrgCodeFor( Review review )
		{
			return review.OrgCode;
		}

		private static bool IsCompleted( Review review )
		{
			var reviewOutcome = ReviewOutcome( review );
			return !string.IsNullOrEmpty( reviewOutcome );
		}

		private static string ReviewOutcome( Review review )
		{
			var reviewOutcome = review.OutcomeCode;

			return reviewOutcome;
		}

		private static decimal WeightedScore( int multiplier, string reviewOutcome )
		{
			var score = 0.0M;

			if ( reviewOutcome.Equals( DataConstants.PatReviewOutcome_Valid_NFA ) )
				score = 1.0M;
			else if ( reviewOutcome.Equals( DataConstants.PatReviewOutcome_Valid_AdminDeficiency ) )
				score = 0.6M;
			else if ( reviewOutcome.Equals( DataConstants.PatReviewOutcome_Invalid_AdminDeficiency ) )
				score = 0.6M;

			if ( score > 0.0M )
				score *= multiplier;
			return score;
		}

		private static int MultiplierFor( Project project, bool randomFlag, bool nationalFlag )
		{
			var multiplier = 0;
			if ( project.IsContractMonitoringOrContractSiteVisit() )
			{
				if ( randomFlag )
					multiplier = 1;
			}
			else
			{
				if ( randomFlag )
					multiplier = nationalFlag ? 4 : 1;
				else
					multiplier = nationalFlag ? 2 : 0;
			}
			return multiplier;
		}
	}

	/// <summary>
	///   For each subject area of focus youe will have 3 categories 4 pt, 2 pt and 1 pt categories
	/// </summary>
	public class CountStat
	{
		public decimal FourPoints { get; set; }
		public decimal TwoPoints { get; set; }
		public decimal OnePoints { get; set; }
		public int FourCount { get; set; }
		public int TwoCount { get; set; }
		public int OneCount { get; set; }

		public string Programme { get; set; }
		public string Quarter { get; set; }
		public string Subject { get; set; }
		public string SubjectArea { get; set; }

		public List<int> Reviews { get; set; }

		public string ReviewList()
		{
			var reviewList = Reviews.Aggregate( string.Empty, ( current, i ) => current + string.Format( "{0},", i ) );
			if (reviewList.Length > 60)
				reviewList = reviewList.Substring( 0, 60 );
			return reviewList;
		}

		public decimal TotalPoints()
		{
			return FourPoints + TwoPoints + OnePoints;
		}

		public decimal TotalReviews()
		{
			return FourCount + TwoCount + OneCount;
		}

		public decimal ComplianceIndicator()
		{
			var divisor = Divisor(); //  we are counting zeros only if they have a non zero modifier!
			if ( divisor > 0 )
				return TotalPoints() / divisor;

			return 0.0M;
		}

		public int Divisor()
		{
			var divisor = ( 4 * FourCount ) + ( 2 * TwoCount ) + OneCount; 
			return divisor;
		}

		public override string ToString()
		{
			return string.Format( "{0}-{1}-{2} ({3}) pts {4}-{5}-{6} revs ({7}) div {9} CI={8:0.00#}",
										 FourPoints, TwoPoints, OnePoints, TotalPoints(),
										 FourCount, TwoCount, OneCount, TotalReviews(), ComplianceIndicator(), Divisor() );
		}
	}
}