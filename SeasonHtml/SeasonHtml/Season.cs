using System;
using System.Collections.Generic;
using System.Text;

namespace SeasonHtml
{
	public class Season
	{
		const string K_TflOutputFolder = "tfl-out";

		public int Year { get; set; }
		public HtmlFile HtmlFile { get; set; }

		public Season( int year )
		{
			Year = year;
		}

		public void Render()
		{
			//  output the file
			HtmlFile = new HtmlFile(
				FileName(),
				$"{Year} Season reports");
			HtmlFile.AddToHead(Header());
			Body();
			HtmlFile.AddToBody(Footer());
			HtmlFile.Render();
		}

		private string FileName()
		{
			return $"nfl-{Year}.htm";
		}

		private string LastTwoYear()
		{
			return Year.ToString().Substring(2,2);
		}

		private void Body()
		{
			HtmlFile.AddToBody(Heading());
			HtmlFile.AddToBody(HtmlLib.DivOpenId("myAccordian"));
			HtmlFile.AddToBody(HtmlLib.DivOpen(string.Empty));

			HtmlFile.AddToBody(PreseasonReports());

			HtmlFile.AddToBody(NflTeamReports());
			HtmlFile.AddToBody(RostersByPosition());

			HtmlFile.AddToBody(WeeklyReports());

			HtmlFile.AddToBody(HtmlLib.DivClose());
			HtmlFile.AddToBody(HtmlLib.DivClose());
		}

		private string RostersByPosition()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.DivOpen(string.Empty));
			AddLine(sb, SubHeading("Rosters by Position"));
			AddLine(sb, HtmlLib.TableWithBorderOpen());
			string[] headerParams = { "Scope", "QB", "RB", "WR", "TE", "PK" };
			AddLine(sb, TableHeader(headerParams));
			AddLine(sb, RosterLine("G1-Hotlist", "G1"));
			AddLine(sb, RosterLine("ESPN-Hotlist", "RR"));
			AddLine(sb, OldRosters("G1"));
			AddLine(sb, OldRosters("YH"));
			AddLine(sb, Starters("G1"));
			AddLine(sb, Rookies("G1"));

