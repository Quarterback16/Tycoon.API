using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.ModelBinders;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;

namespace Employment.Web.Mvc.Infrastructure.Helpers
{
    /// <summary>
    /// Represents a helper that is used to assist with unit testing.
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// Gets the model state from Model binding the specified view model.
        /// </summary>
        /// <remarks>
        /// Intended for use in View Models tests.
        /// </remarks>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="viewModel">The view model instance.</param>
        /// <returns>The model state of the view model.</returns>
        public static ModelStateDictionary GetModelState<T>(T viewModel) where T : class
        {
            var modelBinder = new ModelBindingContext
            {
                ModelMetadata = new InfrastructureModelMetadataProvider().GetMetadataForType(() => viewModel, viewModel.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
            };

            new InfrastructureModelBinder().BindModel(new ControllerContext(), modelBinder);

            return modelBinder.ModelState;
        }
    }
}
