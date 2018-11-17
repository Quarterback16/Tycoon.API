using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using RosterLib.Models;
using TFLLib;

namespace RosterGridTests
{
   /// <summary>
   ///  Tests for the RosterGrid logic.
   /// </summary>
	[TestClass]
   public class RosterGridTests
	{
		#region  Gordon

		[TestMethod]
      public void TestNibbleRating()
      {
         var rg = new RosterGrid.RosterGrid();
         var r = new NibbleRanker("2006");
         r.RefreshRatings( 1 );  //  workout the ratings after week 1
         var game = new NFLGame("2006:02-A"); // BB @ MD         

         var actual = r.GetRating( game );
         Assert.AreEqual( -1, actual.AwayOff, string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Away Offence for week ", 2, actual.AwayOff, -1 ) );
         Assert.AreEqual(-1, actual.AwayDef, string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Away Offence for week ", 2, actual.AwayDef, -1));
         Assert.AreEqual(-1, actual.HomeOff, string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Away Offence for week ", 2, actual.HomeOff, -1));
         Assert.AreEqual(1, actual.HomeDef, string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Away Offence for week ", 2, actual.HomeDef, -1));
      }

      [TestMethod]
      public void TestAveragesWeekFive()
      {
         var s = new NflSeason("2006");
         const int week = 5;
         const decimal expected = 20.0M;
         var actual = s.AverageScoreAfterWeek(week);
         Assert.AreEqual(expected, actual,
                         string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Average score after week", week, actual, expected));
      }

      [TestMethod]
      public void TestAveragesWeekFour()
      {
         var s = new NflSeason("2006");
         const int week = 4;
         const decimal expected = 20.0M;
         var actual = s.AverageScoreAfterWeek(week);
         Assert.AreEqual(expected, actual,
                         string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Average score after week", week, actual, expected));
      }

      [TestMethod]
      public void TestAveragesWeekThree()
      {
         var s = new NflSeason("2006");
         const int week = 3;
         const decimal expected = 19.0M;
         var actual = s.AverageScoreAfterWeek(week);
         Assert.AreEqual(expected, actual,
                         string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Average score after week", week, actual, expected));
      }

      [TestMethod]
      public void TestAveragesWeekTwo()
      {
         var s = new NflSeason("2006");
         const int week = 2;
         const decimal expected = 18.0M;
         var actual = s.AverageScoreAfterWeek(week);
         Assert.AreEqual(expected, actual,
                         string.Format("{0} {1:F} was {2}, not {3:F}",
                                       "Average score after week", week, actual, expected));
      }

      [TestMethod]
      public void TestAverages()
      {
         NflSeason s = new NflSeason( "2006" );
         int week = 1;
         decimal expected = 17.0M;
         decimal actual = s.AverageScoreAfterWeek( week );
         Assert.AreEqual(expected, actual,
                         string.Format("{0} {1:F} was {2}, not {3}",
                                       "Average score after week", week, actual, expected ) );
      }

      [TestMethod]
      public void TestMeATS()
      {
         int weekNo = 6;
         int season = 2006;
         NFLWeek week = new NFLWeek(season, weekNo);
         int actual = week.MyAtsCorrect();
         int expected = 8;
         Assert.AreEqual(expected, actual,
                         string.Format("My ATS total was {0} in week {1} of {2}, not {3}",
                                       expected, weekNo, season, actual));
      }
            
      [TestMethod]
      public void TestHomeWins()
      {
         int weekNo = 15;
         int season = 2006;
         NFLWeek week = new NFLWeek(season, weekNo );
         int actual = week.TotalHomeWins();
         int expected = 8;
         Assert.AreEqual( expected, actual,
                         string.Format("There were {0} home wins in week {1} of {2}, not {3}", 
                                       expected, weekNo, season, actual ) );
      }      
      
      [TestMethod]      
      public void TestMySpread()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLWeek week = new NFLWeek(2006, 6);
         int actualCorrect = week.SpreadTotalCorrect();
         int expectedCorrect = 7;
         Assert.AreEqual(expectedCorrect, actualCorrect,
                         string.Format("Bookies tipped {0} games correctly in week 6 of 2005", expectedCorrect));
      }

      [TestMethod]      
      public void TestMyTips()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLWeek week = new NFLWeek( 2006, 5 );
         int actualCorrect = week.MyTipsCorrect();
         int expectedCorrect = 14;
         Assert.AreEqual(expectedCorrect, actualCorrect, 
                         string.Format( "I tipped {0} games correctly in week 5 of 2005", expectedCorrect ) );
      }

      [TestMethod]      
      public void TestWarnerGS4Score()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
      	NFLWeek theWeek = new NFLWeek( 2008, 21, false );
			GS4Scorer gs = new GS4Scorer( theWeek );
      	gs.ScoresOnly = true;
         NFLPlayer kurt = new NFLPlayer("WARNKU01");
         kurt.PlayerCat = "1";
         decimal nScores = gs.RatePlayer( kurt, theWeek );
         const int expectedScores = 3;
         Assert.AreEqual( expectedScores, nScores, "Warner had 3 Scores in the SuperBowl of 2008");
      }

      [TestMethod]      
      public void TestKerryCollinsGS4Score()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
      	NFLWeek theWeek = new NFLWeek( 2008, 11, false );
			GS4Scorer gs = new GS4Scorer( theWeek );
      	gs.ScoresOnly = true;
         NFLPlayer kerry = new NFLPlayer("COLLKE01");
         kerry.PlayerCat = "1";
         decimal nScores = gs.RatePlayer( kerry, theWeek );
         const int expectedScores = 3;
         Assert.AreEqual( expectedScores, nScores, "Collins had 3 Scores in the Week 11 of 2008");
      }

      [TestMethod]      
      public void TestBulgerTDPasses()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
			GS4Scorer gs = new GS4Scorer(new NFLWeek(2006, 9, false));
         NFLPlayer mark = new NFLPlayer("BULGMA01");
         mark.PlayerCat = "1";
         gs.RatePlayer(mark, new NFLWeek( 2006, 9, false ) );
         int actualTDp = mark.ProjectedTDp;
         const int expectedTDp = 1;
         Assert.AreEqual(expectedTDp, actualTDp, "Bulger had 1 Tdp in Week 9 of 2006");
         
      }

      [TestMethod]
      public void TestBulgerPoints()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
			GS4Scorer gs = new GS4Scorer(new NFLWeek(2006, 9, false));
         NFLPlayer mark = new NFLPlayer("BULGMA01");
         mark.PlayerCat = "1";
			decimal actualPts = gs.RatePlayer(mark, new NFLWeek(2006, 9, false));

         const int expectedPts = 9;
         Assert.AreEqual(expectedPts, actualPts, "Bulger had 9 GS4 points in Week 9 of 2006");
      }
            
      [TestMethod]
      public void TestRBurgerPoints()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
			GS4Scorer gs = new GS4Scorer(new NFLWeek(2005, 9, false));
         NFLPlayer ben = new NFLPlayer("ROETBE01");
         ben.PlayerCat = "1";
			decimal actualPts = gs.RatePlayer(ben, new NFLWeek(2005, 9, false));
         
         int expectedPts = 12;
         Assert.AreEqual(expectedPts, actualPts, "Roethlesburger had 12 GS4 points in Week 9 of 2006");
      }
      
