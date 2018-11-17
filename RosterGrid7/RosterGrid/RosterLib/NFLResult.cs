﻿namespace RosterLib
{
	public struct NFLResult
	{
		public int HomeScore { get; set; }
		public int AwayScore { get; set; }
		public int Spread { get; set; }
		public string HomeTeam { get; set; }
		public string AwayTeam { get; set; }

		public int HomeTDp { get; set; }
		public int HomeTDr { get; set; }
		public int HomeFg { get; set; }
		public int HomeTDd { get; set; }
		public int HomeTDs { get; set; }
		public int HomeYDp { get; set; }
		public int HomeYDr { get; set; }

		public int AwayTDp { get; set; }
		public int AwayTDr { get; set; }
		public int AwayFg { get; set; }
		public int AwayTDd { get; set; }
		public int AwayTDs { get; set; }
		public int AwayYDp { get; set; }
		public int AwayYDr { get; set; }

		public NFLResult( string home, int homePts, string away, int awayPts ) : this()
		{
			HomeTeam = home;
			AwayTeam = away;
			HomeScore = homePts;
			AwayScore = awayPts;
			Spread = homePts - awayPts;
			HomeTDp = 0;
			HomeTDr = 0;
			HomeTDs = 0;
			HomeTDd = 0;
			HomeYDp = 0;
			HomeYDr = 0;
			HomeFg = 0;
			AwayTDr = 0;
			AwayTDp = 0;
			AwayTDs = 0;
			AwayTDd = 0;
			AwayYDp = 0;
			AwayYDr = 0;
			AwayFg = 0;
		}

		public int Margin()
		{
			return ( HomeScore - AwayScore );
		}

		public string WinningTeam()
		{
			return ( Margin() > 0 ) ? HomeTeam : AwayTeam;
		}

		public bool HomeWin()
		{
			return WinningTeam() == HomeTeam;
		}

		public string LosingTeam()
		{
			return ( Margin() < 0 ) ? HomeTeam : AwayTeam;
		}

		public int WinningScore()
		{
			return ( Margin() > 0 ) ? HomeScore : AwayScore;
		}

		public int LosingScore()
		{
			return ( Margin() > 0 ) ? AwayScore : HomeScore;
		}

		public string PredictedScore()
		{
			return string.Format( "{0} {1}-{2} {3}", 
			                      WinningTeam(), WinningScore(), LosingTeam(), LosingScore() );
		}

		public string LogResult()
		{
			//const string formatVerbose = "{0} {1} pts Tdp:{2} Tdr:{3} @ {4} {5} pts Tdp:{6} Tdr:{7}";
			//var result = string.Format( formatVerbose,
			//        AwayTeam, AwayScore, AwayTDp, AwayTDr, HomeTeam, HomeScore, HomeTDp, HomeTDr);
			const string formatSimple = "{0} {1} @ {2} {3} :  {4}";
			var result = string.Format( formatSimple, AwayTeam, AwayScore, HomeTeam, HomeScore, Margin() );

          return result;
		}

		public int MarginForTeam( string teamCode )
		{
			if ( IsHomeTeam( teamCode ) )
				return Margin();

			return 0-Margin();
		}

		private bool IsHomeTeam( string teamCode )
		{
			return ( HomeTeam == teamCode );
		}

		internal string WinningTeamAts( decimal spread )
		{
			decimal adjustedMargin;

			if ( spread == 0.0M ) return WinningTeam();  //  off the board
			if ( spread == 0.5M ) return WinningTeam();  //  pickem game

			if ( spread > 0 ) //  home team favoured
			{
				var adjustedHomeScore = HomeScore - spread;
				adjustedMargin = adjustedHomeScore - AwayScore;
				//Utility.Announce( string.Format( "Spread:{0:00.0} score {1}-{2} @ {3}-{4} ",
				//   spread, AwayTeam, AwayScore, HomeTeam, adjustedHomeScore ) );
				if ( adjustedMargin.Equals( 0.0M ) ) return "TIE";
			}
			else
			{
				//  away team favoured
				var adjustedAwayScore = AwayScore + spread;
				adjustedMargin = HomeScore - adjustedAwayScore;
				//Utility.Announce( string.Format( "Spread:{0:00.0} score {1}-{2} @ {3}-{4} ",
				//   spread, AwayTeam, adjustedAwayScore, HomeTeam, HomeScore ) );
				if ( adjustedMargin.Equals( 0.0M ) ) return "TIE";
			}

			return ( adjustedMargin > 0 ) ? HomeTeam : AwayTeam;
		}

		public bool IsValid()
		{
			var homeScore = ( ( HomeTDp + HomeTDr + HomeTDd + HomeTDs ) * 7 ) + ( HomeFg * 3 );
			var isValid = ( homeScore  == HomeScore );
			if ( isValid )
			{
				var awayScore = ( ( AwayTDp + AwayTDr + AwayTDd + AwayTDs ) * 7 ) + ( AwayFg * 3 );
				isValid =  ( awayScore == AwayScore );
				if ( !isValid )
					Utility.Announce( string.Format( "Away score should be {0}", awayScore ) );
			}
			else
				Utility.Announce( string.Format( "Home score should be {0}", homeScore ) );
			return isValid;
		}

		public int TotalHomeTDs()
		{
			return HomeTDd + HomeTDp + HomeTDr + HomeTDs;
		}

		public int TotalAwayTDs()
		{
			return AwayTDd + AwayTDp + AwayTDr + AwayTDs;
		}
	}
}