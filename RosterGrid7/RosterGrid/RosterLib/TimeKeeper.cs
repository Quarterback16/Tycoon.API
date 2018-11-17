using System;
using System.Runtime.InteropServices;

namespace RosterLib
{
	public class TimeKeeper
	{
		private static TimeKeeper _instance;

		private TimeKeeper()
		{
		}

		public static TimeKeeper Instance
		{
			get { return _instance ?? ( _instance = new TimeKeeper() ); }
		}

		/// <summary>
		///   Preseason is basically any date that is not the Proper Season
		///   During the proper season the Current Week global will be 
		///   something between 1 and 21.
		///   Here we introduce a dependancy on an XML file containing
		///   the dates for all the Superbowls.  Working backwards from
		///   the date of the Superbowl (always played on a Sunday)
		///   you can figure out the dates for each week of the season.
		/// </summary>
		/// <param name="theDate">the date in focus - usually left at default of current date</param>
		/// <returns></returns>
		public static bool IsPreseason( [Optional] DateTime theDate )
		{
			var focusDate = theDate == new DateTime( 1 , 1, 1 ) ? DateTime.Now: theDate;
			return !IsProperSeason( focusDate );
		}

		public static bool IsPreseason()
		{
			return !IsProperSeason( DateTime.Now );
		}

		public static bool IsProperSeason( [Optional] DateTime theDate )
		{
			var focusDate = theDate == new DateTime( 1, 1, 1 ) ? DateTime.Now : theDate;
			return false;
		}
	}
}
