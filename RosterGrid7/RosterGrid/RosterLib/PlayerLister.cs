using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RosterLib
{
	/// <summary>
	/// Summary description for PlayerLister.
	/// </summary>
	public class PlayerLister
	{
		public string  FileOut { get; set; }

		public ArrayList PlayerList;
		private IRatePlayers _mMyScorer;
		private string _mFormat = string.Empty;

		public bool AllWeeks { get; set; }
		public int WeeksToGoBack { get; set; }

		public string SortOrder { get; set; }
		public string CatCode { get; set; }
		public string Position { get; set; }
		public string FantasyLeague { get; set; }

		public string Season { get; set; }
		public int Week { get; set; }

		public string SubHeader { get; set; }

		public bool RenderToCsv { get; set; }

		public bool PlayoffsOnly { get; set; }

		public bool StartersOnly { get; set; }

		public bool OnesAndTwosOnly { get; set; }

		public bool FreeAgentsOnly { get; set; }

		public bool ActivesOnly { get; set; }

		public bool PrimariesOnly { get; set; }

		public TeamCheckList Tc { get; set; }

		public PlayerLister(string catCode,
		                    bool faOnly,
		                    [Optional] string fantasyLeague,
		                    [Optional] bool startersOnly
			)
		{
			PrimariesOnly = true;
			ActivesOnly = true;
			FreeAgentsOnly = false;
			StartersOnly = startersOnly;
			PlayerList = new ArrayList();
			var ds = Utility.TflWs.GetPlayers(catCode);
			var dt = ds.Tables[0];
			foreach (DataRow dr in dt.Rows)
			{
				var p = new NFLPlayer(dr, fantasyLeague);
				var bAdd = !(faOnly) || p.IsFreeAgent();
				if (ActivesOnly)
					bAdd = (bAdd) && p.IsActive();
				if (StartersOnly)
					bAdd = (bAdd) && p.IsStarter();
				if (PlayoffsOnly)
					bAdd = (bAdd) && p.IsPlayoffBound();
				if (PrimariesOnly)
					bAdd = (bAdd) && ! p.IsItalic(); //  dont want FB, TE or punters
				if ( OnesAndTwosOnly )
					bAdd = ( bAdd ) && p.IsOneOrTwo(); 

				if (bAdd)
					PlayerList.Add(p);
			}
			WeeksToGoBack = Constants.K_WEEKS_IN_A_SEASON; //  default
		}

		public PlayerLister()
		{
			PrimariesOnly = true;
			ActivesOnly = true;
			FreeAgentsOnly = false;
			Tc = new TeamCheckList();
			PlayerList = new ArrayList();

			//Collect(catCode, sPos, fantasyLeague, startersOnly, tc);

			WeeksToGoBack = Constants.K_WEEKS_IN_A_SEASON; // default
		}

		public void Clear()
		{
			PlayerList.Clear();
		}

		public void Collect(string catCode, string sPos, string fantasyLeague )
		{
			Clear();
			DataSet ds;
			if (string.IsNullOrEmpty(sPos))
				ds = Utility.TflWs.GetPlayers(catCode);
			else
				ds = sPos.Equals("KR")
				     	? Utility.TflWs.GetReturners()
				     	: Utility.TflWs.GetPlayers(catCode, sPos);

			var dt = ds.Tables[0];
			foreach (DataRow dr in dt.Rows)
			{
				if (dr.RowState != DataRowState.Deleted)
				{
					var p = new NFLPlayer(dr, fantasyLeague);

					var bAdd = true;
					if (FreeAgentsOnly) bAdd = p.IsFreeAgent();
					if (PlayoffsOnly) bAdd = (bAdd) && p.IsPlayoffBound();
					bAdd = (bAdd) && p.IsFantasyOffence();
					bAdd = (bAdd) && p.IsActive();
					if (StartersOnly)
						bAdd = (bAdd) && p.IsStarter();
					if ( OnesAndTwosOnly )
						bAdd = ( bAdd ) && p.IsOneOrTwo(); 

					if (bAdd)
					{
						AnnounceAdd(catCode, sPos, p);

						PlayerList.Add(p);
						if (StartersOnly)
						{
							if (sPos != null)
								if (sPos != "WR") Tc.TickOff(p.TeamCode, sPos); //  there r 2 WRs
						}
					}
				}
			}
			AnnounceTotal(sPos);
		}

		[Conditional( "DEBUG" )]
		private void AnnounceTotal(string sPos)
		{
			Utility.Announce(
				string.Format("PlayerLister.init {0} {1} players added to the list",
				              PlayerList.Count, sPos));
		}

		[Conditional("DEBUG")]
		private static void AnnounceAdd(string catCode, string sPos, NFLPlayer p)
		{
			Utility.Announce(
				string.Format("PlayerLister.Collect Adding {0,-12}-{3}-{4} to {1} - {2} list",
				              p.PlayerNameShort, catCode, sPos, p.CurrTeam.TeamCode, p.PlayerRole));
		}

		public void Load()
		{
			var tc = new TeamCheckList();
			PlayerList = new ArrayList();
			var ds = Utility.TflWs.GetPlayers(CatCode, Position);
			var dt = ds.Tables[0];
#if DEBUG
			Utility.Announce( string.Format( "{0} candidate players", dt.Rows.Count ) );
#endif
			foreach (DataRow dr in dt.Rows)
			{
				if (dr.RowState == DataRowState.Deleted) continue;

				var p = new NFLPlayer(dr, FantasyLeague);
				var bAdd = true;
				if (FreeAgentsOnly)
				{
					if (! string.IsNullOrEmpty(FantasyLeague))
						//  lookup owner
					{
						if (p.Owner.Equals("**"))
						{
#if DEBUG
							Utility.Announce(string.Format("  Player {0,-15} owned by {1} playoffs {2} starter {3}  active {4}",
							                               p.PlayerNameShort, p.Owner, (p.IsPlayoffBound() ? "Yes" : "No "),
							                               (p.IsStarter() ? "Yes" : "No "), (p.IsActive() ? "Yes" : "No ")));
#endif
						}
						else
							bAdd = false;
					}
				}
				if ( FantasyLeague == Constants.K_LEAGUE_Gridstats_NFL1 )
					bAdd = (bAdd) && p.IsPlayoffBound();
				bAdd = (bAdd) && p.IsActive();
				if (StartersOnly)
					bAdd = (bAdd) && p.IsStarter();

				if (bAdd)
				{
#if DEBUG
					//Utility.Announce(string.Format("    Adding Player {0,-15}", p.PlayerNameShort));
#endif
					PlayerList.Add(p);
					if (Position != "WR") tc.TickOff(p.TeamCode, Position); //  there r 2 WRs
				}
			}
			WeeksToGoBack = Constants.K_WEEKS_IN_A_SEASON; // default
#if DEBUG
			Utility.Announce(string.Format("PlayerLister.init {0} {1} players added to the list", PlayerList.Count,
			                                         Position));
			Utility.Announce(string.Format("Teams missing {1} are {0}", tc.TeamsLeft(), Position));
#endif
		}

		public void SetScorer(IRatePlayers ss)
		{
			_mMyScorer = ss;
		}

		public void SetFormat(string theFormat)
		{
			_mFormat = theFormat;
		}

		public string Render(string header)
		{
			if (_mFormat.Equals("weekly"))
			{
				var html = new RenderStatsToWeekly(_mMyScorer)
				           	{CurrentSeasonOnly = true, FullStart = AllWeeks};
				FileOut = html.RenderData(PlayerList, header, _mMyScorer.Week);
			}
			else
			{
				var html = new RenderStatsToHtml
				           	{
				           		RenderToCsv = RenderToCsv,
				           		Season = Season,
				           		Week = Week,
				           		WeeksToGoBack = WeeksToGoBack
				           	};
				if (! string.IsNullOrEmpty(SubHeader)) html.SubHeader = SubHeader;
                html.FileOut = string.Format("{0}{2}\\{1}.htm",
                    Utility.OutputDirectory(), header, Utility.CurrentSeason());
				FileOut = html.RenderData(PlayerList, header, SortOrder, _mMyScorer);
			}
			return FileOut;
		}

		public string RenderProjection(string header)
		{
			var html = new RenderStatsToHtml
			           	{
			           		RenderToCsv = RenderToCsv,
			           		Season = Season,
			           		Week = Week,
			           		LongStats = true,
			           		WeeksToGoBack = WeeksToGoBack
			           	};
			if (!string.IsNullOrEmpty(SubHeader)) html.SubHeader = SubHeader;

			FileOut = html.RenderProjectedData(PlayerList, header, SortOrder, _mMyScorer);

			return FileOut;
		}

		public void RenderReturners([Optional] string season)
		{
			if (_mFormat.Equals("weekly"))
			{
				var html = new RenderStatsToWeekly(_mMyScorer)
				           	{CurrentSeasonOnly = true, FullStart = AllWeeks};
				html.RenderData(PlayerList, season, _mMyScorer.Week);
				FileOut = html.FileOut;
			}
			else
			{
				var html = new RenderStatsToHtml
				           	{
				           		RenderToCsv = RenderToCsv,
				           		Season = Season,
				           		Week = Week,
				           		WeeksToGoBack = WeeksToGoBack,
				           		LongStats = false,
				           		SupressZeros = false
				           	};
				if (! string.IsNullOrEmpty(SubHeader)) html.SubHeader = SubHeader;

				html.FileOut = string.Format( "{0}{1}//Returners//{1}.htm", Utility.OutputDirectory(), season );

				html.RenderData(PlayerList, season, SortOrder, _mMyScorer);
				FileOut = html.FileOut;
			}
		}

        public void Render()
        {
            Render(FileOut);
        }
    }
}