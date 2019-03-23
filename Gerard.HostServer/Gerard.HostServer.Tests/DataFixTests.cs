using Gerard.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System;

namespace Gerard.HostServer.Tests
{
	[TestClass]
	public class DataFixTests
	{
		private DataFixer _sut;

		public Logger Logger { get; set; }

		[TestInitialize]
		public void TestInitialize()
		{
			_sut = SystemUnderTest();
		}

		private DataFixer SystemUnderTest()
		{
			Logger = LogManager.GetCurrentClassLogger();
			var lib = Utility.TflWs;
			var tfl = new TflService(lib, Logger);
			return new DataFixer(
				tfl,
				new NLogAdaptor());
		}

		[TestMethod]
		public void Fixer_CanGetPlayer_Ok()
		{
			var command = new DataFixCommand
			{
				FirstName = "Joe",
				LastName = "Montana",
				TeamCode = "SF"
			};
			var result = _sut.GetPlayer(command);
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Fixer_CanGetStatleshipPlayer_Ok()
		{
			var command = new DataFixCommand
			{
				FirstName = "Nick",
				LastName = "Mullens",
				TeamCode = "SF"
			};
			var result = _sut.GetStatleshipPlayer(command);
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Fixer_CanPutPlayersDob_Ok()
		{
			var command = new DataFixCommand
			{
				FirstName = "Joe",
				LastName = "Montana",
				TeamCode = "SF"
			};
			var player = _sut.GetPlayer(command);
			var statPlayer = new Player
			{
				BirthDate = DateTime.Parse(player.DBirth)
			};
			var result = _sut.PutDob(
				player,
				statPlayer);

			Assert.IsTrue(result);
		}
	}
}
