using System;
using TipIt.Implementations;
using TipIt.Interfaces;
using TipIt.Models;

namespace TipIt.TippingStrategies
{
    public class TableTipster : BaseTipster, ITipster
    {
        public string[] RankedTeams { get; set; }
        public TableTipster(TippingContext context) 
            :  base( context)
        {
            RankedTeams = new string[16]
            {
                "MELB",
                "SYDR",
                "SSYD",
                "CANB",
                "PARR",
                "MANL",
                "SHRK",
                "BRIS",
                "WTIG",
                "PENR",
                "NEWC",
                "BULL",
                "NZW",
                "NQLD",
                "DRAG",
                "TITN"
            };
        }

        public string ShowTips(
            string league, 
            int round)
        {
            if (league == "NRL")
            {
                RankedTeams = new string[16]
                {
                    "MELB",
                    "SYDR",
                    "SSYD",
                    "CANB",
                    "PARR",
                    "MANL",
                    "SHRK",
                    "BRIS",
                    "WTIG",
                    "PENR",
                    "NEWC",
                    "BULL",
                    "NZW",
                    "NQLD",
                    "DRAG",
                    "TITN"
                };
            }
            else
            {
                RankedTeams = new string[18]
                {
                    "GEEL",
                    "BL",
                    "RICH",
                    "COLL",
                    "WCE",
                    "GWS",
                    "WB",
                    "ESS",
                    "HAW",
                    "PORT",
                    "ADEL",
                    "NMFC",
                    "FRE",
                    "STK",
                    "SYD",
                    "CARL",
                    "MELB",
                    "GCFC"
                };
            }
            Tip(league, round);
            return Output();
        }

        public void Tip(
            string league, 
            int round)
        {
            Predictions.Clear();
            var sched = Context.LeagueSchedule[league][round];
            foreach (var game in sched)
            {
                var prediction = new PredictedResult(game);
                //  just uses table position to predict
                var homeRank = GetRank(game.HomeTeam);
                var awayRank = GetRank(game.AwayTeam);
                if (homeRank < awayRank)
                {
                    prediction.HomeWin = true;
                    prediction.AwayWin = false;
                }
                else
                {
                    prediction.HomeWin = false;
                    prediction.AwayWin = true;
                }
                Predictions.Add(prediction);
            }
        }

        private int GetRank(string team)
        {
            return Array.IndexOf(RankedTeams, team);
        }


    }
}
