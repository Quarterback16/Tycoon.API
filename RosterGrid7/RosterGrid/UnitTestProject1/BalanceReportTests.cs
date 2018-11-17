using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class BalanceReportTests
	{

		[TestMethod]
		public void BalanceReport2012Test()
		{
			const string season = "2012";
			var myTeamLister = new FakeTeamMetricsLister();
			var br = new BalanceReport { Season = season, TeamList = myTeamLister.GetTeams(season) };
			br.Render();
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestFormattingOfBalanceReport()
		{
			const string season = "2011";
			var myTeamLister = new FakeTeamMetricsLister();
			var br = new BalanceReport { Season = season, TeamList = myTeamLister.GetTeams(season) };
			br.Render();
			Assert.IsTrue(true);
		}



	}
}
