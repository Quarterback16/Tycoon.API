using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SelectionTypeAttribute" />.
    /// </summary>
    [TestClass]
    public class SelectionTypeAttributeTest
    {
        private SelectionTypeAttribute SystemUnderTest()
        {
            return new SelectionTypeAttribute(SelectionType.Default);
        }

        /// <summary>
        /// Test property validates.
        /// </summary>
        [TestMethod]
        public void SelectionType_IsValid_Validates()
        {
            Assert.IsTrue(SystemUnderTest().IsValid(null));
        }
    }
}
