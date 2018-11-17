using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	// filter
	public class PullMetricsFromPrediction
	{
		public PullMetricsFromPrediction( PlayerGameProjectionMessage input )
		{
			Process( input );
		}

		private void Process( PlayerGameProjectionMessage input )
		{
			DoRushingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoRushingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			DoPassingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoPassingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			DoKickingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoKickingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			if ( input.Game.PlayerGameMetrics.Count < 12 )
				Utility.Announce( string.Format( "Missing metrics from {0}", input.Game.ToString() ) ); 
		}

		#region Kicking

		private void DoKickingUnit(PlayerGameProjectionMessage input, string teamCode, bool isHome)
		{
			var kicker = GetKicker(teamCode);
			if ( kicker != null )
			{
				var projFG = (int) ( ( ( isHome ) ? input.Prediction.HomeFg : input.Prediction.AwayFg ) );
				var projPat = (int) ( ( ( isHome ) ? input.Prediction.TotalHomeTDs() : input.Prediction.TotalAwayTDs() ) );
				AddKickingPlayerGameMetric( input, kicker.PlayerCode, projFG, projPat );
			}
			else
				Utility.Announce( string.Format( "No kicker found for {0}", teamCode ) );
		}

		private NFLPlayer GetKicker(string teamCode)
		{
			var team = new NflTeam(teamCode);
			team.SetKicker();
			return team.Kicker;
		}

		private void AddKickingPlayerGameMetric(PlayerGameProjectionMessage input,
	                   string playerId, int projFg, int projPat)
		{
			var pgm = new PlayerGameMetrics();
			pgm.PlayerId = playerId;
			pgm.GameKey = input.Game.GameKey();
			pgm.ProjFG = projFg;
			pgm.ProjPat = projPat;
			input.Game.PlayerGameMetrics.Add(pgm);
		}

		#endregion

		#region  Passing

		private void DoPassingUnit( PlayerGameProjectionMessage input, string teamCode, bool isHome )
		{
			var unit = new PassUnit();
			unit.Load( teamCode );

			// give it to the QB
			var projYDp = (int) ( ( ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp ) );
			var projTDp = (int) ( ( ( isHome ) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp ) );
			AddPassinglayerGameMetric( input, unit.Q1.PlayerCode, projYDp, projTDp );
			// Receivers
			var projYDc = (int)(.4 * ((isHome) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp));
			var projTDc = W1TdsFrom((isHome) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp);
			projYDc = AllowForInjuryRisk( unit.W1, projYDc );
			AddCatchingPlayerGameMetric( input, unit.W1.PlayerCode, projYDc, projTDc );
			projYDc = (int)(.25 * ((isHome) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp));
			projTDc = W2TdsFrom((isHome) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp);
			projYDc = AllowForInjuryRisk( unit.W2, projYDc );
			AddCatchingPlayerGameMetric( input, unit.W2.PlayerCode, projYDc, projTDc );
			if (unit.W3 != null)
			{
				projYDc = (int)(.1 * ((isHome) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp));
				projTDc = 0;
				projYDc = AllowForInjuryRisk( unit.W3, projYDc );
				AddCatchingPlayerGameMetric(input, unit.W3.PlayerCode, projYDc, projTDc);
			}
			projYDc = (int)(.25 * ((isHome) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp));
			projTDc = TETdsFrom((isHome) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp);
			projYDc = AllowForInjuryRisk( unit.TE, projYDc );
			AddCatchingPlayerGameMetric(input, unit.TE.PlayerCode, projYDc, projTDc);
		}

		private void AddCatchingPlayerGameMetric(PlayerGameProjectionMessage input, 
			string playerId, int projYDc, int projTDc)
		{
			var pgm = new PlayerGameMetrics();
			pgm.PlayerId = playerId;
			pgm.GameKey = input.Game.GameKey();
			pgm.ProjYDc = projYDc;
			pgm.ProjTDc = projTDc;
#if DEBUG
			//Utility.Announce( pgm.ToString() );
#endif
			input.Game.PlayerGameMetrics.Add(pgm);
		}

		private int W1TdsFrom(int totalTds)
		{
			int tds = 0;
			switch (totalTds)
			{
				case 1:
					tds = 1;
					break;
				case 2:
					tds = 1;
					break;
				case 3:
					tds = 1;
					break;
				case 4:
					tds = 2;
					break;
				case 5:
					tds = 2;
					break;
				case 6:
					tds = 3;
					break;
				default:
					break;
			}
			return tds;
		}

		private int W2TdsFrom(int totalTds)
		{
			int tds = 0;
			switch (totalTds)
			{
				case 1:
					tds = 0;
					break;
				case 2:
					tds = 1;
					break;
				case 3:
					tds = 1;
					break;
				case 4:
					tds = 1;
					break;
				case 5:
					tds = 2;
					break;
				case 6:
					tds = 2;
					break;
				default:
					break;
			}
			return tds;
		}

		private int TETdsFrom(int totalTds)
		{
			int tds = 0;
			switch (totalTds)
			{
				case 1:
					tds = 0;
					break;
				case 2:
					tds = 0;
					break;
				case 3:
					tds = 1;
					break;
				case 4:
					tds = 1;
					break;
				case 5:
					tds = 1;
					break;
				case 6:
					tds = 1;
					break;
				default:
					break;
			}
			return tds;
		}

		private void AddPassinglayerGameMetric( PlayerGameProjectionMessage input, string playerId, int projYDp, int projTDp )
		{
			var pgm = new PlayerGameMetrics();
			pgm.PlayerId = playerId;
			pgm.GameKey = input.Game.GameKey();
			pgm.ProjYDp = projYDp;
			pgm.ProjTDp = projTDp;
#if DEBUG
			Utility.Announce( pgm.ToString() );
#endif
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		#endregion

		#region  Rushing

		private void DoRushingUnit( PlayerGameProjectionMessage input, string teamCode, bool isHome )
		{
			var ru = new RushUnit();
			ru.Load( teamCode );

			if (ru.IsAceBack)
			{
				//  R1
				var percentageOfAction = 0.7M;
				if (ru.R2 == null) percentageOfAction = 0.9M;
				var projYDr = (int)(percentageOfAction * ((isHome) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr));
				//  Injury penalty
				projYDr = AllowForInjuryRisk(ru.AceBack, projYDr);
				var projTDrAce = R1TdsFrom((isHome) ? input.Prediction.HomeTDr : input.Prediction.AwayTDr);
				var isVulture = AllowForVulturing(ru.AceBack.PlayerCode, ref projTDrAce, ru);
				AddPlayerGameMetric(input, ru.AceBack.PlayerCode, projYDr, projTDrAce);
				//  R2 optional
				if (ru.R2 != null)
				{
					projYDr = (int)(.2 * ((isHome) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr));
					projYDr = AllowForInjuryRisk(ru.AceBack, projYDr);
					int projTDr = R2TdsFrom((isHome) ? input.Prediction.HomeTDr : input.Prediction.AwayTDr);
					if (isVulture)
						projTDr = AllowForR2BeingTheVulture(projTDr, ru.R2.PlayerCode, ru);
					AddPlayerGameMetric(input, ru.R2.PlayerCode, projYDr, projTDr);
				}
			}
			else 
			{ 
				//  Comittee
				var percentageOfAction = 0.5M;
				foreach (var runner in ru.Starters)
				{
					var projYDr = (int)(percentageOfAction * ((isHome) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr));
					projYDr = AllowForInjuryRisk(runner, projYDr);
					var projTDr = R1TdsFrom((isHome) ? input.Prediction.HomeTDr : input.Prediction.AwayTDr);

					AddPlayerGameMetric( input, runner.PlayerCode, projYDr, projTDr);			
				}
			}

		}

		private int AllowForR2BeingTheVulture( int projTDr, string r2Id, RushUnit ru )
		{
 			if ( r2Id == ru.GoalLineBack.PlayerCode )
				projTDr++;
			return projTDr;
		}

		private bool AllowForVulturing( string runnerId, ref int projTDr, RushUnit ru )
		{
			bool isVulture = false;

			if ( projTDr > 0 )
			{
				if ( ru.GoalLineBack != null )
				{
					if ( ru.GoalLineBack.PlayerCode != runnerId )
					{
						//  Vulture
						projTDr--;
						isVulture = true;
					}
				}
			}
			return isVulture;
		}

		private int R1TdsFrom( int totalTdr )
		{
			int tdrs = 0;
			switch ( totalTdr )
			{
				case 1:
					tdrs = 1;
					break;
				case 2:
					tdrs = 1;
					break;
				case 3:
					tdrs = 2;
					break;
				case 4:
					tdrs = 3;
					break;
				case 5:
					tdrs = 3;
					break;
				case 6:
					tdrs = 3;
					break;
				default:
					break;
			}
			return tdrs;
		}

		private int R2TdsFrom( int totalTdr )
		{
			int tdrs = 0;
			switch ( totalTdr )
			{
				case 1:
					tdrs = 0;
					break;
				case 2:
					tdrs = 1;
					break;
				case 3:
					tdrs = 1;
					break;
				case 4:
					tdrs = 1;
					break;
				case 5:
					tdrs = 2;
					break;
				case 6:
					tdrs = 2;
					break;
				default:
					break;
			}
			return tdrs;
		}

		#endregion

		private static void AddPlayerGameMetric( PlayerGameProjectionMessage input, string playerId, int projYDr, int projTDr )
		{
			var pgm = new PlayerGameMetrics();
			pgm.PlayerId = playerId;
			pgm.GameKey = input.Game.GameKey();
			pgm.ProjYDr = projYDr;
			pgm.ProjTDr = projTDr;
#if DEBUG
			Utility.Announce( pgm.ToString() );
#endif
			if (input.Game.PlayerGameMetrics == null) input.Game.PlayerGameMetrics = new List<PlayerGameMetrics>();
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		public static int AllowForInjuryRisk( NFLPlayer p, int proj )
		{

#if DEBUG
			if ( p == null )
				Utility.Announce("AllowForInjuryRisk:Null Player");
#endif
			var nInjury = 0;
			Int32.TryParse( p.Injuries(), out nInjury );
			if ( nInjury > 0 )
			{
				decimal injChance = ( ( nInjury * 10.0M ) / 100.0M );
				decimal effectiveness = 1 - injChance;
				proj = (int) ( proj * effectiveness );
			}
			return proj;
		}

	}
}
