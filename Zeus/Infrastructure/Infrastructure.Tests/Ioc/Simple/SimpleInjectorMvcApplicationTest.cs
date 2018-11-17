using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Ioc.Simple;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Ioc.Simple
{
    /// <summary>
    /// Unit tests for <see cref="SimpleInjectorMvcApplication" />.
    /// </summary>
    [TestClass]
    public class SimpleInjectorMvcApplicationTest
    {
        public class TestMvcApplication : SimpleInjectorMvcApplication
        {
            public IBootstrapper TestCreateBootstrapper()
            {
                return base.CreateBootstrapper();
            }
        }

        [TestMethod]
        public void SimpleInjectorMvcApplication_CreateBootstrapper()
        {
            var bootstrapper = new TestMvcApplication().TestCreateBootstrapper();

            Assert.IsNotNull(bootstrapper);
        }
    }
}
