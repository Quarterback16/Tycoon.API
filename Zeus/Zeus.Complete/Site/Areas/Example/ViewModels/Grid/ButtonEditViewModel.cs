using System.ComponentModel.DataAnnotations;

using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Grid
{
    /// <summary>
    /// Defines the View Model nested within <see cref="DisplayViewModel" />, <see cref="SingleViewModel" /> and <see cref="MultipleViewModel" />.
    /// </summary>
    [DisplayName("Claim")]
    [Link("Go back", Action = "Button")]
    public class ButtonEditViewModel
    {
        /// <summary>
        /// Message
        /// </summary>
        public ContentViewModel Message { get; set; }

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
        /// Whether the user has selected this.
        /// </summary>
        /// <remarks>
        /// Only used when <see cref="SelectionTypeAttribute" /> is set to <see cref="SelectionType.Multiple" />.
        /// </remarks>
        [Selector]
        [Hidden]
        [Bindable]
        public bool Selected { get; set; }

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