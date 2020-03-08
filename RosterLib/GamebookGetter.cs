using Helpers;
using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RosterLib
{
	public class GamebookGetter : IGetGamebooks
	{
		public readonly IDownloader Downloader;

		public GamebookGetter( IDownloader downloader )
		{
			Downloader = downloader;
		}

		public int DownloadWeek( NFLWeek week )
		{
			var gameDict = new Dictionary<string, NFLGame>();
			var dlCount = 0;
			var seed = Int32.Parse( Seed( week ) );
			if (seed == 0)
			{
				Console.WriteLine("Unable to read Game Seed");
				return seed;
			}
			var origSeed = seed;
			foreach ( NFLGame game in week.GameList() )
			{
				if ( game.GameDate <= DateTime.Now )
					gameDict.Add( game.Index(), game );
				Console.WriteLine(
					"{0} : {1} : {2} : {3}",
					game.GameName(),
					game.GameApKey(),
					game.Id,
					game.Index());
			}
			var list = gameDict.Keys.ToList();
			list.Sort();

			// Loop through keys.
			foreach ( var key in list )
			{
				Console.WriteLine("{0}: {1}", key, gameDict[key]);
				var g = gameDict[ key ];
				if ( g.Id == "0" )
					g.Id = seed.ToString();
				var gotIt = false;
				var offSet = -2;
				while (!gotIt)
				{
					var url = g.GamebookUrl();
					Console.WriteLine(url);
					var uri = new Uri(url);
					if (Downloader.GotIt(uri))
					{
						break;
					}
					else
					{
						if (Downloader.DownloadPdf(uri))
							dlCount++;
						else
						{
							offSet++;
							var nextId = origSeed + offSet;
							if (offSet > 15)
								break;
							g.Id = nextId.ToString();
						}
					}
				}
				seed++;
                Console.WriteLine( "" );
            }
            Console.WriteLine( "{0} downloaded", dlCount );
            return dlCount;
		}

		public void SetOutputFolder( string folder )
		{
			Downloader.OutputFolder = folder;
		}

		public string Seed( NFLWeek week )
		{
			var gameSeed = "";
			foreach ( NFLGame game in week.GameList() )
			{
				gameSeed = game.Id;
				break;
			}
			Console.WriteLine($"Game Seed: {gameSeed}");
			return gameSeed;
		}
	}
}