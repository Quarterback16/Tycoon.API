using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Ioc.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Ioc.Unity
{
    /// <summary>
    /// Unit tests for <see cref="UnityMvcApplication" />.
    /// </summary>
    [TestClass]
    public class UnityMvcApplicationTest
    {
        public class TestMvcApplication : UnityMvcApplication
        {
            public IBootstrapper TestCreateBootstrapper()
            {
                return base.CreateBootstrapper();
            }
        }

        [TestMethod]
        public void UnityMvcApplication_CreateBootstrapper()
        {
            var bootstrapper = new TestMvcApplication().TestCreateBootstrapper();

            Assert.IsNotNull(bootstrapper);
        }
    }
}
