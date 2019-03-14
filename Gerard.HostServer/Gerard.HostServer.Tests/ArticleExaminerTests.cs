using Gerard.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System;

namespace Gerard.HostServer.Tests
{
	public class ArticleExaminerTests
	{
		#region constructor

		private ArticleExaminer sut;

		public Logger Logger { get; set; }

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private ArticleExaminer SystemUnderTest()
		{
			Logger = LogManager.GetCurrentClassLogger();
			var lib = Utility.TflWs;
			var tfl = new TflService(lib, Logger);
			return new ArticleExaminer(
				tfl,
				new NLogAdaptor());
		}

		#endregion constructor

		#region Injuries (severe)

		[TestMethod]
		public void TestTornACL()
		{
			//  those that put a player out for the season
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "49ers QB Blaine Gabbert tore his ACL in Sunday night's preseason opener."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsInjury);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("SF", result.TeamId);
			Assert.AreEqual("INJURY", result.RecommendedAction);  //  mark him as OUT
		}

		[TestMethod]
		public void TestBadInjury()
		{
			//  those that put a player out for the season
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2015, 08, 08),
				ArticleText = "Jets TE Zach Sudfeld tore his ACL during Organized Team Activities and will miss the 2015 season."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsInjury);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("NJ", result.TeamId);
			Assert.AreEqual("INJURY", result.RecommendedAction);  //  mark him as OUT
		}

		[TestMethod]
		public void TestInjuredReserve()
		{
			//  those that put a player out for the season
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 12, 08),
				ArticleText = "Giants placed DB Josh Gordy on injured reserve, ending his season."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsInjury);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("NG", result.TeamId);
			Assert.AreEqual("INJURY", result.RecommendedAction);  //  mark him as OUT
		}

		#endregion Injuries (severe)

		#region Signings

		[TestMethod]
		public void TestPwaSigning()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 3, 6),
				ArticleText = "Bills signed DE Owa Odighizuwa to a one-year contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("SIGN", result.RecommendedAction);
			Assert.AreEqual("BB", result.TeamId);
		}

		[TestMethod]
		public void TestAcquiringTalib()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 3, 6),
				ArticleText = "Rams acquired CB Aqib Talib from the Broncos in exchange for a fifth-round pick."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("TRADE", result.RecommendedAction);
			Assert.AreEqual("LR", result.TeamId);
		}

		[TestMethod]
		public void TestSigningRobertQuinn()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 02, 28),
				ArticleText = "ESPN's Adam Schefter reports the Dolphins have acquired DE Robert Quinn from the Rams."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("TRADE", result.RecommendedAction);
		}
		[TestMethod]
		public void TestOfferSheet()
		{
			//  Ignore Offer sheets (not official)
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Packers restricted free agent CB Sean Richardson has signed an offer sheet with the Raiders."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		[TestMethod]
		public void TestLocatesSigningRestrictedFreeAgent()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 05, 08),
				ArticleText = "Browns re-signed restricted free agent ILB Craig Robertson to a one-year, $2.356 million contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("ROBECR01", result.PlayerId);
			Assert.AreEqual("CL", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestLocatesSigningFormerlyOf()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Chargers signed G/T Chris Hairston, formerly of the Bills."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("HAIRCH01", result.PlayerId);
			Assert.AreEqual("LC", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestLocatesSigning()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Dolphins signed WR LaRon Byrd."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("BYRDLA01", result.PlayerId);
			Assert.AreEqual("MD", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestLocatesSigningStyle2()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Eagles agreed to terms with QB Tim Tebow on a one-year contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("TEBOTI01", result.PlayerId);
			Assert.AreEqual("PE", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestDarickRogers()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Chiefs released WR Da'Rick Rogers."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("ROGEDA01", result.PlayerId);
			Assert.AreEqual("KC", result.TeamId);
			Assert.AreEqual("CUT", result.RecommendedAction);
		}

		[TestMethod]
		public void TestCaseyWalker()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Ravens re-signed exclusive rights free agent DT Casey Walker to a one-year, $510,000 contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("WALKCA01", result.PlayerId);
			Assert.AreEqual("BR", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestJerodMayo()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Patriots ILB Jerod Mayo has agreed to a pay cut, signing a new one-year, $4.5 million contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("MAYOJE01", result.PlayerId);
			Assert.AreEqual("NE", result.TeamId);
			Assert.AreEqual("SIGN", result.RecommendedAction);
		}

		[TestMethod]
		public void TestKaelinBurnet()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Titans re-signed LB Kaelin Burnett to a one-year contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("SIGN", result.RecommendedAction);
			Assert.AreEqual("TT", result.TeamId);
			Assert.AreEqual("BURNKA01", result.PlayerId);
		}

		[TestMethod]
		public void TestGeorgeJohnson()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 04, 08),
				ArticleText = "Bucs acquired DE George Johnson and a late-round pick from the Lions in exchange for a late-round pick."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("TRADE", result.RecommendedAction);
			Assert.AreEqual("TB", result.TeamId);
			Assert.AreEqual("JOHNGE01", result.PlayerId);
		}

		#endregion Signings

		#region Cuts


		[TestMethod]
		public void PayCut_IsNotACut()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 2, 26),
				ArticleText = "NFL Network's Tom Pelissero reports Marshawn Lynch took a $500,000 pay cut from the Raiders before a $1 million roster bonus was due on Saturday."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("IGNORE", result.RecommendedAction);  //  its a pay cut
		}

		[TestMethod]
		public void TestFreeAgentBranch()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 2, 26),
				ArticleText = "The Patriots will not exercise DT Alan Branch's 2018 team option, letting him become a free agent."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsTrue(result.PlayerId.Equals("BRANAL01"));
			Assert.IsTrue(result.PlayerFirstName.Equals("Alan"));
			Assert.IsTrue(result.PlayerLastName.Equals("Branch"));

			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("CUT", result.RecommendedAction);  //  cut because article indicates he is a free agent
		}


		[TestMethod]
		public void TestFreeAgentRoberts()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 2, 26),
				ArticleText = "Free agent Andre Roberts expects to test free agency."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsTrue(result.PlayerId.Equals("ROBEAN01"));
			Assert.IsTrue(result.PlayerFirstName.Equals("Andre"));
			Assert.IsTrue(result.PlayerLastName.Equals("Roberts"));

			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("CUT", result.RecommendedAction);  //  cut because article indicates he is a free agent
		}

		[TestMethod]
		public void TestFreeAgentPosition()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 10, 3),
				ArticleText = "Troy Renck of Denver 7 reports free agent LT Russell Okung is drawing interest from the Panthers and Giants."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsTrue(result.PlayerId.Equals("OKUNRU01"));
			Assert.IsTrue(result.PlayerFirstName.Equals("Russell"));
			Assert.IsTrue(result.PlayerLastName.Equals("Okung"));

			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("CUT", result.RecommendedAction);  //  cutting loose a player at end of his contract
		}

		[TestMethod]
		public void TestFreeAgent()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "NFL Network's Ian Rapoport reports the 49ers are setting their sights' on free agent Pierre Garcon."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsTrue(result.PlayerId.Equals("GARCPI01"));
			Assert.IsTrue(result.PlayerFirstName.Equals("Pierre"));
			Assert.IsTrue(result.PlayerLastName.Equals("Garcon"));

			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("CUT", result.RecommendedAction);  //  cutting loose a player at end of his contract
		}

		[TestMethod]
		public void TestJoshCribbsFreeAgent()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 04, 06),
				ArticleText = "Free agent KR Josh Cribbs has retired."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsTrue(result.PlayerId.Equals("CRIBJO01"));
			Assert.IsTrue(result.PlayerFirstName.Equals("Josh"));
			Assert.IsTrue(result.PlayerLastName.Equals("Cribbs"));

			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreNotEqual("CUT", result.RecommendedAction);
		}

		[TestMethod]
		public void TestFirstWaiver()
		{
			//  Ignore Offer sheets (not official)
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Cardinals waived/released CB Roc Carmichael and RB Zach Bauman."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("AC", result.TeamId);
			Assert.AreEqual("IGNORE", result.RecommendedAction);  //  dont know him
		}

		[TestMethod]
		public void TestLocatesWaivers()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Chiefs waived TE Sean McGrath."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("MCGRSE01", result.PlayerId);
			Assert.AreEqual("KC", result.TeamId);
			Assert.AreEqual("WAIVER", result.RecommendedAction);
		}

		[TestMethod]
		public void TestIgnoresUnknownReleases()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Lions released G Rodney Austin."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		[TestMethod]
		public void TestLocatesReleases()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Cowboys released RB DeMarco Murray."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.AreEqual("CUT", result.RecommendedAction);
		}

		[TestMethod]
		public void TestInconsequentialCutSideEffect()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Ego Ferguson and Jeremiah Ratliff are candidates to start at defensive end now that Ray McDonald has been cut."
			};
			Console.WriteLine(article);
			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		#endregion Cuts

		#region Trades

		[TestMethod]
		public void TestSmithTrade()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 3, 10),
				ArticleText = "Eagles acquired CB Daryl Worley in exchange for WR Torrey Smith."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("TRADE", result.RecommendedAction);
			Assert.AreEqual("PE", result.TeamId);
		}

		[TestMethod]
		public void TestLocatesTrade()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 04, 08),
				ArticleText = "Bucs acquired DE George Johnson and a late-round pick from the Lions in exchange for a late-round pick."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("JOHNGE01", result.PlayerId);
			Assert.AreEqual("TB", result.TeamId);
			Assert.AreEqual("TRADE", result.RecommendedAction);
		}

		#endregion Trades

		#region Retirements

		[TestMethod]
		public void TestBergersRetirements()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 03, 25),
				ArticleText = "RG Joe Berger announced his retirement after 13 NFL seasons."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
		}


		[TestMethod]
		public void TestLocatesRetirements()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 03, 01),
				ArticleText = "Former Cardinals SS Adrian Wilson announced his retirement from football."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
		}

		[TestMethod]
		public void TestTroyPolamalu()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 03, 08),
				ArticleText = "Troy Polamalu is retiring from the NFL."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
		}

		[TestMethod]
		public void TestBradyEwing()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Free agent FB Bradie Ewing has retired from the NFL."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
			Assert.AreEqual("EWINBR01", result.PlayerId);
		}

		[TestMethod]
		public void TestRyanHarris()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 3, 3),
				ArticleText = "Steelers OT Ryan Harris retired from the NFL after 10 seasons."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
			Assert.AreEqual("HARRRY01", result.PlayerId);
		}

		[TestMethod]
		public void TestCJ()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 3, 18),
				ArticleText = "Texans TE C.J.Fiedorowicz has retired after four seasons in the NFL."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsCut);
			Assert.IsFalse(result.IsWaiver);
			Assert.IsFalse(result.IsSigning);
			Assert.IsTrue(result.IsRetirement);
			Assert.AreEqual("RETIRED", result.RecommendedAction);
			Assert.AreEqual("FIEDC.01", result.PlayerId);
		}

		#endregion Retirements

		#region Irrelevants

		[TestMethod]
		public void TestPlayersigningaERFA()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 03, 11),
				ArticleText = "ESPN's Field Yates reports Lions DE Kerry Hyder signed his ERFA tender worth $638,000."
			};
			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		[TestMethod]
		public void TestExtraTraining()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 03, 08),
				ArticleText = "Zach Ertz spent two weeks this offseason working with former Cowboys OL coach Hudson Houck on his blocking."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		[TestMethod]
		public void TestMinorInjury()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Shaun Draughn missed the 49ers' second preseason game with a rib injury."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		#endregion Irrelevants

		#region Waiver Claim

		[TestMethod]
		public void TestWaiverClaim()
		{
			//  Ignore Offer sheets (not official)
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 08, 08),
				ArticleText = "Bears claimed OT Paul Cornick off waivers from the Broncos."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);  //  waiver claim treated the same
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("NEWBIE", result.RecommendedAction);  //  dont know him
		}

		#endregion Waiver Claim

		#region Draft picks

		[TestMethod]
		public void TestDraftPick()
		{
			//  Ignore Offer sheets (not official)
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 04, 28),
				ArticleText = "Jets signed No. 6 overall pick DE Leonard Williams to a four-year, $18.6 million contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		[TestMethod]
		public void TestDraftPick2()
		{
			//  Ignore Offer sheets (not official)
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2017, 05, 05),
				ArticleText = "Seahawks signed seventh-round DB Ryan Murphy to a four-year contract."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsFalse(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("IGNORE", result.RecommendedAction);
		}

		#endregion Draft picks

		#region Newbies

		[TestMethod]
		public void TestNewbieSigning()
		{
			var article = new NewsArticleCommand
			{
				ArticleDate = new DateTime(2018, 3, 18),
				ArticleText = "Seahawks signed S Maurice Alexander."
			};
			Console.WriteLine(article);

			var result = sut.ExamineArticle(article);
			Console.WriteLine(result);
			result.DumpEvent();
			Assert.IsTrue(result.IsSigning);
			Assert.IsFalse(result.IsRetirement);
			Assert.IsFalse(result.IsWaiver);
			Assert.AreEqual("NEWBIE", result.RecommendedAction);
			Assert.AreEqual("SS", result.TeamId);
			Assert.AreEqual("S", result.Position);
		}

		#endregion
	}
}
