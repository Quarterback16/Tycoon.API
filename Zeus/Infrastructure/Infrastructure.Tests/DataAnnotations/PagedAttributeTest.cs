using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="PagedAttribute" />.
    /// </summary>
    [TestClass]
    public class PagedAttributeTest
    {
        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null submit type.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithAction()
        {
            var action = "foo";
            var sut = new PagedAttribute(action);

            Assert.IsNotNull(sut);
            Assert.AreEqual(action, sut.Action);
        }
    }
}
