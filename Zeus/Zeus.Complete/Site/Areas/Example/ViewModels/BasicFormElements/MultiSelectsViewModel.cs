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
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.BasicFormElements
{
    [Group("Overview")]
    [Group("Multi selects")]
    [Button("Submit", GroupName = "Multi selects")]
    [Button("Clear", GroupName = "Multi selects", Clear=true)]
    [Group("Custom models")]
    [Button("Submit", GroupName = "Custom models")]
    [Button("Clear", GroupName = "Custom models", Clear = true)]
    public class MultiSelectsViewModel
    {
        [Display(GroupName = "Overview")]
        public ContentViewModel Overview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Use the ""Selection"" attribute in places where the user is required to select options from a small, defined list of options.")
                    .AddParagraph(@"Examples in this section sometimes use the ""MultiSelectList"" type as their basis instead of IEnumerable<string>. ")
                    .AddParagraph("In these cases, to retrieve the selected value in your controller after submission:")
                    .AddPreformatted(@"        var selectedValues = model.Ages.SelectedValues.Cast<string>();");
                ;
                return content;
            }
        }

        #region by selection attribute
        [Display(GroupName = "Multi selects")]
        public ContentViewModel ContentForOptions
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection example using hard-coded selection.
        /// It is required to specify both the value of each option, and the text that will be displayed to the user.
        [Display(GroupName = ""Multi selects"", Name = ""Options (multiple selection)"")]
        [Selection(SelectionType.Multiple, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [Bindable]
        public IEnumerable&lt;string&gt; Options { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Multi selects", Name = "Options (multiple selection)")]
        [Selection(SelectionType.Multiple, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [Bindable]
        public IEnumerable<string> Options { get; set; }
        #endregion

        #region by selection attribute with default
        [Display(GroupName = "Multi selects")]
        public ContentViewModel ContentForOptionsDefault
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection can also have defaults
        [Display(GroupName = ""Multi selects"", Name = ""Options with default"")]
        [Selection(SelectionType.Multiple, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [DefaultValue(new [] { ""2"", ""5"" })]
        [Bindable]
        public IEnumerable&lt;string&gt; OptionsDefault { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Multi selects", Name = "Options with default")]
        [Selection(SelectionType.Multiple, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [Bindable]
        [DefaultValue(new[] { "2", "5" })]
        public IEnumerable<string> OptionsDefault { get; set; }
        #endregion

        #region checkboxes
        [Display(GroupName = "Multi selects")]
        public ContentViewModel ContentForOptionsCheckBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection can be made with checkboxes
        [Display(GroupName = ""Multi selects"", Name = ""Checkboxes"")]
        [Selection(SelectionType.Multiple, new[] { ""1"", ""2"", ""3"", ""4"", ""5"" }, new[] { ""Option 1"", ""Option 2"", ""Option 3"", ""Option 4"", ""Option 5"" })]
        [DataType(CustomDataType.CheckBoxList)]
        [Bindable]
        public IEnumerable&lt;string&gt; OptionsCheckBox { get; set; }");

                return content;
            }
        }

        [Display(GroupName = "Multi selects", Name = "Checkboxes")]
        [Selection(SelectionType.Multiple, new[] { "1", "2", "3", "4", "5" }, new[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" })]
        [DataType(CustomDataType.CheckBoxList)]
        [Bindable]
        public IEnumerable<string> OptionsCheckBox { get; set; }
        #endregion

        #region Custom models overview

        [Display(GroupName = "Custom models")]
        public ContentViewModel CustomModelOverview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("If the selection attribute is not appropriate, you can build your own select list for use. ")
                    .AddParagraph("The data in this example was populated in the Controller's GET action in this way:")
                    .AddPreformatted(@"        var model = new SingleSelectsViewModel();
        // MultiSelectList
        model.Ages = AdwService.GetListCodes(""AGE"").ToMultiSelectList(m => m.Code, m => m.Description);
        model.Checkboxes = AdwService.GetListCodes(""AGE"").ToMultiSelectList(m => m.Code, m => m.Description);

        // Enumerable<SelectListItem>
        model.States = AdwService.GetListCodes(""STT"").ToSelectListItem(m => m.Code, m => m.Description);

        return View(model);")
                    .AddParagraph("You do not need to repeat this population in the POST action as it is automatically bound on submit.")
                    .AddParagraph("Although we have used the ADW service to populate this example, if you actually were selecting an ADW code, consider the AdwSelection attribute. ")
                    .AddLink("AdwSelection", "ServerSideElements", "More about the AdwSelection attribute.")
                    ;

                return content;
            }
        }

        #endregion

        #region Multi select box

        [Display(GroupName = "Custom models")]
        public ContentViewModel ContentForAges
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection using a prepopulated MultiSelectList
        [Display(GroupName = ""Multi selects"", Name = ""Ages (multiple selection)"")]
        [Bindable]
        public MultiSelectList Ages { get; set; }")
                ;

                return content;
            }
        }

        [Display(GroupName = "Custom models", Name = "Ages (multiple selection)")]
        [Bindable]
        public MultiSelectList Ages { get; set; }

        #endregion

        #region Checkbox List

        [Display(GroupName = "Custom models")]
        public ContentViewModel ContentForCheckboxes
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection rendered as a checkbox list
        [Display(GroupName = ""Multi selects"", Name = ""Checkboxes"")]
        [Bindable]
        public MultiSelectList Checkboxes { get; set; }")
                    ;
                return content;
            }
        }

        [Display(GroupName = "Custom models", Name = "Checkboxes")]
        [Bindable]
        [DataType(CustomDataType.CheckBoxList)]
        public MultiSelectList Checkboxes { get; set; }

        #endregion

        #region Enumerable<SelectListItem>

        [Display(GroupName = "Custom models")]
        public ContentViewModel ContentForStates
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection using an enumerbale as the data type
        [Display(GroupName = ""Multi selects"", Name = ""States"")]
        [SelectionType(SelectionType.Multiple)]
        [Bindable]
        public IEnumerable<SelectListItem> States { get; set; }")
                    .AddParagraph("Note that the property is decorated with the [SelectionType] attribute as the selection type cannot be implied from the property type.")
                    .AddParagraph("When using this method, getting the selected values is slight more complicated:")
                    .AddPreformatted(@"        var selectedItems = model.States.Where(s => s.Selected);

        if (selectedItems.Any())
        {
                selectedValues = selectedItems.Select(m => m.Value);
        }");

                return content;
            }
        }

        [Display(GroupName = "Custom models", Name = "States")]
        [SelectionType(SelectionType.Multiple)]
        [Bindable]
        public IEnumerable<SelectListItem> States { get; set; }

        #endregion

    }
}