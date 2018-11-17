using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="EnumerableExtension" />.
    /// </summary>
    [TestClass]
    public class EnumerableExtensionTest
    {
        /// <summary>
        /// Test for each runs successfully.
        /// </summary>
        [TestMethod]
        public void ForEach_Valid()
        {
            var list = new List<int> { 1 }.AsEnumerable();

            bool called = false;

            list.ForEach(i => called = true);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void SelectListItemTest()
        {
            var list = new Dictionary<string,string> { {"1" ,"one"}};
            var sel = list.ToSelectListItem(m=>m.Key, m=>m.Value);
            
            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.Count()==1);
            Assert.IsTrue(sel.First().Value == "1");
            Assert.IsTrue(sel.First().Text.Equals("one"));
        }

        [TestMethod]
        public void ToSelectListItemTest()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" }, { "4", "four" } };
            var sel = list.ToSelectListItem(m => m.Key, m => m.Value,"3");
            
            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.First(s=>s.Selected).Value == "3");
            Assert.IsTrue(sel.First(s=>s.Selected).Text.Equals("three"));
        }

        [TestMethod]
        public void ToSelectListItemTest_SelectValuesArray()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" }, { "4", "four" } };
            var sel = list.ToSelectListItem(m => m.Key, m => m.Value, new [] { "2", "3"});

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.First(s => s.Selected).Value == "2");
            Assert.IsTrue(sel.First(s => s.Selected).Text.Equals("two"));

            Assert.IsTrue(sel.Last(s => s.Selected).Value == "3");
            Assert.IsTrue(sel.Last(s => s.Selected).Text.Equals("three"));
        }


        [TestMethod]
        public void ToSelectListItemTest_SelectedValueArray()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" }, { "4", "four" } };
            var sel = list.ToSelectListItem(m => m.Key, m => m.Value, new List<string>{ "2" });

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.First(s => s.Selected).Value == "2");
            Assert.IsTrue(sel.First(s => s.Selected).Text.Equals("two"));
        }


        [TestMethod]
        public void SelectListTest()
        {
            var list = new Dictionary<string, string> { { "1", "one" } };
            var sel = list.ToSelectList(m => m.Key, m => m.Value);

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.Count() == 1);
            Assert.IsTrue(sel.First().Value == "1");
            Assert.IsTrue(sel.First().Text.Equals("one"));
        }


        [TestMethod]
        public void SelectList_WithSelectValue()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" }, { "4", "four" } };
            var sel = list.ToSelectList(m => m.Key, m => m.Value, "3");

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.First(s=>s.Selected).Value == "3");
            Assert.IsTrue(sel.First(s => s.Selected).Text.Equals("three"));
        }


        [TestMethod]
        public void SelectListItemTest_WithSelectValues()
        {
            var list = new Dictionary<string, string> { { "1", "one" } , {"2","two"} , {"3","three"} };
            
            var sel = list.ToSelectListItem(m => m.Key, m => m.Value, new [] {"2"});

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.Count() == 3);
            Assert.IsTrue(sel.First(m=>m.Selected).Value == "2");
            Assert.IsTrue(sel.First(m => m.Selected).Text == "two");
        }

        [TestMethod]
        public void SelectListItemTest_WithSelectValues1()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" } };
            var l = new List<string> {"2"};
            var sel = list.ToSelectListItem(m => m.Key, m => m.Value, l);

            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.Count() == 3);
            Assert.IsTrue(sel.First(m => m.Selected).Value == "2");
            Assert.IsTrue(sel.First(m => m.Selected).Text == "two");
        }


        [TestMethod]
        public void MultiSelectListItemTest()
        {
            var list = new Dictionary<string, string> { { "1", "one" } , {"2","two"} , {"3","three"} };

            var sel = list.ToMultiSelectList(m=>m.Key,m=>m.Value);
            Assert.IsNotNull(sel);
            Assert.IsTrue(sel.Count() == 3);

        }

        [TestMethod]
        public void MultiSelectListItemTest_WithSelectValues()
        {
            var list = new Dictionary<string, string> { { "1", "one" }, { "2", "two" }, { "3", "three" }, {"4","four"} };

            var sel = list.ToMultiSelectList(m => m.Key, m => m.Value, new [] {"2","3"});
            Assert.IsNotNull(sel);
            Assert.IsNotNull(sel.SelectedValues);

            Assert.IsTrue(sel.Count() == 4);
            Assert.IsTrue(sel.First(m => m.Selected).Value == "2");
            Assert.IsTrue(sel.First(m => m.Selected).Text == "two");

            Assert.IsTrue(sel.Last(m=>m.Selected).Value == "3");
            Assert.IsTrue(sel.Last(m => m.Selected).Text == "three");
        }
    }
}