using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class PassUnitTests
	{
		[TestMethod]
		public void TestPlayerPassingUnitGetsLoaded()
		{
			var sut = new NflTeam( "JJ" );  // dependancy on DB, thus an integration test
			sut.LoadPassUnit();
			Assert.IsTrue( sut.PassUnit.Quarterbacks.Count > 0 );
			Assert.IsTrue( sut.PassUnit.Receivers.Count > 0 );
			Utility.Announce( string.Format( "   Loaded {0} QBs", sut.PassUnit.Quarterbacks.Count ) );
			Utility.Announce( string.Format( "   Loaded {0} Receivers", sut.PassUnit.Receivers.Count ) );
			Assert.IsFalse( sut.PassUnit.HasIntegrityError() );
			Utility.Announce( "No integrity errors" );
		}

		[TestMethod]
		public void TestAllPassingUnits2013()
		{
			var errors = 0;
			var s = new NflSeason( "2013", true );
			foreach ( var t in s.TeamList )
			{
				t.LoadPassUnit();
				if ( t.PassUnit.HasIntegrityError() )
				{
					errors++;
					Utility.Announce( string.Format( "   Need to fix {0}", t.Name ) );
				}
			}
			Utility.Announce( string.Format( "   There are {0} broken teams", errors ) );
			Assert.AreEqual( 0, errors );
		}
	}
}
