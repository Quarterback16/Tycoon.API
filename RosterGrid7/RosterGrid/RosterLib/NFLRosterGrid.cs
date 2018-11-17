using System;
using System.Collections;

namespace RosterLib
{
	/// <summary>
	/// Summary description for NFLRosterGrid.
	/// </summary>
	public class NFLRosterGrid
	{
		public string FileOut = "";
		public string StrCat  = "2";
		public ArrayList DivList;
		public bool ShowBackups = true;

		public string Focus { get; set; }

		public NFLRosterGrid( string catIn )
		{
			DivList =  new ArrayList();
			StrCat = catIn;
			LoadDivisions( Utility.CurrentSeason() );
			Focus = string.Empty;
		}

		private void LoadDivisions(string seasonIn)
		{
			var div1 = new NFLDivision( "East", "NFC", "A", seasonIn, StrCat );
			DivList.Add( div1 );
			var div2 = new NFLDivision( "North", "NFC", "B", seasonIn, StrCat );
			DivList.Add( div2 );
			var div3 = new NFLDivision( "South", "NFC", "C", seasonIn, StrCat );
			DivList.Add( div3 );
			var div4 = new NFLDivision( "West", "NFC", "D", seasonIn, StrCat );
			DivList.Add( div4 );
			var div5 = new NFLDivision( "East", "AFC", "E", seasonIn, StrCat );
			DivList.Add( div5 );
			var div6 = new NFLDivision( "North", "AFC", "F", seasonIn, StrCat );
			DivList.Add( div6 );
			var div7 = new NFLDivision( "South", "AFC", "G", seasonIn, StrCat );
			DivList.Add( div7 );
			var div8 = new NFLDivision( "West", "AFC", "H", seasonIn, StrCat );
			DivList.Add( div8 );
		}

		public void CurrentRoster()
		{
			FileOut = string.Format( "{0}\\{3}\\Rosters\\{1}\\Roster-{2}-{4}.htm",
				Utility.OutputDirectory(), Utility.CurrentLeague, Focus, Utility.CurrSeason, Utility.CurrentWeek() );
			var h = new HtmlFile( FileOut, 
				Utility.CategoryOut( StrCat ) + " Roster as of " + DateTime.Now.ToString("dd MMM yy") +
				                        "  Week " + Utility.CurrentWeek() );
												
			h.AddToBody( Header() );
			h.AddToBody( TeamListOut() );
			h.AddToBody( Footer() );
			h.Render();

			//  copy to a current report (ignoring week)
			var currentFile = string.Format( "{0}{3}\\Rosters\\{1}\\Roster-{2}.htm",
					 Utility.OutputDirectory(), Utility.CurrentLeague, Focus, Utility.CurrSeason );
			h.Render( currentFile );
		}

		public void CurrentInjuries()
		{
         FileOut = string.Format("{0}Rosters\\{2}\\{3}\\Injuries{1}.htm", Utility.OutputDirectory(), StrCat,
				Utility.CurrSeason, Utility.CurrentLeague );
			var h = new HtmlFile( FileOut, Utility.CategoryOut( StrCat ) + " Injuries as of " + 
				                                 DateTime.Now.ToString("dd MMM yy") + 
															"  Week " + Utility.CurrentWeek() );
			h.AddToBody( Header() );
			h.AddToBody( InjuryListOut() );
			h.AddToBody( Footer() );
			h.Render();
		}

		private string TeamListOut()
		{
			var s = HtmlLib.TableOpen("BORDER=1 BORDERCOLOR=BLACK") + "\n" + HtmlLib.TableRowOpen() + "\n";
			var nCount = 0;
			var myEnumerator = DivList.GetEnumerator();
			while ( myEnumerator.MoveNext() )
			{
				nCount++;
				if ( nCount == 5 )
				{
					s += HtmlLib.TableRowClose() + "\n" + HtmlLib.TableRowOpen();
					nCount = 0;
				}

				var d = (NFLDivision) myEnumerator.Current;
				var strPos = Focus ?? string.Empty;
				s += d.DivHtml( ShowBackups, StrCat, strPos );				 
			}
			
			return s + HtmlLib.TableRowClose() + "\n" + HtmlLib.TableClose() + "\n";
		}

		private string InjuryListOut()
		{
			var s = HtmlLib.TableOpen("BORDER=1 BORDERCOLOR=BLACK") + "\n" +
				HtmlLib.TableRowOpen() + "\n";
			var nCount = 0;
			var myEnumerator = DivList.GetEnumerator();
			while ( myEnumerator.MoveNext() )
			{
				nCount++;
				if ( nCount == 5 )
				{
					s += HtmlLib.TableRowClose() + "\n" + HtmlLib.TableRowOpen();
					nCount = 0;
				}

				var d = (NFLDivision) myEnumerator.Current;
				s += d.DivInjuries();				 
			}
			
			return s + HtmlLib.TableRowClose() + "\n" +
				HtmlLib.TableClose() + "\n";
		}

		private static string Header()
		{
			return "<Date>";
		}

		private static string Footer()
		{
			const string strFooter = "";

			return strFooter;
		}
	}
}
