using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="TriggerAttribute" />.
    /// </summary>
    [TestClass]
    public class TriggerAttributeTest
    {
        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null submit type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullSubmitTypeArgument_ThrowsArgumentNullException()
        {
            new TriggerAttribute(null);
        }

        /// <summary>
        /// Test instantiated with valid argument is successful.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithValidSubmitTypeArgument_Success()
        {
            var submitType = "foo";
            var sut = new TriggerAttribute(submitType);

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.SubmitType == submitType);
        }
    }
}
