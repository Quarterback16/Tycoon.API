using System;
using System.Reflection;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests
{
    /// <summary>
    /// Unit tests for <see cref="MvcApplication" />.
    /// </summary>
    [TestClass]
    public class MvcApplicationTest
    {
        private class TestMvcApplication : MvcApplication
        {
            protected override IBootstrapper CreateBootstrapper()
            {
                return mockBootStrapper.Object;
            }
        }

        private static Mock<IBootstrapper> mockBootStrapper = new Mock<IBootstrapper>();
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void MvcApplication_StartAndEnd_BootstrapperUsed()
        {
            mockBootStrapper.Setup(m => m.Container).Returns(new Mock<IContainerProvider>().Object);
            var mockHttpContext = new Mock<HttpContextBase>();

            var sut = new TestMvcApplication();

            sut.Init();

            sut.GetType().GetMethod("Application_Start", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(sut, null);

            Assert.IsNotNull(sut.Container);

            sut.GetType().GetMethod("Application_End", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(sut, null);
            
            mockBootStrapper.Verify(m => m.Start(), Times.Once());
            mockBootStrapper.Verify(m => m.End(), Times.Once());
        }
    }
}
