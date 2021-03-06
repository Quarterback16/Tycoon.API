﻿using GameLogService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterService;
using System;
using System.Collections.Generic;

namespace GameLog.Tests
{
    [TestClass]
    public class GameLogServiceTests
    {
        private GameStatsRepository _sut;
        const string K_CurrentSeason = "1987";
        private int[] weeks;
		public static List<string> Positions { get; private set; }
		public Dictionary<string,int> Wages { get; set; }

		[TestInitialize]
        public void Initialise()
		{
            _sut = new GameStatsRepository();
            Positions = new List<string>{ "QB", "RB", "WR", "TE", "KK"};
            //weeks = new int[] { 1, 2, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            weeks = new int[] { 1, 2, 7, 8, 9, 10 };
            //weeks = new int[] { 3,4,5,8 };
            //weeks = new int[] { 11,12,14,15 };

            Wages = new Dictionary<string, int>
			{
				{ "Dan Marino", 13 },
                { "Herschel Walker",  5 },
                { "Ruben Mayes",  2 },
                { "Stephone Paige",  4 },
                { "Tony Franklin",  4 },
                { "Mark Duper",  3 },
            };
		}

        [TestMethod]
        public void GameStatsRepository_ForEddieLee_ReturnsStatsIinCorrectWeeks()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Eddie Lee Ivery"
            };

            Console.WriteLine(
                _sut.PlayerLogUrl(
                    playerModel));

            var result = _sut.GetGameStats(
                model: playerModel);

            // week 12 he had 3TDs
            Assert.AreEqual(
                3, 
                result[11].RushingTds);

            _sut.SendToConsole(
                playerModel);
            _sut.SendLineToConsole(
                playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_ComparePlayers_Returns16rows()
		{
			PlayerLine(
                "Joe Montana");
            PlayerLine(
                "Dan Marino");
        }


		[TestMethod]
        public void GameStatsRepository_ForPlayer_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1986",
                PlayerName = "Lorenzo Hampton",
                Position = "RB"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_ForGaryAnderson_RB_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1986",
                PlayerName = "Gary Anderson",
                Position = "RB"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepoForEntireTeamEntireYear()
        {
            var fantasyTeam = "CD";
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var roster = rosterService.GetRoster(
                fantasyTeam);
            var teamList = new List<(string, string)>();
            AddRosterInOrder(
                roster, 
                teamList);
            foreach (var item in teamList)
            {
                var playerModel = new PlayerReportModel
                {
                    Season = "1985",
                    PlayerName = item.Item1
                };

                if (item.Item2.Equals("KK"))
                {
                    _sut.GetKickerStats(
                        model: playerModel);
                    _sut.SendKickerLineToConsole(
                        playerModel);
                }
                else
                {
                    _sut.GetGameStats(
                        model: playerModel);
                    _sut.SendLineToConsole(
                        playerModel);
                }
            }
        }

        [TestMethod]
        public void GameStatsRepoForEntireTeam()
		{
			var fantasyTeam = "CD";
			var rosterService = new RetroRosters(
				new RosterEventStore());
			var roster = rosterService.GetRoster(
				fantasyTeam);
			var teamList = new List<(string, string)>();
			AddRosterInOrder(
                roster, 
                teamList);

            foreach (var item in teamList)
			{
				var playerModel = new PlayerReportModel
				{
					Season = K_CurrentSeason,
					PlayerName = item.Item1
				};
                if (playerModel.PlayerName.Equals("Gary Anderson"))
                    playerModel.Position = "RB";

				if (item.Item2.Equals("KK"))
				{
					_sut.GetKickerStats(
						model: playerModel);
					_sut.SendKickerToConsole(
						playerModel,
						weeks);
				}
				else
				{
					_sut.GetGameStats(
						model: playerModel);
					_sut.SendToConsole(
						playerModel,
						weeks);
				}
			}
		}

        [TestMethod]
        public void GameStatsRepoForSinglePlayer()
        {
            var fantasyTeam = "CD";
            var singlePlayer = "Jeff Kemp";
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var roster = rosterService.GetRoster(
                fantasyTeam);
            var teamList = new List<(string, string)>();
            AddRosterInOrder(
                roster,
                teamList);

            foreach (var item in teamList)
            {
                if (!item.Item1.Equals(singlePlayer))
                    continue;
                var playerModel = new PlayerReportModel
                {
                    Season = K_CurrentSeason,
                    PlayerName = item.Item1
                };
                if (playerModel.PlayerName.Equals("Gary Anderson"))
                    playerModel.Position = "RB";

                if (item.Item2.Equals("KK"))
                {
                    _sut.GetKickerStats(
                        model: playerModel);
                    _sut.SendKickerToConsole(
                        playerModel,
                        weeks);
                }
                else
                {
                    _sut.GetGameStats(
                        model: playerModel);
                    _sut.SendToConsole(
                        playerModel,
                        weeks);
                }
            }
        }

        [TestMethod]
        public void SeasonRosterForEntireTeam()
		{
			var teamList = BuildTeamList("CD");
			var seasonRoster = new SeasonRoster();
			foreach (var item in teamList)
			{
				var playerModel = new PlayerReportModel(
					K_CurrentSeason,
					playerName: item.Item1,
					item.Item2);

				playerModel.GameLog = _sut.GetStats(
					playerModel);

				seasonRoster.AddLog(
					playerModel);
			}
			foreach (var w in weeks)
			{
				Console.WriteLine(
					"Week {0,2} {1}",
					w,
					seasonRoster.Points(w));
				Console.WriteLine();
				Console.WriteLine(
					seasonRoster.Lineup(w));
			}
			DisplayContributions(
                seasonRoster);
			DisplayTotalPoints(
				weeks,
				seasonRoster);
			DisplayNonContributors(
				teamList,
				seasonRoster);
		}

		private void DisplayContributions(
            SeasonRoster seasonRoster)
		{
			Console.WriteLine();
			Console.WriteLine(
				seasonRoster.ContributionsOut(Wages));
		}

		private static void DisplayNonContributors(
            List<(string, string)> teamList, 
            SeasonRoster seasonRoster)
		{
			Console.WriteLine("Non-Contributors");
			foreach (var player in teamList)
			{
				if (seasonRoster.ContributionOf(player.Item1) == 0)
					Console.WriteLine($"   {player.Item1}");
			}
		}

		[TestMethod]
        public void SeasonRosterBattleVsDuckHunters()
		{
			var teamList = BuildTeamList("DD");
			var seasonRosterHunters = new SeasonRoster();
			foreach (var item in teamList)
			{
				var playerModel = new PlayerReportModel(
					K_CurrentSeason,
					playerName: item.Item1,
					item.Item2);

				playerModel.GameLog = _sut.GetStats(
					playerModel);

				seasonRosterHunters.AddLog(
					playerModel);
			}
			foreach (var w in weeks)
			{
				Console.WriteLine(
					"Week {0,2} {1}",
					w,
					seasonRosterHunters.Points(w));
				Console.WriteLine();
			}
			DisplayTotalPoints(
				weeks,
				seasonRosterHunters);

			var teamList2 = BuildTeamList("CD");
			var seasonRosterDeloreans = new SeasonRoster();
			foreach (var item in teamList2)
			{
				var playerModel = new PlayerReportModel(
					K_CurrentSeason,
					playerName: item.Item1,
					item.Item2);

				playerModel.GameLog = _sut.GetStats(
					playerModel);

				seasonRosterDeloreans.AddLog(
					playerModel);
			}
			foreach (var w in weeks)
			{
				Console.WriteLine(
					"Week {0,2} {1}",
					w,
					seasonRosterDeloreans.Points(w));
				Console.WriteLine();
			}
			DisplayTotalPoints(
				weeks,
				seasonRosterDeloreans);
			HeadToHead(
                weeks, 
                seasonRosterHunters, 
                seasonRosterDeloreans);
		}

		private void HeadToHead(
            int[] weeks, 
            SeasonRoster seasonRosterHunters, 
            SeasonRoster seasonRosterDeloreans)
		{
            var record = new Record("Deloreans 1986");
			foreach (var w in weeks)
			{
				var myScore = seasonRosterDeloreans.Score(w);
				var hisScore = seasonRosterHunters.Score(w);
                record.Update(
                    myScore, 
                    hisScore);
				Console.WriteLine(
					"Week {0,2} Deloreans {1} DuckHunters {2} {3}",
					w,
					myScore,
					hisScore,
					ResultOut(
						myScore,
						hisScore));
			}
			Console.WriteLine();
			Console.WriteLine(record);
		}

		private static void DisplayTotalPoints(
            int[] weeks, 
            SeasonRoster seasonRoster)
		{
			Console.WriteLine();
			Console.WriteLine(
				$"Total Pts: {seasonRoster.TotalPoints}");
			Console.WriteLine(
				$"  Avg Pts: {seasonRoster.TotalPoints / weeks.Length}");
			Console.WriteLine();
		}

		private string ResultOut(
            int myScore, 
            int hisScore)
		{
            if (myScore > hisScore)
                return "WIN";
            if (myScore == hisScore)
                return "TIE";
            return "LOSS";
		}

		private static List<(string, string)> BuildTeamList(
            string fantasyTeam)
		{
			var rosterService = new RetroRosters(
				new RosterEventStore());
			var roster = rosterService.GetRoster(
				fantasyTeam);
			var teamList = new List<(string, string)>();
			AddRosterInOrder(
				roster,
				teamList);
			return teamList;
		}

		[TestMethod]
        public void SeasonRosterForEntireTeamMinusOne()
        {
            var fantasyTeam = "CD";
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var roster = rosterService.GetRoster(
                fantasyTeam);
            var teamList = new List<(string, string)>();
            AddRosterInOrder(
                roster,
                teamList,
                skipPlayer:"Dorset");

            var seasonRoster = new SeasonRoster();
            foreach (var item in teamList)
            {
                var playerModel = new PlayerReportModel(
                    K_CurrentSeason,
                    playerName: item.Item1,
                    item.Item2);

                playerModel.GameLog = _sut.GetStats(
                    playerModel);

                seasonRoster.AddLog(
                    playerModel);
            }
            foreach (var w in weeks)
            {
                Console.WriteLine(
                    "Week {0,2} {1}",
                    w,
                    seasonRoster.Points(w));
                Console.WriteLine();
                Console.WriteLine(
                    seasonRoster.Lineup(w));
            }
            Console.WriteLine();
            Console.WriteLine(
                seasonRoster.ContributionsOut(Wages));
            Console.WriteLine();
            Console.WriteLine(
                $"Total Pts: {seasonRoster.TotalPoints}");
            Console.WriteLine(
                $"  Avg Pts: {seasonRoster.TotalPoints / weeks.Length}");
        }

        [TestMethod]
        public void GameStatsRepository_ForKicker_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = K_CurrentSeason,
                PlayerName = "Tony Franklin",
                Position = "K"
            };

