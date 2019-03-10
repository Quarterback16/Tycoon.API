using System;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TflService.Tests
{
	[TestClass]
	public class TflServiceTests
	{
		private TflService sut;
		public ILog Logger { get; set; }

//		private Mock<ITflDataLibrarian> _mockLibrarian;

		//[TestInitialize]
		//public void TestInitialize()
		//{
		//	sut = SystemUnderTest();
		//}

		//private TflService SystemUnderTest()
		//{
		//	Logger = LogManager.GetCurrentClassLogger();
		//	//var lib = RosterLib.Utility.TflWs;
		//	_mockLibrarian = new Mock<ITflDataLibrarian>();
		//	return new TflService(
		//		_mockLibrarian.Object,
		//		Logger);
		//}

		#region  End Contract Method

		[TestMethod]
		public void EndContract_WhenNotSameDayAsLastContract_DoesntSkipCut()
		{
			//_mockLibrarian
			//	.Setup(x => x.LastContract(It.IsAny<string>()))
			//	.Returns(new DateTime(1993, 3, 18));
			//var dummyPlayer = new Mock<INFLPlayer>();
			//dummyPlayer
			//	.Setup(x => x.PlayerCode)
			//	.Returns("MONTJO01");
			//dummyPlayer
			//	.Setup(x => x.TeamCode)
			//	.Returns("SF");
			//Assert.IsTrue(
			//	sut.EndContract(
			//		dummyPlayer.Object,
			//		new DateTime(2018, 3, 18),
			//		isRetirement: true));
			//_mockLibrarian
			//	.Verify(
			//		x => x.RetirePlayer(
			//			It.IsAny<DateTime>(),
			//			"MONTJO01"),
			//		Times.Once);
			//_mockLibrarian
			//	.Verify(
			//		x => x.SetCurrentTeam(
			//			"MONTJO01",
			//			"??"),
			//		Times.Once);
		}

		//[TestMethod]
		//public void EndContract_OnPlayerWithNoTeam_JustLogsInfo()
		//{
		//	var dummyPlayer = new Mock<INFLPlayer>();
		//	dummyPlayer
		//		.Setup(x => x.PlayerCode)
		//		.Returns("MONTJO01");
		//	dummyPlayer
		//		.Setup(x => x.TeamCode)
		//		.Returns("??");

		//	var result = sut.EndContract(
		//		dummyPlayer.Object,
		//		new DateTime(2018, 3, 18),
		//		isRetirement: false);
		//	Assert.IsFalse(result);
		//	_mockLibrarian.Verify(
		//		x => x.SetCurrentTeam("MONTJO01", "??"),
		//		Times.Never);
		//}

		//[TestMethod]
		//public void EndContract_OnPlayerWithNullTeam_LogsError()
		//{
		//	var dummyPlayer = new Mock<INFLPlayer>();
		//	dummyPlayer
		//		.Setup(x => x.PlayerCode)
		//		.Returns("MONTJO01");

		//	var result = sut.EndContract(
		//		dummyPlayer.Object,
		//		new DateTime(2018, 3, 18),
		//		isRetirement: false);
		//	Assert.IsFalse(result);
		//	_mockLibrarian.Verify(
		//		x => x.SetCurrentTeam("MONTJO01", "??"),
		//		Times.Never);
		//}

		#endregion

	}
}
