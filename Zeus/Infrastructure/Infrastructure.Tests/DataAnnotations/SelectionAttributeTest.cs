using System;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SelectionAttribute" />.
    /// </summary>
    [TestClass]
    public class SelectionAttributeTest
    {
        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null keys argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullKeysArgument_ThrowsArgumentNullException()
        {
            new SelectionAttribute(SelectionType.Default, null, new[] { "Value1", "Value2" });
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with empty keys argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithEmptyKeysArgument_ThrowsArgumentNullException()
        {
            new SelectionAttribute(SelectionType.Default, new string [] { }, new[] { "Value1", "Value2" });
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null values argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullValuesArgument_ThrowsArgumentNullException()
        {
            new SelectionAttribute(SelectionType.Default, new[] { "Key1", "Key2" }, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with empty values argument.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithEmptyValuesArgument_ThrowsArgumentException()
        {
            new SelectionAttribute(SelectionType.Default, new[] { "Key1", "Key2" }, new string[] { });
        }

        /// <summary>
        /// Test <see cref="ArgumentException" /> is thrown when instantiated with different lengths for the keys and values arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_CalledWithDifferentLengthKeysAndValuesArgument_ThrowsArgumentException()
        {
            new SelectionAttribute(SelectionType.Default, new[] { "Key1", "Key2" }, new[] { "Value1" });
        }

        /// <summary>
        /// Test <see cref="ArgumentException" /> is thrown when instantiated with keys containing a null value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_CalledWithKeysContainingNullArgument_ThrowsArgumentException()
        {
            new SelectionAttribute(SelectionType.Default, new[] { null, "Key2" }, new[] { "Value1", "Value2" });
        }

        /// <summary>
        /// Test <see cref="ArgumentException" /> is thrown when instantiated with keys containing an empty value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_CalledWithKeysContainingEmptyArgument_ThrowsArgumentException()
        {
            new SelectionAttribute(SelectionType.Default, new[] { string.Empty, "Key2" }, new[] { "Value1", "Value2" });
        }

        /// <summary>
        /// Test property validates for selection type none if a selection exists.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeNoneWithSelectionThatExists_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.None, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid("Key1"));
        }

        /// <summary>
        /// Test property fails for selection type none if a selection does not exist.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeNoneWithSelectionThatDoesNotExist_Fails()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.None, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsFalse(systemUnderTest.IsValid("NotExist"));
        }

        /// <summary>
        /// Test property validates  for selection type single if a selection exists.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeSingleWithSelectionThatExists_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Single, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid("Key1"));
        }

        /// <summary>
        /// Test property fails for selection type single if a selection does not exist.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeSingleWithSelectionThatDoesNotExist_Fails()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Single, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsFalse(systemUnderTest.IsValid("NotExist"));
        }

        /// <summary>
        /// Test property validates  for selection type multiple if a selection exists.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeMultipleWithSelectionThatExists_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid(new [] {"Key1", "Key2"}));
        }

        /// <summary>
        /// Test property fails for selection type multiple if a selection does not exist.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeMultipleWithSelectionThatDoesNotExist_Fails()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsFalse(systemUnderTest.IsValid(new [] {"NotExist"}));
        }

        /// <summary>
        /// Test property validates for selection type multiple if a selection does not exist.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionTypeMultipleWithEmptySelection_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid(new[] { string.Empty }));
        }

        /// <summary>
        /// Test property validates.
        /// </summary>
        [TestMethod]
        public void Selection_SelectionEmpty_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Default, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid(string.Empty));
        }

        /// <summary>
        /// Test property validates.
        /// </summary>
        [TestMethod]
        public void Selection_IsValidNullSelection_Validates()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Default, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            Assert.IsTrue(systemUnderTest.IsValid(null));
        }

        /// <summary>
        /// Test property returns select list items.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsMultiple_ReturnsSelectListItems()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });
            
            var selectItems = systemUnderTest.GetSelectListItems();

            Assert.IsTrue(selectItems.Any());
        }

        /// <summary>
        /// Test property returns select list items with selected.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsWithSelectedHavingEmptyMultiple_ReturnsSelectListItemsWithSelected()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            var selectItems = systemUnderTest.GetSelectListItems(new[] { string.Empty, "Key1" });
            
            Assert.IsTrue(selectItems.Any(m => m.Selected));
        }

        /// <summary>
        /// Test property returns select list items with selected.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsWithSelectedMultiple_ReturnsSelectListItemsWithSelected()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Multiple, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            var selectItems = systemUnderTest.GetSelectListItems(new[] { "Key1" });

            Assert.IsTrue(selectItems.Any(m => m.Selected));
        }

        /// <summary>
        /// Test property returns select list items.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsSingle_ReturnsSelectListItems()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Single, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            var selectItems = systemUnderTest.GetSelectListItems();

            Assert.IsTrue(selectItems.Any());
        }

        /// <summary>
        /// Test property returns select list items with selected.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsWithSelectedHavingEmptySingle_ReturnsSelectListItemsWithSelected()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Single, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            var selectItems = systemUnderTest.GetSelectListItems(string.Empty);

            Assert.IsTrue(selectItems.Any(m => !m.Selected));
        }

        /// <summary>
        /// Test property returns select list items with selected.
        /// </summary>
        [TestMethod]
        public void Selection_GetSelectListItemsWithSelectedSingle_ReturnsSelectListItemsWithSelected()
        {
            var systemUnderTest = new SelectionAttribute(SelectionType.Single, new[] { "Key1", "Key2" }, new[] { "Value1", "Value2" });

            var selectItems = systemUnderTest.GetSelectListItems("Key1");

            Assert.IsTrue(selectItems.Any(m => m.Selected));
        }
    }
}
