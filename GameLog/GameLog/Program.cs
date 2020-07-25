using HtmlAgilityPack;
using System;

namespace GameLog
{
	class Program
	{
		static void Main()
		{
			// enter year
			var season = "1984";
			// enter player name
			var playerName = "Joe Montana";
			// scrape it
			Scrape(
				season,
				playerName);
			// display results
			Console.ReadLine();
		}

		private static void Scrape(
			string season,
			string playerName)
		{
			var letter_url = "https://www.pro-football-reference.com/players/M/";
			var players_url = "https://www.pro-football-reference.com/players/{letter}/{player_code}.htm";

			HtmlWeb web = new HtmlWeb();

			var htmlDoc = web.Load(letter_url);

			//var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
			//foreach (var node in anchorNodes)
			//{
			//	Console.WriteLine(node.OuterHtml);
			//}

			var nodes = htmlDoc.DocumentNode.SelectNodes("//p");
			foreach (var node in nodes)
			{
				if (node.InnerText.Contains(playerName))
				{
					var selectedNode = node.InnerText;
					Console.WriteLine(selectedNode);
					Console.WriteLine(node.OuterHtml);
					//  if the year is in range
					//  get the player code from the node
					//  u;timately need to get to https://www.pro-football-reference.com/players/M/MontJo01/gamelog/1984/
				}
			}


			//var doc = new HtmlDocument();
			//doc.LoadHtml(@"<html><body><div id='foo'>text</div></body></html>");
			//var div = doc.GetElementbyId("foo");

			//// Show info
			//System.Console.WriteLine(div.OuterHtml);

			//System.Console.WriteLine(div);
		}
	}
}
