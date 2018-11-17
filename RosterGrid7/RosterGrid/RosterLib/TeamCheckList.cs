using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RosterLib
{
	public class TeamCheckList
	{
		public Hashtable HtTeam { get; set; }
		public Hashtable HtDone { get; set; }

		public TeamCheckList()
		{
			string theKey;
			HtTeam = new Hashtable();
			HtDone = new Hashtable();
			DataSet ds = Utility.TflWs.GetTeams( Utility.CurrentSeason() );
			DataTable dt = ds.Tables[0];
			foreach (DataRow dr in dt.Rows )
			{
				theKey = dr["TEAMID"].ToString();
				HtTeam.Add( theKey, dr );
			}
		}

		public void TickOff( string theKey, string sPos )
		{
			if ( HtTeam.ContainsKey( theKey ) )
				HtTeam.Remove( theKey );

#if DEBUG
			if ( HtDone.ContainsKey( theKey ) )
					RosterLib.Utility.Announce( "We have a duplicate " + sPos + " for " + theKey );
			else
				HtDone.Add( theKey, theKey );
#endif
		}

		public string TeamsLeft()
		{
			StringBuilder sb = new StringBuilder();
			IDictionaryEnumerator myEnumerator = HtTeam.GetEnumerator();
			while ( myEnumerator.MoveNext() )
			{
				sb.Append( myEnumerator.Key + "," );
			}
			return sb.ToString();
		}

	}
}
