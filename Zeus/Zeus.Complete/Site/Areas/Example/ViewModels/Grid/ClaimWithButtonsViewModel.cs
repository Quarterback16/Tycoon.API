using System;
using System.ComponentModel.DataAnnotations;

using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Grid
{
    /// <summary>
    /// Defines the View Model nested within <see cref="DisplayViewModel" />, <see cref="SingleViewModel" /> and <see cref="MultipleViewModel" />.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="LinkAttribute" /> for inline buttons.  By default, the [Key] property will be passed as a parameter.  If you need something differrent, you can specify the Parameters in the [Link] attribute.
    /// </remarks>
    [Serializable]
    [DisplayName("Claim")]
    [Link("Edit", Order = 1, Action = "ButtonEdit")]
    [Link("Conditional", ActionForDependencyType.Enabled, "Value", ComparisonType.EqualTo, "UserType_DEWR", Order = 2, Action = "ButtonEdit")]
    public class ClaimWithButtonsViewModel
    {
        /// <summary>
        /// Based on the object instance hash code.
        /// </summary>
        /// <remarks>
        /// Hash is taken care of by <see cref="IMappingEngine" />, using the map configured in <See cref="ExampleMapper" />.
        /// </remarks>
        [Key]
        [DescriptionKey]
        [Bindable]
        public string HashKey { get; set; }

        /// <summary>
        /// Type of claim.
        /// </summary>
        [Bindable]
        public string ClaimType { get; set; }

        /// <summary>
        /// Value of claim.
        /// </summary>
        [Bindable]
        public string Value { get; set; }

    }
}