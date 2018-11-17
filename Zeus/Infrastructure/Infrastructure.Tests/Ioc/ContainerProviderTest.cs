using System;
using System.Linq;
using System.Reflection;
using Employment.Web.Mvc.Infrastructure.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Ioc
{
    /// <summary>
    /// Unit tests for <see cref="ContainerProvider" />.
    /// </summary>
    [TestClass]
    public class ContainerProviderTest
    {
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

        /// <summary>
        /// Test dispose.
        /// </summary>
        [TestMethod]
        public void ContainerProvider_Dispose_Disposes()
        {
            TestContainerProvider containerProvider;

            using (containerProvider = new TestContainerProvider())
            {
                Assert.IsTrue(!GetDisposedValue(containerProvider));
            }

            try
            {
                Assert.IsTrue(GetDisposedValue(containerProvider));
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
    }
}
