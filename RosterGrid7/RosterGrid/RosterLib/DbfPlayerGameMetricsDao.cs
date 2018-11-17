using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RosterLib
{
	public class DbfPlayerGameMetricsDao : IPlayerGameMetricsDao
	{
		public PlayerGameMetrics Get( string playerCode, string gameCode )
		{
			PlayerGameMetrics pgm = null;
			
			var ds = Utility.TflWs.GetPlayerGameMetrics( playerCode, gameCode );

			if ( ds.Tables[ 0 ].Rows.Count == 1 )
			{
				pgm = new PlayerGameMetrics();
				pgm.PlayerId = playerCode;
				pgm.GameKey = gameCode;
				pgm.ProjTDp = IntValue( ds, "projtdp" );
				pgm.ProjTDr = IntValue( ds, "projtdr" );
				pgm.ProjTDc = IntValue( ds, "projtdc" );
				pgm.ProjYDp = IntValue( ds, "projydp" );
				pgm.ProjYDr = IntValue( ds, "projydr" );
				pgm.ProjYDc = IntValue( ds, "projydc" );
				pgm.ProjFG = IntValue(ds, "projfg");
				pgm.ProjPat = IntValue(ds, "projpat");
			}
			return pgm;
		}

		private static int IntValue( System.Data.DataSet ds, string fieldName )
		{
			return Int32.Parse( ds.Tables[ 0 ].Rows[ 0 ][ fieldName ].ToString() );
		}

		public void Save( PlayerGameMetrics pgm )
		{
			var oldPgm = Get( pgm.PlayerId, pgm.GameKey );
			if ( oldPgm == null )
				Utility.TflWs.InsertPlayerGameMetric( 
					      pgm.PlayerId, pgm.GameKey,
							pgm.ProjYDp, pgm.YDp, pgm.ProjYDr, pgm.YDr,
							pgm.ProjTDp, pgm.TDp, pgm.ProjTDr, pgm.TDr, pgm.ProjTDc, pgm.TDc, pgm.ProjYDc, pgm.YDc,
							pgm.ProjFG, pgm.FG, pgm.ProjPat, pgm.Pat
					);
			else
				Utility.TflWs.UpdatePlayerGameMetric(
							pgm.PlayerId, pgm.GameKey,
							pgm.ProjYDp, pgm.YDp, pgm.ProjYDr, pgm.YDr,
							pgm.ProjTDp, pgm.TDp, pgm.ProjTDr, pgm.TDr, pgm.ProjTDc, pgm.TDc, pgm.ProjYDc, pgm.YDc,
							pgm.ProjFG, pgm.FG, pgm.ProjPat, pgm.Pat
					);
		}


		public List<PlayerGameMetrics> GetWeek( string season, string week )
		{
			var pgmList = new List<PlayerGameMetrics>();
			DataSet ds = Utility.TflWs.GetAllPlayerGameMetrics( season, week );
			DataTable dt = ds.Tables[ 0 ];
			foreach ( DataRow dr in dt.Rows )
			{
				var pgm = new PlayerGameMetrics();
				pgm.PlayerId = dr["PLAYERID"].ToString();
				pgm.GameKey = dr["GAMECODE"].ToString();
				pgm.ProjTDp = IntValue( dr, "projtdp" );
				pgm.ProjTDr = IntValue( dr, "projtdr" );
				pgm.ProjTDc = IntValue( dr, "projtdc" );
				pgm.ProjYDp = IntValue( dr, "projydp" );
				pgm.ProjYDr = IntValue( dr, "projydr" );
				pgm.ProjYDc = IntValue( dr, "projydc" );
				pgm.ProjFG = IntValue( dr, "projfg" );
				pgm.ProjPat = IntValue( dr, "projpat" );
				pgmList.Add( pgm );
			}
#if DEBUG
			Utility.Announce(string.Format("Metric records loaded : {0}", pgmList.Count ));
#endif
			return pgmList;
		}

		private static int IntValue( System.Data.DataRow dr, string fieldName )
		{
			return Int32.Parse( dr[ fieldName ].ToString() );
		}

		public void ClearGame( string gameKey )
		{
			Utility.TflWs.ClearPlayerGameMetrics( gameKey );
		}
	}
}
