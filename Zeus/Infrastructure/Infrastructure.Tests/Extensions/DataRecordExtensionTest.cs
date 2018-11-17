using System.Data;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="DataRecordExtension" />.
    /// </summary>
    [TestClass]
    public class DataRecordExtensionTest
    {
        /// <summary>
        /// Test returns true when a matching column name is found.
        /// </summary>
        [TestMethod]
        public void HasColumn_WithMatchingColumn_ReturnsTrue()
        {
            var columnName = "foo";

            var mockDataRecord = new Mock<IDataRecord>();

            mockDataRecord.Setup(m => m.FieldCount).Returns(1);
            mockDataRecord.Setup(m => m.GetName(It.IsAny<int>())).Returns("foo");

            Assert.IsTrue(mockDataRecord.Object.HasColumn(columnName));
        }

        /// <summary>
        /// Test returns false when a matching column name is found.
        /// </summary>
        [TestMethod]
        public void HasColumn_WithoutMatchingColumn_ReturnsFalse()
        {
            var columnName = "foo";

            var mockDataRecord = new Mock<IDataRecord>();

            mockDataRecord.Setup(m => m.FieldCount).Returns(1);
            mockDataRecord.Setup(m => m.GetName(It.IsAny<int>())).Returns("bar");

            Assert.IsFalse(mockDataRecord.Object.HasColumn(columnName));
        }
    }
}
