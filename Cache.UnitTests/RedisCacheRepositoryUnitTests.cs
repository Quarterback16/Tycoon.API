using Cache.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Cache.UnitTests
{
	[TestClass]
	public class RedisCacheRepositoryUnitTests
	{
		private RedisCacheRepository _sut;

		private Mock<ISerializer> _mockSerializer;
		private Mock<ILog> _mockLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockSerializer = new Mock<ISerializer>();
			_mockLogger = new Mock<ILog>();

			_sut = new RedisCacheRepository(
				connectionString: "",  // for unit testing we dont want to connect
				environment: "unittest-environment", 
				functionalArea: "unit-test", 
				serializer: _mockSerializer.Object, 
				logger: _mockLogger.Object);
		}

		[TestMethod]
		public void Cache_WithoutConnectioString_ConstructsOk()
		{
			Assert.IsNotNull(_sut);
			Assert.IsFalse(_sut.IsActive);
		}
	}
}