			AddLine(sb, HtmlLib.TableClose());
			AddLine(sb, HtmlLib.DivClose());
			return sb.ToString();
		}

		private string Rookies(string league)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"{league}-rookies"));
			AddLine(sb, HtmlLib.TableData(RookieLink("QB", league)));
			AddLine(sb, HtmlLib.TableData(RookieLink("RB", league)));
			AddLine(sb, HtmlLib.TableData(RookieLink("WR", league)));
			AddLine(sb, HtmlLib.TableData(RookieLink("TE", league)));
			AddLine(sb, HtmlLib.TableData(RookieLink("PK", league)));
			return sb.ToString();
		}

		private string RookieLink(string playerType, string league)
		{
			//file:///W:/medialists/dropbox/gridstat/2018/Rookies/G1-Rookies-QB.htm
			return HtmlLib.Href(
				$"..\\{Year}\\Rookies\\{league}-Rookies-{playerType}.htm",
				$"{playerType}");
		}

		private string Starters(string league)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"{league}-Starters"));
			AddLine(sb, HtmlLib.TableData(StarterLink("QB", league)));
			AddLine(sb, HtmlLib.TableData(StarterLink("RB", league)));
			AddLine(sb, HtmlLib.TableData(StarterLink("WR", league)));
			AddLine(sb, HtmlLib.TableData(StarterLink("TE", league)));
			AddLine(sb, HtmlLib.TableData(StarterLink("PK", league)));
			return sb.ToString();
		}

		private string StarterLink(string playerType, string league)
		{
			return HtmlLib.Href(
				$"..\\{Year}\\Starters\\{league}-Starters-{playerType}.htm",
				$"{playerType}");
		}

		private string OldRosters(string league)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"{league}-{Year}"));
			AddLine(sb, HtmlLib.TableData(OldRosterLink("QB", league)));
			AddLine(sb, HtmlLib.TableData(OldRosterLink("RB", league)));
			AddLine(sb, HtmlLib.TableData(OldRosterLink("WR", league)));
			AddLine(sb, HtmlLib.TableData(OldRosterLink("TE", league)));
			AddLine(sb, HtmlLib.TableData(OldRosterLink("PK", league)));
			return sb.ToString();
		}

		private string OldRosterLink(string playerType, string league)
		{
			return HtmlLib.Href(
				$"..\\{Year}\\Rosters\\{league}\\Roster-{playerType}.htm",
				$"{playerType}");
		}

		private string RosterLine(
			string lineHeader,
			string fantasyLeague)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData(lineHeader));
			AddLine(sb, HtmlLib.TableData(RosterLink("QB",fantasyLeague)));
			AddLine(sb, HtmlLib.TableData(RosterLink("RB", fantasyLeague)));
			AddLine(sb, HtmlLib.TableData(RosterLink("WR", fantasyLeague)));
			AddLine(sb, HtmlLib.TableData(RosterLink("TE", fantasyLeague)));
			AddLine(sb, HtmlLib.TableData(RosterLink("PK", fantasyLeague)));
			return sb.ToString();
		}

		private string RosterLink(
			string playerType,
			string fantasyLeague)
		{
			return HtmlLib.Href(
				$"..\\{Year}\\HotLists\\HotList-{fantasyLeague}-{playerType}.htm",
				$"{playerType}");
		}

		private string TableHeader(string[] headerParams)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			foreach (var item in headerParams)
			{
				AddLine(sb, HtmlLib.TableHeader(item));
			}
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string TallyStats(StringBuilder sb)
		{
			AddLine(sb, ScopeHeader("TFL Tally Stats"));
			AddLine(sb, TallyLine("Carries","R"));
			AddLine(sb, TallyLine("Rushing Yards", "Y"));
			AddLine(sb, TallyLine("Passing Yards", "S"));
			AddLine(sb, TallyLine("Reception Yards", "G"));
			AddLine(sb, TallyLine("Interceptions", "M"));
			AddLine(sb, TallyLine("Sacks", "Q"));
			return sb.ToString();
		}

		private string TallyLine(string tallyName, string tally)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData(tallyName));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(TallyLink(i, tally)));
			AddLine(sb, HtmlLib.TableRowClose());
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"{tallyName}-YTD"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(TallyLink(i, tally, true)));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string TallyLink(int i, string tally, bool isYtd = false)
		{
			var link = $"..\\{K_TflOutputFolder}\\{tally}\\ZT{tally}{LastTwoYear()}{i:0#}";
			if (isYtd)
				link = $"{link}Y";
			return HtmlLib.Href(
				link + ".txt",
				$"{i:0#}");
		}

		private string NflTeamReports()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.DivOpen(string.Empty));
			AddLine(sb, SubHeading("NFL Team and Unit Reports"));
			AddLine(sb, HtmlLib.TableWithBorderOpen());
			AddLine(sb, ConferenceRow("NFC"));
			AddLine(sb, DivisionRow());
			AddLine(sb, NfcTeamCardRow());
			AddLine(sb, ConferenceRow("AFC"));
			AddLine(sb, DivisionRow());
			AddLine(sb, AfcTeamCardRow());
			AddLine(sb, HtmlLib.TableClose());
			AddLine(sb, HtmlLib.DivClose());
			return sb.ToString();
		}

		private string AfcTeamCardRow()
		{
			var sb = new StringBuilder();
			TeamCells(sb, new string[] { "BR", "HT", "BB", "DB" });
			TeamCells(sb, new string[] { "CI", "IC", "MD", "KC" });
			TeamCells(sb, new string[] { "CL", "JJ", "NE", "OR" });
			TeamCells(sb, new string[] { "PS", "TT", "NJ", "LC" });
			return sb.ToString();
		}

		private string NfcTeamCardRow()
		{
			var sb = new StringBuilder();
			TeamCells(sb, new string[] { "CH", "AF", "DC", "AC" });
			TeamCells(sb, new string[] { "DL", "CP", "NG", "SF" });
			TeamCells(sb, new string[] { "GB", "NO", "PE", "LR" });
			TeamCells(sb, new string[] { "MV", "TB", "WR", "SS" });
			return sb.ToString();
		}

		private void TeamCells(StringBuilder sb, string[] teamCode)
		{
			AddLine(sb, HtmlLib.TableRowOpen());
			foreach (var team in teamCode)
			{
				RosterRpt(sb, team);
			}
			AddLine(sb, HtmlLib.TableRowClose());
		}

		private void RosterRpt(
			StringBuilder sb,
			string teamCode)
		{
			AddLine(
				sb,
				HtmlLib.TableDataAttr(
					strData: HtmlLib.Href(
						fileLink: TflRosterReportFileName(
							teamCode,
							offset: -1),
						label: "<"),
					strAttr: "align='center' width='20'"));
			AddLine(
				sb,
				HtmlLib.TableDataAttr(
					strData: HtmlLib.Href(
						fileLink: ButlerDepthChartFileName(teamCode),
						label: teamCode),
					strAttr: "align='center' colspan='4' width='100'"));
			AddLine(
				sb,
				HtmlLib.TableDataAttr(
					strData: HtmlLib.Href(
						fileLink: TflRosterReportFileName(
							teamCode,
							offset: 0),
						label: ">"),
					strAttr: "align='center' width='20'"));
		}

		private string ButlerDepthChartFileName(string teamCode)
		{
			return $@"../{
				Year
				}/DepthCharts/{
				teamCode
				}.htm";
		}

		private string TflRosterReportFileName(
			string teamCode,
			int offset)
		{
			return $@"..\\{
				K_TflOutputFolder
				}\\ZREP{
				teamCode
				}{
				YearSuffix(offset)
				}.TXT";
		}

		private string YearSuffix(int offset)
		{
			var theYear = Year + offset;
			return theYear.ToString().Substring(2, 2);
		}

		private string ConferenceRow(string conference)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableDataAttr(
				conference,
				"colspan='24' align='center'"));
			AddLine(sb, HtmlLib.TableDataClose());
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string DivisionRow()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			DivisionHeader(sb, "North");
			DivisionHeader(sb, "South");
			DivisionHeader(sb, "East");
			DivisionHeader(sb, "West");
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private static void DivisionHeader(
			StringBuilder sb,
			string division)
		{
			AddLine(sb, HtmlLib.TableDataAttr(
				division,
				"colspan='6' align='center'"));
			AddLine(sb, HtmlLib.TableDataClose());
		}

		private string WeeklyReports()
		{
			var sb = new StringBuilder();
			AddLine(sb,	SubHeading("Weekly Reports"));
			AddLine(sb, HtmlLib.TableWithBorderOpen());
			AddLine(sb, HtmlLib.TableHeaderOpen());
			AddLine(sb, ScopeHeader("Scope"));

			AddLine(sb, PickupCharts());
			AddLine(sb, PickupSummaries());
			AddLine(sb, RankingMetrics());
			AddLine(sb, PlayerProjections("QB"));
			AddLine(sb, PlayerProjections("RB"));
			AddLine(sb, PlayerProjections("WR"));
			AddLine(sb, PlayerProjections("TE"));
			AddLine(sb, PlayerProjections("PK"));
			AddLine(sb, NibbleTips());
			AddLine(sb, Standings());
			AddLine(sb, Performance("QB"));
			AddLine(sb, Performance("RB"));
			AddLine(sb, Performance("WR"));
			AddLine(sb, Performance("TE"));
			AddLine(sb, Performance("PK"));
			DefensiveReports(sb);
			TallyStats(sb);

			AddLine(sb, HtmlLib.TableClose());
			return sb.ToString();
		}

		private string DefensiveReports(StringBuilder sb)
		{
			AddLine(sb, ScopeHeader("Defensive Reports"));
			AddLine(sb, TopDefense());
			return sb.ToString();
		}

		private string TopDefense()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData("In Form D"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(DefenseLink(i, "Defensive Scoring")));
			AddLine(sb, HtmlLib.TableRowClose());
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"Patsies"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(DefenseLink(i, "Team to beat")));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string DefenseLink(int w, string linkName)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\defense\\{linkName}-{w:0#}.htm",
				$"{w:0#}"));
			return sb.ToString();
		}

		private string Standings()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData("Standings"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(Standing(i)));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string NibbleTips()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"Nibble tips"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(NibbleTip(i)));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string NibbleTip(int i)
		{
			return HtmlLib.Href(
				$"..\\{K_TflOutputFolder}\\ZTIPS{i:0#}.txt",
				$"{i:0#}");
		}

		private string Standing(int i)
		{
			return HtmlLib.Href(
				$"..\\{K_TflOutputFolder}\\ZSTD{LastTwoYear()}{i:0#}.txt",
				$"{i:0#}");
		}

		private string Performance(string playerType)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"YH Perf - {playerType}"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(PlayerPerformance(i, playerType)));
			AddLine(sb, HtmlLib.TableRowClose());

			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"YH Perf - {playerType} last 4"));
			for (int i = 0; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(
					PlayerPerformanceLast(
						weeksToGoBack: 4,
						weekNo: i,
						playerType: playerType)));
			AddLine(sb, HtmlLib.TableRowClose());

			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"YH Perf - {playerType} last game"));
			for (int i = 0; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(
					PlayerPerformanceLast(
						weeksToGoBack: 1,
						weekNo: i,
						playerType: playerType)));
			AddLine(sb, HtmlLib.TableRowClose());

			return sb.ToString();
		}

		private string PlayerPerformanceLast(
			int weeksToGoBack,
			int weekNo,
			string playerType)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$@"..\\{
					Year
					}\\Performance\\YH\\{
					playerType
					}\\Perf last {
					weeksToGoBack
					} upto Week {weekNo:0#}.htm",
				$"{weekNo:0#}"));
			return sb.ToString();
		}

		private string PlayerPerformance(
			int i,
			string playerType)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\Performance\\YH\\{playerType}\\Perf  upto Week {i:0#}.htm",
				$"{i:0#}"));
			return sb.ToString();
		}

		private string PlayerProjections(string playerType)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData($"{playerType} Projections"));
			for (int i = 0; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(PlayerProjection(i,playerType)));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string PlayerProjection(
			int i,
			string playerType)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\scorecards\\{playerType}-Week-{i:0#}.htm",
				$"{i:0#}"));
			return sb.ToString();
		}

		private string RankingMetrics()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData("Ranking Metrics"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 18; i++)
				AddLine(sb, HtmlLib.TableData(RankingMetric(i)));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string RankingMetric(int i)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\Metrics\\MetricTable--{i:0#}.htm",
				$"{i:0#}"));
			return sb.ToString();
		}

		private string PickupSummaries()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData("Pickup summary"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 22; i++)
			{
				AddLine(sb, HtmlLib.TableData(PickupSummary(i)));
			}
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string PickupSummary(int i)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\Projections\\Pickup-Summary-Week-{i}.htm",
				$"{i:0#}"));
			return sb.ToString();
		}

		private string PickupCharts()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableData("Pickup chart"));
			AddLine(sb, HtmlLib.TableData(string.Empty));
			for (int i = 1; i < 22; i++)
			{
				AddLine(sb, HtmlLib.TableData(PickupChart(i)));
			}
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string PickupChart(int i)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.Href(
				$"..\\{Year}\\Projections\\Pickup-Chart-Week-{i:0#}.htm",
				$"{i:0#}"));
			return sb.ToString();
		}

		private string ScopeHeader(string whatScope)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.TableRowOpen());
			AddLine(sb, HtmlLib.TableRowHeaderOpen("width='100'"));
			AddLine(sb, whatScope);
			AddLine(sb, HtmlLib.TableRowHeaderClose());
			for (int i = 0; i < 22; i++)
				AddLine(sb, HtmlLib.TableHeader($"{i:0#}"));
			AddLine(sb, HtmlLib.TableRowClose());
			return sb.ToString();
		}

		private string SubHeading(string header)
		{
			var sb = new StringBuilder();
			AddLine(sb,
				HtmlLib.H3(
					HtmlLib.AnchorHref(header)));
			return sb.ToString();
		}

		private string PreseasonReports()
		{
			var sb = new StringBuilder();
			AddLine(sb, 
				HtmlLib.H3(
					HtmlLib.AnchorHref("Preseason Reports")));
			var items = new Dictionary<string, string>
			{
				{
					$"..\\{Year-1}\\Balance.htm",
					"Balance Report from last year"
				},
				{
					$"..\\{Year}\\FreeAgentMarket\\faMarket.htm",
					"Free Agent Analysis"
				},
				{
					$"..\\{Year}\\Players\\PlayersProbablyRetired-{Year}.htm ",
					"Auto Retired Players"
				},
				{
					$"..\\{Year}\\Players\\PlayerReportsDeleted-{Year}.htm ",
					"Auto Deleted Player Reports"
				}
			};
			AddLine(sb, HtmlLib.OrderedList(items));

			AddLine(sb,	HtmlLib.H4(	"Once Schedule Available"));
			items = new Dictionary<string, string>
			{
				{
					$"..\\{Year}\\StrengthOfSchedule.htm",
					"Strength Of Schedule"
				},
				{
					$"..\\{Year}\\Projections\\Proj-Spread-{Year}.htm",
					$"{Year} Win Projections"
				},
				{
					$"..\\{Year}\\Projections\\Playercsv.htm",
					$"{Year} Player FP Projections"
				},
				{
					$"..\\{Year}\\Metrics\\MetricTable--01.htm",
					$"{Year} Team Metrics"
				},
			};
			AddLine(sb, HtmlLib.OrderedList(items));
			return sb.ToString();
		}

		private string Heading()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.DivOpenClass("header row"));
			var link = HtmlLib.Href(
				fileLink: "..\\tfl-out\\currstnd.txt",
				label: Year.ToString());
			AddLine(sb, HtmlLib.H2($"Season {link}"));
			AddLine(sb, HtmlLib.DivClose());
			return sb.ToString();
		}

		private string Heading(string header)
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.DivOpenClass("header row"));
			AddLine(sb, HtmlLib.H2(header));
			AddLine(sb, HtmlLib.DivClose());
			return sb.ToString();
		}

		private string Header()
		{
			var sb = new StringBuilder();
			AddLine(sb,"  <meta name = 'viewport' content ='width=device-width;initial-scale = 1.0;'maximum-scale = 1.0;/>");
			AddLine(sb,"  <meta name = 'apple-mobile-web-app-capable' content = 'yes' />");
			AddLine(sb,string.Empty);
			AddLine(sb,HtmlLib.Comment("JQuery UI"));
			AddLine(sb,HtmlLib.CssLink("../css/jquery-ui-1.10.0.custom.min.css"));
			AddLine(sb,HtmlLib.CssLink("../css/base.css"));
			AddLine(sb,HtmlLib.CssLink("../css/tablet.css"));
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.Comment("Javascript"));
			AddLine(sb,HtmlLib.JSScriptFile("../js/jquery-1.8.2.min.js"));
			AddLine(sb,HtmlLib.JSScriptFile("../js/jquery-ui-1.10.0.custom.min.js"));
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.StyleOpen());
			AddLine(sb,"  .class1 A:link {text-decoration: yellow;}");
			AddLine(sb,"  .left.col {width: 250px;}");
			AddLine(sb,"  .right.col {left: 250px;right: 0;}");
			AddLine(sb,"  .header.row {height: 75px;	background: #333;}");
			AddLine(sb,"  .body.row {top: 75px;bottom: 50px;background: #000;padding: 1em;}");
			AddLine(sb,"  .footer.row {height: 75px;top: 20px;bottom: 0;background: #333;}");
			AddLine(sb,"  body {color: White;background: #000;font - family: Segoe UI, Helvetica, Arial, Sans-Serif;}");
			AddLine(sb,"  .header, .footer {padding: 0 1em;}");
			AddLine(sb,HtmlLib.StyleClose());
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.ScriptOpen());
			var script = @"
        $(function () {

            $('#myAccordian').accordion({
                header: 'h3',

				activate: false,
                heightStyle: 'fill',
                collapsible: true,
                autoHeight: false
			});
        });";
			AddLine(sb,script);
			AddLine(sb,HtmlLib.ScriptClose());

			return sb.ToString();
		}

		private static void AddLine(StringBuilder sb, string line)
		{
			sb.AppendLine(line);
		}

		private static string Footer()
		{
			return string.Empty;
		}
	}
}
