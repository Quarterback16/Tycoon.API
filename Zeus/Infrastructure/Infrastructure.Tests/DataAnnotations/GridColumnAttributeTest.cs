using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="GridColumnAttribute" />.
    /// </summary>
    [TestClass]
    public class GridColumnAttributeTest
    {
        /// <summary>
        /// Test <see cref="ArgumentOutOfRangeException" /> is thrown when instantiated with a value below the allowed range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_CalledWithBelowAllowedRange_ThrowsArgumentOutOfRangeException()
        {
            new GridColumnAttribute(0);
        }

        /// <summary>
        /// Test <see cref="ArgumentOutOfRangeException" /> is thrown when instantiated with a value above the allowed range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_CalledWithAboveAllowedRange_ThrowsArgumentOutOfRangeException()
        {
            new GridColumnAttribute(101);
        }

        /// <summary>
        /// Test instantiated with valid argument is successful.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithValidValue_Success()
        {
            var width = 50;
            var sut = new GridColumnAttribute(width);

            Assert.IsNotNull(sut);
            Assert.AreEqual(width, sut.Width);
        }
    }
}
