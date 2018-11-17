using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Layout
{
    [DisplayName("Dynamic display of elements")]
    [Group("Dynamic display", Order = 1)]
    [Group("Text matching", Order = 5)]
    [Group("If", Order = 15)]
    [Group("Buttons", Order = 20)]
    [Button("Submit", "Dynamic display", Order = 1)]
    [Button("Submit", "Text matching", Order = 1)]
    [Button("Button that enables", "Buttons", ActionForDependencyType.Enabled, "ButtonEnabler", ComparisonType.EqualTo, true, Order = 2)]
    [Button("Button that disables", "Buttons", ActionForDependencyType.Disabled, "ButtonEnabler", ComparisonType.EqualTo, true, Order = 3)]
    [Button("Button that becomes visible", "Buttons", ActionForDependencyType.Visible, "ButtonEnabler", ComparisonType.EqualTo, true, Order = 4)]
    [Button("Button that hides", "Buttons", ActionForDependencyType.Hidden, "ButtonEnabler", ComparisonType.EqualTo, true, Order = 5)]
    [Button("Surprise!", "Buttons", ActionForDependencyType.Visible, "ButtonRevealer", ComparisonType.RegExMatch, ".*tt.*", Order = 0, Primary = true)]
    [Link("Link that enables", ActionForDependencyType.Enabled, "ButtonEnabler", ComparisonType.EqualTo, true, Order = 8, GroupName = "Buttons")]
    public class DynamicDisplayViewModel
    {
        public DynamicDisplayViewModel()
        {
            NestedGroupDetails = new NestedGroupViewModel();
        }

        [Display(GroupName = "Dynamic display")]
        public ContentViewModel Introduction
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"Individual elements within your from can be modified based on the values of other elements in the form. This allows complex conditional logic to be applied to the display of your form.")
                    .AddParagraph(@"For example, you could set a field in your form to be editable only once a user had selected a checkbox, by using an attribute [EditableIfTrue(""CheckboxName"")]")
                    .AddParagraph(@"The attribute to use is generally described by a Characteristic (e.g. ""Editable""), a Comparision (e.g. ""IfTrue""), and a dependant property or value (e.g. ""CheckboxName"")")
                    .AddParagraph(@"The available Characteristics are:")
                    .AddListItem("Bindable")
                    .AddListItem("Clear")
                    .AddListItem("Editable")
                    .AddListItem("ReadOnly")
                    .AddListItem("Required")
                    .AddListItem("Visible")
                    .AddParagraph("The available Comparisons are:")
                    .AddListItem("IfTrue")
                    .AddListItem("IfFalse")
                    .AddListItem("IfEmpty")
                    .AddListItem("IfNotEmpty")
                    .AddListItem("IfRegExMatch")
                    .AddListItem("IfNotRegExMatch")
                    .AddListItem("If")
                    .AddListItem("IfNot")
                    .AddParagraph("When using conditional logic that examines form fields, the field must be tabbed out of before the conditional change will take place")
                    .AddPreformatted(@"        // Example properties
        [Bindable]
        [Switcher(""True"", ""False"")]
        [Display(GroupName = ""Dynamic display"", Name=""Switch me and watch the page change"")]
        public bool IsEnabled { get; set; }

        [Bindable]
        [VisibleIfTrue(""IsEnabled"")]
        [Display(GroupName = ""Dynamic display"")]
        public string VisibleIfTrue { get; set; }

        [Bindable]
        [EditableIfTrue(""IsEnabled"")]
        [Display(GroupName = ""Dynamic display"")]
        public string EditableIfTrue { get; set; }
    ")
                    ;
            }
        }

        #region if true / if false

        [Bindable]
        [Display(GroupName = "Dynamic display", Name = "Switch me and watch the page change")]
        [Switcher("True", "False")]
        public bool IsEnabled { get; set; }

        [Bindable]
        [EditableIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("c", RowType.Half)]
        public string EditableIfTrue { get; set; }

        [Bindable]
        [EditableIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("c", RowType.Half)]
        public string EditableIfFalse { get; set; }

        [Bindable]
        [ReadOnlyIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfTrue { get; set; }

        [Bindable]
        [ReadOnlyIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfFalse { get; set; }

        [Bindable]
        [RequiredIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("e", RowType.Half)]
        public string RequiredIfTrue { get; set; }

        [Bindable]
        [RequiredIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("e", RowType.Half)]
        public string RequiredIfFalse { get; set; }

        [Bindable]
        [ClearIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("f", RowType.Half)]
        public string ClearIfTrue { get; set; }

        [Bindable]
        [ClearIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("f", RowType.Half)]
        public string ClearIfFalse { get; set; }

        [BindableIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("g", RowType.Half)]
        public string BindableIfTrue { get; set; }

        [BindableIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("g", RowType.Half)]
        public string BindableIfFalse { get; set; }

        [Bindable]
        [VisibleIfTrue("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("b", RowType.Half)]
        public string VisibleIfTrue { get; set; }

        [Bindable]
        [VisibleIfFalse("IsEnabled")]
        [Display(GroupName = "Dynamic display")]
        [Row("b", RowType.Half)]
        public string VisibleIfFalse { get; set; }

        #endregion

        #region text comparisons

        [Bindable]
        [Display(GroupName = "Text matching", Name = "Set me to a word with a double 't' in it. (don't forget to tab out)")]
        public string DependantString { get; set; }

        [Bindable]
        [EditableIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("c", RowType.Half)]
        public string EditableIfRegExMatch { get; set; }

        [Bindable]
        [EditableIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("c", RowType.Half)]
        public string EditableIfEmpty { get; set; }

        [Bindable]
        [ReadOnlyIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfRegExMatch { get; set; }

        [Bindable]
        [ReadOnlyIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfEmpty { get; set; }

        [Bindable]
        [RequiredIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("e", RowType.Half)]
        public string RequiredIfRegExMatch { get; set; }

        [Bindable]
        [RequiredIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("e", RowType.Half)]
        public string RequiredIfEmpty { get; set; }

        [Bindable]
        [ClearIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("f", RowType.Half)]
        public string ClearIfRegExMatch { get; set; }

        [Bindable]
        [ClearIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("f", RowType.Half)]
        public string ClearIfEmpty { get; set; }

        [BindableIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("g", RowType.Half)]
        public string BindableIfRegExMatch { get; set; }

        [BindableIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("g", RowType.Half)]
        public string BindableIfEmpty { get; set; }

        [Bindable]
        [VisibleIfRegExMatch("DependantString", ".*tt.*")]
        [Display(GroupName = "Text matching")]
        [Row("b", RowType.Half)]
        public string VisibleIfRegExMatch { get; set; }

        [Bindable]
        [VisibleIfEmpty("DependantString")]
        [Display(GroupName = "Text matching")]
        [Row("b", RowType.Half)]
        public string VisibleIfEmpty { get; set; }

        #endregion

        #region if and ifnot
        [Display(GroupName = "If")]
        public ContentViewModel Code
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"The ""If"" and ""IfNot"" comparisons allow you to seelct your own comparison from a list for greater flexibility")
                    .AddPreformatted(@"        // Example properties
        [Bindable]
        [ReadOnlyIfNot(""DependentInt"", ""9"")]
        [Display(GroupName = ""If"")]
        [Row(""a"", RowType.Half)]
        public string ReadOnlyIfNot9 { get; set; }

        [Bindable]
        [ReadOnlyIf(""DependentInt"", ComparisonType.EqualTo, 2)]
        [Display(GroupName = ""If"")]
        [Row(""b"", RowType.Half)]
        public string ReadOnlyIfEqualTo2 { get; set; }
")
                    ;
            }
        }

        [Bindable]
        [Display(GroupName = "If", Name = "Set a value")]
        public int DependentInt { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", "9")]
        [Display(GroupName = "If")]
        [Row("a", RowType.Half)]
        public string ReadOnlyIf9 { get; set; }

        [Bindable]
        [ReadOnlyIfNot("DependentInt", "9")]
        [Display(GroupName = "If")]
        [Row("a", RowType.Half)]
        public string ReadOnlyIfNot9 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.EqualTo, 2)]
        [Display(GroupName = "If")]
        [Row("b", RowType.Half)]
        public string ReadOnlyIfEqualTo2 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.NotEqualTo, 5)]
        [Display(GroupName = "If")]
        [Row("b", RowType.Half)]
        public string ReadOnlyIfNotEqualTo5 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.GreaterThan, 3)]
        [Display(GroupName = "If")]
        [Row("c", RowType.Half)]
        public string ReadOnlyIfGreaterThan3 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.GreaterThanOrEqualTo, 3)]
        [Display(GroupName = "If")]
        [Row("c", RowType.Half)]
        public string ReadOnlyIfGreaterThanOrEqualTo3 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.LessThan, 7)]
        [Display(GroupName = "If")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfLessThan7 { get; set; }

        [Bindable]
        [ReadOnlyIf("DependentInt", ComparisonType.LessThanOrEqualTo, 7)]
        [Display(GroupName = "If")]
        [Row("d", RowType.Half)]
        public string ReadOnlyIfLessThanOrEqualTo7 { get; set; }

        #endregion

        #region Buttons

        [Display(GroupName="Buttons")]
        public ContentViewModel ButtonCode
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Buttons can be affected too.")
                    .AddPreformatted(@"
    [Button(""Button that enables"", ""Buttons"", ActionForDependencyType.Enabled, ""ButtonEnabler"", ComparisonType.EqualTo, true, Order = 2    )]
    [Button(""Button that disables"",""Buttons"", ActionForDependencyType.Disabled, ""ButtonEnabler"", ComparisonType.EqualTo, true, Order = 3  )]
    [Button(""Button that becomes visible"", ""Buttons"", ActionForDependencyType.Visible, ""ButtonEnabler"", ComparisonType.EqualTo, true, Order = 4   )]
    [Button(""Button that hides"", ""Buttons"", ActionForDependencyType.Hidden, ""ButtonEnabler"", ComparisonType.EqualTo, true, Order = 5)]
    [Link(""Link that enables"", ActionForDependencyType.Enabled, ""ButtonEnabler"", ComparisonType.EqualTo, true, Order = 8, GroupName = ""Buttons"")]
    public class DynamicDisplayViewModel
    {
        ....
    }")             ;
            }
        }

        [Bindable]
        [Switcher("True", "False")]
        [Display(GroupName = "Buttons", Name = "Switch me to change buttons")]
        public bool ButtonEnabler { get; set; }

        [Bindable]
        [Display(GroupName = "Buttons", Name = "Set me to a word containing a double t to reveal a primary button")]
        public string ButtonRevealer { get; set; }

        [Bindable]
        [Display(GroupName = "Buttons", Name = "Set me to 'foo' to reveal a nested form")]
        public string NestedRevealer { get; set; }

        [Bindable]
        [VisibleIf("NestedRevealer", "foo")]
        [Display(GroupName = "Buttons")]
        public NestedGroupViewModel NestedGroupDetails { get; set; }

        #endregion
    }
    

    [Group("Nested")]
    public class NestedGroupViewModel
    {
        [Bindable]
        [Display(GroupName = "Nested")]
        public string Foo { get; set; }

        [Bindable]
        [Display(GroupName = "Nested")]
        public string Bar { get; set; }

    }
}