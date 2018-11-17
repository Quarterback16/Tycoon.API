using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ModelBinders;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Infrastructure.Tests.ModelMetadataProviders
{
    /// <summary>
    /// Unit tests for <see cref="InfrastructureModelMetadataProvider" />.
    /// </summary>
    [TestClass]
    public class InfrastructureModelMetadataProviderTest
    {
        public class Model
        {
            [Bindable]
            [Hidden(LabelOnly = true)]
            public string Property1 { get; set; }

            [Bindable]
            [Hidden]
            public string Property2 { get; set; }

            [Bindable]
            [RequiredIf("Property1", "foo")]
            public string Property3 { get; set; }

            [Bindable]
            [VisibleIf("Property1", "foo")]
            public string Property4 { get; set; }

            [Bindable]
            [EditableIf("Property1", "foo")]
            public string Property5 { get; set; }

            [Bindable]
            [ReadOnlyIf("Property1", "foo")]
            public string Property6 { get; set; }

            [Bindable]
            public ContentViewModel Property7 { get; set; }

            [Bindable]
            [Selection(SelectionType.Single, new[] { "Key1" }, new [] { "Value1" })]
            public string Property8 { get; set; }

            [Bindable]
            [AdwSelection(SelectionType.Single, AdwType.ListCode, "FOO")]
            public string Property9 { get; set; }

            [Bindable]
            public ComparisonType Property10 { get; set; }

            [Bindable]
            public int Property11 { get; set; }

            [Bindable]
            [SelectionType(SelectionType.Single)]
            public IEnumerable<SelectListItem> Property12 { get; set; }

            [Bindable]
            [SelectionType(SelectionType.Multiple)]
            public IEnumerable<NestedModel> NestedPropertyModels { get; set; }
        }

        public class NestedModel
        {
            [Selector]
            public bool Selected { get; set; }

            [Key]
            public long ID { get; set; }

            [Bindable]
            [Hidden(LabelOnly = true)]
            public string Property1 { get; set; }

            [Bindable]
            [Hidden]
            public string Property2 { get; set; }

            [Bindable]
            [RequiredIf("Property1", "foo")]
            public string Property3 { get; set; }

            [Bindable]
            [VisibleIf("Property1", "foo")]
            public string Property4 { get; set; }

            [Bindable]
            [EditableIf("Property1", "foo")]
            public string Property5 { get; set; }

            [Bindable]
            [ReadOnlyIf("Property1", "foo")]
            public string Property6 { get; set; }

            [Bindable]
            public ContentViewModel Property7 { get; set; }

            [Bindable]
            [Selection(SelectionType.Single, new[] { "Key1" }, new[] { "Value1" })]
            public string Property8 { get; set; }

            [Bindable]
            [AdwSelection(SelectionType.Single, AdwType.ListCode, "FOO")]
            public string Property9 { get; set; }

            [Bindable]
            public ComparisonType Property10 { get; set; }

            [Bindable]
            public int Property11 { get; set; }

            [Bindable]
            [SelectionType(SelectionType.Single)]
            public IEnumerable<SelectListItem> Property12 { get; set; }
        }

        /// <summary>
        /// Test create metadata returns metadata.
        /// </summary>
        [TestMethod]
        public void InfrastructureModelMetadataProvider_CreateMetadata_ReturnsMetadata()
        {
            var model = new Model();

            model.NestedPropertyModels = new[] { new NestedModel { Property12 = new[] { new SelectListItem { Text = "Text", Value = "Value" } } } };
            model.Property12 = new[] { new SelectListItem { Text = "Text", Value = "Value" } };

            ModelBindingContext modelBinder = new ModelBindingContext
            {
                ModelMetadata = new InfrastructureModelMetadataProvider().GetMetadataForType(() => model, model.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
            };

            var bindedModel = new InfrastructureModelBinder().BindModel(new ControllerContext(), modelBinder);

            var metadata = new InfrastructureModelMetadataProvider().GetMetadataForType(() => bindedModel, bindedModel.GetType());

            Assert.IsNotNull(metadata);

            var metadatas = new InfrastructureModelMetadataProvider().GetMetadataForProperties(bindedModel, bindedModel.GetType());

            foreach (var meta in metadatas)
            {
                Assert.IsNotNull(metadata);
            }

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(bindedModel))
            {
                metadata = new InfrastructureModelMetadataProvider().GetMetadataForProperty(() => bindedModel, bindedModel.GetType(), property.Name);

                Assert.IsNotNull(metadata);
            }
        }
    }
}
