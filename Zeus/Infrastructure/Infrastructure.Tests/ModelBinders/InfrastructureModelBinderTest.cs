using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.ModelBinders;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;
using System.Collections;

namespace Employment.Web.Mvc.Infrastructure.Tests.ModelBinders
{
    /// <summary>
    /// Unit tests for <see cref="InfrastructureModelBinder" />.
    /// </summary>
    [TestClass]
    public class InfrastructureModelBinderTest
    {
        public class Model
        {
            public string Property1 { get; set; }

            [Bindable]
            public string Property2 { get; set; }

            [Bindable]
            [SelectionType(SelectionType.Single)]
            public IEnumerable<SelectListItem> Property3 { get; set; }

            [Bindable]
            public SelectList Property4 { get; set; }

            [Bindable]
            public MultiSelectList Property5 { get; set; }

            [Bindable]
            public IEnumerable<string> Property6 { get; set; }
        }

        public class ModelIf
        {
            public bool Bindable { get; set; }

            public string Property1 { get; set; }

            [BindableIfTrue("Bindable")]
            public string Property2 { get; set; }

            [BindableIfTrue("Bindable")]
            [SelectionType(SelectionType.Single)]
            public IEnumerable<SelectListItem> Property3 { get; set; }

            [BindableIfTrue("Bindable")]
            public SelectList Property4 { get; set; }

            [BindableIfTrue("Bindable")]
            public MultiSelectList Property5 { get; set; }

            [BindableIfTrue("Bindable")]
            public IEnumerable<string> Property6 { get; set; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new InfrastructureModelBinder();
        }

        /// <summary>
        /// Returns a model binding context that lays out the model values as if they had been POST'ed from a form
        /// </summary>
        /// <param name="enumerableSpecification">The list of key->value pairs that was used to populate the models enumerable elements.</param>
        /// <param name="model">A model containing the data to be bound</param>
        /// <returns></returns>
        private ModelBindingContext GetModelBinderContextFor(object model)
        {
            var valueProvider = new NameValueCollection();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(model))
            {
                object value = property.GetValue(model);

                if (value is IEnumerable<SelectListItem>)
                {
                    int index = 0;
                    IEnumerable<string> selectedValues = (value as IEnumerable<SelectListItem>).Where(i => i.Selected).Select(i => i.Value);
                    valueProvider[property.Name] = string.Join(",", selectedValues); // Specify selected values
                    foreach (SelectListItem v in value as IEnumerable<SelectListItem>)
                    {
                        valueProvider[property.Name + "[" + index + "].Selected"] = v.Selected.ToString();
                        valueProvider[property.Name + "[" + index + "].Text"] = v.Text;
                        valueProvider[property.Name + "[" + index + "].Value"] = v.Value;
                        ++index;
                    }
                }
                else if (value is IEnumerable<string>)
                {
                    int index = 0;
                    valueProvider[property.Name] = string.Join(",", value as IEnumerable<string>); // Specify selected values
                    foreach (string v in value as IEnumerable<string>)
                    {
                        valueProvider[property.Name + "[" + index + "].Text"] = String.Empty;
                        valueProvider[property.Name + "[" + index + "].Value"] = v;
                        ++index;
                    }
                }
                else
                {
                    valueProvider[property.Name] = value != null ? value.ToString() : string.Empty;
                }
            }

            ModelBindingContext modelBinder = new ModelBindingContext
            {
                ModelMetadata = new InfrastructureModelMetadataProvider().GetMetadataForType(null , model.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(valueProvider, CultureInfo.InvariantCulture)
            };

            return modelBinder;
        }

        /// <summary>
        /// Used to test that two select list items are equal. Throws assert failed exception if they are not
        /// </summary>
        private void AssertSelectListItemAreEqual(SelectListItem first, SelectListItem second)
        {
            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Value, second.Value);
            Assert.AreEqual(first.Selected, second.Selected);
        }

        /// <summary>
        /// Used to test that two enumerable lists are equal. This will go through each list and test each successive item
        /// against the next. It will throw an AssertFailed Exception if the corresponding items are not equal or if the
        /// enumerables have different lengths.
        /// Works on Enumerables of SelectListItems and objects
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void AssertEnumerableAreEqual(IEnumerable first, IEnumerable second)
        {
            var a = first.GetEnumerator();
            var b = second.GetEnumerator();
            bool ax = a.MoveNext();
            bool bx = b.MoveNext();
            while(ax && bx){
                if (a.Current is SelectListItem)
                {
                    AssertSelectListItemAreEqual(a.Current as SelectListItem, b.Current as SelectListItem);
                }
                else
                {
                    Assert.AreEqual(a.Current, b.Current);
                }
                ax = a.MoveNext();
                bx = b.MoveNext();
            }
            Assert.AreEqual(ax, bx); // Make sure the enumerations were of the same length
        }

