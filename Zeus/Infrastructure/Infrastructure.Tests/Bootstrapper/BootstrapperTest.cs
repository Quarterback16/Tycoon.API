using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Bootstrapper
{
    /// <summary>
    /// Unit tests for <see cref="Infrastructure.Bootstrapper.Bootstrapper" />.
    /// </summary>
    [TestClass]
    public class BootstrapperTest
    {
        private class TestBootstrapper : Infrastructure.Bootstrapper.Bootstrapper
        {
            private IContainerProvider container { get; set; }

            public void SetContainer(IContainerProvider container)
            {
                this.container = container;
            }

            public override IContainerProvider CreateContainer()
            {
                return container;
            }
        }

        private class TestContainerProvider : ContainerProvider
        {
            public override Interfaces.IContainerProvider RegisterByConfiguration()
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterType<TServiceType, TImplementationType>()
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterType<TServiceType, TImplementationType>(
                Infrastructure.Types.LifetimeType lifetime)
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterType(System.Type serviceType,
                                                                       System.Type implementationType)
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterType(System.Type serviceType,
                                                                       System.Type implementationType,
                                                                       Infrastructure.Types.LifetimeType lifetime)
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterInstance<TServiceType>(object instance)
            {
                return this;
            }

            public override Interfaces.IContainerProvider RegisterInstance(System.Type serviceType, object instance)
            {
                return this;
            }

            public override TServiceType GetService<TServiceType>()
            {
                return default(TServiceType);
            }

            public override object GetService(System.Type serviceType)
            {
                return null;
            }

            public override System.Collections.Generic.IEnumerable<TServiceType> GetServices<TServiceType>()
            {
                return Enumerable.Empty<TServiceType>();
            }

            public override System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
            {
                return Enumerable.Empty<object>();
            }

            public override void Inject(object instance)
            {

            }
        }

        private static Mock<IContainerProvider> mockContainerProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();

            var registrations = new List<IRegistration> {new Mock<IRegistration>().Object};
            //var profileExpressions = new Mock<IEnumerable<IProfileExpression>>();

            mockContainerProvider.Setup(p => p.GetServices<IRegistration>()).Returns(registrations);
            //mockContainerProvider.Setup(e => e.GetServices<IProfileExpression>()).Returns(profileExpressions.Object);
        }

        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Test dispose.
        /// </summary>
        [TestMethod]
        public void Bootstrapper_Dispose_DisposesContainerProvider()
        {
            TestBootstrapper sut;

            using (sut = new TestBootstrapper())
            {
                sut.SetContainer(new TestContainerProvider());

                Assert.IsTrue(!GetDisposedValue(sut.Container));
            }

            try
            {
                Assert.IsTrue(GetDisposedValue(sut.Container));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ObjectDisposedException);
            }
        }

        private bool GetDisposedValue(object container)
        {
            var field = typeof(ContainerProvider).GetField("disposed", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)field.GetValue(container);
        }

        /// <summary>
        /// Test start.
        /// </summary>
        [TestMethod]
        public void Bootstrapper_Start_RunsRegistrations()
        {
            var sut = new TestBootstrapper();

            sut.SetContainer(mockContainerProvider.Object);

            sut.Start();

            mockContainerProvider.Verify();
        }

        /// <summary>
        /// Test end.
        /// </summary>
        [TestMethod]
        public void Bootstrapper_End()
        {
            var sut = new TestBootstrapper();

            sut.SetContainer(mockContainerProvider.Object);

            sut.End();
        }
    }
}
