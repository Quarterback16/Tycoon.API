using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate that the property is used for storing a users selection(s) of items within an enumerable.
    /// </summary>
    /// <remarks>
    /// A property within the enumerable group view model must be decorated with <see cref="KeyAttribute" />. This property should be a unique value such as a record ID.
    /// If <see cref="SelectionType" /> is single:
    /// - The selector should be on a property in the parent view model that contains the enumerable group.
    /// - The property decorated with the selector should be of the same type as the property within the enumerable group that is decorated with the <see cref="KeyAttribute" />.
    /// - If there are more than 1 selector in the parent view model, then each selector must have the <see cref="TargetProperty" /> defined and they must have unique target properties.
    /// - Only one selector per enumerable group.
    /// If <see cref="SelectionType" /> is multiple:
    /// - The selector should be on a property within the enumerable group view model.
    /// - The property decorated with the selector should be of type <see cref="bool" />.
    /// - Only one property within the enumerable group view model can be the selector.
    /// Supports having multiple enumerable group view models in the same parent model however, if there is more than one with a <see cref="SelectionType" /> of single then <see cref="TargetProperty" /> is required.
    /// </remarks>
    /// <example>
    /// This is an example for a <see cref="SelectionType" /> of single.
    /// <code><![CDATA[
    /// 
    /// [SelectionType(SelectionType.Single)]
    /// public class UserViewModel
    /// {
    ///     [Key]
    ///     [Bindable]
    ///     public UserID { get; set; }
    /// 
    ///     [Bindable]
    ///     public Name { get; set; }
    /// }
    /// 
    /// public class ParentViewModel
    /// {
    ///     [Selector]
    ///     [Hidden]
    ///     [Bindable]
    ///     public SelectedKey { get; set; }
    ///
    ///     [Bindable]
    ///     public IEnumerabe<UserViewModel> Users { get; set; }
    /// }
    /// 
    /// ]]></code>
    /// </example>
    /// <example>
    /// This is an example for a <see cref="SelectionType" /> of multiple.
    /// <code><![CDATA[
    /// 
    /// [SelectionType(SelectionType.Multiple)]
    /// public class UserViewModel
    /// {
    ///     [Selector]
    ///     [Bindable]
    ///     public bool Selected { get; set; }
    /// 
    ///     [Key]
    ///     [Bindable]
    ///     public UserID { get; set; }
    /// 
    ///     [Bindable]
    ///     public Name { get; set; }
    /// }
    /// 
    /// public class ParentViewModel
    /// {
    ///     [Bindable]
    ///     public IEnumerabe<UserViewModel> Users { get; set; }
    /// }
    /// 
    /// ]]></code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class SelectorAttribute : Attribute
    {
        /// <summary>
        /// The name of the enumerable property that the selector is intended for.
        /// </summary>
        /// <remarks>
        /// Only necessary when selector is in the parent view model, not in the enumerable view model, such as when SelectionType is single.
        /// </remarks>
        public string TargetProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.SelectorAttribute" /> class.
        /// </summary>
        public SelectorAttribute()
        {

        }
    }
}
