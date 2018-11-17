using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.Models
{
    
    
    /// <summary>
    ///This is a test class for CodeModelTest and is intended
    ///to contain all CodeModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CodeModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

       /// <summary>
        ///A test for CodeModel Constructor
        ///</summary>
        [TestMethod()]
        public void CodeModelConstructorTest()
        {
            var target = new CodeModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Code
        ///</summary>
        [TestMethod()]
        public void CodeTest()
        {
            var target = new CodeModel();
            const string expected = "Test";
            target.Code = expected;
            string actual = target.Code;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            var target = new CodeModel();
            const string expected = "Description";
            target.Description = expected;
            string actual = target.Description;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for EndDate
        ///</summary>
        [TestMethod()]
        public void EndDateTest()
        {
            var target = new CodeModel(); 
            var expected = new DateTime?(DateTime.Now);
            target.EndDate = expected;
            DateTime? actual = target.EndDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ShortDescription
        ///</summary>
        [TestMethod()]
        public void ShortDescriptionTest()
        {
            var target = new CodeModel();
            const string expected = "ShortDesc";
            target.ShortDescription = expected;
            string actual = target.ShortDescription;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for StartDate
        ///</summary>
        [TestMethod()]
        public void StartDateTest()
        {
            var target = new CodeModel(); 
            var expected = new DateTime?(DateTime.Now); 
            target.StartDate = expected;
            DateTime? actual = target.StartDate;
            Assert.AreEqual(expected, actual);
        }
    }
}
