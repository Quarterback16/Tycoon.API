using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfAttributeTest
    {
        private readonly Dictionary<string, string> Greetings = new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" }, { "adios", "adios" } };

        private class Model : ContingentModel<ClearIfAttribute>
        {
            public string SingleSelect { get; set; }

            //SelectList dependent on SelectList
            [ClearIf("SingleSelect", "hello")]
            public string SingleToSingle { get; set; }

            [ClearIf("SingleSelect", ComparisonType.NotEqualTo, "hello")]
            public string SingleToSingleNotEqual { get; set; }

            [ClearIf("SingleSelect", "hello", PassOnNull = true)]
            public string SingleToSinglePassOnNull { get; set; }

            [ClearIf("SingleSelect", new object[] { "hello", "world" })]
            public string SingleToSingleMultipleExpected { get; set; }

            [ClearIf("SingleSelect", ComparisonType.NotEqualTo, new object[] { "hello", "world" })]
            public string SingleToSingleMultipleExpectedNotEqual { get; set; }

            [ClearIf("SingleSelect", "hello", FailOnNull = true)]
            public string SingleToSingleFailOnNull { get; set; }

            //MultiSelectList dependent on SelectList
            [ClearIf("SingleSelect", "hello")]
            public IEnumerable<string> MultiToSingleOneExpected { get; set; }

            [ClearIf("SingleSelect", ComparisonType.NotEqualTo, "hello")]
            public IEnumerable<string> MultiToSingleOneExpectedNotEqual { get; set; }

            [ClearIf("SingleSelect", new[] { "hello", "world" })]
            public IEnumerable<string> MultiToSingleManyExpected { get; set; }

            [ClearIf("SingleSelect", ComparisonType.NotEqualTo, new[] { "hello", "world" })]
            public IEnumerable<string> MultiToSingleManyExpectedNotEqual { get; set; }


            public MultiSelectList MultiSelect { get; set; }

            //SelectList dependent on MultiSelectList
            [ClearIf("MultiSelect", "hello")]
            public string SingleToMultiOneExpected { get; set; }

            [ClearIf("MultiSelect", ComparisonType.NotEqualTo, "hello")]
            public string SingleToMultiOneExpectedNotEqual { get; set; }

            [ClearIf("MultiSelect", new[] { "hello", "world" })]
            public string SingleToMultiManyExpected { get; set; }

            [ClearIf("MultiSelect", ComparisonType.NotEqualTo, new[] { "hello", "world" })]
            public string SingleToMultiManyExpectedNotEqual { get; set; }

            //MultiSelectList dependent on MultiSelectList
            [ClearIf("MultiSelect", "hello")]
            public IEnumerable<string> MultiToMultiOneExpected { get; set; }

            [ClearIf("MultiSelect", ComparisonType.NotEqualTo, "hello")]
            public IEnumerable<string> MultiToMultiOneExpectedNotEqual { get; set; }

            [ClearIf("MultiSelect", new[] { "hello", "world" })]
            public IEnumerable<string> MultiToMultiManyExpected { get; set; }

            [ClearIf("MultiSelect", ComparisonType.NotEqualTo, new[] { "hello", "world" })]
            public IEnumerable<string> MultiToMultiManyExpectedNotEqual { get; set; }
        }

        #region SelectList with single expected value dependent on SelectList

        /// <summary>
        /// Test property is clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithExpectedValue_Clear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsTrue(model.IsConditionMet("SingleToSingle"));
        }
        /// <summary>
        /// Test property is clear if condition is not met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionNotMetWithExpectedValue_Clear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("SingleToSingleNotEqual"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithDifferentValue_NotClear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("SingleToSingle"));
        }
        /// <summary>
        /// Test property is not clear if condition is met with a expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithExpectedValue_NotClear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleNotEqual"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithEmptyString_NotClear()
        {
            var model = new Model { SingleSelect = string.Empty };
            Assert.IsFalse(model.IsConditionMet("SingleToSingle"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNull_NotClear()
        {
            var model = new Model { SingleSelect = null };
            Assert.IsFalse(model.IsConditionMet("SingleToSingle"));
        }

        /// <summary>
        /// Test property is clear if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNullAndPassOnNull_Clear()
        {
            var model = new Model { SingleSelect = null };
            Assert.IsTrue(model.IsConditionMet("SingleToSinglePassOnNull"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with an empty string and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithEmptyStringAndPassOnNull_NotClear()
        {
            var model = new Model { SingleSelect = string.Empty };
            Assert.IsFalse(model.IsConditionMet("SingleToSinglePassOnNull"));
        }

        /// <summary>
        /// Test property is not cleared if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotClear()
        {
            var model = new Model { SingleSelect = null };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleFailOnNull"));
        }
        #endregion

        #region SelectList with multiple expected values dependent on SelectList
        /// <summary>
        /// Test property is clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneOfTheExpectedValues_Clear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsTrue(model.IsConditionMet("SingleToSingleMultipleExpected"));
        }
        /// <summary>
        /// Test property is clear if condition is not met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionNotMetWithOneOfTheExpectedValues_Clear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("SingleToSingleMultipleExpectedNotEqual"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithDifferentValueToOneOfTheExpectedValues_NotClear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleMultipleExpected"));
        }
        /// <summary>
        /// Test property is not clear if condition is met with a expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneOfTheExpectedValues_NotClear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleMultipleExpectedNotEqual"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotClear()
        {
            var model = new Model { SingleSelect = string.Empty };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleMultipleExpected"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_NotClear()
        {
            var model = new Model { SingleSelect = null };
            Assert.IsFalse(model.IsConditionMet("SingleToSingleMultipleExpected"));
        }

        #endregion

        #region SelectList with single expected value dependent on MultiSelectList
        /// <summary>
        /// Test property is clear if the condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("SingleToMultiOneExpected"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "hello" });
            Assert.IsTrue(model.IsConditionMet("SingleToMultiOneExpected"));
        }
        /// <summary>
        /// Test property is not clear if the condition didn't a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("SingleToMultiOneExpected"));
        }

        /// <summary>
        /// Test property is not clear if the condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("SingleToMultiOneExpectedNotEqual"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "hello" });
            Assert.IsFalse(model.IsConditionMet("SingleToMultiOneExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is clear if the condition didn't a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("SingleToMultiOneExpectedNotEqual"));
        }
        #endregion

        #region SelectList with mulitple expected values dependent on MultiSelectList
        /// <summary>
        /// Test property is clear if a condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultipleExpected_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("SingleToMultiManyExpected"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "world" });
            Assert.IsTrue(model.IsConditionMet("SingleToMultiManyExpected"));
        }
        /// <summary>
        /// Test property is not clear if no conditions got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultipleExpected_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("SingleToMultiManyExpected"));
        }

        /// <summary>
        /// Test property is clear if a condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultipleExpected_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("SingleToMultiManyExpectedNotEqual"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "world" });
            Assert.IsFalse(model.IsConditionMet("SingleToMultiManyExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is not clear if no conditions got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultipleExpected_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("SingleToMultiManyExpectedNotEqual"));
        }
        #endregion

        #region MultiSelectList with single expected value dependent on SelectList
        /// <summary>
        /// Test property is clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithMatch_Clear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsTrue(model.IsConditionMet("MultiToSingleOneExpected"));
        }
        /// <summary>
        /// Test property is not clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatch_NotClear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("MultiToSingleOneExpected"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithMatch_NotClear()
        {
            var model = new Model { SingleSelect = "hello" };
            Assert.IsFalse(model.IsConditionMet("MultiToSingleOneExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatch_Clear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("MultiToSingleOneExpectedNotEqual"));
        }
        #endregion

        #region MultiSelectList with multiple expected values dependent on SelectList
        /// <summary>
        /// Test property is clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithMatch_MultiSelect_MulitpleExpected_Clear()
        {
            var model = new Model { SingleSelect = "world" };
            Assert.IsTrue(model.IsConditionMet("MultiToSingleManyExpected"));
        }
        /// <summary>
        /// Test property is not clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatch_MultiSelect_MulitpleExpected_NotClear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("MultiToSingleManyExpected"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithMatch_MultiSelect_MulitpleExpected_NotClear()
        {
            var model = new Model { SingleSelect = "world" };
            Assert.IsFalse(model.IsConditionMet("MultiToSingleManyExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatch_MultiSelect_MulitpleExpected_Clear()
        {
            var model = new Model { SingleSelect = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("MultiToSingleManyExpectedNotEqual"));
        }
        #endregion

        #region MultiSelectList with single expected value dependent on MultiSelectList
        /// <summary>
        /// Test property is clear if the condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultiSelect_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("MultiToMultiOneExpected"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "hello" });
            Assert.IsTrue(model.IsConditionMet("MultiToMultiOneExpected"));
        }
        /// <summary>
        /// Test property is not clear if the condition didn't a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultiSelect_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("MultiToMultiOneExpected"));
        }

        /// <summary>
        /// Test property is not clear if the condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultiSelect_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("MultiToMultiOneExpectedNotEqual"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "hello" });
            Assert.IsFalse(model.IsConditionMet("MultiToMultiOneExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is clear if the condition didn't a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultiSelect_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("MultiToMultiOneExpectedNotEqual"));
        }
        #endregion

        #region MultiSelectList with multiple expected values dependent on MultiSelectList
        /// <summary>
        /// Test property is clear if a condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultipleExpected_MultiSelect_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("MultiToMultiManyExpected"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "world" });
            Assert.IsTrue(model.IsConditionMet("MultiToMultiManyExpected"));
        }
        /// <summary>
        /// Test property is not clear if no conditions got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultipleExpected_MultiSelect_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "adios", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("MultiToMultiManyExpected"));
        }

        /// <summary>
        /// Test property is clear if a condition got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithOneMatch_MultipleExpected_MultiSelect_NotClear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "hello", "goodbye" }) };
            Assert.IsFalse(model.IsConditionMet("MultiToMultiManyExpectedNotEqual"));

            model.MultiSelect = GetMultiSelectList(new[] { "goodbye", "world" });
            Assert.IsFalse(model.IsConditionMet("MultiToMultiManyExpectedNotEqual"));
        }
        /// <summary>
        /// Test property is not clear if no conditions got a match.
        /// </summary>
        [TestMethod]
        public void ClearIf_ConditionMetWithNoMatches_MultipleExpected_MultiSelect_Clear()
        {
            var model = new Model { MultiSelect = GetMultiSelectList(new[] { "goodbye" }) };
            Assert.IsTrue(model.IsConditionMet("MultiToMultiManyExpectedNotEqual"));
        }
        #endregion

        private MultiSelectList GetMultiSelectList(IEnumerable<string> selected)
        {
            return new MultiSelectList(Greetings, "Key", "Value", selected);
        }
    }
}
