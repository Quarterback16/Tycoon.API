using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="CopyAttribute" />.
    /// </summary>
    [TestClass]
    public class CopyAttributeAttributeTest
    {
        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with a null value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNull_ThrowsArgumentNullException()
        {
            new CopyAttribute(null);
        }

        /// <summary>
        /// Test instantiated with valid argument is successful.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithValidValue_Success()
        {
            var dependentProperty = "foo";
            var sut = new CopyAttribute(dependentProperty);

            Assert.IsNotNull(sut);
            Assert.AreEqual(dependentProperty, sut.DependentProperty);
        }
    }
}
