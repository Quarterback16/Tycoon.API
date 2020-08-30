using GameLogService.Model;
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

        [TestInitialize]
        public void Initialise()
		{
            _sut = new GameStatsRepository();
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
                "Richard Todd");
            PlayerLine(
                "Steve DeBerg");
            PlayerLine(
                "Warren Moon");
        }

		private List<GameStats> PlayerLine(
            string playerName,
            string season = "1984")
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

		[TestMethod]
        public void GameStatsRepository_ForDwightClark_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Dwight Clark"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_ForFrddieSolommon_Returns10catches()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Freddie Solomon"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
            Assert.AreEqual(0, result[0].ReceivingTds);
            Assert.AreEqual(0, result[1].ReceivingTds);
            Assert.AreEqual(1, result[2].ReceivingTds);
            Assert.AreEqual(1, result[3].ReceivingTds);
            Assert.AreEqual(0, result[4].ReceivingTds);
            Assert.AreEqual(0, result[5].ReceivingTds);
            Assert.AreEqual(0, result[6].ReceivingTds);
            Assert.AreEqual(0, result[7].ReceivingTds);
            Assert.AreEqual(1, result[8].ReceivingTds);
            Assert.AreEqual(1, result[9].ReceivingTds);
            Assert.AreEqual(2, result[10].ReceivingTds);
            Assert.AreEqual(0, result[11].ReceivingTds);
            Assert.AreEqual(1, result[12].ReceivingTds);
            Assert.AreEqual(1, result[13].ReceivingTds);
            Assert.AreEqual(1, result[14].ReceivingTds);
            Assert.AreEqual(1, result[15].ReceivingTds);
        }

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
                season: "1984",
                playerName: "Joe Montana");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/",
                result);
        }

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

        [TestMethod]
        public void GameStatsRepository_WithLinkText_KnowsPlayerCareerRange()
        {
            //Joe Montana+ (QB) 1979-1994
            var result = _sut.CareerRange(
                nodeText: "Joe Montana+ (QB) 1979-1994");

            Console.WriteLine(result);
            Assert.AreEqual(
                (1979, 1994),
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
        public void GameStatsRepoForEntireTeamEntireYear()
        {
            var fantasyTeam = "CD";
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var roster = rosterService.GetRoster(
                fantasyTeam);
            var teamList = new List<(string, string)>();
            AddRosterInOrder(roster, teamList);
            foreach (var item in teamList)
            {
                var playerModel = new PlayerReportModel
                {
                    Season = "1984",
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
			var fantasyTeam = "BR";
			var rosterService = new RetroRosters(
				new RosterEventStore());
			var roster = rosterService.GetRoster(
				fantasyTeam);
			var teamList = new List<(string, string)>();
			AddRosterInOrder(roster, teamList);
            //int[] weeks = new int[] { 1, 2, 3 };
            //int[] weeks = new int[] { 5, 6, 7, 8 };
            //int[] weeks = new int[] { 9, 10, 11, 12 };
            int[] weeks = new int[] { 13, 14, 15, 16 };
            foreach (var item in teamList)
			{
				var playerModel = new PlayerReportModel
				{
					Season = "1984",
					PlayerName = item.Item1
				};

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

		private static void AddRosterInOrder(
			List<string> roster,
			List<(string, string)> teamList)
		{
			AddPartial(
				"QB",
				roster,
				teamList);
			AddPartial(
				"RB",
				roster,
				teamList);
			AddPartial(
				"TE",
				roster,
				teamList);
			AddPartial(
				"WR",
				roster,
				teamList);
			AddPartial(
				"KK",
				roster,
				teamList);
		}

		private static void AddPartial(
            string desiredPos,
			List<string> roster,
			List<(string, string)> teamList)
		{
			foreach (var player in roster)
			{
				var thePlayer = player.Substring(7, 20)
					.Trim();
				var thePos = player.Substring(5, 2);
                if (thePos.Equals(desiredPos))
				    teamList.Add((thePlayer, thePos));
			}
		}

		[TestMethod]
        public void GameStatsRepository_ForKicker_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Uwe Von Schamann"
            };

            var result = _sut.GetKickerStats(
                model: playerModel);

            _sut.SendKickerToConsole(
                playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesTdp_ForQuarterbacks()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var qbList = LoadQuarterbacksFrom1984();
			foreach (var item in qbList)
			{
                WriteLabel(rosterService, item);
                WriteTdpStats(item);
                Console.WriteLine();
			}
		}

        [TestMethod]
        public void GameStatsRepository_GeneratesTdc_ForReceivers()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var recList = LoadReceiversFrom1984Part2();
            foreach (var item in recList)
            {
                WriteLabel(rosterService, item);
                WriteTdcStats(item);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesTdr_ForQuarterbacks()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var qbList = LoadQuarterbacksFrom1984();
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
            var qbList = LoadRunningbacksFrom1984();
            foreach (var item in qbList)
            {
                WriteLabel(rosterService, item);
                WriteTdrStats(item);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void GameStatsRepository_GeneratesKickingPts_ForKickers()
        {
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var kList = LoadKickersFrom1984();
            foreach (var item in kList)
            {
                WriteLabel(rosterService, item);
                WriteKickStats(item);
                Console.WriteLine();
            }
        }

        private void WriteKickStats(
            (string, string) item)
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = "1984"
            };
            var stats = _sut.GetKickerStats(
                model);
            WriteSeasonKicking(stats);
        }

        private void WriteTdcStats(
            (string, string) item)
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = "1984"
            };
            var stats = _sut.GetGameStats(
                model);
            WriteSeasonTdc(stats);
        }

        private void WriteTdrStats(
            (string, string) item)
        {
            var model = new PlayerReportModel
            {
                PlayerName = item.Item1,
                Season = "1984"
            };
            var stats = _sut.GetGameStats(
                model);
            WriteSeasonTdr(stats);
        }

        private void WriteTdpStats(
            (string, string) item)
		{
			var model = new PlayerReportModel
			{
				PlayerName = item.Item1,
				Season = "1984"
			};
			var stats = _sut.GetGameStats(
				model);
			WriteSeasonTdp(stats);
		}

		private static void WriteLabel(
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

    }
}
