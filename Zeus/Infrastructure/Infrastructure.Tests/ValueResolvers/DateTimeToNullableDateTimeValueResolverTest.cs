using System;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="DateTimeToNullableDateTimeValueResolver" />.
    ///</summary>
    [TestClass]
    public class DateTimeToNullableDateTimeValueResolverTest
    {
  
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Test resolves source of <see cref="DateTime.MinValue" /> to destination of null.
        ///</summary>
        [TestMethod]
        public void DateTimeToNullableDateTimeValueResolver_ResolveWithSourceMinValue_ResolvesDestinationAsNull()
        {
            DateTime source = DateTime.MinValue;
            DateTime? destination = DateTime.MaxValue;

            destination = DateTimeToNullableDateTimeValueResolver.Resolve(source);

            Assert.IsNull(destination);
            Assert.IsFalse(((DateTime?)destination).HasValue);
        }

        /// <summary>
        /// Test resolves source of a value to destination of same value.
        ///</summary>
        [TestMethod]
        public void DateTimeToNullableDateTimeValueResolver_ResolveWithSourceValue_ResolvesDestinationAsSameValue()
        {
            DateTime source = DateTime.MaxValue;
            DateTime? destination = DateTime.MinValue;

            destination = DateTimeToNullableDateTimeValueResolver.Resolve(source);

            Assert.IsNotNull(destination);
            Assert.IsTrue(destination.HasValue);
            Assert.IsNotNull(destination.Value);
            Assert.IsTrue(destination.Value.Equals(source));
        }
    }
}
