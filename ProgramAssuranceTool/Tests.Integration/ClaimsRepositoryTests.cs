using System;
using System.IO;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool;
using ProgramAssuranceTool.ViewModels.Sample;

namespace Tests.Integration
{
	[TestClass]
	public class ClaimsRepositoryTests
	{
		[TestInitialize]
		public void Init()
		{
			HttpContext.Current = new HttpContext(
				new HttpRequest( "", "http://tempuri.org", "" ),
				new HttpResponse( new StringWriter() )
				);
		}

		[TestMethod]
		public void TestGetClaimSample()
		{
			var sut = new PatService();
			var criteria = new SampleCriteria
				{
				ClaimType =  "EPTC"
				, OrgCode =  "WRKW"
				, EsaCode = "4LSC"
				, SiteCode = ""
				, FromClaimDate = new DateTime( 1, 1, 1 )
				, ToClaimDate =  new DateTime( 1, 1, 1 )
				, MaxSampleSize = 10
				, IncludeSpecialClaims = false
				, RequestingUser = "Unit-Test"
				};

			var vm = sut.ExtractClaims( criteria );

			Assert.IsNotNull( vm.Claims );
			Assert.IsTrue( vm.Claims.Count <= 10 );
			Console.WriteLine( "Claim sample call found {0} claims", vm.Claims.Count );
		}

		[TestMethod]
		public void TestGetBadClaimSampleSize()
		{
			var sut = new PatService();
			var criteria = new SampleCriteria
			{
				ClaimType = "EPTC",
				OrgCode = "WRKW",
				EsaCode = "4LSC",
				SiteCode = "",
				FromClaimDate = new DateTime( 1, 1, 1 ),
				ToClaimDate = new DateTime( 1, 1, 1 ),
				MaxSampleSize = -1,
				IncludeSpecialClaims = true,
				RequestingUser = "Unit-Test"
			};

			var vm = sut.ExtractClaims( criteria );

			Assert.IsNotNull( vm.Claims );
		}

		[TestMethod]
		public void TestGetBadSite()
		{
			var sut = new PatService();
			var criteria = new SampleCriteria
			{
				ClaimType = "EPTC",
				OrgCode = "WRKW",
				EsaCode = "4LSC",
				SiteCode = "X$%F",
				FromClaimDate = new DateTime( 1, 1, 1 ),
				ToClaimDate = new DateTime( 1, 1, 1 ),
				MaxSampleSize = -1,
				IncludeSpecialClaims = true,
				RequestingUser = "Unit-Test"
			};

			var vm = sut.ExtractClaims( criteria );

			Assert.IsTrue( vm.ErrorMessage.Length > 0 );
		}

		[TestMethod]
		public void TestGetClaimSampleFromDefaultView()
		{
			var sut = new PatService();
			var vm = new CreateSampleViewModel();
			vm.UseDummyValues();
			var csvm = sut.ExtractClaims( vm.Criteria );

			Assert.AreEqual( string.Empty, csvm.ErrorMessage );
			Assert.IsNotNull( csvm.Claims );
			Console.WriteLine( "Claim sample call found {0} claims", csvm.Claims.Count );

			sut.SaveExtract( vm.SessionKey, csvm.Claims, "UnitTest" );
		}

		[TestMethod]
		public void TestP82GetClaimRelatedDetails()
		{
			var sut = new ClaimsRepository();
			const int claimId = 376070024;
			const int seqNo = 1;
			var claim = sut.GetRelatedData( claimId, seqNo );
			Assert.IsTrue( claim.ErrorMessage == null );
		}

		[TestMethod]
		public void TestAdwClaimListForPat()
		{
			var sut = new AdwRepository();
			var list = sut.ListRelatedCode( DataConstants.AdwRelatedListCodeForPatClaimTypes, "IND", false, "N" );
			foreach ( var selectListItem in list )
			{
				Console.WriteLine( "{0} = {1}", selectListItem.Value, selectListItem.Text );				
			}
			Assert.IsTrue( list.Count == 212 );
		}

		[TestMethod]
		public void TestAdwEsaListForPat()
		{
			var sut = new AdwRepository();
			var list = sut.ListCode( DataConstants.AdwListCodeForEsaCodes );
			foreach ( var selectListItem in list )
			{
				Console.WriteLine( "{0} = {1}", selectListItem.Value, selectListItem.Text );
			}
			Assert.IsTrue( list.Count > 0 );
		}
	}
}