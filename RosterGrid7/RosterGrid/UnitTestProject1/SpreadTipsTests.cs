using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class SpreadTipsTests
	{
		[TestMethod]
		public void SpreadTipsSpreadPredictionTest()
		{
			//  Arrange
			var sut = new NFLGame( "2012:01-A" );  // YYYY:0W-X
			//  Act
			sut.CalculateSpreadResult();
			//  Assert
			Assert.AreEqual( 23, sut.BookieTip.HomeScore );
			Assert.AreEqual( 20, sut.BookieTip.AwayScore );
		}

		[TestMethod]
		public void Generate2012SpreadTipsTest()
		{
			//  Arrange
			var storer = new DbfPredictionStorer();
			var sut = new SpreadTips(storer);
			//  Act
			sut.SaveTipsFor("2012");
			//  Assert
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void Generate2011SpreadTipsTest()
		{
			//  Arrange
			var storer = new DbfPredictionStorer();
			var sut = new SpreadTips( storer );
			//  Act
			sut.SaveTipsFor( "2011" );
			//  Assert
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void Generate2010SpreadTipsTest()
		{
			//  Arrange
			var storer = new DbfPredictionStorer();
			var sut = new SpreadTips( storer );
			//  Act
			sut.SaveTipsFor( "2010" );
			//  Assert
			Assert.IsTrue( true );
		}

	}
}