            var result = _sut.GetKickerStats(
                model: playerModel);

            _sut.SendKickerToConsole(
                playerModel);

            Assert.AreEqual(16, result.Count);
        }

        #region  Player code tests
        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1984",
                playerName: "Joe Montana");

            Console.WriteLine(result);
            Assert.AreEqual(
                "MontJo01",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForGaryAnderson_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1986",
                playerName: "Gary Anderson",
                position: "RB");

            Console.WriteLine(result);
            Assert.AreEqual(
                "AndeGa00",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForFaudReveiz_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1985",
                playerName: "Fuad Reveiz");

            Console.WriteLine(result);
            Assert.AreEqual(
                "reveifua01",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForTheotisBrown_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1985",
                playerName: "Theotis Brown");

            Console.WriteLine(result);
            Assert.AreEqual(
                "BrowTh00",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForBobbyJohnson_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1986",
                playerName: "Bobby Johnson");

            Console.WriteLine(result);
            Assert.AreEqual(
                "JohnBo20",
                result);
        }

        #endregion

        #region  Unit Tests

        [TestMethod]
        public void GameStatsRepository_ForSurnameM_KnowsListUrl()
        {
            var result = _sut.PlayerListUrl(
                letter: "M");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsListUrl()
        {
            var result = _sut.PlayerListUrl(
                season: "1985",
                playerName: "Joe Montana");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsGameLogUrl()
        {
            var result = _sut.PlayerLogUrl(
                season: "1984",
                playerName: "Joe Montana");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/MontJo01/gamelog/1984/",
                result);
        }

        [TestMethod]
        public void GameStatsRepo_WithPlayerNameMontana_ReturnsM()
        {
            var result = _sut.FirstLetterOfSurname(
                "Joe Montana");

            Assert.AreEqual("M", result);
        }

        [TestMethod]
        public void GameStatsRepository_ForARetiredPlayer_ReturnsEmpty()
        {
            var result = _sut.PlayerCode(
                season: "1984",
                playerName: "Brian Sipe");

            Console.WriteLine(result);
            Assert.AreEqual(
                "",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_WithLinkHtml_KnowsPlayerCode()
        {
            //<p><a href="/players/M/MontJo01.htm">Joe Montana</a>+ (QB) 1979-1994</p>
            var result = _sut.PlayerCodeFrom(
                href: @"<p><a href=""/players/M/MontJo01.htm"">Joe Montana</a>+ (QB) 1979-1994</p>");

            Console.WriteLine(result);
            Assert.AreEqual(
                "MontJo01",
                result);
        }

        #endregion

        #region  Google Sheets
        [TestMethod]
        public void GameStatsRepository_KnowsOwnerOf()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            WriteLabel(rosterService, ("Dan Marino","MD"));
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesTdp_ForQuarterbacks()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var qbList = LoadQuarterbacksFrom1987();
			foreach (var item in qbList)
			{
                WriteLabel(rosterService, item);
                WriteTdpStats(item,K_CurrentSeason);
                Console.WriteLine();
			}
		}

        [TestMethod]
        public void GameStatsRepository_GeneratesTdc_ForReceivers()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var recList = LoadReceiversFrom1987();
            foreach (var item in recList)
            {
                WriteLabel(rosterService, item);
                WriteTdcStats(
					item: item,
					season: K_CurrentSeason);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesTdr_ForQuarterbacks()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var qbList = LoadQuarterbacksFrom1987();
            foreach (var item in qbList)
			{
				WriteLabel(rosterService, item);
				WriteTdrStats(item);
				Console.WriteLine();
			}
		}

        [TestMethod]
        public void GameStatsRepository_GeneratesTdr_ForRunningbacks()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var qbList = LoadRunningbacksFrom1987();
            foreach (var item in qbList)
            {
                WriteLabel(rosterService, item);
                WriteTdrStats(
                    item,
                    K_CurrentSeason);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesKickingPts_ForKickers()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var kList = LoadKickersFrom1987();
            foreach (var item in kList)
            {
                WriteLabel(
                    rosterService, 
                    item);
                WriteKickStats(
                    item,
                    K_CurrentSeason);
                Console.WriteLine();
            }
        }
        #endregion

        #region Privates

        private List<GameStats> PlayerLine(
            string playerName,
            string season = "1985")
        {
            var playerModel = new PlayerReportModel
            {
                Season = season,
                PlayerName = playerName
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendLineToConsole(playerModel);
            Assert.AreEqual(16, result.Count);
            return result;
        }

        private static void AddRosterInOrder(
            List<string> roster,
            List<(string, string)> teamList,
            string skipPlayer = "")
        {
			foreach (var pos in Positions)
			{
                AddPartial(
                    pos,
                    roster,
                    teamList,
                    skipPlayer);
            }       
        }

        private static void AddPartial(
            string desiredPos,
            List<string> roster,
            List<(string, string)> teamList,
            string skipPlayer = "")
        {
            foreach (var player in roster)
            {
                var thePlayer = player.Substring(7, 20)
                    .Trim();
                if (!string.IsNullOrEmpty(skipPlayer))
				{
                    if (thePlayer.Contains(skipPlayer))
                        continue;
				}
                var thePos = player.Substring(5, 2);
                if (thePos.Equals(desiredPos))
                    teamList.Add((thePlayer, thePos));
            }
        }
        private void WriteKickStats(
            (string, string) item,
            string season = "1984")
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = season
            };
            var stats = _sut.GetKickerStats(
                model);
            WriteSeasonKicking(
                stats);
        }

        private void WriteTdcStats(
            (string, string) item,
            string season = "1984")
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = season
            };
            var stats = _sut.GetGameStats(
                model);
            WriteSeasonTdc(stats);
        }

        private void WriteTdrStats(
            (string, string) item,
            string season = "1984")
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = season
            };
            var stats = _sut.GetGameStats(
                model);
            WriteSeasonTdr(stats);
        }

        private void WriteTdpStats(
            (string, string) item,
            string season = "1984")
		{
			var model = new PlayerReportModel
			{
				PlayerName = item.Item1,
				Season = season
			};
			var stats = _sut.GetGameStats(
				model);
			WriteSeasonTdp(stats);
		}

		public static void WriteLabel(
            RetroRosters rosterService,
            (string, string) item)
		{
            var playerName = item.Item1.Trim();
            var id = rosterService.GetIdOf(
                playerName);
            WriteColumn(id.ToString(), 3);
            WriteColumn(playerName, 20);
			WriteColumn(item.Item2, 3);
			var fantasyTeam = rosterService.GetOwnerOf(
                playerName,
				"**");
            var price = rosterService.GetPriceOf(
                playerName);
            WriteColumn($"{price,4}", 4);
            WriteColumn(fantasyTeam, 2);
		}

        private void WriteSeasonKicking(
            List<GameStats> stats,
            string delimiter = " ")
        {
            for (int w = 1; w < 17; w++)
            {
                var kPtsForTheWeek = 0;
                foreach (var game in stats)
                {
                    if (game.Week.Equals(w))
                    {
                        kPtsForTheWeek = game.KickingPoints();
                        break;
                    }
                }
                Console.Write(
                    $"{kPtsForTheWeek}{delimiter}");
            }
        }

        private void WriteSeasonTdc(
            List<GameStats> stats,
            string delimiter = " ")
        {
            for (int w = 1; w < 17; w++)
            {
                var tdcForTheWeek = 0;
                foreach (var game in stats)
                {
                    if (game.Week.Equals(w))
                    {
                        tdcForTheWeek = game.ReceivingTds;
                        break;
                    }
                }
                Console.Write(
                    $"{tdcForTheWeek}{delimiter}");
            }
        }

        private void WriteSeasonTdr(
            List<GameStats> stats,
            string delimiter = " ")
        {
            for (int w = 1; w < 17; w++)
            {
                var tdrForTheWeek = 0;
                foreach (var game in stats)
                {
                    if (game.Week.Equals(w))
                    {
                        tdrForTheWeek = game.RushingTds;
                        break;
                    }
                }
                Console.Write(
                    $"{tdrForTheWeek}{delimiter}");
            }
        }

        private void WriteSeasonTdp(
            List<GameStats> stats,
            string delimiter = " ")
		{
			for (int w = 1; w < 17; w++)
			{
                var tdpForTheWeek = 0;
				foreach (var game in stats)
				{
                    if (game.Week.Equals(w))
					{
                        tdpForTheWeek = game.PassingTds;
                        break;
					}
				}
                Console.Write(
                    $"{tdpForTheWeek}{delimiter}");
            }
		}

		private static void WriteColumn(
            string part,
            int colLength)
		{
            var delimiter = " ";
            part += new string(
				c: ' ',
				count: colLength);
            Console.Write($"{part.Substring(0,colLength)}{delimiter}");
		}

		private List<(string, string)> LoadQuarterbacksFrom1984()
		{
			var qbList = new List<(string, string)>
			{
                ("Dan Marino", "MD"),
                ("Dave Krieg", "SS"),
                ("Neil Lomax", "SL"),
				("Joe Montana", "SF"),
                ("Lynn Dickey", "GB"),
                ("Joe Theismann", "WR"),
                ("Tony Eason", "NG"),
                ("Phil Simms", "NG"),
                ("Steve DeBerg", "TB"),
                ("Dan Fouts", "SD"),
                ("John Elway", "DB"),
                ("Gary Danielson", "DL"),
                ("Ron Jaworski", "PE"),
                ("Mark Malone", "PS"),
                ("Bill Kenney", "KC"),
                ("Marc Wilson", "OR"),
                ("Paul McDonald", "CL"),
                ("Pat Ryan", "NJ"),
                ("Jeff Kemp", "LA"),
                ("Joe Ferguson", "BB"),
                ("Warren Moon", "HO"),
                ("Steve Bartkowski", "AF"),
                ("Richard Todd", "NG"),
                ("Danny White", "NG"),
                ("Ken Anderson", "CI"),
                ("Tommy Kramer", "MV"),
                ("Jim McMahon", "CH"),
                ("Mike Pagel", "IC"),
                ("David Woodley", "PS"),
                ("Gary Hogeboom", "DC"),
                ("Dave Wilson", "NO"),
                ("Todd Blackledge", "KC"),
                ("Ken O'Brien", "NG"),
                ("Jim Plunkett", "LR"),
                ("Ed Luther", "SD"),
                ("Wade Wilson", "MV"),
                ("Matt Cavanaugh", "SF"),
                ("Joe Dufek", "BB"),
                ("Gary Kubiak", "DB"),
                ("Turk Schonert", "CI"),
                ("Rich Campbell", "GB"),
                ("Boomer Esiason", "CI"),
                ("Steve Fuller", "CH"),
                ("Steve Grogan", "NE"),
                ("Joe Pisarcik", "PE"),
                ("Art Schichter", "CL"),
            };
			return qbList;
        }

        private List<(string, string)> LoadQuarterbacksFrom1985()
        {
            var qbList = new List<(string, string)>
            {
                ("Dan Marino", "MD"),
                ("Dave Krieg", "SS"),
                ("Joe Montana", "SF"),
                ("Boomer Esiason", "CI"),
                ("Dan Fouts", "SD"),
                ("Ken O'Brien", "NG"),
                ("John Elway", "DB"),
                ("Phil Simms", "NG"),
                ("Danny White", "NG"),
                ("Tommy Kramer", "MV"),
                ("Steve DeBerg", "TB"),
                ("Neil Lomax", "SL"),
                ("Ron Jaworski", "PE"),
                ("Eric Hipple", "DL"),
                ("Bill Kenney", "KC"),
                ("Marc Wilson", "OR"),
                ("Dieter Brock", "LR"),
                ("Warren Moon", "HO"),
                ("Lynn Dickey", "GB"),
                ("Jim McMahon", "CH"),
                ("Mike Pagel", "IC"),
                ("Mark Malone", "PS"),
                ("Tony Eason", "NG"),
                ("Dave Wilson", "NO"),
                ("Mark Herrmann", "SD"),
                ("Joe Theismann", "WR"),
                ("Bernie Kosar", "CL"),
                ("Gary Danielson", "DL"),
                ("David Archer", "AF"),
                ("Steve Grogan", "NE"),
                ("David Woodley", "PS"),
                ("Todd Blackledge", "KC"),
                ("Vince Ferragamo", "BB"),
                ("Jay Schroeder", "WR"),
                ("Bobby Hebert", "NO"),
                ("Gary Hogeboom", "DC"),
                ("Steve Bartkowski", "AF"),
                ("Jim Zorn", "GB"),
                ("Steve Young", "TB"),
                ("Jim Plunkett", "OR"),
                ("Wade Wilson", "MV"),
                ("Richard Todd", "NG"),
                ("Oliver Luck", "HO"),
                ("Ken Anderson", "CI"),
                ("Matt Cavanaugh", "SF"),
                ("Randall Cunningham", "PE"),
                ("Jeff Kemp", "LA"),
            };
            return qbList;
        }

        private List<(string, string)> LoadQuarterbacksFrom1987()
        {
            var qbList = new List<(string, string)>
            {
                ("Joe Montana","SFO"),
                ("Dan Marino","MIA"),
                ("Neil Lomax","STL"),
                ("Dave Krieg","SEA"),
                ("Randall Cunningham","PHI"),
                ("Bernie Kosar","CLE"),
                ("Warren Moon","HOU"),
                ("Jim Kelly","BUF"),
                ("John Elway","DEN"),
                ("Phil Simms","NYG"),
                ("Boomer Esiason","CIN"),
                ("Bill Kenney","KAN"),
                ("Bobby Hebert","NOR"),
                ("Wade Wilson","MIN"),
                ("Steve DeBerg","TAM"),
                ("Ken O'Brien","NYJ"),
                ("Marc Wilson","RAI"),
                ("Danny White","DAL"),
                ("Jay Schroeder","WAS"),
                ("Jim McMahon","CHI"),
                ("Doug Williams","WAS"),
                ("Chuck Long","DET"),
                ("Scott Campbell","ATL"),
                ("Steve Young","SFO"),
                ("Steve Grogan","NWE"),
                ("Dan Fouts","SDG"),
                ("Jim Everett","RAM"),
                ("Gary Hogeboom","IND"),
                ("Randy Wright","GNB"),
                ("Jack Trudeau","IND"),
                ("Tom Ramsey","NWE"),
                ("Mark Malone","PIT"),
                ("Mike Tomczak","CHI"),
                ("Vinny Testaverde","TAM"),
                ("Jeff Rutledge","NYG"),
                ("Don Majkowski","GNB"),
                ("Jeff Kemp","SEA"),
                ("Ken Karcher","DEN"),
                ("Todd Hons","DET"),
                ("Vince Evans","RAI"),
                ("Steve Dils","RAM"),
                ("Steve Bono","PIT"),
                ("Kevin Sweeney","DAL"),
                ("Pat Ryan","NYJ"),
                ("Ed Rubbert","WAS"),
                ("Tommy Kramer","MIN"),
                ("Erik Kramer","ATL"),
                ("Mike Hohensee","CHI"),
                ("John Fourcade","NOR"),
                ("Gary Danielson","CLE"),
                ("Scott Tinsley","PHI"),
                ("Alan Risher","GNB"),
                ("Steve Pelluer","DAL"),
                ("Brent Pease","HOU"),
                ("Bruce Mathison","SEA"),
                ("Kyle Mackey","MIA"),
                ("Tony Eason","NWE"),
                ("Mike Busch","NYG"),
                ("Tony Adams","MIN"),
                ("Dave Wilson","NOR"),
                ("Willie Totten","BUF"),
                ("Mike Hold","TAM"),
                ("Rusty Hilger","RAI"),
                ("Reggie Collier","PIT"),
                ("Steve Bradley","CHI"),

            };
            return qbList;
        }

        private List<(string, string)> LoadQuarterbacksFrom1986()
        {
            var qbList = new List<(string, string)>
            {
                ("Dan Marino", "MD"),
                ("Ken O'Brien", "NG"),
                ("Boomer Esiason", "CI"),
                ("Tommy Kramer", "MV"),
                ("Jay Schroeder", "WR"),
                ("Jim Kelly", "BB"),
                ("Phil Simms", "NG"),
                ("Dave Krieg", "SS"),
                ("John Elway", "DB"),
                ("Tony Eason", "NE"),
                ("Bernie Kosar", "CL"),
                ("Randy Wright", "GB"),
                ("Dan Fouts", "SD"),
                ("Mark Malone", "PS"),
                ("Jim Plunkett", "OR"),
                ("Warren Moon", "HO"),
                ("Neil Lomax", "SL"),
                ("Bill Kenney", "KC"),
                ("Marc Wilson", "OR"),
                ("Danny White", "NG"),
                ("Jeff Kemp", "LA"),
                ("Dave Wilson", "NO"),
                ("David Archer", "AF"),
                ("Todd Blackledge", "KC"),
                ("Eric Hipple", "DL"),
                ("Steve Grogan", "NE"),
                ("Jack Trudeau", "IC"),
                ("Steve Pelluer", "SS"),
                ("Steve Young", "TB"),
                ("Joe Montana", "SF"),
                ("Ron Jaworski", "PE"),
                ("Randall Cunningham", "PE"),
                ("Jim Everett", "LA"),
            };
            return qbList;
        }

        private List<(string, string)> LoadRunningbacksFrom1984()
        {
            var rbList = new List<(string, string)>
            {
                ("Eric Dickerson", "LA"),
                ("John Riggins", "WR"),
                ("Marcus Allen", "OR"),
                ("Gerald Riggs", "AF"),
                ("James Wilder", "TB"),
                ("Pete Johnson", "CI"),
                ("Walter Payton", "CH"),
                ("Larry Kinnebrew", "CI"),
                ("Stump Mitchell", "SL"),
                ("Earnest Jackson", "SD"),
                ("Greg Bell", "BB"),
                ("Woody Bennett", "MD"),
                ("Rob Carpenter", "NG"),
                ("Roger Craig", "SF"),
                ("Tony Paige", "NJ"),
                ("Wendell Tyler", "SF"),
                ("Ottis Anderson", "SL"),
                ("Tony Dorsett", "DC"),
                ("Eddie Ivery", "GB"),
                ("Larry Moriarty", "BB"),
                ("Frank Pollard", "PS"),
                ("Mike Pruit", "CL"),
                ("Tony Collins", "NE"),
                ("Hokie Gajan", "NO"),
                ("Randy McMillan", "CL"),
                ("Freeman McNeil", "MV"),
                ("Timmy Newsome", "DC"),
                ("Billy Sims", "DL"),
                ("Theotis Brown", "KC"),
                ("Earl Campbell", "HO"),
                ("Jessie Clark", "GB"),
                ("Gerry Ellis", "GB"),
                ("Herman Heard", "KC"),
                ("Eric Lane", "SS"),
                ("Buford McGee", "SD"),
                ("Joe Morris", "NG"),
                ("Matt Suhey", "CH"),
                ("Modi Tatupu", "NE"),
                ("Sammy Winder", "DB"),
                ("Otis Wonsley", "WR"),
                ("Ted Brown", "MV"),
                ("Lynn Cain", "AF"),
                ("Curtis Dickey", "CL"),
                ("Frank Hawkins", "OR"),
                ("James Jones", "DL"),
                ("Darrin Nelson", "MV"),
                ("Bill Ring", "SF"),
            };
            return rbList;
        }

        private List<(string, string)> LoadRunningbacksFrom1985()
        {
            var rbList = new List<(string, string)>
            {
                ("Joe Morris", "NG"),
                ("Eric Dickerson", "LA"),
                ("Marcus Allen", "OR"),
                ("Ron Davenport", "MD"),
                ("Gerald Riggs", "AF"),
                ("James Wilder", "TB"),
                ("Tim Spencer", "SD"),
                ("Walter Payton", "CH"),
                ("Roger Craig", "SF"),
                ("Larry Kinnebrew", "CI"),
                ("Curt Warner", "SS"),
                ("Earnest Byner", "CL"),
                ("Greg Bell", "BB"),
                ("Sammy Winder", "DB"),
                ("John Riggins", "WR"),
                ("Mike Rozier", "HO"),
                ("Tony Paige", "NJ"),
                ("Tony Dorsett", "DC"),
                ("George Rogers", "WR"),
                ("Walter Abercrombie", "PS"),
                ("Kevin Mack", "CL"),
                ("James Brooks", "CI"),
                ("Randy McMillan", "IC"),
                ("Stump Mitchell", "SL"),
                ("Ted Brown", "MV"),
                ("James Jones", "DL"),
                ("Wendell Tyler", "SF"),
                ("George Wonsley", "IC"),
                ("Earnest Jackson", "SD"),
                ("Craig James", "NE"),
                ("Darrin Nelson", "MV"),
                ("Jessie Clark", "GB"),
                ("Tony Nathan", "MD"),
                ("Gerry Ellis", "GB"),
                ("Gene Lang", "DB"),
                ("Herman Heard", "KC"),
                ("Ottis Anderson", "SL"),
                ("Gary Anderson", "SD"),
                ("Frank Hawkins", "OR"),
                ("Steve Sewell", "DB"),
                ("Alvin Moore", "DL"),
                ("Alfred Anderson", "MV"),
                ("Calvin Thomas", "CH"),
                ("Freeman McNeil", "MV"),
                ("Frank Pollard", "PS"),
                ("Tony Collins", "NE"),
                ("Larry Moriarty", "BB"),
                ("Lorenzo Hampton", "MD"),
                ("Keith Griffin", "WR"),
                ("Charles White", "LR"),
                ("Gerald Willhite", "DB"),
                ("Ethan Horton", "KC"),
                ("Buford McGee", "SD"),
                ("Allen Rice", "MV"),
                ("George Adams", "NG"),
                ("Mike Pruit", "CL"),
                ("Lionel James", "SD"),
                ("Timmy Newsome", "DC"),
                ("Albert Bentley", "IC"),
                ("Hokie Gajan", "NO"),
                ("Modi Tatupu", "NE"),
            };
            return rbList;
        }

        private List<(string, string)> LoadRunningbacksFrom1987()
        {
            var rbList = new List<(string, string)>
            {
                ("Charles White","RAM"),
                ("Johnny Hector","NYJ"),
                ("Curt Warner","SEA"),
                ("Larry Kinnebrew","CIN"),
                ("Earnest Byner","CLE"),
                ("Herschel Walker","DAL"),
                ("Albert Bentley","IND"),
                ("Dalton Hilliard","NOR"),
                ("Earl Ferrell","STL"),
                ("Eric Dickerson","2TM"),
                ("Sammy Winder","DEN"),
                ("George Rogers","WAS"),
                ("Troy Stradford","MIA"),
                ("Rueben Mayes","NOR"),
                ("Kevin Mack","CLE"),
                ("Marcus Allen","RAI"),
                ("Anthony Toney","PHI"),
                ("Brent Fullwood","GNB"),
                ("D.J. Dozier","MIN"),
                ("Robb Riddick","BUF"),
                ("Wade Wilson","MIN"),
                ("Walter Payton","CHI"),
                ("Garry James","DET"),
                ("Bo Jackson","RAI"),
                ("John Elway","DEN"),
                ("Mike Rozier","HOU"),
                ("Roger Craig","SFO"),
                ("Stump Mitchell","STL"),
                ("Joe Morris","NYG"),
                ("Christian Okoye","KAN"),
                ("Tony Collins","NWE"),
                ("Neal Anderson","CHI"),
                ("Frank Pollard","PIT"),
                ("Keith Byars","PHI"),
                ("Kenneth Davis","GNB"),
                ("Reggie Dupard","NWE"),
                ("Herman Heard","KAN"),
                ("Gary Anderson","SDG"),
                ("Randall Cunningham","PHI"),
                ("Paul Ott","Car"),
                ("Derrick McAdoo","STL"),
                ("Gary Ellerson","DET"),
                ("Ronald Scott","MIA"),
                ("Alvin Blount","DAL"),
                ("Mark Malone","PIT"),
                ("Warren Moon","HOU"),
                ("Jay Schroeder","WAS"),
                ("Gerald Riggs","ATL"),
                ("Darrin Nelson","MIN"),
                ("Walter Abercrombie","PIT"),
                ("Ronnie Harmon","BUF"),
                ("Jeff Smith","TAM"),
                ("Gene Lang","DEN"),
                ("Jamie Mueller","BUF"),
                ("Lionel Vital","WAS"),
                ("Alfred Anderson","MIN"),
                ("Dwight Beverly","NOR"),
                ("Larry Mason","CLE"),
                ("Karl Bernard","DET"),
                ("Rick Fenney","MIN"),
                ("Dave Krieg","SEA"),
                ("Barry Word","NOR"),
                ("Joe Dudek","DEN"),
                ("Craig Ellis","RAI"),
                ("Allen Pinkett","HOU"),
                ("Lionel James","SDG"),
                ("Timmy Newsome","DAL"),
                ("Chris Brewer","CHI"),
                ("Jim McMahon","CHI"),
                ("Scott Campbell","ATL"),
                ("Steve Grogan","NWE"),
                ("Steve Sewell","DEN"),
                ("Wayne Wilson","WAS"),
                ("Kyle Mackey","MIA"),
                ("Nuu Faaola","NYJ"),
                ("Dan Fouts","SDG"),
                ("Buford Jordan","NOR"),
                ("Tommy Kramer","MIN"),

            };
            return rbList;
        }

        private List<(string, string)> LoadRunningbacksFrom1986()
        {
            var rbList = new List<(string, string)>
            {
                ("George Rogers", "WR"),
                ("Joe Morris", "NG"),
                ("Curt Warner", "SS"),
                ("Herschel Walker", "DC"),
                ("Eric Dickerson", "LA"),
                ("Kevin Mack", "CL"),
                ("Gerald Riggs", "AF"),
                ("Sammy Winder", "DB"),
                ("Lorenzo Hampton", "MD"),
                ("Walter Payton", "CH"),
                ("Rueben Mayes", "SS"),
                ("James Jones", "DL"),
                ("Johnny Hector", "NJ"),
                ("Larry Kinnebrew", "CI"),
                ("Roger Craig", "SF"),
                ("Buford McGee", "SD"),
                ("Walter Abercrombie", "PS"),
                ("Curtis Dickey", "CL"),
                ("Tim Spencer", "SD"),
                ("Earnest Jackson", "SD"),
                ("Freeman McNeil", "NJ"),
                ("Marcus Allen", "OR"),
                ("James Brooks", "CI"),
                ("Tony Dorsett", "DC"),
                ("Stump Mitchell", "SL"),
                ("Joe Cribbs", "SF"),
                ("Dalton Hilliard", "NO"),
                ("Gerald Willhite", "DB"),
                ("Thomas Sanders", "CH"),
                ("Mike Rozier", "HO"),
                ("Darrin Nelson", "MV"),
                ("Craig James", "NE"),
                ("James Wilder", "TB"),
                ("Earnest Byner", "CL"),
                ("Greg Bell", "BB"),
                ("John Riggins", "WR"),
                ("Tony Paige", "NJ"),
                ("George Wonsley", "IC"),
                ("Tony Collins", "NE"),
            };
            return rbList;
        }

        //private List<(string, string)> LoadQuarterbacksFrom1984Fix()
        //{
        //    var qbList = new List<(string, string)>
        //    {
        //        ("Dan Marino", "MD"),
        //        ("Joe Montana", "SF"),
        //    };
        //    return qbList;
        //}

        private List<(string, string)> LoadKickersFrom1984()
        {
            var qbList = new List<(string, string)>
            {
                ("Paul McFadden", "PE"),
                ("Mike Lansford", "LA"),
                ("Ray Wersching", "SF"),
                ("Gary Anderson", "PS"),
                ("Matt Bahr", "CL"),
                ("Mark Moseley", "WR"),
                ("Nick Lowery", "KC"),
                ("Neil O'Donoghue", "CI"),
                ("Rafael Septien", "DC"),
                ("Jim Breech", "CI"),
                ("Tony Franklin", "NE"),
                ("Bob Thomas", "CH"),
                ("Rich Karlis", "DB"),
                ("Morten Andersen", "NO"),
                ("Chris Bahr", "OR"),
                ("Norm Johnson", "SS"),
                ("Mick Luckhurst", "AF"),
                ("Eddie Murray", "DL"),
                ("Jan Stenerud", "MV"),
                ("Obed Ariri", "TB"),
                ("Rolf Benirschke", "SD"),
                ("Ali Haji-Sheikh", "NG"),
                ("Pat Leahy", "NJ"),
                ("Raul Allegre", "BC"),
                ("Joe Cooper", "HO"),
                ("Al Del Greco", "GB"),
                ("Uwe von Schamann", "MD"),
                ("Joe Danelo", "BB"),
                ("Florian Kempf", "HO"),
                ("Dean Biasucci", "BC"),
                ("Eddie Garcia", "GB"),
            };
            return qbList;
        }

        private List<(string, string)> LoadKickersFrom1985()
        {
            var pkList = new List<(string, string)>
            {
                ("Gary Anderson", "PS"),
                ("Kevin Butler", "CH"),
                ("Morten Andersen", "NO"),
                ("Pat Leahy", "NJ"),
                ("Eddie Murray", "DL"),
                ("Paul McFadden", "PE"),
                ("Jim Breech", "CI"),
                ("Mick Luckhurst", "AF"),
                ("Tony Franklin", "NE"),
                ("Nick Lowery", "KC"),
                ("Rich Karlis", "DB"),
                ("Mark Moseley", "WR"),
                ("Donald Igwebuike", "TB"),
                ("Mike Lansford", "LA"),
                ("Fuad Reveiz", "MD"),
                ("Tony Zendejas", "HO"),
                ("Chris Bahr", "OR"),
                ("Rafael Septien", "DC"),
                ("Al Del Greco", "GB"),
                ("Bob Thomas", "SD"),
                ("Raul Allegre", "BC"),
                ("Jan Stenerud", "MV"),
                ("Norm Johnson", "SS"),
                ("Matt Bahr", "CL"),
                ("Ray Wersching", "SF"),
                ("Scott Norwood", "BB"),
                ("Neil O'Donoghue", "SL"),
                ("Eric Schubert", "NG"),
            };
            return pkList;
        }

        private List<(string, string)> LoadKickersFrom1986()
        {
            var pkList = new List<(string, string)>
            {
                ("Tony Franklin", "NE"),
                ("Kevin Butler", "CH"),
                ("Morten Andersen", "NO"),
                ("Ray Wersching", "SF"),
                ("Raul Allegre", "NG"),
                ("Norm Johnson", "SS"),
                ("Chuck Nelson", "MV"),
                ("Tony Zendejas", "HO"),
                ("Gary Anderson", "PS"),
                ("Chris Bahr", "OR"),
                ("Paul McFadden", "PE"),
                ("Rich Karlis", "DB"),
                ("Matt Bahr", "CL"),
                ("Nick Lowery", "KC"),
                ("Eddie Murray", "DL"),
                ("Jim Breech", "CI"),
                ("Al Del Greco", "GB"),
                ("Scott Norwood", "BB"),
                ("Donald Igwebuike", "TB"),
                ("Rolf Benirschke", "SD"),
                ("Pat Leahy", "NJ"),
                ("Rafael Septien", "DC"),
                ("Mick Luckhurst", "AF"),
                ("Fuad Reveiz", "MD"),
                ("Dean Biasucci", "IC"),
                ("Mark Moseley", "WR"),
                ("Max Zendejas", "WR"),
                ("Ali Haji-Sheikh", "AF"),
                ("John Lee", "SL"),
                ("Eric Schubert", "SL"),
                ("Steve Cox", "WR"),
            };
            return pkList;
        }

        private List<(string, string)> LoadKickersFrom1987()
        {
            var pkList = new List<(string, string)>
            {
                ("Morten Andersen","NOR"),
                ("Dean Biasucci","IND"),
                ("Jim Breech","CIN"),
                ("Gary Anderson","PIT"),
                ("Roger Ruzek","DAL"),
                ("Eddie Murray","DET"),
                ("Tony Zendejas","HOU"),
                ("Kevin Butler","CHI"),
                ("Nick Lowery","KAN"),
                ("Chris Bahr","RAI"),
                ("Rich Karlis","DEN"),
                ("Pat Leahy","NYJ"),
                ("Raul Allegre","NYG"),
                ("Mike Lansford","RAM"),
                ("Paul McFadden","PHI"),
                ("Max Zendejas","GNB"),
                ("Tony Franklin","NWE"),
                ("Norm Johnson","SEA"),
                ("Jeff Jaeger","CLE"),
                ("Donald Igwebuike","TAM"),
                ("Chuck Nelson","MIN"),
                ("Vince Abbott","SDG"),
                ("Ali Haji-Sheikh","WAS"),
                ("Ray Wersching","SFO"),
                ("Scott Norwood","BUF"),
                ("Mick Luckhurst","ATL"),
                ("Jim Gallery","STL"),
                ("Al Del Greco","2TM"),
                ("Fuad Reveiz","MIA"),
                ("Mike Prindle","DET"),
                ("John Diettrich","HOU"),
                ("Van Tiffin","2TM"),
                ("Matt Bahr","CLE"),
                ("Florian Kempf","NOR"),
                ("Luis Zendejas","DAL"),
            };
            return pkList;
        }

        private List<(string, string)> LoadReceiversFrom1984Part1()
        {
            var playerList = new List<(string, string)>
            {
                ("Mark Clayton","MD"),
                ("Roy Green", "SL"),
                ("Steve Largent","SEA"),
                ("John Stallworth","PIT"),
                ("Freddie Solomon","SFO"),
                ("Daryl Turner","SEA"),
                ("Paul Coffman","GNB"),
                ("Louis Lipps","PIT"),
                ("Mike Quick","PHI"),
                ("Mark Duper      ","MIA"),
                ("Todd Christensen", "OR"),
                ("Preston Dennard  ","BUF"),
                ("Bobby Johnson  ","NYG"),
                ("James Lofton  ","GNB"),
                ("Art Monk      ","WAS"),
                ("Derrick Ramsey  ","NWE"),
                ("Wesley Walker  ","NYJ"),
                ("Steve Watson  ","DEN"),
                ("Stacey Bailey  ","ATL"),
                ("Hoby Brenner  ","NOR"),
                ("Ray Butler      ","CLT"),
                ("Wes Chandler  ","SDG"),
                ("Dwight Clark  ","SFO"),
                ("Cris Collinsworth  ","CIN"),
                ("Henry Ellard  ","RAM"),
                ("Willie Gault  ","CHI"),
                ("Butch Johnson  ","DEN"),
                ("Charlie Joiner  ","SDG"),
                ("Nat Moore      ","MIA"),
                ("Zeke Mowatt      ","NYG"),
                ("Mickey Shuler  ","NYJ"),
                ("Leonard Thompson  ","DET"),
                ("Gerald Carter  ","TAM"),
                ("Clint Didier  ","WAS"),
                ("Bruce Hardy      ","MIA"),
                ("Tony Hill      ","DAL"),
                ("Kevin House      ","TAM"),
                ("James Jones      ","DET"),
                ("Doug Marsh      ","CRD"),
                ("Stanley Morgan  ","NWE"),
                ("Ozzie Newsome  ","CLE"),
                ("Pat Tilley      ","CRD"),
                ("Jerry Bell      ","TAM"),
                ("Ron Brown      ","RAM"),
                ("Carlos Carson  ","KAN"),
                ("Earl Cooper      ","SFO"),
                ("Doug Cosbie      ","DAL"),
                ("Lin Dawson      ","NWE"),
                ("Bobby Duckworth  ","SDG"),
                ("Byron Franklin  ","BUF"),
                ("Drew Hill      ","RAM"),
                ("Leo Lewis      ","MIN"),
                ("Lionel Manuel  ","NYG"),
                ("Henry Marshall  ","KAN"),
                ("Calvin Muhammad  ","WAS"),
                ("Stephone Paige  ","KAN"),
            };
            return playerList;
        }

        private List<(string, string)> LoadReceiversFrom1984Part2()
        {
            var playerList = new List<(string, string)>
            {
                ("Tim Smith      ","OTI"),
                ("Stephen Starring  ","NWE"),
                ("Ed West          ","GNB"),
                ("Dokie Williams  ","RAI"),
                ("Adger Armstrong  ","TAM"),
                ("Brian Brennan  ","CLE"),
                ("Charlie Brown  ","WAS"),
                ("Ted Brown      ","MIN"),
                ("Arthur Cox      ","ATL"),
                ("Phil Epps      ","GNB"),
                ("Eugene Goodlow  ","NOR"),
                ("Stanford Jennings  ","CIN"),
                ("Billy Johnson  ","ATL"),
                ("Dan Johnson    ","MIA"),
                ("Vyto Kab      ","PHI"),
                ("Clarence Kay  ","DEN"),
                ("David Lewis      ","DET"),
                ("Dennis McKinnon  ","CHI"),
                ("Willie Scott  ","KAN"),
                ("Eric Sievers  ","SDG"),
                ("Ron Springs      ","DAL"),
                ("Weegie Thompson  ","PIT"),
                ("Mike Tice      ","SEA"),
                ("Jamie Williams  ","OTI"),
                ("Wayne Wilson  ","NOR"),
                ("Tony Woodruff  ","PHI"),
                ("Tyrone Young", "NO"),
            };
            return playerList;
        }

        private List<(string, string)> LoadReceiversFrom1985Part1()
        {
            var playerList = new List<(string, string)>
            {
("Daryl Turner","SEA"),
("Louis Lipps","PIT"),
("Mike Quick","PHI"),
("Wes Chandler","SDG"),
("Dwight Clark","SFO"),
("Stephone Paige","KAN"),
("Drew Hill","HOU"),
("Mike Renfro","DAL"),
("Eddie Brown","CIN"),
("Anthony Carter","MIN"),
("Jimmie Giles","TAM"),
("Bobby Johnson","NYG"),
("Mickey Shuler","NYJ"),
("Tony Hill","DAL"),
("Charlie Joiner","SDG"),
("Nat Moore","MIA"),
("Irving Fryar","NWE"),
("Rodney Holman","CIN"),
("Dennis McKinnon","CHI"),
("Roger Craig","SFO"),
("Lionel James","SDG"),
("Todd Christensen","RAI"),
("Steve Largent","SEA"),
("Doug Cosbie","DAL"),
("Paul Coffman","GNB"),
("Pat Tilley","STL"),
("Eric Sievers","SDG"),
("Pat Beach","IND"),
("John Stallworth","PIT"),
("Gary Clark","WAS"),
("Cris Collinsworth","CIN"),
("John Spagnola","PHI"),
("Billy Johnson","ATL"),
("Ozzie Newsome","CLE"),
("Steve Watson","DEN"),
("James Brooks","CIN"),
("Henry Ellard","RAM"),
("Leonard Thompson","DET"),
("Roy Green","STL"),
("Lionel Manuel","NYG"),
("Dokie Williams","RAI"),
("Kevin House","TAM"),
("Stanley Morgan","NWE"),
("Wesley Walker","NYJ"),
("Butch Woolfolk","HOU"),

            };
            return playerList;
        }

        private List<(string, string)> LoadReceiversFrom1987()
        {
            var playerList = new List<(string, string)>
            {
        ("Jerry Rice ","SFO"),
        ("Mike Quick","PHI"),
        ("J.T. Smith","STL"),
        ("Steve Largent","SEA"),
        ("Mark Bavaro","NYG"),
        ("Mark Duper","MIA"),
        ("Pete Mandley","DET"),
        ("Gary Clark","WAS"),
        ("Carlos Carson","KAN"),
        ("Webster Slaughter","CLE"),
        ("Mark Clayton","MIA"),
        ("Eric Martin","NOR"),
        ("Vance Johnson","DEN"),
        ("Anthony Carter","MIN"),
        ("Willie Gault","CHI"),
        ("Ernest Givins","HOU"),
        ("Drew Hill","HOU"),
        ("Brian Brennan","CLE"),
        ("Robert Awalt","STL"),
        ("Art Monk","WAS"),
        ("Lionel Manuel","NYG"),
        ("John Tice","NOR"),
        ("Daryl Turner","SEA"),
        ("Al Toon","NYJ"),
        ("Andre Reed","BUF"),
        ("Kelvin Bryant","WAS"),
        ("James Lofton","RAI"),
        ("Gerald Carter","TAM"),
        ("Floyd Dixon","ATL"),
        ("Ray Butler","SEA"),
        ("Irving Fryar","NWE"),
        ("Mike Wilson","SFO"),
        ("Dwight Clark","SFO"),
        ("Dokie Williams","RAI"),
        ("Curtis Duncan","HOU"),
        ("Chris Burkett","BUF"),
        ("Mike Renfro","DAL"),
        ("Roy Green","STL"),
        ("Stephone Paige","KAN"),
        ("Matt Bouza","IND"),
        ("Rick Massie","DEN"),
        ("Kellen Winslow","SDG"),
        ("Bill Brooks","IND"),
        ("Henry Ellard","RAM"),
        ("Neal Anderson","CHI"),
        ("Eddie Brown","CIN"),
        ("Tony Collins","NWE"),
        ("Mickey Shuler","NYJ"),
        ("Lionel James","SDG"),
        ("Stanley Morgan","NWE"),
        ("Walter Stanley","GNB"),
        ("John Williams","SEA"),
        ("Ricky Sanders","WAS"),
        ("Doug Cosbie","DAL"),
        ("Frankie Neal","GNB"),
        ("Kelvin Edwards","DAL"),
        ("Calvin Magee","TAM"),
        ("Aubrey Matthews","ATL"),
        ("Tom Rathman","SFO"),
        ("Mike Jones","NOR"),
        ("Mark Carrier","TAM"),
        ("John Frank","SFO"),
        ("James Pruitt","MIA"),
        ("Cedric Jones","NWE"),
        ("Kenny Jackson","PHI"),
        ("Stacey Bailey","ATL"),
        ("Mike Martin","CIN"),
        ("Walter Murray","IND"),
        ("Jay Novacek","STL"),
        ("Stephen Starring","NWE"),
        ("Rodney Carter","PIT"),
        ("Glen Kozlowski","CHI"),
        ("Robb Riddick","BUF"),
        ("Anthony Allen","WAS"),
        ("Jamie Williams","HOU"),
        ("Ron Heller","SFO"),
        ("Derek Tennell","CLE"),
        ("Carl Aikens","RAI"),
        ("Ronnie Harmon","BUF"),
        ("Earnest Byner","CLE"),
        ("Gary Anderson","SDG"),
        ("Todd Christensen","RAI"),
        ("Stump Mitchell","STL"),
        ("John Stallworth","PIT"),
        ("Wes Chandler","SDG"),
        ("John Spagnola","PHI"),
        ("Stanford Jennings","CIN"),
        ("Steve Jordan","MIN"),
        ("Albert Bentley","IND"),
        ("Phil Epps","GNB"),
        ("Timmy Newsome","DAL"),
        ("Ricky Nattiel","DEN"),
        ("Bruce Hardy","MIA"),
        ("Rodney Holman","CIN"),
        ("Ron Brown","RAM"),
        ("Mark Jackson","DEN"),
        ("Bobby Micho","DEN"),
        ("Leo Lewis","MIN"),
        ("Bruce Hill","TAM"),
        ("Kurt Sohn","NYJ"),
        ("James Brooks","CIN"),
        ("Jonathan Hayes","KAN"),
        ("Damone Johnson","RAM"),
        ("Hoby Brenner","NOR"),
        ("Jeff Smith","TAM"),
        ("Lonzell Hill","NOR"),
        ("Greg Baty","2TM"),
        ("James Brim","MIN"),
        ("Eric Kattus","CIN"),
        ("Cap Boso","CHI"),
        ("Gene Lang","DEN"),
        ("Curt Warner","SEA"),
        ("Bo Jackson","RAI"),
        ("Stephen Baker","NYG"),
        ("Troy Johnson","STL"),
        ("Trumaine Johnson","BUF"),
        ("Jimmy Teal","SEA"),
        ("Mike Tice","SEA"),
        ("D.J. Dozier","MIN"),
        ("Gregg Garrity","PHI"),
        ("Perry Kemp","CLE"),
        ("Larry Linne","NWE"),
        ("Clarence Weathers","CLE"),
        ("Milton Barney","ATL"),
        ("Edwin Lovelady","NYG"),
        ("James Noble","IND"),
        ("Darrell Grymes","DET"),
        ("Jon Francis","RAM"),
        ("Phil Freeman","TAM"),
        ("Gerald McNeil","CLE"),
        ("Danny Bradley","DET"),
        ("Cornell Burbage","DAL"),
        ("Eddie Hunter","2TM"),
        ("Hassan Jones","MIN"),
        ("Stacy Robinson","NYG"),
        ("Cris Carter","PHI"),
        ("Eric Streater","TAM"),
        ("Dan Johnson","MIA"),
        ("James McDonald","RAM"),
        ("Lyneal Alston","PIT"),
        ("Carl Hilton","MIN"),
        ("Butch Rolle","BUF"),
            };
            return playerList;
        }

        private List<(string, string)> LoadReceiversFrom1986()
        {
            var playerList = new List<(string, string)>
            {
                ("Jerry Rice","SFO"),
                ("Wesley Walker","NYJ"),
                ("Mark Duper","MIA"),
                ("Stephone Paige","KAN"),
                ("Stanley Morgan","STL"),
                ("Cris Collinsworth","CIN"),
                ("Mark Clayton","MIA"),
                ("Steve Largent","SEA"),
                ("Mike Quick","PHI"),
                ("Todd Christensen","RAI"),
                ("Al Toon","NYJ"),
                ("Gary Anderson","SDG"),
                ("Bill Brooks","MIA"),
                ("Dokie Williams","RAI"),
                ("Gary Clark","WAS"),
                ("Andre Reed","BUF"),
                ("Anthony Carter","MIN"),
                ("Nat Moore","MIA"),
                ("Daryl Turner","SEA"),
                ("J.T. Smith","STL"),
                ("Steve Jordan","MIN"),
                ("Brian Brennan","MIA"),
                ("Irving Fryar","NWE"),
                ("Roy Green","STL"),
                ("Kenny Jackson","PHI"),
                ("Jessie Hester","RAI"),
                ("Tony Collins","NWE"),
                ("Matt Bouza","IND"),
                ("Drew Hill","HOU"),
                ("Kellen Winslow","SDG"),
                ("Bruce Hardy","MIA"),
                ("Jeff Chadwick","DET"),
                ("Calvin Magee","TAM"),
                ("Willie Gault","CHI"),
                ("Mike Sherrard","DAL"),
                ("Eric Martin","NOR"),
                ("Bobby Johnson","NYG"),
                ("Sammy Winder","DEN"),
                ("Leonard Thompson","DET"),
                ("Weegie Thompson","PIT"),
                ("Art Monk","WAS"),
                ("Mickey Shuler","NYJ"),
                ("Mark Bavaro","NYG"),
                ("James Lofton","GNB"),
                ("Charlie Brown","ATL"),
                ("Eddie Brown","CIN"),
                ("Wes Chandler","SDG"),
                ("James Brooks","CIN"),
                ("Phil Epps","GNB"),
                ("Webster Slaugter","CLE"),
                ("Jimmie Giles","2TM"),
                ("Chris Burkett","BUF"),
                ("Clint Didier","WAS"),
                ("Henry Ellard","RAM"),
                ("Hassan Jones","MIN"),
                ("Carlos Carson","KAN"),
                ("Ray Butler","SEA"),
                ("Dan Johnson","MIA"),
                ("Gerald Willhite","DEN"),
                ("Earnest Givins","HOU"),
                ("Lorenzo Hampton","MIA"),
                ("Earl Ferrell","STL"),
                ("Darrin Nelson","MIN"),
                ("Tony Hill","DAL"),
                ("Pete Metzelaars","BUF"),
                ("Mike Jones","NOR"),
                ("Timmy Newsome","DAL"),
                ("Steve Watson","DEN"),
                ("Kevin Bryant","WAS"),
                ("Ozzie Newsome","CLE"),
                ("Lewis Lipps","PIT"),
                ("Walter Payton","CHI"),
                ("John Tice","NOR"),
                ("Jeff Smith","KAN"),
                ("Allen Rice","MIN"),
                ("Rich Erenberg","PIT"),
                ("Ron Brown","RAM"),
                ("Mike Renfro","DAL"),
                ("Ken Whisenhunt","ATL"),
                ("Mike Young","RAM"),
                ("Lionel Manuel","NYG"),
                ("Willie Scott","NWE"),
            };
            return playerList;
        }
    }
    #endregion
}
