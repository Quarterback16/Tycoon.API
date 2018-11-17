using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class TippingCompTests
	{
		[TestMethod]
		public void TestTippingController2013()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.Index("2013");
			//  Assert
			Assert.IsTrue(true);
		}


		[TestMethod]
		public void TestTippingController2012()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.Index("2012");
			//  Assert
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestTippingComp()
		{
			//  Arrange
			var tipsters = new Dictionary<string, WinLossRecord>
			               	{
			               		{"Angie", new WinLossRecord(3, 1, 0)},
			               		{"Bob", new WinLossRecord(2, 2, 0)},
			               		{"Charlie", new WinLossRecord(0, 4, 0)},
										{"Denis", new WinLossRecord(4,0,0)}
			               	};
			var tc = new TippingComp(tipsters);

			//  Act
			tc.Render();

			//  Assert
			Assert.IsTrue(File.Exists(tc.OutputFilename), string.Format("Cannot find {0}", tc.OutputFilename));
		}

		[TestMethod]
		public void TestTippingController2011()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.Index("2011");
			//  Assert
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestTippingController2010()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.Index( "2010" );
			//  Assert
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestTippingControllerAts()
		{
			//  Arrange
			var tc = new TippingController();
			//  Act
			tc.IndexAts("2010");
			//  Assert
			Assert.IsTrue(true);
		}

	}
}
