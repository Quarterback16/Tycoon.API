using RosterLib.Interfaces;

namespace RosterLib
{
	public class CommitteeAllocateTDrStrategy : IAllocateTDrStrategy
	{
		public void Allocate(
			RushUnit ru,
			int nTDr,
			PlayerGameMetricsCollection pgms)
		{
			if (nTDr == 0)
				return;

			foreach ( var starter in ru.Starters )
			{
				int tdsLostToLeakage = nTDr % 2;
				nTDr -= tdsLostToLeakage;
				if (nTDr == 0)
					break;
				var pgm = pgms.GetPgmFor( 
					starter.PlayerCode );
				pgm.ProjTDr += nTDr / ru.Starters.Count;  // 50-50
				pgms.Update( 
					pgm );
			}
		}
	}
}