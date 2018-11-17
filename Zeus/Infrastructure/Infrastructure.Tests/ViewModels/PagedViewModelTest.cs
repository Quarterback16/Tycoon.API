using Employment.Web.Mvc.Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.ViewModels
{
    /// <summary>
    ///This is a test class for PagedViewModelTest and is intended
    ///to contain all PagedViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PagedViewModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        private class TestPagedViewModel: PagedViewModel
        {
            public TestPagedViewModel()
            {
            }
        }

        /// <summary>
        ///A test for HasMorePages
        ///</summary>
        [TestMethod()]
        public void HasMorePagesTest()
        {
            PagedViewModel target = new TestPagedViewModel();
            target.Total = 100;
            target.Row = 1;
            Assert.IsTrue(target.HasMorePages());

            target.Total = 100;
            target.Row = 101;
            Assert.IsFalse(target.HasMorePages());
            
            target.Total = -1;
            Assert.IsFalse(target.HasMorePages());
        }


        /// <summary>
        ///A test for PageNumber
        ///</summary>
        [TestMethod()]
        public void PageNumberTest()
        {
            PagedViewModel target = new TestPagedViewModel();
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
            PagedViewModel target = new TestPagedViewModel();
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
            PagedViewModel target = new TestPagedViewModel();
            const string expected = "test";
            target.PropertyName = expected;
            string actual = target.PropertyName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Row
        ///</summary>
        [TestMethod()]
        public void RowTest()
        {
            PagedViewModel target = new TestPagedViewModel();
            const int expected = 5; 
            target.Row = expected;
            int actual = target.Row;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Total
        ///</summary>
        [TestMethod()]
        public void TotalTest()
        {
            PagedViewModel target = new TestPagedViewModel();
            const int expected = 50;
            target.Total = expected;
            int actual = target.Total;
            Assert.AreEqual(expected, actual);
        }
    }
}