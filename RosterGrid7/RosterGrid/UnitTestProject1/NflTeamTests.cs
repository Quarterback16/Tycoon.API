using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class NflTeamTests
	{
		[TestMethod]
		public void TestAllKickers()
		{
			//TODO:  This is really a data integrity test
			var errors = 0;
			var s = new NflSeason("2013", true);
			foreach (var t in s.TeamList)
			{
				var kicker = t.SetKicker();
				if (kicker == null)
					errors++;
			}
			Utility.Announce(string.Format("There are {0} broken teams", errors));
			Assert.AreEqual(0, errors);
		}
	}
}
