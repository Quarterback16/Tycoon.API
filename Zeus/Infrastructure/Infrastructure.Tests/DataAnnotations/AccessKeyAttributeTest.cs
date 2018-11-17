using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="AccessKeyAttribute" />.
    /// </summary>
    [TestClass]
    public class AccessKeyAttributeTest
    {
        [TestMethod]
        public void Constructor_CalledWithChar()
        {
            var key = 'a';
            var sut = new AccessKeyAttribute(key);

            Assert.IsNotNull(sut);
            Assert.AreEqual(key, sut.Key);
        }
    }
}
