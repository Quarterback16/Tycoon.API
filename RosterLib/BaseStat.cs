﻿using System;
using System.Data;

namespace RosterLib
{
   public abstract class BaseStat : IStat
    {
        public abstract string Name { get; }
        public string PlayerId { get; set; }
        public string TeamId { get; set; }
		public string OpponentTeamId { get; set; }
		public decimal Quantity { get; set; }
        public string Season { get; set; }
        public string Week { get; set; }
        public string GameNo { get; set; }
        public string StatType { get; set; }

        public string Error { get; set; }

        public void Dump()
        {
			var game = new NFLGame(
				gameKey: $"{Season}:{Week:00}-{GameNo}");
			OpponentTeamId = game.Opponent(TeamId);

			Utility.Announce(
				$@"{Season}:{Week}-{GameNo} {TeamId} {
					PlayerId
					} has {Quantity} {Name} vs {OpponentTeamId}");
        }

        public bool IsValid()
        {
            bool isValid = true;

            string allTeams = "CH AF DC AC DL CP NG SF GB NO PE LR MV TB WR SS BR HT BB DB CI IC MD KC CL JJ NE OR PS TT NJ LC";
            isValid = (allTeams.IndexOf(TeamId) > -1);
            Error = (isValid ? string.Empty : "Invalid Team");

            if (isValid)
            {
                string allGames = "A B C D E F G H I J K L M N O P";
                isValid = (allGames.IndexOf( GameNo ) > -1 );
                Error = (isValid ? string.Empty : "Invalid Game code");
            }

            if (isValid)
            {
                string allStats = "QYRCASZPGM";
                isValid = (allStats.IndexOf( StatType ) > -1);
                Error = (isValid ? string.Empty : "Invalid Stat code");
            }

            return isValid;
        }

        public virtual bool IsReasonable()
        {
            return false;
        }

        public void Load( DataRow dr)
        {
            Season = dr["SEASON"].ToString();
            Week = dr["WEEK"].ToString();
            GameNo = dr["GAMENO"].ToString();
            TeamId = dr["TEAMID"].ToString();
            PlayerId = dr["PLAYERID"].ToString();
            StatType = dr["STAT"].ToString();
            Quantity = Decimal.Parse(dr["QTY"].ToString());
        }
    }
}
