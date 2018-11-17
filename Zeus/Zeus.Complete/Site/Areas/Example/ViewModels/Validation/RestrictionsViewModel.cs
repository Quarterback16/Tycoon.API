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

namespace Employment.Web.Mvc.Area.Example.ViewModels.Validation
{
    [Group("Restrictions")]
    [Button("Submit", GroupName = "Restrictions")]
    [Group("Dependencies")]
    [Button("Submit", GroupName = "Dependencies")]
    public class RestrictionsViewModel
    {
        [Display(GroupName = "Restrictions")]
        public ContentViewModel ContentForOverview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Restrictions are attributes that can be applied to your data field, to ensure that values provided will be within particular ranges. Inputs outside your specifications will cause validation errors to be displayed to the user. The basic restrictions available are: ")
                    .AddListItem("Max - Ensure a numeric value is no more than the specified value")
                    .AddListItem("Min - Ensure a numeric value is no less than the specified value")
                    .AddListItem("String Length - Ensures a given string is no more than a specific length")
                    .AddListItem("Numeric Length - Ensures a given numeric value is no more than a specific length (excluding +/-, decimal points, etc.)")
                    .AddListItem("ABN - Ensure a given value is a valid ABN. (Checks format only, not actual registration)")
                    .AddPreformatted(@"        // Example properties:
        [Bindable]
        [Display(GroupName = ""Restrictions"", Name = ""Numeric length of 4"")]
        [NumericLength(4)]
        public decimal NumericLengthOf4 { get; set; }

        [Bindable]
        [Display(GroupName=""Restrictions"", Name=""ABN"")]
        [Abn()]
        public int Abn { get; set; }
");

                return content;
            }
        }

        #region basic restrictions
        [Bindable]
        [Display(GroupName = "Restrictions", Name = "Maximum of 50")]
        [Max(50)]
        public decimal MaximumOf50 { get; set; }

        [Bindable]
        [Display(GroupName="Restrictions", Name="Minimum of 50")]
        [Min(50)]
        public decimal MinimumOf50 { get; set; }

        [Display(Name = "Maximum length of 5", GroupName = "Restrictions")]
        [StringLength(5)]
        [Bindable]
        public string MaximumLengthOf5 { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Maximum character count of 400", GroupName = "Restrictions")]
        [StringLength(400)]
        [Bindable]
        public string MaximumLengthOf400 { get; set; }

        [Bindable]
        [Display(GroupName = "Restrictions", Name = "Numeric length of 4")]
        [NumericLength(4)]
        public decimal NumericLengthOf4 { get; set; }

        [Bindable]
        [Display(GroupName="Restrictions", Name="ABN")]
        [Abn()]
        public int Abn { get; set; }

        #endregion

        #region dependency restrictions

        [Display(GroupName = "Dependencies")]
        public ContentViewModel ContentForDependencies
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Restrictions can also be made in relation to other properties within the page. The basic restrictions available are: ")
                    .AddListItem("EqualTo")
                    .AddListItem("NotEqualTo")
                    .AddListItem("LessThan")
                    .AddListItem("GreaterThan")
                    .AddListItem("LessThanOrEqualTo")
                    .AddListItem("GreaterThanOrEqualTo")
                    .AddListItem("Is - A more flexible option that allow the specificaiton of a comparison in code")
                    .AddPreformatted(@"        // Example properties
        [Bindable]
        [Display(GroupName = ""Dependencies"", Name = ""Foo"")]
        public int Foo { get; set; }

        [Bindable]
        [Display(GroupName = ""Dependencies"", Name = ""LessThanOrEqualToFoo"")]
        [LessThanOrEqualTo(""Foo"")]
        [Row(""c"", RowType.Half)]
        public int LessThanOrEqualToFoo { get; set; }

        [Bindable]
        [Display(GroupName = ""Dependencies"", Name = ""Is (Equal to Foo)"")]
        [Is(ComparisonType.EqualTo, ""Foo"")]
        public int IsEqualToFoo { get; set; }
");

                return content;
            }
        }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "Foo")]
        public int Foo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "EqualToFoo")]
        [EqualTo("Foo")]
        [Row("a", RowType.Half)]
        public int EqualToFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "NotEqualToFoo")]
        [NotEqualTo("Foo")]
        [Row("a", RowType.Half)]
        public int NotEqualToFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "LessThanFoo")]
        [LessThan("Foo")]
        [Row("b", RowType.Half)]
        public int LessThanFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "GreaterThanFoo")]
        [GreaterThan("Foo")]
        [Row("b", RowType.Half)]
        public int GreaterThanFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "LessThanOrEqualToFoo")]
        [LessThanOrEqualTo("Foo")]
        [Row("c", RowType.Half)]
        public int LessThanOrEqualToFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "GreaterThanOrEqualToFoo")]
        [GreaterThanOrEqualTo("Foo")]
        [Row("c", RowType.Half)]
        public int GreaterThanOrEqualToFoo { get; set; }

        [Bindable]
        [Display(GroupName = "Dependencies", Name = "Is (Equal to Foo)")]
        [Is(ComparisonType.EqualTo, "Foo")]
        public int IsEqualToFoo { get; set; }

        #endregion
    }
}