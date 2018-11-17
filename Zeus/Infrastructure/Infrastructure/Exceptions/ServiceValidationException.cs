using System;
using System.Collections.Generic;
using System.Linq;

namespace Employment.Web.Mvc.Infrastructure.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a service validation fails.
    /// </summary>
    [Serializable]
    public class ServiceValidationException : Exception
    {
        /// <summary>
        /// Collection of service validation errors.
        /// </summary>
        public Dictionary<string, string> Errors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceValidationException" /> class.
        /// </summary>
        /// <param name="error">The service validation error message.</param>
        public ServiceValidationException(string error) : base(error)
        {
            Errors = new Dictionary<string, string> { { string.Empty, error } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceValidationException" /> class.
        /// </summary>
        /// <param name="error">The service validation error.</param>
        /// <param name="parameterName">The name of the parameter that caused the error.</param>
        public ServiceValidationException(string error, string parameterName) : base(error)
        {
            Errors = new Dictionary<string, string> { { parameterName, error } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceValidationException" /> class.
        /// </summary>
        /// <param name="errors">The collection of service validation errors.</param>
        public ServiceValidationException(Dictionary<string, string> errors) : base(GetFirstErrorMessage(errors))
        {
            Errors = errors;
        }

        private static string GetFirstErrorMessage(Dictionary<string, string> errors)
        {
            return errors.First().Value;
        } 
    }
}
