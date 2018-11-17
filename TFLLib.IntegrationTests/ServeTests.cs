using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFLLib.IntegrationTests
{
	[TestClass]
	public class ServeTests : IntegrationTestsBase
	{
		[TestInitialize]
		public void Setup()
		{
			Initialise();
		}

		[TestMethod]
		public void LastContract_ForJoeMontana_ReturnsCorrectDate()
		{
			var result = Sut.LastContract("MONTJO01");
			Assert.AreEqual(
				expected: new DateTime(1993, 4, 26), 
				actual: result);
		}
	}
}
