using System;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    
    internal class TestComparison
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

        /// <summary>
    ///This is a test class for ComparisonTypeExtensionTest and is intended
    ///to contain all ComparisonTypeExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComparisonTypeExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest()
        {
            var t = new TestComparison();
            Assert.IsNotNull(t);
            t.Value = 5;
            t.Text = "ABC";

            Assert.IsTrue(ComparisonType.EqualTo.Compare(t.Value, 5)); // ==5
            Assert.IsTrue(ComparisonType.GreaterThan.Compare(t.Value, 4));  // >4
            Assert.IsTrue(ComparisonType.LessThan.Compare(t.Value, 6));  // < 6
            Assert.IsTrue(ComparisonType.NotEqualTo.Compare(t.Value, 55));  // !=
            Assert.IsTrue(ComparisonType.GreaterThanOrEqualTo.Compare(t.Value, 5)); // =
            Assert.IsTrue(ComparisonType.LessThanOrEqualTo.Compare(t.Value, 5));  // =

            Assert.IsTrue(ComparisonType.RegExMatch.Compare(t.Text,"[ABC]"));
            Assert.IsTrue(ComparisonType.NotRegExMatch.Compare(t.Text, "[XYZ]"));
        }

        [TestMethod()]
        public void CompareTestWithNull()
        {
            Assert.IsFalse(ComparisonType.EqualTo.Compare(null, 5));
            Assert.IsFalse(ComparisonType.GreaterThan.Compare(null, 4));  // >4
            Assert.IsFalse(ComparisonType.LessThan.Compare(null, 6));  // < 6
            Assert.IsTrue(ComparisonType.NotEqualTo.Compare(null, 55));  // !=
            Assert.IsFalse(ComparisonType.GreaterThanOrEqualTo.Compare(null, 5)); // =
            Assert.IsFalse(ComparisonType.LessThanOrEqualTo.Compare(null, 5));  // =

            Assert.IsFalse(ComparisonType.RegExMatch.Compare(null, "[ABC]"));
            Assert.IsTrue(ComparisonType.NotRegExMatch.Compare(null, "[XYZ]"));
        }

        [TestMethod()]
        public void CompareTestBothNull()
        {
            Assert.IsTrue(ComparisonType.EqualTo.Compare(null, null));
            Assert.IsFalse(ComparisonType.GreaterThan.Compare(null, null));
            Assert.IsFalse(ComparisonType.LessThan.Compare(null, null));
            Assert.IsFalse(ComparisonType.NotEqualTo.Compare(null, null)); 
            Assert.IsTrue(ComparisonType.GreaterThanOrEqualTo.Compare(null, null));
            Assert.IsTrue(ComparisonType.LessThanOrEqualTo.Compare(null, null));  
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CompareRegexNull_ExpectedException()
        {
            Assert.IsFalse(ComparisonType.RegExMatch.Compare(null, null));
            Assert.Fail("Expected Exception");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CompareNotRegexNull_ExpectedException()
        {
            Assert.IsFalse(ComparisonType.NotRegExMatch.Compare(null, null));
            Assert.Fail("Expected Exception");
        }
    }
}