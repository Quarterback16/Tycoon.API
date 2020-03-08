using System;

namespace TipIt.Tests
{
    public class BaseTests
    {
        protected dynamic ConvertAflTeam(string teamName)
        {
            if (teamName == "Carlton")
                return "CARL";
            if (teamName == "Essendon")
                return "ESS";
            if (teamName == "Richmond")
                return "RICH";
            if (teamName == "Geelong Cats")
                return "GEEL";
            if (teamName == "Brisbane Lions")
                return "BL";
            if (teamName == "Collingwood")
                return "COLL";
            if (teamName == "West Coast Eagles")
                return "WCE";
            if (teamName == "GWS Giants")
                return "GWS";
            if (teamName == "Western Bulldogs")
                return "WB";
            if (teamName == "Hawthorn")
                return "HAW";
            if (teamName == "Port Adelaide")
                return "PORT";
            if (teamName == "Adelaide Crows")
                return "ADEL";
            if (teamName == "North Melbourne")
                return "NMFC";
            if (teamName == "Fremantle")
                return "FRE";
            if (teamName == "St Kilda")
                return "STK";
            if (teamName == "Sydney Swans")
                return "SYD";
            if (teamName == "Melbourne")
                return "MELB";
            if (teamName == "Gold Coast Suns")
                return "GCFC";
            return teamName;
        }

        protected dynamic ConvertNrlTeam(string teamName)
        {
            if (teamName == "Titans")
                return "TITN";
            if (teamName == "Dragons")
                return "DRAG";
            if (teamName == "Cowboys")
                return "NQLD";
            if (teamName == "Warriors")
                return "NZW";
            if (teamName == "Bulldogs")
                return "BULL";
            if (teamName == "Knights")
                return "NEWC";
            if (teamName == "Panthers")
                return "PENR";
            if (teamName == "Wests Tigers")
                return "WTIG";
            if (teamName == "Broncos")
                return "BRIS";
            if (teamName == "Sharks")
                return "SHRK";
            if (teamName == "Sea Eagles")
                return "MANL";
            if (teamName == "Eels")
                return "PARR";
            if (teamName == "Raiders")
                return "CANB";
            if (teamName == "Rabbitohs")
                return "SSYD";
            if (teamName == "Storm")
                return "MELB";
            if (teamName == "Roosters")
                return "SYDR";
            return teamName;
        }

        protected string ConvertDate(string gameDate)
        {
            var theDate = DateTime.Parse(gameDate);
            return theDate.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
