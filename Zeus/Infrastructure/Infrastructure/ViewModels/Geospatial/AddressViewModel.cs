using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial
{
    /// <summary>
    /// The View Model for <see cref="AddressModel"/>.
    /// </summary>
    [Group(GroupNames.Address, Order = 1, OverrideNameWithPropertyValue = "OverrideAddressGroupName", GroupType = GroupType.Logical)]
    [Group(GroupNames.CustomAddress, ActionForDependencyType.Visible, "CustomAddress", ComparisonType.EqualTo, true, Order = 2, OverrideNameWithPropertyValue = "OverrideCustomAddressGroupName")]
    [Serializable]
    public class AddressViewModel : IValidatableObject
    {
        internal const string AjaxProperty = "SingleLineAddress";
        internal const string AjaxPropertyModelMetadataKey = "IsAddressViewModelAjaxProperty";
        internal const string SingleLineAddressValidationError = "Please enter the {0}";
        internal const string AddressLine1ValidationError = "Please enter the first address line";
        internal const string LocalityValidationError = "Please enter the Suburb";
        internal const string StateValidationError = "Please enter the State";
        internal const string PostcodeValidationError = "Please enter the Postcode";
        internal const string ReturnLatLongData = "ReturnLatLongDetails";

        /// <summary>
        /// Override value for Address group.
        /// </summary>
        [Hidden(ExcludeFromView = true)]
        public string OverrideAddressGroupName { get { return string.Format("{0} {1}", AddressType, GroupNames.Address); } }

        /// <summary>
        /// Override value for CustomAddress group.
        /// </summary>
        [Hidden(ExcludeFromView = true)]
        public string OverrideCustomAddressGroupName { get { return string.Format("{0} {1}", AddressType, GroupNames.CustomAddress); } }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="AddressViewModel"/> is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if read only; otherwise, <c>false</c>.
        /// </value>
        [Hidden]
        [Bindable]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AddressViewModel"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [Hidden]
        [Bindable]
        public bool Required { get; set; }


        /// <summary>
        /// Whether to return Latitude, longitude (co-ordinate) information along with regions and Confidence level.
        /// </summary>
        [Bindable]
        [Hidden]
        public bool ReturnLatLongDetails { get; set; }

        /// <summary>
        /// Latitude data.
        /// </summary>
        [Bindable]
        [Hidden]
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude data.
        /// </summary>
        [Bindable]
        [Hidden]
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the type of the address displayed in the Address Editor Groups.
        /// </summary>
        /// <value>
        /// The type of the address.
        /// </value>
        [Hidden]
        [Bindable]
        public string AddressType { get; set; }

        /// <summary>
        /// An override for <see cref="SingleLineAddress" /> when <see cref="ReadOnly" /> is <c>true</c>.
        /// </summary>
        [Hidden(ExcludeFromView = true)]
        public string DisplayNameForSingleLineAddress
        {
            get { return string.Format((ReadOnly) ? "{0} Address" : "Find / select valid {0} Address", AddressType); }
        }

        /// <summary>
        /// Gets the value of the single line address field.
        /// </summary>
        /// <value>
        /// The single line address.
        /// </value>
        /// <remarks>
        /// Note, if you change the name of this property then you must update its name in <see cref="AjaxProperty" /> as well.
        /// </remarks>
        [Display(GroupName = GroupNames.Address, Name = "Find / select valid Australian address", Order = 1)]
        [Row("1", RowType.Flow)]
        [RequiredIfTrue("Required", ErrorMessage = SingleLineAddressValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]
        [EditableIfFalse("CustomAddress")]
        [ClearIfTrue("CustomAddress")]
        [OverrideDisplayName("DisplayNameForSingleLineAddress")]
        public string SingleLineAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Line1))
                {
                    return string.Empty;
                }

                if (string.IsNullOrWhiteSpace(Line2))
                {
                    return string.Format("{0} {1} {2} {3}", Line1, Locality, State, Postcode);
                }

                if (string.IsNullOrWhiteSpace(Line3))
                {
                    return string.Format("{0} {1} {2} {3} {4}", Line1, Line2, Locality, State, Postcode);
                }

                return string.Format("{0} {1} {2} {3} {4} {5}", Line1, Line2, Line3, Locality, State, Postcode);
            }
            set
            {
                
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the custom address fields are displayed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the custom address fields are displayed; otherwise, <c>false</c>.
        /// </value>
        [Bindable]
        [Display(GroupName = GroupNames.Address, Name = "OR check this if not found", Order = 2)]
        [DataType(CustomDataType.CheckBoxList)]
        [VisibleIfFalse("ReadOnly")]
        [Row("1", RowType.Flow)]
        public bool CustomAddress { get; set; }

        /// <summary>
        /// Gets or sets the first line of the Address.
        /// </summary>
        /// <value>
        /// The first line of the Address.
        /// </value>
        [Bindable]
        [Display(Name = "Address Line 1", GroupName = GroupNames.CustomAddress, Order = 3)]
        [Row("2")]
        [RequiredIfTrue("Required", ErrorMessage = AddressLine1ValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]
        [StringLength(50)]
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the Address.
        /// </summary>
        /// <value>
        /// The second line of the Address.
        /// </value>
        [Bindable]
        [Display(Name = "Address Line 2", GroupName = GroupNames.CustomAddress, Order = 4)]
        [Row("3")]
        [ReadOnlyIfTrue("ReadOnly")]
        [StringLength(50)]
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the third line of the Address.
        /// </summary>
        /// <value>
        /// The third line of the Address.
        /// </value>
        [Bindable]
        [Display(Name = "Address Line 3", GroupName = GroupNames.CustomAddress, Order = 5)]
        [Row("4")]
        [ReadOnlyIfTrue("ReadOnly")]
        [StringLength(50)]
        public string Line3 { get; set; }

        /// <summary>
        /// Gets or sets the locality of the Address. The locality is the town, suburb or other locality name of the Address.
        /// </summary>
        /// <value>
        /// The locality of the Address.
        /// </value>
        [Bindable]
        [Display(Name = "Suburb", GroupName = GroupNames.CustomAddress, Order = 8)]
        [Row("5", RowType.Third)]
        [AjaxSelection("GetLocality", Controller = "Ajax", Area = "", Parameters = new[] { "Postcode" })]
        [EditableIfNotEmpty("Postcode")]
        [RequiredIfTrue("Required", ErrorMessage = LocalityValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the state the Address is contained in.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Bindable]
        [Display(Name = "State", GroupName = GroupNames.CustomAddress, Order = 6)]
        [Row("5", RowType.Third)]
        [AjaxSelection("GetState", Controller = "Ajax", Area = "")]
        [RequiredIfTrue("Required", ErrorMessage = StateValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the post code of the Address.
        /// </summary>
        /// <value>
        /// The post code.
        /// </value>
        [Bindable]
        [Display(Name = "Postcode", GroupName = GroupNames.CustomAddress, Order = 7)]
        [Row("5", RowType.Third)]
        [AjaxSelection("GetPostcode", Controller = "Ajax", Area = "", Parameters = new[] { "State" })]
        [EditableIfNotEmpty("State")]
        [RequiredIfTrue("Required", ErrorMessage = PostcodeValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the reliability of the Address.
        /// </summary>
        /// <value>
        /// The reliability of the Address.
        /// </value>
        [Bindable]
        [Display(Name = "Reliability", GroupName = GroupNames.CustomAddress, Order = 1)]
        [Hidden]
        public AddressReliability Reliability { get; set; }

        /// <summary>
        /// Server-side validation to make sure that the minimum is supplied if any of the custom address properties have a value or the address is required.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A list of validation results, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // If any properties have a value or is required
            if (Required || !string.IsNullOrWhiteSpace(Line1) || !string.IsNullOrWhiteSpace(Line2) || !string.IsNullOrWhiteSpace(Line3) || !string.IsNullOrEmpty(State) || !string.IsNullOrWhiteSpace(Postcode) || !string.IsNullOrWhiteSpace(Locality))
            {
                // Show only one error for the single line address if not a custom address
                if (!CustomAddress)
                {
                    if (string.IsNullOrEmpty(SingleLineAddress))
                    {
                        results.Add(new ValidationResult(string.Format(SingleLineAddressValidationError, OverrideAddressGroupName), new[] { "SingleLineAddress" }));
                    }

                    return results;
                }

                // Make sure the minium is supplied so show an error for each field
                if (string.IsNullOrWhiteSpace(Line1))
                {
                    results.Add(new ValidationResult(AddressLine1ValidationError, new[] { "Line1" }));
                }

                if (string.IsNullOrEmpty(State))
                {
                    results.Add(new ValidationResult(StateValidationError, new[] { "State" }));
                }

                if (string.IsNullOrEmpty(Postcode))
                {
                    results.Add(new ValidationResult(PostcodeValidationError, new[] { "Postcode" }));
                }

                if (string.IsNullOrEmpty(Locality))
                {
                    results.Add(new ValidationResult(LocalityValidationError, new[] { "Locality" }));
                }
            }

            return results;
        }
    }
}
