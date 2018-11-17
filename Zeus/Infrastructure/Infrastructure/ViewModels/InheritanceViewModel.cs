using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines an inheritance View Model.
    /// </summary>
    /// <remarks>
    /// View Models that use inheritance should have their base View Model inherit from this class for Model Binding to function correctly.
    /// </remarks>
    [Serializable]
    public class InheritanceViewModel
    {
        private string inheritanceType;

        /// <summary>
        /// The inheritance type to help resolve the object during Model Binding.
        /// </summary>
        [Bindable]
        [Hidden]
        public string InheritanceType
        {
            get
            {
                if (string.IsNullOrEmpty(inheritanceType))
                {
                    var type = GetType();

                    inheritanceType = string.Format("{0}, {1}", type.FullName, type.Assembly.FullName.Substring(0, type.Assembly.FullName.IndexOf(',')));
                }

                return inheritanceType;
            }
            set { inheritanceType = value; }
        }

        /// <summary>
        /// The type of the parent object.
        /// </summary>
        [Bindable]
        [Hidden]
        public string ParentType { get; set; }

        /// <summary>
        /// Property name in parent object.
        /// </summary>
        [Bindable]
        [Hidden]
        public string PropertyNameInParent { get; set; }
    }
}