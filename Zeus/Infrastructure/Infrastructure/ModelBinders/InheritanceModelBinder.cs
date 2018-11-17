using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.ModelBinders
{
    /// <summary>
    /// Represents a custom Model Binder that maps a request to a data object with support for class inheritance. 
    /// </summary>
    public class InheritanceModelBinder : InfrastructureModelBinder
    {
        private IBuildManager BuildManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IBuildManager>() : null;
            }
        }

        /// <summary>Binds the model by using the specified controller context and binding context.</summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>The bound object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext" />parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ValueProvider.ContainsPrefix(string.Format("{0}.InheritanceType", bindingContext.ModelName)))
            {
                // Get inheritance type
                ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(string.Format("{0}.InheritanceType", bindingContext.ModelName));

                if (valueResult != null && !string.IsNullOrEmpty(valueResult.AttemptedValue))
                {
                    // Resolve actual type of model
                    var type = BuildManager.ResolveType(valueResult.AttemptedValue);
                    
                    // Create new model binding context for actual type
                    var modelBindingContext = new ModelBindingContext
                    {
                        ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => bindingContext.Model, type),
                        ModelName = bindingContext.ModelName,
                        ModelState = bindingContext.ModelState,
                        PropertyFilter = bindingContext.PropertyFilter,
                        ValueProvider = bindingContext.ValueProvider
                    };

                    return base.BindModel(controllerContext, modelBindingContext);
                }
            }
            
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
