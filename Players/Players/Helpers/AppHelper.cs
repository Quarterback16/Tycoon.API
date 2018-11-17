using System;
using System.Configuration;

namespace Players.Helpers
{
	public class AppHelper
	{
		public static string ContentRoot
		{
			get
			{
				var rootUrl = ConfigurationManager.AppSettings[ "RootUrl" ];
				const string contentVirtualRoot = "/Content";
				return String.Format( "{0}{1}", rootUrl, contentVirtualRoot );
			}
		}

		public static string ScriptRoot
		{
			get
			{
				var rootUrl = ConfigurationManager.AppSettings[ "RootUrl" ];
				const string contentVirtualRoot = "/Scripts";
				return String.Format( "{0}{1}", rootUrl, contentVirtualRoot );
			}
		}

		public static string ImageRoot
		{
			get { return String.Format( "{0}/{1}", ContentRoot, "Images" ); }
		}

		public static string CssRoot
		{
			get { return String.Format( "{0}/{1}", ContentRoot, "CSS" ); }
		}

		/// <summary>
		/// To return how many pages in total based on the total number of records and  page size
		/// </summary>
		/// <param name="totRecs">Total number of records</param>
		/// <param name="pageSize">Size of the page/ records per page</param>
		/// <returns></returns>
		public static int PagesInTotal( int totRecs, int pageSize )
		{
			if ( totRecs.Equals( 0 ) ) return 0;
			var numPages = ( totRecs / pageSize ) + 1;
			var remainder = totRecs % pageSize;
			if ( remainder == 0 ) numPages--;
			return numPages;
		}

		public static string JScriptUrl( string jsFile )
		{
			return String.Format( "{0}/{1}?{2}", ScriptRoot, jsFile, 1 );
		}
	}
}