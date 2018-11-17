using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.Types
{
    /// <summary>
    ///This is a test class for PageMetadataTest and is intended
    ///to contain all PageMetadataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageMetadataTest
    {
        private class GridModel
        {
            [Key]
            public int ID { get; set; }

            public string Data { get; set; }
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for PageMetadata Constructor
        ///</summary>
        [TestMethod()]
        public void PageMetadataConstructorTest()
        {
            var target = new PageMetadata();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for HasMorePages
        ///</summary>
        [TestMethod()]
        public void HasMorePagesTest_ExpectFalse()
        {
            var target = new PageMetadata(); 
            bool actual = target.HasMorePages();
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///A test for HasMorePages
        ///</summary>
        [TestMethod()]
        public void HasMorePagesTest_ExpectTrue()
        {
            var target = new PageMetadata();
            target.Total = 100;
            target.PageSize = 10;
            target.PageNumber = 2;
            bool actual = target.HasMorePages();
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for HasMorePages
        ///</summary>
        [TestMethod()]
        public void HasMorePagesTest_ExpectFalse_PagesEnd()
        {
            var target = new PageMetadata();
            target.Total = 100;
            target.PageSize = 10;
            target.PageNumber = 11;
            bool actual = target.HasMorePages();
            Assert.AreEqual(false, actual);
        }


        /// <summary>
        ///A test for ModelType
        ///</summary>
        [TestMethod()]
        public void ModelTypeTest()
        {
            var target = new PageMetadata(); 
            target.ModelType = typeof(GridModel);
            Type actual = target.ModelType;
            Assert.AreEqual(typeof(GridModel), actual);
        }

        /// <summary>
        ///A test for PageNumber
        ///</summary>
        [TestMethod()]
        public void PageNumberTest()
        {
            var target = new PageMetadata();
            const int expected = 5;
            target.PageNumber = expected;
            int actual = target.PageNumber;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PageSize
        ///</summary>
        [TestMethod()]
        public void PageSizeTest()
        {
            var target = new PageMetadata(); 
            const int expected = 5;
            target.PageSize = expected;
            int actual = target.PageSize;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PropertyName
        ///</summary>
        [TestMethod()]
        public void PropertyNameTest()
        {
            var target = new PageMetadata();
            const string expected = "Property";
            target.PropertyName = expected;
            string actual = target.PropertyName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Total
        ///</summary>
        [TestMethod()]
        public void TotalTest()
        {
            var target = new PageMetadata(); 
            const int expected = 10;
            target.Total = expected;
            int actual = target.Total;
            Assert.AreEqual(expected, actual);
        }
    }
}