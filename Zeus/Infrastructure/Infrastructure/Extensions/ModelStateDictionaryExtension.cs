using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Web.Mvc.ModelStateDictionary" />.
    /// </summary>
    public static class ModelStateDictionaryExtension
    {
        /// <summary>
        /// Merges the dictionary of errors into the model state.
        /// </summary>
        /// <param name="modelState">An instance of model state.</param>
        /// <param name="errors">A dictionary of errors.</param>
        public static void Merge(this ModelStateDictionary modelState, Dictionary<string, string> errors)
        {
           foreach (var error in errors)
           {
               var modelStateError = modelState.FirstOrDefault(m => m.Key == error.Key);

               // Ensure we don't add duplicates by only adding if:
               // - ModelSate doesn't have a matching key yet
               // - ModelState has a matching key but doesn't have a matching error message for that key
               if (!modelState.ContainsKey(error.Key) || !(modelStateError.Value != null && modelStateError.Value.Errors != null && modelStateError.Value.Errors.Any(e => e.ErrorMessage == error.Value)))
               {
                   modelState.AddModelError(error.Key, error.Value);
               }
           }
        }
    }
}
