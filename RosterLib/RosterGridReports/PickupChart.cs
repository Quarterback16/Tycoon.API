using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib.RosterGridReports
{
	public class PickupChart : RosterGridReport
	{
		public int Week { get; set; }

		public SimplePreReport Report { get; set; }

		public PickupSummary PickupSummary { get; set; }
        public bool PlayerReports { get; set; }

        public PickupChart( 
            IKeepTheTime timekeeper, 
            int week,
            bool playerReports = false ) : base( timekeeper )
		{
			Name = "Pickup Chart";
			Season = timekeeper.CurrentSeason( DateTime.Now );
			Week = week;
			Report = new SimplePreReport
			{
				ReportType = "Pickup Chart",
				Folder = "Projections",
				Season = Season,
				InstanceName = $"Pickup-Chart-Week-{Week:0#}"
			};
			PickupSummary = new PickupSummary(timekeeper, week);
            PlayerReports = playerReports;
		}

		public override void RenderAsHtml()
		{
			Report.Body = GenerateBody();
			Report.RenderHtml();
			FileOut = Report.FileOut;
			PickupSummary.RenderAsHtml();
		}

		public string GenerateBody()
		{
			var bodyOut = new StringBuilder();

			var winners = GetWinners();
			var losers = GetLosers();

			//  We now sort by Points For as this correlates better with FPts

			var completeList = new List<Teamer>();
			foreach (var winner in winners)
				completeList.Add(winner);
			foreach (var loser in losers)
				completeList.Add(loser);
			var sortedList = completeList.OrderByDescending(
				x => x.PointsFor);

			var c = new YahooCalculator();
			var lineNo = 0;

			foreach ( var tm in sortedList)
				lineNo = GenerateChart( bodyOut, c, lineNo, tm );

			return bodyOut.ToString();
		}

		public int GenerateChart(
		   StringBuilder bodyOut, YahooCalculator c, int lineNo, IWinOrLose team )
		{
			team.Team.LoadKickUnit();
			team.Team.LoadRushUnit();
			team.Team.LoadPassUnit();
			var qb = GetQBBit(team, c);
			var rb = GetRunnerBit(team, c);
			var gameBit = GameBit(team);
			var timeBit = TimeBit(team);

			lineNo++;
			bodyOut.Append($"{lineNo,2} {timeBit} {gameBit}");
			bodyOut.Append($" {qb}");
			bodyOut.Append($" {rb}");

			SpitOutTheLineFor(team.Team.PassUnit.W1, bodyOut, c, team);
			SpitOutTheLineFor(team.Team.PassUnit.W2, bodyOut, c, team);
			SpitOutTheLineFor(team.Team.PassUnit.TE, bodyOut, c, team);
			SpitOutTheLineFor(team.Team.KickUnit.PlaceKicker, bodyOut, c, team);
			bodyOut.AppendLine();
			return lineNo;
		}

		private void SpitOutTheLineFor(
			NFLPlayer player,
			StringBuilder bodyOut, 
			YahooCalculator c, 
			IWinOrLose team)
		{
			var pb = GetPlayerBit(
				player,
				team, 
				c);
			bodyOut.Append($" {pb}");
		}

		#region Bits and Pieces

		private static string TimeBit( IWinOrLose team )
		{
			var dayName = team.Game.GameDate.ToString( "dddd" ).Substring( 0, 2 );
			var bit = $"{dayName}{team.Game.Hour}";
			return bit;
		}

		private static string GameBit( IWinOrLose team )
		{
			team.Game.CalculateSpreadResult();
			var predictedResult = team.IsWinner
			   ? team.Game.BookieTip.PredictedScore()
			   : team.Game.BookieTip.PredictedScoreFlipped();
			var theLine = team.Game.TheLine( team.Team.TeamCode );
			var url = team.Game.GameProjectionUrl();
			return $"<a href='{url}'>{predictedResult}</a> {theLine,3}";
		}

		public string GetPlayerBit(
			NFLPlayer player,
			IWinOrLose team,
			YahooCalculator calculator,
			bool bLinks = true)
		{
			var bit = NoneBit(team, bLinks);

			if (player != null)
			{
				bit = PlayerPiece(
					player: player,
					game: team.Game,
					calculator: calculator,
					bLinks: bLinks);
			}
			return string.Format("{0,-36}", bit);
		}

		public string GetPlayerWikiBit(
			NFLPlayer player,
			IWinOrLose team,
			YahooCalculator calculator,
			bool bLinks = true)
		{
			var bit = NoneBit(team, bLinks);

			if (player != null)
			{
				bit = PlayerWikiPiece(
					player: player,
					game: team.Game,
					calculator: calculator);
			}
			return string.Format("{0,-28}", bit);
		}

		private string NoneBit( IWinOrLose team, bool bLinks = true )
		{
			if (bLinks)
			{
				var bit = $@" <a href='..\\Roles\\{
					team.Team.TeamCode
					}-Roles-{Week - 1:0#}.htm'>none</a>                                ";
				return bit;
			}
			return "none";
		}

		private string GetQBBit( 
			IWinOrLose team, 
			YahooCalculator calculator,
			bool bLinks = true)
		{
			var bit = NoneBit( team, bLinks );
			if ( team.Team.PassUnit.Q1 != null )
				bit = PlayerPiece( 
					team.Team.PassUnit.Q1, 
					team.Game, 
					calculator,
					bLinks );
			return $"{bit,-36}";
		}

		public string GetRunnerBit( 
			IWinOrLose team, 
			YahooCalculator calculator,
			bool bLinks = true)
		{
			var bit = NoneBit(team, bLinks);
			if ( team.Team.PassUnit.Q1 != null )
			{
				// get the next opponent by using the QB
				var nextOppTeam = team.Team.PassUnit.Q1.NextOpponentTeam( team.Game );

				if ( team.Team.RunUnit == null )
					team.Team.LoadRushUnit();
				else
					Logger.Trace( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
					   team.Team.RunUnit.Runners.Count(), team.Team.RunUnit.AceBack );

				if ( team.Team.RunUnit.AceBack != null )
					bit = PlayerPiece(
						player: team.Team.RunUnit.AceBack,
						game: team.Game,
						calculator: calculator,
						bLinks: bLinks);
				else
				{
					var dualBacks = team.Team.RunUnit.Committee;
					var combinedPts = 0.0M;
					foreach ( NFLPlayer runner in team.Team.RunUnit.Starters )
					{
						calculator.Calculate( runner, team.Game );
						combinedPts += runner.Points;
					}
					if ( !string.IsNullOrWhiteSpace( dualBacks.Trim() ) )
					{
						var dualSpace = 24;
						dualBacks = dualBacks.Substring( 0, dualBacks.Length - 3 );
						if ( dualBacks.Length < dualSpace )
							dualBacks += new string( ' ', dualSpace - dualBacks.Length );
						if ( dualBacks.Length > dualSpace )
							dualBacks = dualBacks.Substring( 0, dualSpace );
					}
					var p = team.Team.RunUnit.R1;

					var matchupLink = "";
					if ( p != null )
					{
						var plusMatchup = PlusMatchup(
							player: p,
							nextOppTeam: nextOppTeam,
							pTeam: p.CurrTeam );
						matchupLink = nextOppTeam.DefensiveUnitMatchUp(
							catCode: p.PlayerCat,
							matchUp: plusMatchup,
							bLinks: bLinks );
					}
					else
						matchupLink = "?" + new String(' ', 20);

					if (bLinks)
						bit = string.Format(
						   "&nbsp;<a href='..\\Roles\\{0}-Roles-{1:0#}.htm'>{3}</a> {2}  {4,2:#0}      ",
						   team.Team.TeamCode,
						   Week - 1,
						   matchupLink,
						   dualBacks,
						   (int) combinedPts  );
					else
						bit = $" {dualBacks} {matchupLink}  {(int)combinedPts,2:#0}      ";

					Logger.Trace( "   >>> No Ace back for {0}", team.Team.Name );
				}
			}
			else
			{
				Logger.Trace( "   >>> No QB1 for {0}", team.Team.Name );
			}
			Logger.Trace( "   >>> bit = {0}", bit );
			return $"{bit,-36}";
		}

		public string GetRunnerWikiBit(
			IWinOrLose team,
			YahooCalculator calculator)
		{
			var bit = NoneBit(team, bLinks: false);
			if (team.Team.PassUnit.Q1 != null)
			{
				// get the next opponent by using the QB
				var nextOppTeam = team.Team.PassUnit.Q1.NextOpponentTeam(team.Game);

				if (team.Team.RunUnit == null)
					team.Team.LoadRushUnit();
				else
					Logger.Trace("   >>> Rush unit loaded {0} rushers; Ace back {1}",
					   team.Team.RunUnit.Runners.Count(), team.Team.RunUnit.AceBack);

				if (team.Team.RunUnit.AceBack != null)
					bit = PlayerWikiPiece(
						player: team.Team.RunUnit.AceBack,
						game: team.Game,
						calculator: calculator);
				else
				{
					var dualBacks = string.Empty;
					var combinedPts = 0.0M;
					foreach (NFLPlayer runner in team.Team.RunUnit.Starters)
					{
						var namePart = NoWiki($"{runner.PlayerNameTo(11)}");
						namePart = UpperCaseMyPlayers(runner, namePart);
						dualBacks += BoldFreeAgents(runner, namePart) + " ";
						calculator.Calculate(runner, team.Game);
						combinedPts += runner.Points;
					}
					if (!string.IsNullOrWhiteSpace(dualBacks.Trim()))
					{
						dualBacks = dualBacks.Substring(0, dualBacks.Length - 1);
						//if (dualBacks.Length < 20)
						//	dualBacks = dualBacks + new string(' ', 20 - dualBacks.Length);
						//if (dualBacks.Length > 20)
						//	dualBacks = dualBacks.Substring(0, 20);
					}
					var p = team.Team.RunUnit.R1;

					var matchupLink = "";
					if (p != null)
					{
						var plusMatchup = PlusMatchup(
							player: p,
							nextOppTeam: nextOppTeam,
							pTeam: p.CurrTeam);
						matchupLink = nextOppTeam.DefensiveUnitMatchUp(
							catCode: p.PlayerCat,
							matchUp: plusMatchup,
							bLinks: false);
					}
					else
						matchupLink = "?" + new String(' ', 20);

					bit = $" {dualBacks} {matchupLink}  ({(int)combinedPts,2:#0})";

					Logger.Trace("   >>> No Ace back for {0}", team.Team.Name);
				}
			}
			else
			{
				Logger.Trace("   >>> No QB1 for {0}", team.Team.Name);
			}
			Logger.Trace("   >>> bit = {0}", bit);
			return $"{bit,-28}";
		}

		public string PlayerPiece( 
            NFLPlayer player, 
            NFLGame game, 
            YahooCalculator calculator,
			bool bLinks = true)
		{
			var nextOppTeam = player.NextOpponentTeam( game );
			var plusMatchup = PlusMatchup(
				player,
				nextOppTeam,
				player.CurrTeam);
			var matchupLink = nextOppTeam.DefensiveUnitMatchUp( 
                player.PlayerCat, 
                plusMatchup,
				bLinks );
			var owners = player.LoadAllOwners();
			calculator.Calculate(
				player,
				game);
			var namePart = bLinks 
				? $"<a href='..\\Roles\\{player.TeamCode}-Roles-{Week - 1:0#}.htm'>{player.PlayerNameTo(11)}</a>"
				: $"{player.PlayerNameTo(11)}";
			if ( player.PlayerCat.Equals( Constants.K_KICKER_CAT ) )
			{
				AddPickup( player, game );
				return string.Format( " {0,-11}  {1}  {2,2:#0}{3} {4}",
				   namePart,
				   owners,
				   player.Points,
				   DomeBit( game, player ),
				   ActualOutput( game, player, null ) );
			}
			AddPickup( player, game );
			return string.Format( "{6}{0,-11}{7} {3}  {1}  {2,2:#0}{5} {4}",
			   namePart,
			   matchupLink,  //  defensiveRating,
			   player.Points,
			   owners,
			   ActualOutput( game, player, null ),
			   DomeBit( game, player ),
			   ReturnerBit( player ),
			   ShortYardageBit( player ) );
		}

		public string PlayerWikiPiece(
			NFLPlayer player,
			NFLGame game,
			YahooCalculator calculator)
		{
			var nextOppTeam = player.NextOpponentTeam(game);
			var plusMatchup = PlusMatchup(player, nextOppTeam, player.CurrTeam);
			var matchupLink = nextOppTeam.DefensiveUnitMatchUp(
				catCode: player.PlayerCat,
				matchUp: plusMatchup,
				bLinks: false);
			var owners = player.LoadAllOwners();
			calculator.Calculate(player, game);
			var namePart = NoWiki($"{player.PlayerNameTo(11)}");
			namePart = UpperCaseMyPlayers(player, namePart);
			namePart = BoldFreeAgents(player, namePart);
			namePart = ItaliseNewPlayers(player, game, namePart);
			var playerPiece = string.Empty;
			if (player.PlayerCat.Equals(Constants.K_KICKER_CAT))
			{
				playerPiece = $@" {namePart,-11}  {
					owners
					}  ({PlayerPointsOut(player)}{DomeBit(game, player).Trim()})";
			}
			else
				playerPiece = $@"{
					ReturnerBit(player)
					}{namePart,-11}{
					ShortYardageBit(player)
					} {owners}  {matchupLink}  ({PlayerPointsOut(player)}{
					DomeBit(game, player)
					})";

			return playerPiece;
		}

		private string NoWiki(string v)
		{
			return $"<nowiki>{v}</nowiki>";
		}

		private static string ItaliseNewPlayers(
			NFLPlayer player, 
			NFLGame game, 
			string namePart)
		{
			if (player.Points == 0 && game.Played())
				namePart = $"//{namePart}//";
			return namePart;
		}

		private static string BoldFreeAgents(
			NFLPlayer player, 
			string namePart)
		{
			if (player.IsFreeAgent())
				namePart = $"**{namePart.Trim()}**";
			return namePart;
		}

		private static string UpperCaseMyPlayers(
			NFLPlayer player, 
			string namePart)
		{
			if (player.Owner == "77")
				namePart = namePart.ToUpper();
			return namePart;
		}

		private string PlayerPointsOut(NFLPlayer player, bool isReport = false)
		{
			var pointsOut = $"{ player.Points,2:#0}";
			if (pointsOut.Length == 1)
				pointsOut = " " + pointsOut;

			if (player.Points > PlayerStandard(player.PlayerCat)
				&& player.IsFreeAgent())
			{
				if (!isReport)
					pointsOut = $"**{pointsOut.Trim()}**";
			}
			return " " + pointsOut + " ";
		}

		private int PlayerStandard(string playerCat)
		{
			if (playerCat.Equals(Constants.K_QUARTERBACK_CAT))
				return 16;
			if (playerCat.Equals(Constants.K_RUNNINGBACK_CAT))
				return 11;
			if (playerCat.Equals(Constants.K_RECEIVER_CAT))
				return 11;
			if (playerCat.Equals(Constants.K_KICKER_CAT))
				return 7;
			return 99;
		}

		private void AddPickup( NFLPlayer p, NFLGame g )
		{
			p.LoadOwner( Constants.K_LEAGUE_Yahoo );
			if ( p.IsFreeAgent() || p.Owner == "77" )
			{
                var prevPts = p.Points;  // so we dont lose Points value
                var pu = new Pickup
                {
                    Season = Season,
                    Player = p,
                    Name = $"{p.PlayerNameTo( 20 )} ({p.TeamCode}) {p.PlayerPos,-10}",
                    Opp = $"{g.OpponentOut( p.TeamCode )}",
                    ProjPts = p.Points,
                    CategoryCode = p.PlayerCat,
                    Pos = p.PlayerPos,
					ActualPts = ActualOutput( g, p, null )
				};
                p.Points = prevPts;
				if ( p.Owner == "77" )
					pu.Name = pu.Name.ToUpper();
				PickupSummary.AddPickup( pu );
                if ( PlayerReports )
                    p.PlayerReport(forceIt:true);
			}
		}

		private string PlusMatchup( 
			NFLPlayer player, 
			NflTeam nextOppTeam, 
			NflTeam pTeam )
		{
			if (pTeam.TeamCode == string.Empty)
				return " ";

			var matchUp = "-";
			var oppRating = nextOppTeam.DefensiveRating( player.PlayerCat );
			var oppNumber = GetAsciiValue( oppRating );
			var plrRating = pTeam.OffensiveRating( player.PlayerCat );
			var plrNumber = GetAsciiValue( plrRating );
			if ( plrNumber <= oppNumber )
			{
				matchUp = "+";
				if ( oppNumber - plrNumber >= 3 )
					matchUp = "*";  //  big mismatch
			}
			return matchUp;
		}

		private static int GetAsciiValue( string rating )
		{
			byte[] value = Encoding.ASCII.GetBytes( rating );
			return value[ 0 ];
		}

		private string ShortYardageBit( NFLPlayer p )
		{
			var shortYardageBit = " ";
			if ( p.IsShortYardageBack() )
			{
				shortYardageBit = "$";
			}
			return shortYardageBit;
		}

		private string ReturnerBit( NFLPlayer p )
		{
			var returnerBit = " ";
			if ( p.IsReturner() )
			{
				returnerBit = "-";
			}
			return returnerBit;
		}

		private string DomeBit( NFLGame g, NFLPlayer p )
		{
			var bit = " ";
			if ( p.IsKicker() )
			{
				if ( g.IsDomeGame() )
					bit = "+";
				else if ( g.IsBadWeather() )
					bit = "-";
			}
			else
			{
				if (p.ScoredLastTwo(TimeKeeper))
					bit = "*";
				else if (p.ScoredLastGame(TimeKeeper))
					bit = "+";
			}
			return bit;
		}

		public string ActualOutput( 
			NFLGame game,
			NFLPlayer player,
			List<NFLPlayer> runners,
			bool isReport = true)
		{
			if (!game.Played(addDay: false))
			{
				if (isReport)
					return "____";
				else
					return string.Empty;
			}

			if ( game.GameWeek == null )
                game.GameWeek = new NFLWeek( game.Season, game.Week );

            var scorer = new YahooScorer( game.GameWeek )
            {
                UseProjections = false
            };

			var nScore = 0.0M;
			if (player == null)  //  no ace back
			{
				if (runners == null )
					return string.Empty;

				foreach (NFLPlayer runner in runners)
				{
					scorer.RatePlayer(runner, game.GameWeek);
					nScore += runner.Points;
				}
				return $"{nScore,2:#0}";
			}

            nScore = scorer.RatePlayer( 
                player, 
                game.GameWeek, 
                takeCache:false );
			player.Points = nScore;
			return PlayerPointsOut(player,isReport);
		}

		#endregion Bits and Pieces

		public IList<Winner> GetWinners()
		{
			var week = new NFLWeek( Season, Week );
			var winners = new List<Winner>();
			foreach ( NFLGame g in week.GameList() )
			{
				g.CalculateSpreadResult();
				var teamCode = g.BookieTip.WinningTeam();
				var winner = new Winner
				{
					PointsFor = g.BookieTip.WinningScore(),
					Team = g.Team( teamCode ),
					Margin = Math.Abs( g.Spread ),
					Home = g.IsHome( teamCode ),
					Game = g
				};
				winners.Add( winner );
			}

			return winners;
		}

		public IList<Loser> GetLosers()
		{
			var week = new NFLWeek( Season, Week );
			var losers = new List<Loser>();
			foreach ( NFLGame g in week.GameList() )
			{
				g.CalculateSpreadResult();
				var teamCode = g.BookieTip.LosingTeam();

				var loser = new Loser
				{
					PointsFor = g.BookieTip.LosingScore(),
					Team = g.Team( teamCode ),
					Margin = Math.Abs( g.Spread ),
					Home = g.IsHome( teamCode ),
					Game = g
				};
				losers.Add( loser );
			}

			return losers;
		}
	}

	public class Teamer : IComparable, IWinOrLose
	{
		public int PointsFor { get; set; }

		public decimal Margin { get; set; }

		public NflTeam Team { get; set; }

		public bool Home { get; set; }

		public bool IsWinner { get; set; }

		public NFLGame Game { get; set; }

		public int CompareTo(object obj)
		{
			var winner2 = (Winner)obj;
			return Margin > winner2.Margin ? 1 : 0;
		}

	}

	public class Winner : Teamer
	{
		public Winner()
		{
			IsWinner = true;
		}

		public override string ToString()
		{
			return $"{Team} by {Margin,4}";
		}
	}

	public class Loser : Teamer
	{
		public Loser()
		{
			IsWinner = false;
		}
	}
}