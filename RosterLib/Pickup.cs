using System;

namespace RosterLib
{
	public class Pickup
	{
		public string LeagueId { get; set; }
		public string Owner { get; set; }
		public string Suffix { get; set; }
		public string Prefix { get; set; }
		public string Season { get; set; }
        public NFLPlayer Player { get; set; }
        public string Name { get; set; }
		public string Opp { get; set; }
		public decimal ProjPts { get; set; }
		public string ActualPts { get; set; }
		public string CategoryCode { get; set; }
		public string  Pos { get; set; }

		public string Category()
		{
			string s = RealCatCode();
			if (s == "1")
				return "QUARTERBACKS";
			if (s == "2")
				return "RUNNING BACKS";
			if (s == "3")
				return "RECEIVERS";
			if (s == "4")
				return "TIGHT ENDS";
			if (s == "5")
				return "KICKERS";
			return "Not defined";
		}

		public string RealCatCode()
		{
			var catOut = "5";
			switch ( CategoryCode )
			{
				case Constants.K_QUARTERBACK_CAT:
					catOut = "1";
					break;

				case "2":
					catOut = "2";
					break;

				case "3":
					if ( Pos.Contains( "TE" ) )
						catOut = "4";
					else
						catOut = "3";
					break;

			}
			return catOut;
		}

		public decimal SortPoints
		{
			get
			{
				return ( ( 10.0M - Decimal.Parse( RealCatCode() ) ) * 100.0M ) + ProjPts;
			}
		}

		public override string ToString()
		{
			return $@"{
				ProjectionLink(),-36
				}    {
				Opp,-10
				} {
				Prefix
				}{
				ProjPts,5
				}{
				Suffix
				} {
				ActualPts
				}";
		}

        private string ProjectionLink()
        {
			UpperCaseStevesPlayers();
            string url = Name.PadRight( 37, ' ' ).Substring( 0, 37 );
            if ( Player.IsPlayerProjection(Season) )
            {
                url = $"<a href =\"..//..//PlayerProjections/{Player.PlayerCode}.htm\">{Name}</a>";
            }
            return url;
        }

		private void UpperCaseStevesPlayers()
		{
			if (OwnedBySteve())
				Name = Name.ToUpper();
			if (Player.IsPlayoffBound())
				Name = $"+{Name}";
			else
				Name = $" {Name}";
		}

		private bool OwnedBySteve()
		{
			if (LeagueId == Constants.K_LEAGUE_Yahoo)
				return Owner == "77";
			if (LeagueId == Constants.K_LEAGUE_Gridstats_NFL1)
				return Owner == "CC";
			if (LeagueId == Constants.K_LEAGUE_Rants_n_Raves)
				return Owner == "BZ";
			return false;
		}
	}
}
