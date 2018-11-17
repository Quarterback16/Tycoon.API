using InternetScanner.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shuttle.Esb;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace InternetScanner.Tests
{
	[TestClass]
    public class InternetRssScannerTests
    {
		private InternetRssScanner _sut;
		private Mock<IServiceBus> _mockServiceBus;
		private Mock<IGetFeedItems> _mockGetFeedItems;
		private Mock<IGotItQuery> _mockGotItQuery;
		private Mock<IQueryHandler<FeedReaderQuery, List<SyndicationItem>>> 
			_mockQueryHandler;

		[TestInitialize]
		public void Setup()
		{
			_mockServiceBus = new Mock<IServiceBus>();
			_mockGetFeedItems = new Mock<IGetFeedItems>();
			_mockGotItQuery = new Mock<IGotItQuery>();
			_mockQueryHandler = 
				new Mock<
						IQueryHandler<
							FeedReaderQuery, 
							List<SyndicationItem>>>();
			var logger = new FakeLogger();
			_sut = new InternetRssScanner(
				_mockServiceBus.Object,
				_mockQueryHandler.Object,
				_mockGotItQuery.Object,
				logger);
		}

		[TestMethod]
		public void Scanner_WithServiceBus_Instantiates()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void Scanner_OnNflFeed_SendsCommands()
		{
			_sut.Scan(
				new FeedReaderQuery(
					new Feed
					{
						Url = "http://www.rotoworld.com/tools/rss/fantasy-football.aspx",
					}));
		}
	}
}
