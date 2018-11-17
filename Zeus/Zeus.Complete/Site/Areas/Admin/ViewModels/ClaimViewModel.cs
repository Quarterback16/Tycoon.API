using System;
using System.ComponentModel.DataAnnotations;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Admin.ViewModels
{
    /// <summary>
    /// Defines the Claim display View Model
    /// </summary>
    [Serializable]
    
    public class ClaimViewModel
    {
        /// <summary>
        /// Based on the object instance hash code.
        /// </summary>
        /// <remarks>
        /// Hash is taken care of by <see cref="IMappingEngine" />, using the map configured in <See cref="ExampleMapper" />.
        /// </remarks>
        [Key]
        [Hidden]
        [Bindable]
        public string HashKey { get; set; }

      
        /// <summary>
        /// Type of claim.
        /// </summary>
        [DescriptionKey]
        [Bindable]
        public string ClaimType { get; set; }

        /// <summary>
        /// Value of claim.
        /// </summary>
        [Bindable]
        public string Value { get; set; }
    }
}