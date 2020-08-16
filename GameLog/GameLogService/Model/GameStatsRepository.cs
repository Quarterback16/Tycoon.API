using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogService.Model
{
	public class GameStatsRepository
	{
		private string _playerListUrl { get; set; }
		private string _playerLogUrl { get; set; }

		public GameStatsRepository()
		{
			_playerListUrl = "https://www.pro-football-reference.com/players/{0}/";
			_playerLogUrl = "https://www.pro-football-reference.com/players/{0}/{1}/gamelog/{2}/";
		}

		public List<GameStats> GetGameStats(
			PlayerReportModel model)
		{
			model.GameLog = GetPlayerStats(
				model.Season,
				model.PlayerName);
			return model.GameLog;
		}

		public string PlayerLogUrl(
			PlayerReportModel playerModel)
		{
			return PlayerLogUrl(
				playerModel.Season,
				playerModel.PlayerName);
		}

		public List<GameStats> GetPlayerStats( 
			string season,
			string playerName)
		{
			var result = new List<GameStats>();
			HtmlWeb web = new HtmlWeb();
			var url = PlayerLogUrl(
				season,
				playerName);
			if (url.StartsWith("no data"))
				return result;
			var htmlDoc = web.Load(
				url);
			var seasonTable = GetSeasonTable(
				htmlDoc.DocumentNode);

			GatherStats(
				seasonTable,
				"pass_td",
				out int[] passTds);
			GatherStats(
				seasonTable,
				"rush_td",
				out int[] rushTds);
			GatherStats(
				seasonTable,
				"rec_td",
				out int[] recTds);

			for (int w = 0; w < 16; w++)
			{
				var gameStat = new GameStats
				{
					Week = w + 1,
					PassingTds = passTds[w],
					RushingTds = rushTds[w],
					ReceivingTds = recTds[w]
				};
				result.Add(gameStat);
			}
			return result;
		}

		public List<GameStats> GetKickerStats(
			PlayerReportModel model)
		{
			model.GameLog = GetKickerStats(
				model.Season,
				model.PlayerName);
			return model.GameLog;
		}

		public List<GameStats> GetKickerStats(
			string season,
			string playerName)
		{
			var result = new List<GameStats>();
			HtmlWeb web = new HtmlWeb();
			var url = PlayerLogUrl(
				season,
				playerName);
			var htmlDoc = web.Load(
				url);
			var seasonTable = GetSeasonTable(
				htmlDoc.DocumentNode);

			GatherStats(
				seasonTable,
				"fgm",
				out int[] fgm);
			GatherStats(
				seasonTable,
				"xpm",
				out int[] xpm);


			for (int w = 0; w < 16; w++)
			{
				var gameStat = new GameStats
				{
					Week = w + 1,
					FieldGoalsMade = fgm[w],
					ExtraPointsMade = xpm[w]
				};
				result.Add(gameStat);
			}
			return result;
		}

		public void SendLineToConsole(
			PlayerReportModel playerModel)
		{
			Console.Write($"{playerModel.PlayerName,25} ");
			foreach (var game in playerModel.GameLog)
			{
				Console.Write($"{game.RushingTds} {game.ReceivingTds} {game.PassingTds} ");
			}
			Console.WriteLine();
		}

		public void SendKickerLineToConsole(
			PlayerReportModel playerModel)
		{
			Console.Write($"{playerModel.PlayerName,25} ");
			foreach (var game in playerModel.GameLog)
			{
				Console.Write($"{game.PassingTds} {game.FieldGoalsMade} {game.ExtraPointsMade} ");
			}
			Console.WriteLine();
		}

		public void SendKickerToConsole(
			PlayerReportModel playerModel,
			int[] weeksOfInterest = null)
		{
			if (weeksOfInterest == null)
				weeksOfInterest = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			Console.WriteLine($"{playerModel.PlayerName} {playerModel.Season}");
			Console.WriteLine();
			var totals = new GameStats();
			var isFirst = false;
			foreach (var game in playerModel.GameLog)
			{
				if (weeksOfInterest.Contains(
					game.Week))
				{
					if (isFirst
						&& (game.Week.Equals(5) ||
						game.Week.Equals(9) ||
						game.Week.Equals(13)))
					{
						WriteKickerTotalLine(totals);
						totals = new GameStats();
						Console.WriteLine();
					}
					Console.WriteLine($"  {game.KickerStats()}");
					totals.FieldGoalsMade += game.FieldGoalsMade;
					totals.ExtraPointsMade += game.ExtraPointsMade;
					isFirst = true;
				}
			}
			WriteKickerTotalLine(totals);
			Console.WriteLine();
		}

		public void SendToConsole(
			PlayerReportModel playerModel,
			int[] weeksOfInterest = null)
		{
			if (weeksOfInterest == null)
				weeksOfInterest = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16 };
			Console.WriteLine($"{playerModel.PlayerName} {playerModel.Season}");
			Console.WriteLine();
			var totals = new GameStats();
			var isFirst = false;
			foreach (var game in playerModel.GameLog)
			{
				if (weeksOfInterest.Contains(
					game.Week))
				{
					if (isFirst 
						&& (game.Week.Equals(5) ||
						game.Week.Equals(9) ||
						game.Week.Equals(13)))
					{
						WriteTotalLine(totals);
						totals = new GameStats();
						Console.WriteLine();
					}
					Console.WriteLine($"  {game}");
					totals.PassingTds += game.PassingTds;
					totals.RushingTds += game.RushingTds;
					totals.ReceivingTds += game.ReceivingTds;
					isFirst = true;
				}
			}
			WriteTotalLine(totals);
			Console.WriteLine();
		}

		private void WriteTotalLine(
			GameStats totals)
		{
			Console.WriteLine(
				$"          {totals.RushingTds}-{totals.ReceivingTds}-{totals.PassingTds}");
			return;
		}

		private void WriteKickerTotalLine(
			GameStats totals)
		{
			Console.WriteLine(
				$"          {totals.FieldGoalsMade}-{totals.ExtraPointsMade}");
			return;
		}

		private static void GatherStats(
			HtmlNode seasonTable,
			string statType,
			out int[] statArray)
		{
			statArray = new int[16];
			var i = 0;
			var seasonNodes = seasonTable.SelectNodes(
						$"//tr/td[@data-stat='{statType}']");
			if (seasonNodes == null)
			{
				for (int w = 0; w < 16; w++)
				{
					statArray[w] = 0;
				}
				return;
			}
			var weekNodes = seasonTable.SelectNodes(
						$"//tr/td[@data-stat='week_num']");
			foreach (var rowNode in seasonNodes)
			{
				int weekNum = WeekNumber(i, weekNodes);
				if (weekNum == 0)
					continue;
				if (string.IsNullOrEmpty(rowNode.InnerText))
				{
					statArray[weekNum - 1] = 0;
				}
				else
				{
					statArray[weekNum - 1] = Int32.Parse(rowNode.InnerText);
				}
				if (i++ == 16)
					break;
			}
		}

		private static int WeekNumber(
			int i, 
			HtmlNodeCollection weekNodes)
		{
			if (string.IsNullOrEmpty(weekNodes[i].InnerText))
				return 0;
			return Int32.Parse(weekNodes[i].InnerText);
		}

		private HtmlNode GetSeasonTable(
			HtmlNode documentNode)
		{
			HtmlNode seasonTable = null;
			var tables = documentNode.SelectNodes(
				"//table");
			foreach (var table in tables)
			{
				if ( table.FirstChild.InnerHtml == "Regular Season Table" )
				{
					seasonTable = table;
					break;
				}
			}
			return seasonTable;
		}

		public string PlayerListUrl(
			string letter)
		{
			return string.Format(_playerListUrl, letter);
		}

		public string PlayerListUrl(
			string season,
			string playerName)
		{
			var letter = FirstLetterOfSurname(
				playerName);
			return string.Format(_playerListUrl, letter);
		}

		public string FirstLetterOfSurname(
			string playerName)
		{
			var pieces = playerName.Split(' ');
			var noOfPieces = pieces.Length;
			var surname = pieces[noOfPieces - 1];
			return surname.Substring(0, 1).ToUpper();
		}

		public string PlayerCode(
			string season,
			string playerName)
		{
			var playerCode = string.Empty;
			HtmlWeb web = new HtmlWeb();
			var url = PlayerListUrl(
				season,
				playerName);
			var htmlDoc = web.Load(
				url);
			var nodes = htmlDoc.DocumentNode.SelectNodes("//p");
			foreach (var node in nodes)
			{
				if (node.InnerText.Contains(playerName))
				{
					var selectedNode = node.InnerText;
					var career = CareerRange(
						node);
					//  if the year is in range
					var year = Int32.Parse(season);
					if ( year >= career.Item1 && year <= career.Item2 )
					{
						//  get the player code from the node
						var anchorNode = node.Descendants("a").FirstOrDefault();
						var hrefPart = anchorNode.Attributes["href"].Value;
						playerCode = PlayerCodeFrom(
							hrefPart);
						//  ultimately need to get to https://www.pro-football-reference.com/players/M/MontJo01/gamelog/1984/
						break;
					}
				}
			}
			return playerCode;
		}

		public string PlayerCodeFrom(
			string href)
		{
			char[] delimiterChars = { '/', '.' };
			string[] words = href.Split(delimiterChars);
			var noWords = words.Length;
			//Console.WriteLine($"{noWords} words in text:");

			//foreach (var word in words)
			//	Console.WriteLine($"<{word}>");
			return words[3];
		}

		public (int,int) CareerRange(
			HtmlNode node)
		{
			var nodeText = node.InnerText;
			return CareerRange(nodeText);
		}

		public (int, int) CareerRange(
			string nodeText)
		{
			//Joe Montana+ (QB) 1979-1994
			char[] delimiterChars = { ' ', '-', '+', '(', ')' };
			string[] words = nodeText.Split(delimiterChars);
			var noWords = words.Length;
			//Console.WriteLine($"{noWords} words in text:");

			//foreach (var word in words)
			//	Console.WriteLine($"<{word}>");

			var fromYear = words[noWords - 2];
			var toYear = words[noWords - 1];

			return (Int32.Parse(fromYear), Int32.Parse(toYear));
		}


		public string CodeFrom(
			string nodeText)
		{
			var code = nodeText;
			return code;
		}

		public string PlayerLogUrl(
			string season, 
			string playerName)
		{
			var letter = FirstLetterOfSurname(
				playerName);
			var playerCode = PlayerCode(
				season,
				playerName);
			if (playerName == "Eddie Ivery")
				playerCode = "IverEd00";
			if (string.IsNullOrEmpty(playerCode))
				return $"no data for {playerName}";
			return string.Format(
				_playerLogUrl, 
				letter, 
				playerCode, 
				season);
		}
	}
}
