using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace Gerard.HostServer.Tests
{
	[TestClass]
	public class ServerTests
	{
		[TestMethod]
		public void DataLibrarian_Constructs_Ok()
		{
			var sut = Utility.TflWs;
			Assert.IsNotNull(sut);
		}

		[TestMethod]
		public void TflService_ReturnsPlayers_Ok()
		{
			var lib = Utility.TflWs;
			var sut = new TflService(lib, LogManager.GetCurrentClassLogger() );
			var p = sut.GetNflPlayer(
				"Joe",
				"Montana");
			Assert.IsNotNull(sut);
			Assert.IsTrue(p.PlayerCode.Equals("MONTJO01"));
		}
	}

}