        /// <summary>
        /// Test null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InfrastructureModelBinder_NullBindingContext_ThrowsArgumentNullException()
        {
            new InfrastructureModelBinder().BindModel(new ControllerContext(), null);
        }
        
        /// <summary>
        /// Test model binding is successful.
        /// </summary>
        [TestMethod]
        public void InfrastructureModelBinder_BindModelByType_ReturnsBindedModel()
        {
            var items = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };

            var model = new Model();

            model.Property1 = "ValueThatWillNotBeBinded";
            model.Property2 = "ValueThatWillBeBinded";
            model.Property3 = items.ToSelectListItem(m => m.Key, m => m.Value);
            model.Property4 = items.ToSelectList(m => m.Key, m => m.Value, "Key2");
            model.Property5 = items.ToMultiSelectList(m => m.Key, m => m.Value, new[]{"Key1", "Key2"});
            model.Property6 = items.Keys;

            Model bindedModel = new InfrastructureModelBinder().BindModel(new ControllerContext(), GetModelBinderContextFor(model)) as Model;

            Assert.IsNotNull(bindedModel);
            Assert.IsNull(bindedModel.Property1);
            Assert.AreEqual(model.Property2, bindedModel.Property2);
            AssertEnumerableAreEqual(model.Property3, bindedModel.Property3);
            AssertEnumerableAreEqual(model.Property4, bindedModel.Property4);
            AssertEnumerableAreEqual(model.Property5, bindedModel.Property5);
            AssertEnumerableAreEqual(model.Property6, bindedModel.Property6);
        }

        /// <summary>
        /// Test model binding is successful.
        /// </summary>
        [TestMethod]
        public void InfrastructureModelBinder_BindModelIfByTypeBindable_ReturnsBindedModel()
        {
            var items = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };

            var model = new ModelIf();

            model.Bindable = true;

            model.Property1 = "ValueThatWillNotBeBinded";
            model.Property2 = "ValueThatWillBeBinded";
            model.Property3 = items.ToSelectListItem(m => m.Key, m => m.Value);
            model.Property4 = items.ToSelectList(m => m.Key, m => m.Value, "Key2");
            model.Property5 = items.ToMultiSelectList(m => m.Key, m => m.Value, new[] { "Key1", "Key2" });
            model.Property6 = items.Keys;

            ModelIf bindedModel = new InfrastructureModelBinder().BindModel(new ControllerContext(), GetModelBinderContextFor(model)) as ModelIf;

            Assert.IsNotNull(bindedModel);
            Assert.IsNull(bindedModel.Property1);
            Assert.AreEqual(model.Property2, bindedModel.Property2);
            AssertEnumerableAreEqual(model.Property3, bindedModel.Property3);
            AssertEnumerableAreEqual(model.Property4, bindedModel.Property4);
            AssertEnumerableAreEqual(model.Property5, bindedModel.Property5);
            AssertEnumerableAreEqual(model.Property6, bindedModel.Property6);
        }

        /// <summary>
        /// Test model binding is successful.
        /// </summary>
        [TestMethod]
        public void InfrastructureModelBinder_BindModelIfByTypeNotBindable_ReturnsBindedModel()
        {
            var items = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };

            var model = new ModelIf();

            model.Property1 = "ValueThatWillNotBeBinded";
            model.Property2 = "ValueThatWillBeBinded";
            model.Property3 = items.ToSelectListItem(m => m.Key, m => m.Value);
            model.Property4 = items.ToSelectList(m => m.Key, m => m.Value, "Key2");
            model.Property5 = items.ToMultiSelectList(m => m.Key, m => m.Value, new[] { "Key1", "Key2" });
            model.Property6 = items.Keys;

            ModelIf bindedModel = new InfrastructureModelBinder().BindModel(new ControllerContext(), GetModelBinderContextFor(model)) as ModelIf;

            Assert.IsNotNull(bindedModel);
            Assert.IsNull(bindedModel.Property1);
            Assert.IsNull(bindedModel.Property2);
            Assert.IsNull(bindedModel.Property3);
            Assert.IsNull(bindedModel.Property4);
            Assert.IsNull(bindedModel.Property5);
            Assert.IsNull(bindedModel.Property6);
        }
    }
}