      [TestMethod]
      public void TestLastScores()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLPlayer player = new NFLPlayer( "COLSMA01" );
         DataSet ds = player.LastScores( "P", 9, 9, "2006", "1" );
         int expectedTDp = 1;
         Assert.AreEqual( expectedTDp, ds.Tables[0].Rows.Count, "Colston had one TD catch in Week 9 of 2006");
      }
      [TestMethod]
      public void TestLastStatsRivers()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLPlayer player = new NFLPlayer("RIVEPH01");
         DataSet ds = player.LastStats("S", 9, 9, "2006" );
         DataRow dr = ds.Tables[ 0 ].Rows[ 0 ];
         int qty = Convert.ToInt32( dr[ 6 ] );
         int expectedYDp = 211;
         Assert.AreEqual(expectedYDp, qty, "Rivers had 220 yards passing in Week 9 of 2006");
      }      
            
      [TestMethod]
      public void TestLastScoresFavre()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLPlayer player = new NFLPlayer("FAVRBR01");
         DataSet ds = player.LastScores("P", 9, 9, "2006", "2" );  //  "2" because the playerid is the QB on a pass
         int expectedTDp = 1;
         Assert.AreEqual(expectedTDp, ds.Tables[0].Rows.Count, "Favre had one TD pass in Week 9 of 2006");
      }      
      
      [TestMethod]
      public void TestGridStatsRankings()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         for (int w = 9; w < 10; w++)
         {
            RosterGrid.RosterGrid.GridStatsQbRankings(w);
            //RosterGrid.RosterGrid.GridStatsRbRankings(w);
            //RosterGrid.RosterGrid.GridStatsPrRankings(w);
            //RosterGrid.RosterGrid.GridStatsKickerRankings(w);
         }
      }
      
      [TestMethod]
      public void TestStarDueDownWeek()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflSeason season = Masters.Sm.GetSeason("2006"); //  this will load the Power Rankings
         NflTeam team = Masters.Tm.GetTeam("2006", "SD");
         NFLGame game = new NFLGame("2006:04-G"); // SD @ BR         
         Assert.IsTrue(DanGordan.StarDueDown(team, game),
                        string.Format("{1} was star due down in Week {2} in season {0} against {3}", 
                                      season.Year, team.TeamCode, game.Week, game.Opponent( team.TeamCode ) ));         
      }
      
      [TestMethod]
      public void TestUnderratedDog()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();   //  need this for data librarian
         NflTeam team = new NflTeam("AC");         
         NFLGame game = new NFLGame("2006:08-A"); // AC @ GB         
         Assert.IsTrue( DanGordan.UnderratedDog( team, game ), string.Format( "{0} is an underrated dog", team.TeamCode ) );
      }
      
      [TestMethod]
      public void TestDueUp()
      {
         var rg = new RosterGrid.RosterGrid(); 
         NflSeason season = Masters.Sm.GetSeason("2006"); //  this will load the Power Rankings
         NflTeam team = Masters.Tm.GetTeam("2006", "HO");
         NFLGame game = new NFLGame("2006:05-D"); // HT @ HO         
         Assert.IsTrue(DanGordan.DueUp(team, game),
                        string.Format("{1} was due up in Week {2} in season {0}", season.Year, team.TeamCode, game.Week ));
         
      }
      
      [TestMethod]
      public void TestDueDown()
      {
         var rg = new RosterGrid.RosterGrid();         
         NflSeason season = Masters.Sm.GetSeason( "2006" ); //  this will load the Power Rankings
         NflTeam team = Masters.Tm.GetTeam( "2006", "NO" );
         NFLGame game = new NFLGame( "2006:04-F" ); // NO @ CP         
         Assert.IsTrue( DanGordan.DueDown( team, game ),
                        string.Format( "NO was due down in Week 4 in season {0}", season.Year ) );
      }

      [TestMethod]
      public void TestRivals()
      {
         var rg = new RosterGrid.RosterGrid();
         var team = new NflTeam( "WR" );
         Assert.IsTrue( team.IsRival( "DC" ), "Cowboys hate the Skins" );
         var anotherTeam = new NflTeam( "DC" );
         Assert.IsTrue( anotherTeam.IsRival( "WR" ), "Cowboys hate the Skins" );
         var aThirdTeam = new NflTeam( "JJ" );
         Assert.IsFalse( aThirdTeam.IsRival( "TB" ), "these teams are not long time rivals" );
      }

      [TestMethod]
      public void TestOpponent()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLGame game = new NFLGame( "2006:09-J" );
         Assert.AreEqual( game.Opponent( "MV" ), "SF" );
         Assert.AreEqual( game.Opponent( "SF" ), "MV" );
      }

      [TestMethod]
      public void TestPlayoffPotential()
      {
         var team = new NflTeam( "PE" );
         Assert.IsTrue( DanGordan.PlayoffPotential( team ),
                        string.Format( "{0} expect to be in the playoffs and are in danger of missing out", team.Name ) );
         var notacontender = new NflTeam( "HT" );
         Assert.IsFalse( DanGordan.PlayoffPotential( notacontender ),
                         string.Format( "{0} are never going to make the playoffs", notacontender.Name ) );
      }

      [TestMethod]
      public void TestBigScorers()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam team = new NflTeam( "CH" );
         NFLGame game = new NFLGame( "2006:06-M" );
         Assert.IsTrue( DanGordan.BigScoringTeam( team, game ),
                        string.Format( "{0} should be a big scoring team", team.Name ) );
      }

      [TestMethod]
      public void TestRout()
      {
         var rg = new RosterGrid.RosterGrid();
         var game = new NFLGame( "2006:08-N" ); //  just for the date         
         Assert.IsTrue( game.WasRout(),
                        string.Format( "{0} lost by {1} or more", game.LosingTeam(), game.MarginOfVictory() ) );
      }

      [TestMethod]
      public void TestSundayOrMondayRoutTwo()
      {
         var t = new NflTeam("CH");
         var game = new NFLGame("2006:09-G"); //  just for the date   
         var prevGame = t.PreviousGame( game.GameDate );
         Assert.IsFalse(DanGordan.SundayOrMondayNightRout(t, game),
                        string.Format("{0} were beaten by {1} points, in week {2} of 2006",
                                       prevGame.LosingTeam(), prevGame.MarginOfVictory(), prevGame.Week ) );
      }
      
      [TestMethod]
      public void TestSundayOrMondayRout()
      {
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "MV" );
         var game = new NFLGame( "2006:09-A" ); //  just for the date         
         Assert.IsTrue( DanGordan.SundayOrMondayNightRout( t, game ),
                        string.Format( "{0} were beaten by {1} points on, in week {2} of 2006",
                                       game.LosingTeam(), game.MarginOfVictory(), game.Week ) );
      }

      [TestMethod]
      public void TestSundayOrMondayHomeDog()
      {
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "MV" );
         var game = new NFLGame( "2006:08-N" );
         Assert.IsTrue( DanGordan.SundayOrMondayNightUnderdog( t, game ),
                        "Vikings were home dogs on MNF, in week 8 of 2006" );
      }

      [TestMethod]
      public void TestMNFFollowup()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "NO" );
         NFLGame game = new NFLGame( "2006:04-F" );
         Assert.IsFalse( DanGordan.MondayNightFollowUp( t, game ),
                         "Saints played last Monday, covered and now plays another div opp" );
         NflTeam sd = new NflTeam( "SD" );
         NFLGame sdGame = new NFLGame( "2006:02-N" );
         Assert.IsTrue( sdGame.HomeTeam == "SD", "Chargers should be hosting" );
         Assert.IsTrue( DanGordan.MondayNightFollowUp( sd, sdGame ),
                        "Chargers played last Monday, covered and now plays a non div opp" );
      }

      [TestMethod]
      public void TestSundayNight()
      {
         var rg = new RosterGrid.RosterGrid();
         var game = new NFLGame( "2006:09-M" );
         Assert.IsTrue( game.IsSundayNight(), "This should be SNF" );
      }

      [TestMethod]
      public void TestMondayNight()
      {
         var rg = new RosterGrid.RosterGrid();
         var game = new NFLGame( "2006:09-N" );
         Assert.IsTrue( game.IsMondayNight(), "This should be MNF" );
      }

      [TestMethod]
      public void TestResultAfterLoss()
      {
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "NE" );
         var spreadRecord = t.SpreadRecordAfterLoss( new DateTime( 2005, 11, 1 ) );
         Assert.IsTrue( spreadRecord <= 0.4M );
      }

      [TestMethod]
      public void TestResultAfterWin()
      {
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "NE" );
			Assert.IsTrue( t.RecordAfterWin( new DateTime( 2005, 11, 1 ) ) > 0.5M );
			var spreadRecord = t.SpreadRecordAfterWin( new DateTime( 2005, 11, 1 ) );
         Assert.IsTrue( spreadRecord < 0.4M );
      }

      [TestMethod]
      public void TestFavourite()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "SS" );
         NFLGame ng = t.NextGame( new DateTime( 2006, 11, 1 ) );
         //  should be vs OR
         Assert.IsTrue( ng.IsFavourite( t ), "Hawks are favoured to beat OR" );
      }

      [TestMethod]
      public void TestDog()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "SF" );
         NFLGame game = new NFLGame( "2006:09-J" );
         Assert.IsTrue( game.IsDog( t ), "Niners are the Dogs" );
      }

      [TestMethod]
      public void TestDivisionalGame()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NFLGame divGame = new NFLGame( "2006:09-C" );
         Assert.IsTrue( divGame.IsDivisionalGame(), "DC vs WR is a divisional game" );
         NFLGame notDivGame = new NFLGame( "2006:09-D" );
         Assert.IsFalse( notDivGame.IsDivisionalGame(), "GB vs BB is not a divisional game" );
      }

      [TestMethod]
      public void TestNextGame()
      {
         //  the next game for the Seahawks after the week 8 game @ KC is MNF vs the Raiders
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "SS" );
         NFLGame ng = t.NextGame( new DateTime( 2006, 11, 1 ) );
         Assert.IsTrue( ng.IsAway( "OR" ), "The Hawks host the Raiders in Week 9" );
      }

      [TestMethod]
      public void TestHomeDogBounce()
      {
         //  AF were home dogs in Week 8 after losing at home as favourites in week 7
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "AF" );
         NFLGame g = new NFLGame( "2006:07-G" );
         Assert.IsTrue( g.HomeDog(), "AF were supposed to be the home dogs" );
         Assert.IsTrue( g.IsHome( t.TeamCode ), "AF were the host" );
         Assert.IsTrue( DanGordan.HomeDogBounce( t, g ),
                        "AF were home dogs in Week 7 after losing at home as favourites in week 6" );
      }

      [TestMethod]
      public void TestEmbarrasment()
      {
         //  see if the embarrasment thing is working, the niners were trounced
         //  41-10 in week 8 of 2006, 30-Oct-06, that should have embarrased them
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         NflTeam t = new NflTeam( "SF" );
         DateTime when = new DateTime( 2006, 11, 1 );
         Assert.IsTrue( DanGordan.TeamEmbarrased( t, when ), "The Niners were embarrased by the Bears in Week 08[:2006]" );
      }

      [TestMethod]
      public void TestRevenge()
      {
         //  see if the revenge thing is working by testing for a 
         //  game that you know is a revenge situation.
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "HT" );
         var opposingTeam = new NflTeam( "DC" );
         var when = new DateTime( 2006, 10, 16 );
         Assert.IsTrue( DanGordan.WantsRevenge( t, opposingTeam, when ),
                        "On the 16th of Oct-2006 the Texans wanted revenge on the Coyboys" );
      }

      [TestMethod]
      public void TestSpreadRangeScore()
      {
         var rg = new RosterGrid.RosterGrid();
         Assert.AreEqual( 1.0M, DanGordan.SpreadRangeScore( -7, -9.0M ) ); //  lost by 7 instead of 9
         Assert.AreEqual( -2.0M, DanGordan.SpreadRangeScore( -3, 3.0M ) ); //  lost by 3 but should have won by 3
         Assert.AreEqual( 1.0M, DanGordan.SpreadRangeScore( 4, 3.0M ) ); //  won by 4 but should have won by 3
         Assert.AreEqual( -5.0M, DanGordan.SpreadRangeScore( 6, 20.0M ) ); //  won by 6 but should have won by 20
      }

      [TestMethod]
      public void TestSpreadRange()
      {
         var rg = new RosterGrid.RosterGrid();
         var t = new NflTeam( "SD" );
         Assert.AreEqual( 16.0M, t.SpreadRange( "2006", "07" ) );
      }

      [TestMethod]
      public void TestInjuryRatio()
      {
         var rg = new RosterGrid.RosterGrid();
         var pm = new PlayerMaster( "Player", "player.xml" );
         var test = pm.InjuryRatio();
         Assert.IsTrue( test > .02M, "Injury ratio should be more than 2%" );
      }

      [TestMethod]
      public void TestGordanRanks()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         GordanRanks gr = new GordanRanks( "2006" );
      }

      [TestMethod]
      public void TestGetGordan()
      {
         var rg = new RosterGrid.RosterGrid();
         var sm = new SeasonMaster( "Season", "season.xml" );
         var testRanks = sm.GetGordan( "2006", "2006SS" );
         var testValue = testRanks[ 0 ].Trim();
         Assert.IsTrue( testValue.Equals( "B" ), "Seahawks started the season with a B rank" );
         testValue = testRanks[ 1 ].Trim();
         Assert.IsTrue( testValue.Equals( "B" ), "Seahawks rating for week 1, 2006 was B" );
      }

      [TestMethod]
      public void TestGetEP()
      {
         //  an example of finding something in a big XML file using XPath
         var rg = new RosterGrid.RosterGrid();
         var epm = new EpMaster( "EP" );
         var testEP = epm.GetEp( "ALEXSH01" );
         Assert.AreEqual( 213.0M, testEP );
      }

      [TestMethod]
      public void TestHomeAwayRatio()
      {
         var rg = new RosterGrid.RosterGrid();
         var gm = new GameMaster( "Game" );
         var test = gm.HomeAwayRatio();
         Assert.IsTrue( test > .5M, "Home ratio should be more than 50%" );
      }

      [TestMethod]
      public void TestFreeAgentReport()
      {
         var rg = new RosterGrid.RosterGrid();
         var far = new FreeAgentReport();
         far.Render();
#if DEBUG
         Assert.AreEqual( 1, far.TeamCount );
#else
         Assert.AreEqual( 32, far.TeamCount );
#endif
      }

      [TestMethod]
      public void TestBigNumberHome()
      {
         //  set up the mock object pretending to be the static RosterGrid
         var rg = new RosterGrid.RosterGrid();
         var g = new NFLGame( "2006:06-L" );
         var bns = new BigNumberScheme();

         Assert.IsNotNull( bns.IsBettable( g ), "Should be a bet" );
      }

      public void TestBigNumberHome2()
      {
         //  spread in this game was not big enough for a bet
         var rg = new RosterGrid.RosterGrid();
         var g = new NFLGame( "2006:06-M" );
         var bns = new BigNumberScheme();

         Assert.IsNull( bns.IsBettable( g ), "Should not be a bet" );
      }

      [TestMethod]
      public void TestScoreOut3Again()
      {
         //  set up the mock object pretending to be the static RosterGrid
         var rg = new RosterGrid.RosterGrid();
         var g = new NFLGame( "2006:06-A" );

         Assert.AreEqual( "BB 17 @ DL 20", g.ScoreOut3() );
      }

      [TestMethod]
      public void TestScoreOut3()
      {
         //  set up the mock object pretending to be the static RosterGrid
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();
         GameMaster gm = new GameMaster( "Game" );
         NFLGame g = gm.GetGame( "2001:15-A" );

         Assert.AreEqual( "AC 13 @ NG 17", g.ScoreOut3() );
      }

		#endregion

		#region  Best Bets

		[TestMethod]
      public void TestBestBetsWeek7()
      {
         //  set up the mock object pretending to be the static RosterGrid
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 7 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 11, thisWeek.Kenny.Wins );
      }

      [TestMethod]
      public void TestBestBetsWeek6()
      {
         //  set up the mock object pretending to be the static RosterGrid
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 6 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 14, thisWeek.Kenny.Wins );
      }

      [TestMethod]
      public void TestBestBetsWeek5()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 5 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 5, thisWeek.Kenny.Wins );
      }

      [TestMethod]
      public void TestBestBetsWeek3()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 3 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 6, thisWeek.Kenny.Wins );
      }

      [TestMethod]
      public void TestBestBetsWeek2()
      {
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 2 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 0, thisWeek.Kenny.Wins );
      }

      [TestMethod]
      public void TestBestBetsWeek1()
      {
         var rg = new RosterGrid.RosterGrid();

         NFLWeek thisWeek = new NFLWeek( 2006, 1 );
         thisWeek.RenderBestBets();
         Assert.AreEqual( 6, thisWeek.Kenny.Wins );
      }

		#endregion

		#region  Config

      [TestMethod]
      public void TestForConfigFile()
      {
         Assert.IsTrue( File.Exists( @"RosterGrid.exe.Config" ),
                        "Config File not found in expected folder" );
      }

      [TestMethod]
      public void TestDateParsing()
      {
			System.Globalization.DateTimeFormatInfo dtfi;

			dtfi = System.Globalization.DateTimeFormatInfo.InvariantInfo;

			var sTo = "28/02/2013";
			//var sTo = "02/28/2013";
			var dTo = sTo == "30/12/1899" ? DateTime.Now : DateTime.Parse(sTo);
			Assert.AreEqual( dTo, new DateTime(2013,2,28));
      }

		[TestMethod]
		public void TestProjectionFileName()
		{
			var rr = new NFLRosterReport("2013");
			//var sTo = "02/28/2013";
			var fileOut = rr.ProjectionFileName( "Spread", "2013", "0" );
			Assert.AreEqual( string.Format("{0}2013//Projections//Proj-Spread-0.htm",
				Utility.OutputDirectory() ), fileOut );
		}

      [TestMethod]
      public void TestPlayer()
      {
         //  set up the mock object pretending to be the static RosterGrid
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         NFLPlayer p = new NFLPlayer( "MONTJO01" );

         Assert.AreEqual( "MONTJO01", p.PlayerCode, "Player code not loaded properly" );
         Assert.AreEqual( "Joe Montana", p.PlayerName, "Player name not loaded properly" );
         Assert.AreEqual( "1", p.PlayerCat, "Category not loaded properly" );
         Assert.AreEqual( "1980", p.RookieYear, "Rookie Year not loaded properly" );
         Assert.IsNotNull( p.CurrTeam, "Team should be set" );
         p.LoadPerformances( true, false, "2006" ); //  Load players career performance
         Assert.IsTrue( p.GamesLoaded, "GamesLoaded flag should be set" );
      }

      [TestMethod]
      public void TestEP()
      {
         //  set up the mock object pretending to be the static RosterGrid
         RosterGrid.RosterGrid rg = new RosterGrid.RosterGrid();

         ////  the EP for the SD PO in Week 1 of 2006 was ..
         NflTeam t = new NflTeam( "SD", "2006" );

         Assert.IsNotNull( t, "Failed to create the Team " );
         Assert.AreEqual( 1.0M, t.GetMetric( "2006", "01", "PO" ) );
      }

		#endregion

		#region  Victory Points

		[TestMethod]
		public void TestVictoryPoints()
		{
			var vp = new VictoryPoints( "2012" );
			vp.RenderAsHtml();
			Assert.IsTrue( File.Exists( vp.FileOut ) );
		}

		#endregion

		#region  Categories

		[TestMethod]
		public void TestCategory7ForMethod()
		{
			const string expectedResult = "7";
			const string testPos = "OT";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory1ForMethod()
		{
			const string expectedResult = "1";
			const string testPos = "QB";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory2ForMethod()
		{
			const string expectedResult = "2";
			const string testPos = "RB";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory3ForMethod()
		{
			const string expectedResult = "3";
			const string testPos = "TE";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory4ForMethod()
		{
			const string expectedResult = "4";
			const string testPos = "K";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory5ForMethod()
		{
			const string expectedResult = "5";
			const string testPos = "ILB";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

		[TestMethod]
		public void TestCategory6ForMethod()
		{
			const string expectedResult = "6";
			const string testPos = "SS";
			var testCat = Utility.CategoryFor(testPos);
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testCat,
			                        string.Format("The category returned for {0} was {1} not {2} as expected",
			                                      testPos, testCat, expectedResult));
		}

      #endregion

		#region  Integrity checks

		[TestMethod]
		public void TestNextId()
		{
			const string expectedResult = "COLOST01";
			var testId = Utility.TflWs.NextId("Steve", "Colonna");
			//  the generic version is almost always preferred because it will verify at compile time that the types match
			Assert.AreEqual(expectedResult, testId, string.Format("The id returned for {0} was {1} not {2} as expected",
			                                                              "Steve Colonna", testId, expectedResult));
		}

		[TestMethod]
		public void TestAllPositionsFromDatabase()
		{
			var dt = Utility.TflWs.GetDistinctPositions();

			foreach (DataRow dr in dt.Rows)
			{
				var testPos = dr["POSDESC"].ToString();
				var testCat = Utility.CategoryFor(testPos);
				Console.WriteLine(testPos);

				Assert.AreNotEqual( "?", testCat,
					 string.Format( "The category returned for {0} was {1}",
					 testPos, testCat ) );
			}
		}

		[TestMethod]
		public void TestJonathonScottExists()
		{
			var testBool = Utility.TflWs.PlayerExists("Jonathan", "Scott", "Texas", "" );
			Assert.IsTrue(testBool, string.Format("The player does not exist"));
		}

		[TestMethod]
		public void TestJoeMontanaExists()
		{
			var testBool = Utility.TflWs.PlayerExists("Joe", "Montana", "Notre Dame", "" );
			Assert.IsTrue(testBool, string.Format("The player does not exist"));
		}

		[TestMethod]
		public void TestJoeMontanaByDobExists()
		{
			var testBool = Utility.TflWs.PlayerExists("Joe", "Montana", "Notre Dame", "06/11/1956");
			Assert.IsTrue(testBool, string.Format("The player does not exist"));
		}

		[TestMethod]
		public void TestAllCollegesFromDatabase()
		{
			var dt = Utility.TflWs.GetDistinctColleges();
			Console.WriteLine(string.Format("There are {0} distinct colleges:-", dt.Rows.Count));

			foreach (var testCollege in from DataRow dr in dt.Rows select dr["COLLEGE"].ToString())
				Console.WriteLine(testCollege);
		}

		[TestMethod]
		public void TestTeamsSetRecordAc2010()
		{
			const string teamCode = "AC";
			const string season = "2010";

			var team = new NflTeam( teamCode );

			team.SetRecord( season );

			Assert.IsTrue( team.Ratings.Equals("EEECEC"), 
				string.Format( "{0} ratings for {1} are incorrect", teamCode, season ) );

		}

		[TestMethod]
		public void TestChangeRatingEdgeCaseBy1()
		{
			var team = new NflTeam("SF");
			var changedRating = team.ChangeRating(0, -1, "EEECEC");
			Assert.IsTrue(changedRating.Equals("EEECEC"), 
				string.Format( "Ratings should still be EEECEC not {0}", changedRating ) );
		}

		[TestMethod]
		public void TestChangeRatingEdgeCaseBy2()
		{
			var team = new NflTeam( "SF" );
			var changedRating = team.ChangeRating( 0, -2, "EEECEC" );
			Assert.IsTrue( changedRating.Equals( "EEECEC" ),
				string.Format( "Ratings should still be EEECEC not {0}", changedRating ) );
		}

		[TestMethod]
		public void TestChangeRatingUpperEdgeCaseBy1()
		{
			var team = new NflTeam( "SF" );
			var changedRating = team.ChangeRating( 0, 1, "AEECEC" );
			Assert.IsTrue( changedRating.Equals( "AEECEC" ),
				string.Format( "Ratings should still be AEECEC not {0}", changedRating ) );
		}

		[TestMethod]
		public void TestChangeRatingUpperEdgeCaseBy2()
		{
			var team = new NflTeam( "SF" );
			var changedRating = team.ChangeRating( 0, 2, "AEECEC" );
			Assert.IsTrue( changedRating.Equals( "AEECEC" ),
				string.Format( "Ratings should still be AEECEC not {0}", changedRating ) );
		}

		[TestMethod]
		public void TestDns()
		{
			var dns = Dns.GetHostName();
			Assert.IsTrue( dns.Equals( "WDE308498" ), string.Format("hostname {0} wrong", dns) );
		}

		[TestMethod]
		public void TestNflComIpAddress()
		{
			var ipHost = Dns.GetHostEntry("www.nfl.com");
			var ipAddress = ipHost.AddressList;
			var strIpAddress = new StringBuilder();

			foreach (var addr in ipAddress)
				strIpAddress.Append( addr.ToString());

			var compareThis = strIpAddress.ToString().Substring(0, 22);
			Assert.IsTrue( compareThis.Equals( "144.135.8.230144.135.8" ), 
				string.Format( "IP Address {0} wrong", strIpAddress ) );
		}

		[TestMethod]
		public void TestMetricsBase()
		{
			var mb = new MetricsBase();
			mb.Load("2010");
			mb.RenderTeam("SF");
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestScheduleEntry()
		{
			var season = new NflSeason( "2012" );
			Assert.IsTrue( season.NumberOfTeams().Equals( 32 ), "There should be 32 teams" );
			Assert.IsTrue( season.NumberOfGames().Equals( 256 ), "There should be 256 games" );
			foreach (var team in season.TeamList )
			{
				team.LoadGames( team.TeamCode, "2012" );
				Assert.IsTrue( team.GameList.Count.Equals( 16 ), 
					string.Format( "{0} should have 16 games", team.TeamCode ) );
			}
		}

		[TestMethod]
		public void TestPlayoffTeams()
		{
			var season = new NflSeason( "2012" );
			var playoffCnt = 0;

			foreach ( var team in season.TeamList.Where( team => team.IsPlayoffBound ) )
			{
				playoffCnt++;
				Utility.Announce( string.Format( "Playoff bound->{0}", team.NameOut() ) );
			}
			Assert.AreEqual( 12,  playoffCnt, "There should be 12 playoff teams" );
		}

		[TestMethod]
		public void TestRosterSizes()
		{
			var season = new NflSeason( "2012" );
			var playerCnt = 0;
			foreach ( var team in season.TeamList )
			{
				team.LoadPlayers();
				playerCnt += team.PlayerList.Count;
				Utility.Announce( string.Format( "{0,-20} has {1} players", team.NameOut(), team.PlayerList.Count ) );
			}
			Assert.AreEqual( 53*32, playerCnt, string.Format( "{0} has {1} players", "NFL", playerCnt ) );	
		}

		[TestMethod]
		public void TestSeasonForJan2011()
		{
			var testDate = new DateTime(2011, 1, 1);
			var theSeason = Utility.SeasonFor( testDate );
			const string correctSeason = "2010";
			Assert.IsTrue( theSeason.Equals( correctSeason ),
				string.Format( "The season for {0:d} should be {1} not {2}", testDate, correctSeason, theSeason ) );
		}

		[TestMethod]
		public void TestSeasonForMar2011()
		{
			var testDate = new DateTime( 2011, 3, 1 );
			var theSeason = Utility.SeasonFor( testDate );
			const string correctSeason = "2011";
			Assert.IsTrue( theSeason.Equals( correctSeason ),
				string.Format( "The season for {0:d} should be {1} not {2}", testDate, correctSeason, theSeason ) );
		}

		[TestMethod]
		public void TestSeasonForJan2010()
		{
			var testDate = new DateTime( 2010, 1, 1 );
			var theSeason = Utility.SeasonFor( testDate );
			const string correctSeason = "2009";
			Assert.IsTrue( theSeason.Equals( correctSeason ),
				string.Format( "The season for {0:d} should be {1} not {2}", testDate, correctSeason, theSeason ) );
		}

		[TestMethod]
		public void TestSeasonForMar2010()
		{
			var testDate = new DateTime( 2010, 3, 1 );
			var theSeason = Utility.SeasonFor( testDate );
			const string correctSeason = "2010";
			Assert.IsTrue( theSeason.Equals( correctSeason ),
				string.Format( "The season for {0:d} should be {1} not {2}", testDate, correctSeason, theSeason ) );
		}

		[TestMethod]
		public void TestUnitRatingsServiceForNiners2011()
		{
			var urs = new UnitRatingsService();
			var team = new NflTeam("SF");
			var testDate = new DateTime( 2011, 3, 1 );
			var ratings = urs.GetUnitRatingsFor( team, testDate );

			const string correctRating = "BCCBBD";  //  PO goes to a B due to Braylon Edwards, PP goes to C (rookies)
			Assert.IsTrue( ratings.Equals( correctRating ),
				string.Format( "The rating for {0} on {1:d} should be {2} not {3}",
				team.NameOut(), testDate, correctRating, ratings ) );
		}

		[TestMethod]
		public void TestUnitRatingsServiceForNiners2010()
		{
			var urs = new UnitRatingsService();
			var team = new NflTeam( "SF" );
			var testDate = new DateTime( 2010, 3, 1 );
			var ratings = urs.GetUnitRatingsFor( team, testDate );

			const string correctRating = "CCDBBD";
			Assert.IsTrue( ratings.Equals( correctRating ),
				string.Format( "The rating for {0} on {1:d} should be {2} not {3}",
				team.NameOut(), testDate, correctRating, ratings ) );

			team = new NflTeam("BB");
			ratings = urs.GetUnitRatingsFor( team, testDate );
			Assert.IsTrue( urs.CacheHit,
				string.Format( "The cache should have been hit for the date of {0:d}", testDate ) );

		}

		[TestMethod]
		public void TestWeek1PredictionForNiners2011()
		{
			var getter = new DbfPredictionQueryMaster();
			var prediction = getter.Get( "unit", "2011", "01", "J" );
			Assert.IsTrue( prediction.HomeScore.Equals( 34 ), 
				string.Format("Home score should be 34 not {0}", prediction.HomeScore ) );
			Assert.IsTrue( prediction.AwayScore.Equals( 10 ), 
				string.Format( "Away score should be 10 not {0}", prediction.AwayScore ) );
		}

		[TestMethod]
		public void TestWeek1PredictionForPats2013()
		{
			var getter = new DbfPredictionQueryMaster();
			var prediction = getter.Get( "unit", "2013", "01", "B" );
			Assert.IsTrue( prediction.HomeScore.Equals( 20 ),
				string.Format( "Home score should be 20 not {0}", prediction.HomeScore ) );
			Assert.IsTrue( prediction.AwayScore.Equals( 31 ),
				string.Format( "Away score should be 31 not {0}", prediction.AwayScore ) );
			Assert.IsTrue( prediction.NflResult.AwayTDp == 2,
				string.Format( "Away TDp should be 2 not {0}", prediction.NflResult.AwayTDp ) );
		}

		#endregion

		#region  GridStatsMaster

		[TestMethod]
		public void TestGridStatsMasterConstructor()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			Assert.IsNotNull( m );
		}

		[TestMethod]
		public void TestGridStatsMasterCalculating()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			m.Calculate( "2012", "07" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestGridStatsMasterCalculatingSeasonToDate()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			m.Calculate( "2012" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		#endregion


		#region  StatMaster

		[TestMethod]
		public void TestStatMasterConstructor()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			Assert.IsNotNull( sm );
		}

		[TestMethod]
		public void TestStatMasterPersistence()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			var stat = new NflStat {Season = "2012", Week = "01", TeamCode = "SF", StatType = "K"};
			sm.PutStat( stat );
			sm.Dump2Xml();
			Assert.IsTrue( File.Exists( sm.Filename ) );
		}

		[TestMethod]
		public void TestStatMasterGet()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			var stat = sm.GetStat( "2012", "01", "SF", "Sacks" );
			Assert.AreEqual( 1.0M, stat.Quantity );
		}

		[TestMethod]
		public void TestStatMasterCalculatingForAWeek()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			sm.Calculate( "2012", "05" );
			sm.Dump2Xml();
			Assert.IsTrue( File.Exists( sm.Filename ) );
		}

		[TestMethod]
		public void TestStatMasterCalculatingAllStatsSeasonToDate()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			sm.Calculate( "2012" );
			sm.Dump2Xml();
			Assert.IsTrue( File.Exists( sm.Filename ) );
		}

		#endregion

		#region  Old Roster Reports

		[TestMethod]
		public void TestOldRosterReport()
		{
			var rg = new NFLRosterGrid( Constants.K_QUARTERBACK_CAT ) { Focus = "QB" };
			rg.CurrentRoster();
			Assert.IsTrue( File.Exists( rg.FileOut ), string.Format( "Cannot find {0}", rg.FileOut ) );
		}

		#endregion

		#region  FA Market

		[TestMethod]
		public void TestFAMarketReport()
		{
			var fa = new FaMarket( Utility.CurrentSeason() );
			fa.RenderAsHtml();
			Assert.IsTrue( File.Exists( fa.FileOut ), string.Format( "Cannot find {0}", fa.FileOut ) );
		}

		#endregion

		#region  Team Cards

		[TestMethod]
		public void TestTeamCardsCurrentSeason()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			foreach ( var team in season.TeamList )
			{
				var tc = new TeamCard( team );
				tc.Render();
			}
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestTeamCardNiners()
		{
			var team = new NflTeam( "SF" );
			var tc = new TeamCard(team);
			tc.Render();
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestTeamCardHoustonTexans()
		{
			var team = new NflTeam( "HT" );
			var tc = new TeamCard( team );
			tc.Render();
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestGetNinersCenter()
		{
			var team = new NflTeam( "SF" );
			var center = team.GetPlayerAt("C", 1, bDef: false, usedVal: false);
			const string actualCenter = "ASnyder";
			Assert.IsTrue( center.PlayerNameShort.Equals( actualCenter ),
			              string.Format("The center should be {0} not {1}",
							  actualCenter, center.PlayerNameShort));
		}

		[TestMethod]
		public void TestGetNinersFullBack()
		{
			var team = new NflTeam( "SF" );
			var player = team.GetPlayerAt( "FB", 1, bDef: false, usedVal: false );
			const string actualPlayer = "BMiller";
			Assert.IsTrue( player.PlayerNameShort.Equals( actualPlayer ),
							  string.Format( "The Running Back should be {0} not {1}",
							  actualPlayer, player.PlayerNameShort ) );
		}

		[TestMethod]
		public void TestGetNinersRunningBack()
		{
			var team = new NflTeam( "SF" );
			var player = team.GetPlayerAt( "RB", 1, bDef: false, usedVal: false );
			const string actualPlayer = "FGore";
			Assert.IsTrue( player.PlayerNameShort.Equals( actualPlayer ),
							  string.Format( "The Running Back should be {0} not {1}",
							  actualPlayer, player.PlayerNameShort ) );
		}

		[TestMethod]
		public void TestGetNinersWideReceiver()
		{
			var team = new NflTeam( "SF" );
			var player = team.GetPlayerAt( "WR", 1, bDef: false, usedVal: false );
			const string actualPlayer = "BEdwards";
			Assert.IsTrue( player.PlayerNameShort.Equals( actualPlayer ),
							  string.Format( "The Wide Receiver should be {0} not {1}",
							  actualPlayer, player.PlayerNameShort ) );
		}

		[TestMethod]
		public void TestNinersUseThreeFourDefense()
		{
			var team = new NflTeam( "SF" );
			team.SetDefence();
			Assert.IsTrue( team.Uses34Defence,  "SF uses 3-4 defence" );
		}

		[TestMethod]
		public void TestGetNinersMiddleLineBacker()
		{
			var team = new NflTeam( "SF" );
			var player = team.GetPlayerAt( "MLB", 1, bDef: true, usedVal: false );
			Assert.IsNull( player, "SF does not use MLB" );
		}

		[TestMethod]
		public void TestForBlankSpotsCurrentSeason()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			var lastTeam = new NflTeam("SF");
			foreach (var team in season.TeamList )
			{
				team.LoadCurrentStarters();
				team.DumpStarters();
				lastTeam = team;
			}
			Assert.IsTrue( lastTeam.StarterList.Count.Equals( 23 ),
							  string.Format( "{0} has {1} starters",
							  lastTeam.NameOut(), lastTeam.StarterList.Count ) );		
		}

		#endregion

		#region  Missing Starters

		[TestMethod]
		public void TestForMissingRunningBacks()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			var rbCount = 0;
			foreach ( var team in season.TeamList )
			{
				team.LoadCurrentStarters();
				rbCount += team.DumpMissingRunningBacks();
			}
			Assert.IsTrue( rbCount.Equals(32), string.Format( "Running backs missing or overstocked - {0}", rbCount ) );
		}

		[TestMethod]
		public void TestForMissingQuarterBacks()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			var qbCount = 0;
			foreach ( var team in season.TeamList )
			{
				team.LoadCurrentStarters();
				qbCount += team.DumpMissingQuarterBacks();
			}
			Assert.IsTrue( qbCount.Equals( 32 ), string.Format( "Quarterbacks missing or overstocked - {0}", qbCount ) );
		}

		[TestMethod]
		public void TestForMissingKickers()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			var kkCount = 0;
			foreach ( var team in season.TeamList )
			{
				team.LoadCurrentStarters();
				kkCount += team.DumpMissingKickers();
			}
			Assert.IsTrue( kkCount.Equals( 32 ), string.Format( "Kickers missing or overstocked - {0}", kkCount ) );
		}

		[TestMethod]
		public void TestForMissingTightEnds()
		{
			var season = new NflSeason( Utility.CurrentSeason(), loadGames: false, loadDivisions: true );
			var teCount = 0;
			foreach ( var team in season.TeamList )
			{
				team.LoadCurrentStarters();
				teCount += team.DumpMissingTightEnds();
			}
			Assert.IsTrue( teCount.Equals( 32 ), string.Format( "Looking for 32 TightEnds not - {0}", teCount ) );
		}

		[TestMethod]
		public void TestNinersForBlankSpotsCurrentSeason()
		{
			var team = new NflTeam( "SF" );
			team.LoadCurrentStarters();
			team.DumpStarters();
			Assert.IsTrue( team.StarterList.Count.Equals( 23 ),
								string.Format( "{0} has {1} starters",
								team.NameOut(), team.StarterList.Count ) );
			
		}

		#endregion

		#region  Tipping

		[TestMethod]
		public void TestTippingComp()
		{
			//  Arrange
			var tipsters = new Dictionary<string, WinLossRecord>
			               	{
			               		{"Angie", new WinLossRecord(3, 1, 0)},
			               		{"Bob", new WinLossRecord(2, 2, 0)},
			               		{"Charlie", new WinLossRecord(0, 4, 0)},
										{"Denis", new WinLossRecord(4,0,0)}
			               	};
			var tc = new TippingComp(tipsters);

			//  Act
			tc.Render();

			//  Assert
			Assert.IsTrue( File.Exists( tc.OutputFilename ), string.Format( "Cannot find {0}", tc.OutputFilename ) );
		}

		[TestMethod]
		public void TestTippingController()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.Index( "2011" );
			//  Assert
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestTippingControllerAts()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.IndexAts( "2010" );
			//  Assert
			Assert.IsTrue( true );
		}

		#endregion

		#region  Predictions

		[TestMethod]
		public void TestPredictionsFor2010()
		{
			const string theSeason = "2010";
			var rr = new NFLRosterReport( theSeason );
			rr.SeasonProjection( "Spread", theSeason, "0", Utility.StartOfSeason( theSeason ) );
			Assert.IsTrue( File.Exists( rr.FileOut ), string.Format( "Cannot find {0}", rr.FileOut ) );
		}

		[TestMethod]
		public void TestPredictionsFor2011()
		{
			const string theSeason = "2011";
			var rr = new NFLRosterReport( theSeason );
			rr.SeasonProjection( "Spread", theSeason, "0", Utility.StartOfSeason( theSeason ) );
			Assert.IsTrue( File.Exists( rr.FileOut ), string.Format( "Cannot find {0}", rr.FileOut ) );
		}

		[TestMethod]
		public void TestPickAScore()
		{
			Assert.IsTrue( Utility.PickAScore( 18 ).Equals( 17 ), "18 gives you 17" );
			Assert.IsTrue( Utility.PickAScore( 19 ).Equals( 20 ), "19 gives you 20" );
			Assert.IsTrue( Utility.PickAScore( 20 ).Equals( 20 ), "20 gives you 20" );
			Assert.IsTrue( Utility.PickAScore( 21 ).Equals( 21 ), "21 gives you 21" );
			Assert.IsTrue( Utility.PickAScore( 22 ).Equals( 21 ), "22 gives you 21" );			
			Assert.IsTrue( Utility.PickAScore( 23 ).Equals( 23 ), "23 gives you 23" );			
		}

		#endregion

		#region  Current Week

		[TestMethod]
		public void TestCurrentWeekAug2011()
		{
			var w = Utility.UpcomingWeek( new DateTime( 2011, 8, 17 ) );
			Assert.IsTrue( w.Season.Equals("2011") && w.WeekNo.Equals( 1 ) );
		}

		[TestMethod]
		public void TestCurrentWeekAug2010()
		{
			var w = Utility.UpcomingWeek( new DateTime( 2010, 8, 17 ) );
			Assert.IsTrue( w.Season.Equals( "2010" ) && w.WeekNo.Equals( 1 ) );
		}

		[TestMethod]
		public void TestCurrentWeekNov2010()
		{
			var w = Utility.UpcomingWeek( new DateTime( 2011, 11, 8 ) );
			Assert.IsTrue( w.Season.Equals( "2011" ) && w.WeekNo.Equals( 10 ) );
		}

		#endregion

		#region  Hillin Stuff

		[TestMethod]
		public void TestHillenReport()
		{
			var r = new HillenTips( "2012", "10" );
			r.Render();
			Assert.IsTrue( File.Exists( r.FileName() ) );
		}

		[TestMethod]
		public void TestHillenMasterFullCalculate()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			m.Calculate( "2012" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestHillenoMasterSeason2012Week09()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			m.Calculate( "2012", "09" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestHillenMasterSeasonSetup()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			m.Calculate( "2012", "01" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestGettingGame()
		{
			var game = Utility.GetGameFor( "2012", "01", "AF" );
			Assert.IsNotNull( game );
		}

		[TestMethod]
		public void TestHillenMasterWeek02PowerRatings()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			m.Calculate( "2012", "02" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestGettingPowerRatingFromXml()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			var rating = m.GetStat( "2012:01:SF" );
			Assert.AreEqual( 31.5, rating, "Power Rating is 31.5 prior to week 1 of 2012" );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek01()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor {PredictionStorer = ps};
			var week = new NFLWeek( 2012, 1 );
			hp.PredictWeek( week );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek02()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor { PredictionStorer = ps };
			var week = new NFLWeek( 2012, 2 );
			hp.PredictWeek( week );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek03()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor { PredictionStorer = ps };
			var week = new NFLWeek( 2012, 3 );
			hp.PredictWeek( week );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek04()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor { PredictionStorer = ps };
			var week = new NFLWeek( 2012, 4 );
			hp.PredictWeek( week );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek08()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor { PredictionStorer = ps };
			var week = new NFLWeek( 2012, 8 );
			hp.PredictWeek( week );
		}

		[TestMethod]
		public void TestHillenPredictorForWeek09()
		{
			var ps = new DbfPredictionStorer();
			var hp = new HillinPredictor { PredictionStorer = ps };
			var week = new NFLWeek( 2012, 9 );
			hp.PredictWeek( week );
		}




		[TestMethod]
		public void TestAdjustedPowerRatingsAfterWeek02()
		{
			var totalPower = 0.0M;
			var season = new NflSeason( "2012" );
			foreach ( var team in season.TeamList )
			{
				team.PowerRating = team.GetPowerRating( "02" );
				totalPower += team.PowerRating;
			}
			season.DumpPowerRatings();
			Assert.AreEqual( 699, totalPower );
		}

		[TestMethod]
		public void TestAdjustedPowerRatingsAfterWeek03()
		{
			var totalPower = 0.0M;
			var season = new NflSeason( "2012" );
			foreach ( var team in season.TeamList )
			{
				team.PowerRating = team.GetPowerRating( "03" );
				totalPower += team.PowerRating;
			}
			season.DumpPowerRatings();
			Assert.AreEqual( 699, totalPower );
		}



		[TestMethod]
		public void TestNinersPowerRatingAfterWeek01of2012()
		{
			var sf = new NflTeam( "SF" );
			var pwr = sf.GetPowerRating( "02" );
			Assert.AreEqual( 34.5, pwr, string.Format( "Power Rating for SF after 2012:01 should be 34.5 not {0}", pwr ) );
		}

		[TestMethod]
		public void TestNinersPowerRatingAfterWeek02of2012()
		{
			var sf = new NflTeam( "SF" );
			var pwr = sf.GetPowerRating( "02" );
			Assert.AreEqual( 33.5, pwr, string.Format( "Power Rating for SF after 2012:02 should be 32.5 not {0}", pwr ) );
		}

		[TestMethod]
		public void TestHillenPredictor()
		{
			var hp = new HillinPredictor();
			var game = new NFLGame( "2012:01-A" );  //  DC @ NG
			var result = hp.PredictGame( game, new FakePredictionStorer(), Utility.StartOfSeason() );
			Assert.IsTrue( result.HomeWin() );
			Assert.IsTrue( result.HomeScore.Equals( 24 ), string.Format( "Home score should be 24 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 21 ), string.Format( "Away score should be 21 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestHillenPredictorNinersPackers()
		{
			var hp = new HillinPredictor();
			var game = new NFLGame( "2012:01-K" );  //  SF @ GB
			var result = hp.PredictGame( game, new FakePredictionStorer(), Utility.StartOfSeason() );
			Assert.IsTrue( result.HomeWin() );
			Assert.AreEqual( 30, result.HomeScore, string.Format( "Home score should be 30 not {0}", result.HomeScore ) );
			Assert.AreEqual( 21, result.AwayScore, string.Format( "Away score should be 21 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestTeamPowerRatingInstantiation()
		{
			var team = new NflTeam( "SF", "2012" );
			Assert.IsTrue( team.PowerRating == 31.5M,
				string.Format( "Power rating should be 31.5 not {0}", team.PowerRating ) );
		}

		[TestMethod]
		public void TestHillinPredictionsFor2012()
		{
			const string season = "2012";
			var fileout = string.Format( "{0}{1}\\Projections\\Hillin.htm",
				Utility.OutputDirectory(), season );
			var ps = new DbfPredictionStorer();
			var predictor = new HillinPredictor
			{
				AuditTrail = false,
				StorePrediction = true,
				PredictionStorer = ps
			};
			predictor.PredictSeason( season, Utility.StartOfSeason( season ), fileout );
			Assert.IsTrue( File.Exists( fileout ), string.Format( "Cannot find {0}", fileout ) );
		}

		#endregion

		#region  XP

		[TestMethod]
		public void TestExperienceMasterInstantiation()
		{
			var em = Masters.Epm;
			Assert.IsNotNull(em);
			var xp = em.GetEp("MANNPE01");
			const decimal expectedXp = 378.0M;
			Assert.IsTrue( xp.Equals( expectedXp ), "PManning has {0} not {1}", expectedXp, xp );
		}

		[TestMethod]
		public void TestExperienceMasterUpdate()
		{
			var em = Masters.Epm;
			Assert.IsNotNull( em );
		}


		[TestMethod]
		public void TestWizSeason2010()
		{
			var em = new WizSeason("2010","01","17");
			Assert.IsNotNull( em );
		}

		#endregion

		#region  Game Master

		[TestMethod]
		public void TestGameMasterInstantiation()
		{
			var m = Masters.Gm;
			Assert.IsNotNull(m);
			Assert.IsTrue(m.HomeAwayRatio() > 0);
		}

		#endregion

		#region Suggested Lineups

		[TestMethod]
		public void TestSuggestedPerfectChallengeLineupForWeek1()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_PerfectChallenge,
													Constants.KOwnerSteveColonna, "CC",
													Utility.CurrentSeason(),
													1 ) { IncludeSpread = true, IncludeRatingModifier = true, IncludeFreeAgents = true };
			r2.Render();
			Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
		}

		[TestMethod]
		public void TestSuggestedPerfectChallengeLineupForWeek1Of2011()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_PerfectChallenge,
													Constants.KOwnerSteveColonna, "CC",
													"2011",
													1 ) { IncludeSpread = true, IncludeRatingModifier = true, IncludeFreeAgents = true };
			r2.Render();
			Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
		}

		[TestMethod]
		public void TestSuggestedGridStatsLineupForWeek1()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Gridstats_NFL1,
												   Constants.KOwnerSteveColonna, "CC",
												   Utility.CurrentSeason(),
												   1 ) 
						{ IncludeSpread = false, IncludeRatingModifier = false};
			r2.Render();
			Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
		}

		[TestMethod]
		public void TestSuggestedYahooLineupForWeek1()
		{
			Utility.ExecuteStep( SuggestedYahooLineupForWeek1, Utility.DoIt );
		}

		private static void SuggestedYahooLineupForWeek1()
		{
			//  including free agents will cause a timeout
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(),
													1 )
													{
														IncludeSpread = true,
														IncludeRatingModifier = true,
														IncludeFreeAgents = true
													};
			r2.Render();
			Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
		}

		[TestMethod]
		public void RunTestYahooRankPoints()
		{
			Utility.ExecuteStep( TestYahooRankPoints, Utility.DoIt );
		}

		[TestMethod]
		public void RunTestCamNewtonAveragePreWeek2()
		{
			Utility.ExecuteStep( TestCamNewtonAveragePreWeek2, Utility.DoIt );
		}

		[TestMethod]
		public void RunTestCamNewtonPointsInWeek1()
		{
			Utility.ExecuteStep( TestCamNewtonPointsInWeek1, Utility.DoIt );
		}

		private static void TestCamNewtonPointsInWeek1()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(),
													2 )
			{
				IncludeSpread = true,
				IncludeRatingModifier = true,
				IncludeFreeAgents = true
			};

			var p = new NFLPlayer( "NEWTCA01" );

			var week =  r2.NflWeek.PreviousWeek(r2.NflWeek,false,false);
			var pts = r2.Scorer.RatePlayer( p, week );
			Assert.AreEqual( 29, pts );
		}

		private static void TestCamNewtonAveragePreWeek2()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(),
													2 )
			{
				IncludeSpread = true,
				IncludeRatingModifier = true,
				IncludeFreeAgents = true
			};

			var p = new NFLPlayer( "NEWTCA01" );

			var pts = r2.AveragePoints( p );
			Assert.AreEqual( string.Format( "{0:0.0}", 9.7 ), string.Format( "{0:0.0}", pts ) );
		}

		[TestMethod]
		public void RunTestYahooRankPointsCamNewtonWeek2()
		{
			Utility.ExecuteStep( TestYahooRankPointsCamNewtonWeek2, Utility.DoIt );
		}

		private static void TestYahooRankPointsCamNewtonWeek2()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(), week:2 ) 
													{ 
														IncludeSpread = true, 
														IncludeRatingModifier = false, 
														IncludeFreeAgents = true };

			var p = new NFLPlayer( "NEWTCA01" );
			var g = new NFLGame( "2011:02-I" );
			var t = new NflTeam( "GB" );

			var pts = r2.RankPoints( p, g, t );
			Assert.AreEqual( string.Format( "{0:0.0}", 7.7 ), string.Format( "{0:0.0}", pts ) );
		}

		[TestMethod]
		public void TestSpreadModifiers()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(),
													2 );
			var m1 = r2.PlayerSpread( 14.0M, isHome:true );
			Assert.AreEqual( 1.4M, m1 );
			var m2 = r2.PlayerSpread( 13.0M, isHome:true );
			Assert.AreEqual( 1.3M, m2 );
			var m3 = r2.PlayerSpread( 9.5M, isHome:true );
			Assert.AreEqual( 1.2M, m3 );
			var m4 = r2.PlayerSpread( 3.0M, isHome:true );
			Assert.AreEqual( 1.1M, m4 );
			var m5 = r2.PlayerSpread( 2.0M, isHome:true );
			Assert.AreEqual( 1.0M, m5 );
			var m6 = r2.PlayerSpread( 14.0M, isHome:false );
			Assert.AreEqual( 0.6M, m6 );
			var m7 = r2.PlayerSpread( 13.0M, isHome:false );
			Assert.AreEqual( 0.7M, m7 );
			var m8 = r2.PlayerSpread( 9.5M, isHome:false );
			Assert.AreEqual( 0.8M, m8 );
			var m9 = r2.PlayerSpread( 3.0M, isHome:false );
			Assert.AreEqual( 0.9M, m9 );
			var m10 = r2.PlayerSpread( 2.0M, isHome:false );
			Assert.AreEqual( 1.0M, m10 );
			var m11 = r2.PlayerSpread( -9.5M, isHome: true );
			Assert.AreEqual( .8M, m11 );
		}

		private static void TestYahooRankPoints()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, "BB",
													Utility.CurrentSeason(),
													1 ) { IncludeSpread = true, IncludeRatingModifier = true, IncludeFreeAgents = true };
			
			var p = new NFLPlayer( "INGRMA02" );
			var g = new NFLGame("2011:01-A");
			var t = new NflTeam( "GB" );

			var pts = r2.RankPoints( p, g, t );
			Assert.AreEqual( -5, pts );
		}

		[TestMethod]
		public void TestYahooRankRatingModifier()
		{
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
													Constants.KOwnerSteveColonna, 
													"BB",
													Utility.CurrentSeason(),
													1 ) 
													{ 
														IncludeSpread = true, 
														IncludeRatingModifier = true, 
														IncludeFreeAgents = true 
													};

			var modifier = r2.RatingModifier( "A" );  //  an A opponent
			Assert.AreEqual( 0.5M, modifier );
		}

		#endregion

		#region Unit Reports

		[TestMethod]
		public void TestAllUnitReports()
		{
			var ur = new UnitReport();
			ur.Render("2010");
			Assert.IsTrue( File.Exists( ur.FileOut ), string.Format( "Cannot find {0}", ur.FileOut ) );
		}

		[TestMethod]
		public void TestNinersProtection()
		{
			var runStorer = new DbfRunStorer();
			Utility.ExecuteStep( NinersProtection, runStorer );
		}

		[TestMethod]
		public void TestTheDtosFunction()
		{
			var theDate = new DateTime( 1959, 11, 8 );
			var dtosDate = Utility.Dtos( theDate );
			Assert.AreEqual( "19591108", dtosDate );
			theDate = new DateTime( 1980, 1, 1 );
			dtosDate = Utility.Dtos( theDate );
			Assert.AreEqual( "19800101", dtosDate );
		}

		[TestMethod]
		public void TestDbfRunStorer()
		{
			var runStorer = new DbfRunStorer();
			var ts = new TimeSpan( 1, 1, 1 );
			runStorer.StoreRun( "Testing 123", ts );
			Assert.IsTrue( true );
		}

		private static void NinersProtection()
		{
			var ur = new UnitReport();
			var team = new NflTeam( "SF" );
			ur.PpSnippet( team );
			Assert.IsTrue( File.Exists( ur.FileOut ), string.Format( "Cannot find {0}", ur.FileOut ) );
		}

		[TestMethod]
		public void TestCreateUnitPerformance()
		{
			var up = new UnitPerformance
			         	{
			         		Season = "2010",
			         		WeekNo = 22,
			         		TeamCode = "SF",
			         		UnitCode = "PO",
			         		Opponent = "vAC",
			         		Leader = "ASmith",
			         		OpponentsLeader = "KRhodes",
			         		OpponentRating = "C",
			         		UnitRating = "D",
			         		Yards = 276,
			         		Touchdowns = 2,
			         		Intercepts = 2,
			         		Sacks = 2
			         	};
			var r = new DbfUnitPerformanceRepository();
			var ok = r.Add( up );
			Assert.IsTrue( ok );
		}

		[TestMethod]
		public void TestReadUnitPerformance()
		{
			//TODO replace this with a real one
			var r = new DbfUnitPerformanceRepository();
			var up = r.GetByKey( "SF", "2010", 22, "PO" );
			Assert.AreEqual( 276, up.Yards ); 
		}

		[TestMethod]
		public void TestDeleteUnitPerformance()
		{
			var r = new DbfUnitPerformanceRepository();
			var up = r.GetByKey( "SF", "2010", 22, "PO" );
			var ok = r.Delete( up );
			var up2 = r.GetByKey( "SF", "2010", 22, "PO" );
			Assert.IsNull( up2.TeamCode );
		}


		#endregion

		#region Yahoo Projections

		[TestMethod]
		public void RunTestYahooProjectionsForQuarterBacks()
		{
			//TODO:  Test this build in projections into weekly GS script
			Utility.ExecuteStep( TestYahooProjectionsForQuarterbacks, Utility.DoIt );
		}

		private static void TestYahooProjectionsForQuarterbacks()
		{
			var yp = new YahooProjector { Lister = { StartersOnly = true } };
			yp.ProjectYahooPerformance( Constants.K_QUARTERBACK_CAT, weekNo: 3, sPos: "QB" );
			Assert.IsTrue( File.Exists( yp.FileOut ), string.Format( "Cannot find {0}", yp.FileOut ) );
		}

		[TestMethod]
		public void RunTestYahooProjectionsForRunningBacks()
		{
			//TODO:  Test this build in projections into weekly GS script
			Utility.ExecuteStep( TestYahooProjectionsForRunningBacks, Utility.DoIt );
		}

		private static void TestYahooProjectionsForRunningBacks()
		{
			var yp = new YahooProjector { Lister = { StartersOnly = true } };
			yp.ProjectYahooPerformance( Constants.K_RUNNINGBACK_CAT, weekNo:3, sPos:"RB" );
			Assert.IsTrue( File.Exists( yp.FileOut ), string.Format( "Cannot find {0}", yp.FileOut ) );
		}

		[TestMethod]
		public void RunTestYahooProjectionsForWideReceivers()
		{
			Utility.ExecuteStep( TestYahooProjectionsForWideouts, Utility.DoIt );
		}

		private static void TestYahooProjectionsForWideouts()
		{
			var yp = new YahooProjector { Lister = { StartersOnly = true } };
			yp.ProjectYahooPerformance( Constants.K_RECEIVER_CAT, weekNo: 15, sPos: "WR" );
			Assert.IsTrue( File.Exists( yp.FileOut ), string.Format( "Cannot find {0}", yp.FileOut ) );
		}

		[TestMethod]
		public void RunTestYahooProjectionsAll()
		{
			//TODO:  this is too big
			Utility.ExecuteStep( TestYahooProjectionsAll, Utility.DoIt );
		}

		private static void TestYahooProjectionsAll()
		{
			var yp = new YahooProjector { Lister = { StartersOnly = true } };
			var week = new NFLWeek( 2011, 3 );
			yp.AllProjections( week );
			Assert.IsTrue( File.Exists( yp.FileOut ), string.Format( "Cannot find {0}", yp.FileOut ) );
		}

		[TestMethod]
		public void TestStatPredictorForChrisJohnson()
		{
			//  johnson was a hold out
			var week = new NFLWeek( 2011, 1 );
			var player = new NFLPlayer("JOHNCH06");
			var sp = new StatProjector(week);
			var points = sp.RatePlayer(player, week);
			Assert.AreEqual( 0, points, string.Format("points should be 0 not {0}", points ) );
		}

		[TestMethod]
		public void TestLastStatsForChrisJohnson()
		{
			var week = new NFLWeek( 2011, 1 );
			var player = new NFLPlayer( "JOHNCH06" );
			var ds = player.LastStats( Constants.K_STATCODE_RUSHING_YARDS, 
				week.PreviousWeek( week, false, true ).WeekNo - 2, week.PreviousWeek( week, false, true ).WeekNo, "2010" );

			var ydr = ds.Tables[0].Rows.Cast<DataRow>()
				.Aggregate(0, (current, dr) => (int) (current + Decimal.Parse(dr["QTY"].ToString())));

			Assert.AreEqual( 227, ydr, string.Format( "ydr should be 227 not {0}", ydr ) );
		}

		[TestMethod]
		public void TestPreviousWeekCalculator()
		{
			var week = new NFLWeek( 2011, 1 );
			var prevWeek = week.PreviousWeek(week,false,false).WeekNo;
			Assert.AreEqual( 21, prevWeek, string.Format( "prev week should be 21 not {0}", prevWeek ) );
		}

		[TestMethod]
		public void TestPreviousRegularWeekCalculator()
		{
			var week = new NFLWeek( 2011, 1 );
			var prevWeek = week.PreviousWeek( week, false, true ).WeekNo;
			Assert.AreEqual( 17, prevWeek, string.Format( "prev week should be 17 not {0}", prevWeek ) );
		}

		[TestMethod]
		public void TestPreviousRegularWeekCalculatorMidSeason()
		{
			var week = new NFLWeek( 2010, 7 );
			var prevWeek = week.PreviousWeek( week, false, true ).WeekNo;
			Assert.AreEqual( 6, prevWeek, string.Format( "prev week should be 6 not {0}", prevWeek ) );
		}

		#endregion

		#region  Roster Summary

		[TestMethod]
		public void TestGridStatsRosterSummary()
		{
			//  Create a GridStats League
			var gridStatsLeague = new GridStatsLeague( "GS1 - Great Britain", Constants.K_LEAGUE_Gridstats_NFL1, 
				"2012", weekNo:2 );
			//  Tell it to produce the Roster Report
			gridStatsLeague.RosterReport();
			Assert.IsTrue( File.Exists( gridStatsLeague.FileOut ), string.Format( "Cannot find {0}", 
				gridStatsLeague.FileOut ) );
		}

		[TestMethod]
		public void TestGridStatsRosterSummaryDumpFileLocation()
		{
			var gridStatsLeague = new GridStatsLeague( "GS1 - Great Britain", Constants.K_LEAGUE_Gridstats_NFL1, "2011", 1 );
			gridStatsLeague.LoadTeams();
			var t = gridStatsLeague.GetTeam( "O001" );
			Assert.AreEqual( "teamdumps\\teamdump-O001-01.htm", t.ShortFileName( 2011, 1 ) );
		}

		#endregion

		#region  Performance Reports

		#region  Yahoo Performance


		#endregion

		#region  GridStats

		[TestMethod]
		public void TestGsPerformanceReportQuarterBacksWeek12()
		{
			var pr = new PerformanceReport( season: "2012", week: 12 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 12, loadGames: false );
			var gs = new GS4Scorer( week ) { ScoresOnly = true, Master = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" ) };
			pr.Scorer = gs;
			pr.Render( catCode: "1", sPos: "QB", leagueId: Constants.K_LEAGUE_Gridstats_NFL1, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestGs4ScorerAccesstoGridStatsXml()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			var pts = m.GetStat( "2012:07:GRIFRO02" );
			Assert.AreEqual( 3, pts, "RGIII got 3 pts in week 7 of 2012" );
		}

		#endregion


		#endregion

		#region  Processing

		[TestMethod]
		public void TestProcessingScores()
		{
			var ds = Utility.TflWs.ScoresDs( "2011", "01", "A");
			Assert.IsTrue( ds.Tables[ 0 ].Rows.Count > 0 );
		}


		#endregion

		#region  Score Report

		//TODO:  need this for more than just one week

		[TestMethod]
		public void TestScoreReport()
		{
			var sr = new ScoresReport {Season = "2011", Week = "01"};
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestSeason2012ScoreReport()
		{
			var sr = new ScoresReport { Season = "2012" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestSeason2011ScoreReport()
		{
			var sr = new ScoresReport { Season = "2011" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestSeason2010ScoreReport()
		{
			var sr = new ScoresReport { Season = "2010" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestScoreReportNinersReturns()
		{
			var sr = new ScoresReport {Season = "2011", Week = "01"};
			sr.Render();
			var t = sr.TeamList[ "SF" ];
			Assert.AreEqual( 1, t.TotTDt );
			Assert.AreEqual( 1, t.TotTDk );
		}

		[TestMethod]
		public void TestSeasonGoallineReport()
		{
			var sr = new GoallineReport { Season = "2008" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestSeason2011GoallineReport()
		{
			var sr = new GoallineReport { Season = "2011" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		[TestMethod]
		public void TestSeason2010GoallineReport()
		{
			var sr = new GoallineReport { Season = "2010" };
			sr.Render();
			Assert.IsTrue( File.Exists( sr.FileOut ), string.Format( "Cannot find {0}", sr.FileOut ) );
		}

		#endregion

		#region  Unit Predictions Season Projection

		[TestMethod]
		public void TestSeasonProjectionTimeTravel()
		{
			var game = new NFLGame( "2011:01-M" );  //  NG @ WR - Game key is YYYY:0W-X
			var prognosticator = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = false,
				StorePrediction = false,
				RatingsService = new UnitRatingsService()
			};
			var persistor = new FakePredictionStorer();
			var result = prognosticator.PredictGame( game, persistor, new DateTime( 2011,9,16 ) );
			Assert.AreEqual( 14, result.AwayScore );
			Assert.AreEqual( 28, result.HomeScore );
		}

		[TestMethod]
		public void TestUnitPredictionAfIc()
		{
			var game = new NFLGame( "2011:09-D" );  //  AF @ IC - Game key is YYYY:0W-X
			var prognosticator = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = false,
				StorePrediction = false,
				RatingsService = new UnitRatingsService()
			};
			prognosticator.RatingsService.ThisSeasonOnly = true;
			var persistor = new FakePredictionStorer();
			var result = prognosticator.PredictGame( game, persistor, new DateTime( 2011, 11, 3 ) );
			Assert.AreEqual( 41, result.AwayScore );
			Assert.AreEqual( 23, result.HomeScore );
		}

		[TestMethod]
		public void TestRatingsServiceCurrencyCheck()
		{
			var rs = new UnitRatingsService();
			var team = new NflTeam("SF");
			Assert.IsTrue( rs.IsCurrent( team, DateTime.Now ) );
		}

		[TestMethod]
		public void TestRatingsServiceCurrencyCheck2()
		{
			var rs = new UnitRatingsService();
			var team = new NflTeam( "SF" );
			Assert.IsFalse( rs.IsCurrent( team, new DateTime( 1959, 11,8 ) ) );
		}

		#endregion

		#region Miller Predictor

		[TestMethod]
		public void TestMillerPredictor()
		{
			var mp = new MillerPredictor { AuditTrail = true };
			var game = new NFLGame( "2011:06-A" );  //  CP @ AF
			var result = mp.PredictGame( game, new FakePredictionStorer(), game.GameDate );
			Assert.IsTrue( result.HomeWin() );
			Assert.IsTrue( result.HomeScore.Equals( 32 ), string.Format( "Home score should be 32 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 31 ), string.Format( "Away score should be 31 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestMillerPredictorOldData()
		{
			var mp = new MillerPredictor { AuditTrail = true };
			var game = new NFLGame( "2010:08-L" );  //  PS @ NO
			var result = mp.PredictGame( game, new FakePredictionStorer(), game.GameDate );
			Assert.IsTrue( result.HomeScore.Equals( 32 ), string.Format( "Home score should be 32 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 31 ), string.Format( "Away score should be 31 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestMillerPredictorOldData2()
		{
			var mp = new MillerPredictor { AuditTrail = true };
			var game = new NFLGame( "2010:15-M" );  //  ?? @ ??
			var result = mp.PredictGame( game, new FakePredictionStorer(), game.GameDate );
			Assert.IsTrue( result.HomeScore.Equals( 19 ), string.Format( "Home score should be 19 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 4 ), string.Format( "Away score should be 4 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestMillerSaintsOffensiveRating()
		{
			var team = new NflTeam( "NO" );
			var nOff = Utility.OffRating( team, new DateTime( 2010, 10, 31 ) );
			Assert.AreEqual( 18, nOff );
		}

		[TestMethod]
		public void TestMillerOffensiveRating()
		{
			var team = new NflTeam( "AF" );
			var nOff = Utility.OffRating( team, new DateTime( 2011, 10, 14 ) );
			Assert.AreEqual( 22, nOff );
		}

		[TestMethod]
		public void TestMillerDefensiveRating()
		{
			var team = new NflTeam( "AF" );
			var nDef = Utility.DefRating( team, new DateTime( 2011, 10, 14 ) );
			Assert.AreEqual( 26, nDef );
		}

		[TestMethod]
		public void TestMillerPanthersOffensiveRating()
		{
			var team = new NflTeam( "CP" );
			var nOff = Utility.OffRating( team, new DateTime( 2011, 10, 14 ) );
			Assert.AreEqual( 25, nOff );
		}

		[TestMethod]
		public void TestMillerPanthersDefensiveRating()
		{
			var team = new NflTeam( "CP" );
			var nDef = Utility.DefRating( team, new DateTime( 2011, 10, 14 ) );
			Assert.AreEqual( 30, nDef );
		}

		[TestMethod]
		public void TestLastFourGames()
		{
			var team = new NflTeam( "SF" );
			team.LoadPreviousRegularSeasonGames( 4, new DateTime( 2011, 10, 14 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var g1 = team.GameList[ 0 ] as NFLGame;
			Assert.IsTrue( g1.AwayNflTeam.TeamCode.Equals( "TB" ) );
		}

		[TestMethod]
		public void TestLastFourGamesSteelers()
		{
			var team = new NflTeam( "PS" );
			team.LoadPreviousRegularSeasonGames( 4, new DateTime( 2010, 12, 23 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
		}

		[TestMethod]
		public void TestLastFourGamesOverSeason()
		{
			var team = new NflTeam( "SF" );
			team.LoadPreviousRegularSeasonGames( 4, new DateTime( 2011, 9, 1 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var g1 = team.GameList[ 0 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g1.AwayNflTeam.TeamCode, "AC,SL,SD,SS" ) || Utility.Contains( g1.HomeNflTeam.TeamCode, "AC,SL,SD,SS" ) );
			var g2 = team.GameList[ 1 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g2.AwayNflTeam.TeamCode, "AC,SL,SD,SS" ) || Utility.Contains( g2.HomeNflTeam.TeamCode, "AC,SL,SD,SS" ) );
			var g3 = team.GameList[ 2 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g3.AwayNflTeam.TeamCode, "AC,SL,SD,SS" ) || Utility.Contains( g3.HomeNflTeam.TeamCode, "AC,SL,SD,SS" ) );
			var g4 = team.GameList[ 3 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g4.AwayNflTeam.TeamCode, "AC,SL,SD,SS" ) || Utility.Contains( g4.HomeNflTeam.TeamCode, "AC,SL,SD,SS" ) );
		}

		[TestMethod]
		public void TestLastFourRegularGamesOverSeason()
		{
			const string K_LastFourOpponents = "CH,NG,NE,DL";
			var team = new NflTeam( "GB" );
			team.LoadPreviousRegularSeasonGames( 4, new DateTime( 2011, 9, 1 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var g1 = team.GameList[ 0 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g1.AwayNflTeam.TeamCode, K_LastFourOpponents ) || Utility.Contains( g1.HomeNflTeam.TeamCode, K_LastFourOpponents ) );
			var g2 = team.GameList[ 1 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g2.AwayNflTeam.TeamCode, K_LastFourOpponents ) || Utility.Contains( g2.HomeNflTeam.TeamCode, K_LastFourOpponents ) );
			var g3 = team.GameList[ 2 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g3.AwayNflTeam.TeamCode, K_LastFourOpponents ) || Utility.Contains( g3.HomeNflTeam.TeamCode, K_LastFourOpponents ) );
			var g4 = team.GameList[ 3 ] as NFLGame;
			Assert.IsTrue( Utility.Contains( g4.AwayNflTeam.TeamCode, K_LastFourOpponents ) || Utility.Contains( g4.HomeNflTeam.TeamCode, K_LastFourOpponents ) );
		}

		[TestMethod]
		public void TestHiScore()
		{
			var team = new NflTeam( "SF" );
			team.LoadPreviousGames( 4, new DateTime( 2011, 10, 14 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var hiScore = Utility.HiScore( team.GameList, team.TeamCode );
			Assert.AreEqual( 48, hiScore );
		}

		[TestMethod]
		public void TestLoScore()
		{
			var team = new NflTeam( "SF" );
			team.LoadPreviousGames( 4, new DateTime( 2011, 10, 14 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var loScore = Utility.LoScore( team.GameList, team.TeamCode );
			Assert.AreEqual( 13, loScore );
		}

		[TestMethod]
		public void TestHiAgainst()
		{
			var team = new NflTeam( "AF" );
			team.LoadPreviousGames( 4, new DateTime( 2011, 10, 14 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var hiAgainst = Utility.HiAgainst( team.GameList, team.TeamCode );
			Assert.AreEqual( 31, hiAgainst );
		}

		[TestMethod]
		public void TestLoAgainst()
		{
			var team = new NflTeam( "AF" );
			team.LoadPreviousGames( 4, new DateTime( 2011, 10, 14 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 4 ) );
			var loAgainst = Utility.LoAgainst( team.GameList, team.TeamCode );
			Assert.AreEqual( 16, loAgainst );
		}

		[TestMethod]
		public void TestSeasonGameList()
		{
			var season = new NflSeason( "2010" );
			//  256 regular season (16x16) + 11 playoffs
			Assert.AreEqual( 267, season.GameList.Count );
		}

		#endregion

		#region  ATS

		[TestMethod]
		public void TestStraightUpPrediction()
		{
			var prediction = new Prediction( "Test", "2011", "01", "F", 9, 20 );
			var result = prediction.CheckResult();
			//   tipped an away win PE@SL which was correct
			Assert.IsTrue( result == TipResult.Correct );
		}

		[TestMethod]
		public void TestAtsPrediction()
		{
			var prediction = new Prediction( "Test", "2011", "01", "F", 9, 20 );
			var result = prediction.CheckResultAts();
			//   tipped an away win PE@SL -11 and spread was -4.5 and result was -18
			Assert.IsTrue( result == TipResult.Correct );
		}

		[TestMethod]
		public void TestSpreadScore()
		{
			var game = new NFLGame( Utility.GameKey( "2011", "01", "F" ) );
			Assert.AreEqual( -13.5, game.SpreadScore() );
		}

		[TestMethod]
		public void TestSpreadWinner()
		{
			var game = new NFLGame( Utility.GameKey( "2011", "01", "F" ) );
			Assert.AreEqual( "Win", game.ResultvsSpread("PE") );
		}

		[TestMethod]
		public void TestSpreadLoser()
		{
			var game = new NFLGame( Utility.GameKey( "2011", "01", "F" ) );
			Assert.AreEqual( "Loss", game.ResultvsSpread( "SL" ) );
		}

		#endregion

		#region  Configuration

		[TestMethod]
		public void TestNflConnectionStringCount()
		{
			ConnectionStringSettingsCollection connections =
				 ConfigurationManager.ConnectionStrings;
			Assert.AreEqual( 1, connections.Count );
		}

		[TestMethod]
		public void TestNflConnectionString()
		{
			var connStr = Utility.NflConnectionString();
			Assert.AreEqual( "Connection Name", connStr );
		}

		[TestMethod]
		public void TestConfigMasterNflConnectionString()
		{
			var connStr = Config.NflConnectionString();
			Assert.AreEqual( "Connection Name", connStr );
		}

		[TestMethod]
		public void TestConfigOutputDirectory()
		{
			var outputDir = Utility.OutputDirectory();
			Assert.IsTrue( outputDir.Length > 0 );
		}
		#endregion

		#region DataLoader

		[TestMethod]
		public void TestDataLoader()
		{
			var dl = new DataLoader();

			ITflDataService ds = new TflDataService();
		}

		#endregion

	}
}