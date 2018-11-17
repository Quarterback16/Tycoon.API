using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class PlayerGameMetrics
	{
		public string PlayerId { get; set; }
		public string GameKey { get; set; }

		public int ProjTDp { get; set; }
		public int TDp  { get; set; }
		public int ProjTDr  { get; set; }
		public int TDr  { get; set; }
		public int ProjTDc  { get; set; }
		public int TDc  { get; set; }
		public int ProjYDp  { get; set; }
		public int YDp  { get; set; }
		public int ProjYDr  { get; set; }
		public int YDr  { get; set; }
		public int ProjYDc  { get; set; }
		public int YDc  { get; set; }
		public int ProjFG{ get; set; }
		public int FG { get; set; }
		public int ProjPat { get; set; }
		public int Pat { get; set; }

		public override string ToString()
		{
			return string.Format( "{0} in {1} projection passing {2}-({3}) running {4}-({5}) catch {6}-({7}) kick {8}-{9}",
				PlayerId, GameKey, ProjYDp, ProjTDp, ProjYDr, ProjTDr, ProjYDc, ProjTDc, ProjFG, ProjPat );
		}

		public void Save( IPlayerGameMetricsDao dao )
		{
			dao.Save( this );
		}

		public string Week()
		{
			return GameKey.Substring( 5, 2 );
		}
	}
}
