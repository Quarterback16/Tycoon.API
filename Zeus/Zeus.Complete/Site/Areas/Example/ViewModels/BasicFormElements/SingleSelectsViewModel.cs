using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Area.Example.Types;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.BasicFormElements
{
    [Group("Overview")]
    [Group("Selection attribute")]
    [Button("Submit", GroupName = "Selection attribute")]
    [Button("Clear", GroupName = "Selection attribute", Clear = true)]
    [Group("Enums")]
    [Button("Submit", GroupName = "Enums")]
    [Button("Clear", GroupName = "Enums", Clear = true)]
    [Group("Custom models")]
    [Button("Submit", GroupName = "Custom models")]
    [Button("Clear", GroupName = "Custom models", Clear = true)]
    public class SingleSelectsViewModel
    {
        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Use the ""Selection"" attribute in places where the user is required to select a single option from a defined list of options.")
                    .AddParagraph(@"Examples in this section sometimes use the ""SelectList"" type as their basis instead of string. ")
                    .AddParagraph("In these cases, to retrieve the selected value in your controller after submission:")
                    .AddPreformatted("        var selectedValue = model.PropertyName.SelectedValue as string;")
                    ;
                return content;
            }
        }


        #region By selection attribute

        [Display(GroupName = "Selection attribute")]
        public ContentViewModel ContentForOption
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A simple case where there are a small number of well known options can be achieved with the ""Selection"" attribute.
        /// It is required to specify both the ""values"" of each option, as well as the text to display to users.
        [Display(GroupName = ""Selection attribute"", Name = ""Option (single selection)"")]
        [Selection(SelectionType.Single, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [Bindable]
        public string Option { get; set; }")
                ;

                return content;
            }
        }

        [Display(GroupName = "Selection attribute", Name = "Option (single selection)")]
        [Selection(SelectionType.Single, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [Bindable]
        public string Option { get; set; }
        #endregion

        #region By selection attribute, with default

        [Display(GroupName = "Selection attribute")]
        public ContentViewModel ContentForOptionWithDefault
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// The ""Selection"" attribute can be used in conjunction with a default option
        [Display(GroupName = ""Selection attribute"", Name = ""Option with default"")]
        [Selection(SelectionType.Single, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [DefaultValue(""3"")]
        [Bindable]
        public string OptionWithDefault { get; set; }")
                ;
                return content;
            }
        }

        [Display(GroupName = "Selection attribute", Name = "Option with default")]
        [Selection(SelectionType.Single, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [DefaultValue("3")]
        [Bindable]
        public string OptionWithDefault { get; set; }
        #endregion

        #region Horizontal radios
        [Display(GroupName = "Selection attribute")]
        public ContentViewModel ContentForOptionRadioHorizontal
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// The ""DataType"" attribute can be used to display horizontal radio buttons. A default should always be used with this option.
        [Display(GroupName = ""Selection attribute"", Name = ""Horizontal radios"")]
        [Selection(SelectionType.Single, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        [DefaultValue(""5"")]
        [Bindable]
        public string OptionRadioHorizontal { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Selection attribute", Name = "Horizontal radios")]
        [Selection(SelectionType.Single, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        [Bindable]
        [DefaultValue("5")]
        public string OptionRadioHorizontal { get; set; }
        #endregion

        #region Vertical radios
        [Display(GroupName = "Selection attribute")]
        public ContentViewModel ContentForOptionRadioVertical
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// The ""DataType"" attribute can be used to display vertical radio buttons. A default should always be used with this option.
        [Display(GroupName = ""Selection attribute"", Order = 6, Name = ""Vertical radios"")]
        [Selection(SelectionType.Single, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [DataType(CustomDataType.RadioButtonGroupVertical)]
        [DefaultValue(""2"")]
        [Bindable]
        public string OptionRadioVertical { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Selection attribute", Order = 6, Name = "Vertical radios")]
        [Selection(SelectionType.Single, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [DataType(CustomDataType.RadioButtonGroupVertical)]
        [Bindable]
        [DefaultValue("2")]
        public string OptionRadioVertical { get; set; }
        #endregion

        #region enums
        
        [Display(GroupName="Enums")]
        public ContentViewModel EnumsOverview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"You can use Enums to drive a single select list also. To do so, decorate your enum options with the ""Description"" Attribute")
                    .AddPreformatted(@"
    /// The default value will only be used in View Models where the enum property is defined as nullable.
    /// If you wish to set a default on a non nullable property, you must explicitly set the value of the first option to 1, so that an unselected value can be identified
    [DefaultValue(Option2)]
    public enum EnumType
    {
        [Description(""Option one"")]
        Option1 = 1,
        [Description(""Option two"")]
        Option2,
        [Description(""Option three"")]
        Option3,
        ... etc. 
    }

    public class SingleSelectsViewModel
    {
        ...

        [Display(GroupName = ""Enums"", Name = ""A nullable enum (default: Option two)"")]
        [Bindable]
        public EnumType? NullableEnum { get; set; }
        
        ...
    }

")
                    ;
            }
        }

        [Display(GroupName = "Enums", Name = "A nullable enum (default: Option two)")]
        [Bindable]
        public EnumType? NullableEnum { get; set; }

        [Display(GroupName = "Enums")]
        public ContentViewModel ContentForEnum
        {
            get
            {
                return new ContentViewModel()
                    .AddPreformatted(@"
        // A non nullable enum will automatically be marked as required, and will default to the first value, unless the first value has been set explicitly to 1.
        [Display(GroupName = ""Enums"", Name = ""An enum"")]
        [Bindable]
        public EnumType Enum { get; set; }
");
            }
        }

        [Display(GroupName = "Enums", Name = "An enum")]
        [Bindable]
        public EnumType Enum { get; set; }

        #endregion

        #region Custom models overview

        /// <summary>
        /// Content for describing the View.
        /// </summary>
        [Display(GroupName = "Custom models")]
        public ContentViewModel CustomModelOverview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("If the selection attribute is not appropriate, you can build your own select list for use. ")
                    .AddParagraph("The data in this example was populated in the Controller's GET action in this way:")
                    .AddPreformatted(@"        var model = new SingleSelectsViewModel();
        // SelectList
        model.Age = AdwService.GetListCodes(""AGE"").ToSelectList(m => m.Code, m => m.Description);
        
        // Enumerable<SelectListItem>
        model.State = AdwService.GetListCodes(""STT"").ToSelectListItem(m => m.Code, m => m.Description);

        return View(model);")
                    .AddParagraph("You do not need to repeat this population in the POST action as it is automatically bound on submit.")
                    .AddParagraph("Although we have used the ADW service to populate this example, if you actually were selecting an ADW code, consider the AdwSelection attribute'")
                    .AddLink("AdwSelection", "ServerSideElements", "More about the AdwSelection attribute.")
                    .AddLineBreak();
                    ;

                return content;
            }
        }

        #endregion

        #region Select list
        [Display(GroupName = "Custom models")]
        public ContentViewModel ContentForAge
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(
                        @"        /// A single selection for ages populated manually.
        [Display(GroupName = ""Custom models"", Name = ""Age"")]
        [Bindable]
        public SelectList Age { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Custom models", Name = "Age")]
        [Bindable]
        public SelectList Age { get; set; }
        #endregion

        #region Enumerable
        [Display(GroupName = "Custom models")]
        public ContentViewModel ContentForState
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A single selection for states populated manually, and using IEnumerable instead of SelectList.
        [Display(GroupName = ""Custom models"", Name = ""State"")]
        [SelectionType(SelectionType.Single)]
        [Bindable]
        public IEnumerable<SelectListItem> State { get; set; }")
                    .AddParagraph("Note that the property is decorated with the [SelectionType] attribute as the selection type cannot be implied from the property type.")
                    .AddParagraph("To retrieve the selected value in your Controller after submit:")
                    .AddPreformatted(@"        var selectedValue = string.Empty;
        var selectedItem = model.State.FirstOrDefault(m => m.Selected);

        if (selectedItem != null)
        {
                selectedValue = selectedItem.Value;
        }");

                return content;
            }
        }

        [Display(GroupName = "Custom models", Name = "State")]
        [SelectionType(SelectionType.Single)]
        [Bindable]
        public IEnumerable<SelectListItem> State { get; set; }
        #endregion


    }
}