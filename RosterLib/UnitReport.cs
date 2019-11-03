using RosterLib.Interfaces;
using System;

namespace RosterLib
{
   /// <summary>
   ///   Dumps out a file for each teams units.
   ///   It is the NFLTeam that knows what a unit report is
   /// </summary>
   public class UnitReport : RosterGridReport, IDisposable
   {
	   public NflSeason SeasonMaster { get; set; }

      public UnitReport( 
		  IKeepTheTime timekeeper ) : base( timekeeper )
	  {
         Name = "Unit Reports";
         LastRun = Utility.TflWs.GetLastRun(Name);
      }

      public override string OutputFilename()
      {
         return string.Format("{0}", RootPath(Season));
      }

      public override void RenderAsHtml()
      {
         Render(Season);
         Finish();
      }

      public void Render(string season)
      {
	      SeasonMaster = SetSeasonMaster( season );
	      foreach (string teamKey in SeasonMaster.TeamKeyList)
         {
            TeamUnits( season, teamKey );
#if DEBUG2
            break;
#endif
         }
      }

	   public static NflSeason SetSeasonMaster( string season )
	   {
		   var s = Masters.Sm.GetSeason( season, teamsOnly: true );
		   return s;
	   }

	   public void TeamUnits( string season, string teamKey )
	   {
		   var t = Masters.Tm.GetTeam( teamKey );
		   Utility.Announce( $"Unit reports for {t.NameOut()}" );

		   var fileOut = $"{Utility.OutputDirectory()}\\{season}\\Units\\{t.TeamCode}-Units.htm";

		   var h = new HtmlFile( fileOut,
			   $@" Unit Reports as of {DateTime.Now.ToString("dd MMM yy")}  Week {
				   Utility.CurrentWeek()
				   }" );

		   h.AddToBody( Header( t ) );
		   h.AddToBody( t.UnitReport() );
		   h.Render();

		   PoSnippet( t );
#if !DEBUG2
           RoSnippet( t );
		   PpSnippet( t );
		   PrSnippet( t );
		   RdSnippet( t );
		   PdSnippet( t );
#endif
	   }

#region Headers

      private static string Header(NflTeam t)
      {
         return HtmlLib.H2(string.Format("Unit Reports for {0} as of {1}", 
            t.NameOut(), DateTime.Now.ToString( "ddd dd MMM yy hh:mm" ) )
            );
      }

      private static string HeaderPo(NflTeam t)
      {
         return HtmlLib.H2($"Passing Unit for {t.NameOut()}");
      }

      private static string HeaderRo(NflTeam t)
      {
         return HtmlLib.H2($"Rushing Unit for {t.NameOut()}");
      }

      private static string HeaderPp(NflTeam t)
      {
         return HtmlLib.H2($"Pass Protection Unit for {t.NameOut()}");
      }

      private static string HeaderPr(NflTeam t)
      {
         return HtmlLib.H2($"Pass Rush Unit for {t.NameOut()}");
      }

      private static string HeaderRd(NflTeam t)
      {
         return HtmlLib.H2($"Run Defense Unit for {t.NameOut()}");
      }

      private static string HeaderPd(NflTeam t)
      {
         return HtmlLib.H2($"Pass Defense Unit for {t.NameOut()}");
      }

#endregion Headers

#region Snippets

      public void PoSnippet(NflTeam t)
      {
         FileOut = $"{RootPath(t.Season)}\\PassOff\\PO-{t.TeamCode}.htm";
         var h = new HtmlFile(
			 FileOut,
             $@" {t.NameOut()} Passing Unit as of {DateTime.Now.ToString("dd MMM yy")}  Week {
				 Utility.CurrentWeek()
				 }");

         h.AddToBody(
			 HeaderPo(t));
         h.AddToBody(
			 t.PoReport());
         h.Render();
      }

      public void RoSnippet(NflTeam t)
      {
         FileOut = string.Format("{0}\\RunOff\\RO-{1}.htm", RootPath(t.Season), t.TeamCode);
         var h = new HtmlFile(FileOut,
                                   string.Format(" {2} Rushing Unit as of {0}  Week {1}",
                                                 DateTime.Now.ToString("dd MMM yy"), Utility.CurrentWeek(), t.NameOut()));

         h.AddToBody(HeaderRo(t));
         h.AddToBody(t.RoReport());
         h.Render();
      }

      public void PpSnippet(NflTeam t)
      {
         FileOut = string.Format("{0}\\Prot\\PP-{1}.htm", RootPath(t.Season), t.TeamCode);
         var h = new HtmlFile(FileOut,
                     string.Format(" {2} Pass Protection Unit as of {0}  Week {1}",
                                    DateTime.Now.ToString("dd MMM yy"), Utility.CurrentWeek(), t.NameOut()));
         h.AddToBody(HeaderPp(t));
         h.AddToBody(t.PpReport());
         h.Render();
      }

      private static string RootPath(string season)
      {
         return string.Format("{0}{1}//Units", Utility.OutputDirectory(), season);
      }

      public void PrSnippet(NflTeam t)
      {
         FileOut = string.Format("{0}\\PassRush\\PR-{1}.htm", RootPath(t.Season), t.TeamCode);
         var h = new HtmlFile(FileOut,
                                   string.Format(" {2} Pass Rush Unit as of {0}  Week {1}",
                                                 DateTime.Now.ToString("dd MMM yy"), Utility.CurrentWeek(), t.NameOut()));

         h.AddToBody(HeaderPr(t));
         h.AddToBody(t.PrReport());
         h.Render();
      }

      public void RdSnippet(NflTeam t)
      {
         FileOut = string.Format("{0}\\RunDef\\RD-{1}.htm", RootPath(t.Season), t.TeamCode);
         var h = new HtmlFile(FileOut,
                                   string.Format(" {2} Run Defense Unit as of {0}  Week {1}",
                                                 DateTime.Now.ToString("dd MMM yy"), Utility.CurrentWeek(), t.NameOut()));

         h.AddToBody(HeaderRd(t));
         h.AddToBody(t.RdReport());
         h.Render();
      }

      public void PdSnippet(NflTeam t)
      {
         FileOut = string.Format("{0}\\PassDef\\PD-{1}.htm", RootPath(t.Season), t.TeamCode);
         var h = new HtmlFile(FileOut,
                                   string.Format(" {2} Pass Defense Unit as of {0}  Week {1}",
                                                 DateTime.Now.ToString("dd MMM yy"), Utility.CurrentWeek(), t.NameOut()));

         h.AddToBody(HeaderPd(t));
         h.AddToBody(t.PdReport());
         h.Render();
      }

#endregion Snippets

#region IDisposable Members

      public void Dispose()
      {
         GC.SuppressFinalize(obj: true); // as a service to those who might inherit from us
      }

#endregion IDisposable Members
   }
}