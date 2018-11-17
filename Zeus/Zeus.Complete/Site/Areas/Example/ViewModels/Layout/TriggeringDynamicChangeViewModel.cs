using System;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Layout
{
    [Group("Overview", Order = 0)]
    [Group("Code", Order = 100)]
    [Group("Select dynamic model", "FullRow", GroupRowType.OneThird, Order = 1)]
    [Group("Dynamic model", "FullRow", GroupRowType.TwoThird, Order = 2)]
    [Button("Submit", "Dynamic model")]
    [Serializable]
    public class TriggeringDynamicChangeViewModel : IValidatableObject
    {
        public TriggeringDynamicChangeViewModel()
        {
            Content = new ContentViewModel();
        }

        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"View models can also be constructed programmatically, using the DynamicViewModel, and its associated classes.")
                    .AddParagraph(@"When contructing view models programmatically, the [Trigger] attribute may be helpful. This attribute causes a post back of the form when a data element has its value changed.")
                    .AddParagraph(@"In the following example, when the radio button has it's value switched, a new dynamic view model is constrcuted and rendered.")
                    ;
            }
                }

        [Display(GroupName = "Code")]
        public ContentViewModel Code
        {
            get
            {
                return new ContentViewModel()
                    .AddPreformatted(@"    // View Model
    public class TriggeringDynamicChangeViewModel : IValidatableObject
    {
        ....

        [Bindable]
        [Trigger(""Change"")]
        [Selection(SelectionType.Single, new [] {""Foo"", ""Bar""}, new [] { ""Show Foo"", ""Show Bar""} )]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        [DefaultValue(""Foo"")]
        [Display(Name = ""Form to show"", GroupName = ""Select dynamic model"", Order = 1)]
        public SelectList FormToShow { get; set; }

        [Bindable]
        [Display(Name = ""Dynamic Property"", GroupName = ""Dynamic model"", Order = 1)]
        public DynamicViewModel MyDynamicProperty { get; set; }
        
        ....
    }

    // Controller Action
    public ActionResult TriggeringDynamicChange(TriggeringDynamicChangeViewModel model, string submitType)
    {
        if (submitType == ""Change"")
        {
            // Ignore model errors for this submit type
            ModelState.Clear();

            // Change which dynamic form to use based on UseAlternative
            model.MyDynamicProperty = model.FormToShow.SelectedValue as string == ""Bar"" ? DynamicBar() : DynamicFoo();
        }

        ....
    }

    // Constructing a dynamic view model
    private DynamicViewModel DynamicFoo()
    {
        var model = new DynamicViewModel();

        // Display text
        model.Add(new LabelViewModel { Value = ""I agree to participate in"" });

        // Hidden label for Hours input (for Accessibility)
        model.Add(new LabelViewModel { ForProperty = ""Hours"", Hidden = true, Value = ""The number of hours of voluntary work per fortnight I agree to participate in"" });

        // Hours input
        model.Add(new IntViewModel { Name = ""Hours"" });

        // Display text
        model.Add(new LabelViewModel { Value = ""hours of voluntary work per fortnight with"" });

        // Hidden label for Provider input (for Accessibility)
        model.Add(new LabelViewModel { ForProperty = ""Provider"", Hidden = true, Value = ""The service provider the work will be done with"" });

        // Providers (would be retrieved from a RHEA service)
        var options = new Dictionary<string, string> { { ""1"", ""Provider 1"" }, { ""2"", ""Provider 2"" }, { ""3"", ""Provider 3"" } };

        // Provider input
        model.Add(new SelectListViewModel { Name = ""Provider"", Value = options.ToSelectListItem(m => m.Key, m => m.Value).ToList() });

        // Display text
        model.Add(new LabelViewModel { Value = ""from"" });

        // Hidden label for Hours input (for Accessibility)
        model.Add(new LabelViewModel { ForProperty = ""FromDate"", Hidden = true, Value = ""The date I will work from"" });

        // FromDate input
        model.Add(new DateViewModel { Name = ""FromDate"" });

        // Display text
        model.Add(new LabelViewModel { Value = ""to"" });

        // Hidden label for Hours input (for Accessibility)
        model.Add(new LabelViewModel { ForProperty = ""ToDate"", Hidden = true, Value = ""The date I will work to"" });

        // ToDate input
        model.Add(new DateViewModel { Name = ""ToDate"" });

        // Display text
        model.Add(new LabelViewModel { Value = ""."" });

        return model;
    }
")
                    ;
            }
        }

        [Display(GroupName = "Select dynamic model", Order = 0)]
        public ContentViewModel Content { get; set; }

        [Bindable]
        [Trigger("Change")]
        [Selection(SelectionType.Single, new [] {"Foo", "Bar"}, new [] { "Show Foo", "Show Bar"} )]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        [DefaultValue("Foo")]
        [Display(Name = "Form to show", GroupName = "Select dynamic model", Order = 1)]
        public string FormToShow { get; set; }

        [Bindable]
        [Display(Name = "Dynamic Property", GroupName = "Dynamic model", Order = 1)]
        public DynamicViewModel MyDynamicProperty { get; set; }


        /// <summary>
        /// Server-side validation of user submitted View Model data.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation result.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (MyDynamicProperty != null)
            {
                var hours = MyDynamicProperty.Get<int>("Hours");
                
                if (hours <= 0)
                {
                    results.Add(new ValidationResult("Hours is required", new [] {"Hours"}));
                }

                var provider = MyDynamicProperty.Get<IEnumerable<SelectListItem>>("Provider");

                if (provider != null && !provider.Any(p => p.Selected))
                {
                    results.Add(new ValidationResult("Provider is required", new[] { "Provider" }));
                }

                if (FormToShow == "Foo")
                {
                    var fromDate = MyDynamicProperty.Get<DateTime>("FromDate");
                    var toDate = MyDynamicProperty.Get<DateTime>("ToDate");

                    if (fromDate > toDate)
                    {
                        results.Add(new ValidationResult("From date must be less than to date", new [] {"FromDate"}));
                    }

                    if (fromDate == DateTime.MinValue)
                    {
                        results.Add(new ValidationResult("From date is required", new[] { "FromDate" }));
                    }

                    if (toDate == DateTime.MinValue)
                    {
                        results.Add(new ValidationResult("To date is required", new[] { "ToDate" }));
                    }
                }
            }

            return results;
        }
    }
}