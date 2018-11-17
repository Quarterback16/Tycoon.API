using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProgramAssuranceTool.Controllers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Interfaces;

namespace ProgramAssuranceTool.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTests
	{
		private Mock<IUserService> mockUserService;
		private Mock<IAdwService> mockAdwService;
		private IPatService FakePatService;

		private HomeController SystemUnderTest()
		{
			return new HomeController( FakePatService, mockUserService.Object, mockAdwService.Object );
		}

		[TestInitialize]
		public void TestInitialize()
		{
			mockUserService = new Mock<IUserService>();
			mockAdwService = new Mock<IAdwService>();
		}


		[TestMethod]
		public void Index()
		{
			// Arrange
			var controller = SystemUnderTest();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsInstanceOfType( result, typeof(ViewResult) );
			Assert.IsTrue( string.IsNullOrEmpty( result.ViewName ) );
		}

		[TestMethod]
		public void About()
		{
			// Arrange
			var controller = SystemUnderTest();

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.IsNotNull( result );
		}

		[TestMethod]
		public void System()
		{
			// Arrange
			var controller = SystemUnderTest();

			// Act
			ViewResult result = controller.System() as ViewResult;

			// Assert
			Assert.IsNotNull( result );
		}
	}
}
