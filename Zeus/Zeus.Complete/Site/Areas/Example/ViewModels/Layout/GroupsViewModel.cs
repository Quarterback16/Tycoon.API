using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using System;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Layout
{
    [DisplayName("Example of layouts using groups")]
    public class GroupsViewModel
    {
        #region Example 1
        public ContentViewModel GroupIntroduction
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"To divide your controls up across different panels on a single page, use the ""Group"" attribute on your view model. The order that groups are displyed in is determined by the numerical order of the group's ""Order"" parameter, with lower numbers being displayed first.")
                    .AddParagraph(@"To place a data element from your view model into a specific group, use the ""Display"" attribute, and specify the ""GroupName"" parameter. Elements that are not attached to a group will display at the end of the page. The ""Order"" parameter can also be used to determine the order of elements within a group.")
                    .AddPreformatted(@"    [Group(""Group example 1"", Order = 0)]
    [Group(""Group example 2"", Order = 10)]
    [Group(""Middle group"", Order = 5)]
    public class GroupExampleViewModel
    {
        [Display(GroupName = ""Group example 1"", Name = ""Foo in group 1"")]
        [Bindable]
        public string FooOne { get; set; }

        [Display(GroupName = ""Group example 1"", Name = ""Bar in group 1"")]
        [Bindable]
        public string BarOne { get; set; }

        [Display(GroupName = ""Group example 2"", Name = ""Foo in group 2"", Order = 0)]
        [Bindable]
        public string FooTwo { get; set; }

        [Display(GroupName = ""Group example 2"", Name = ""Bar in group 2"", Order = -5)]
        [Bindable]
        public string BarTwo { get; set; }

        [Display(GroupName = ""Middle group"", Name = ""Data element in the middle group"")]
        [Bindable]
        public string GroupExampleBar { get; set; }

    }")
                    ;
            }
        }

        public GroupExampleViewModel Example1
        {
            get
            {
                return new GroupExampleViewModel();
            }
        }

        #endregion 

        #region Example 2
        public ContentViewModel NestedGroups
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"Groups can't contain other groups, but by defining other view model types, you can achieve the same effect. You should always use the group type ""Logical"" when putting groups inside other groups.")
                    .AddPreformatted(@"    [Group(""Top group"", Order = 0)]
    public class NestedGroupExampleViewModel
    {
        [Display(GroupName = ""Top group"", Name = ""sub group 1"")]
        public NestedGroupExampleSubViewModel Model1 { get { return new NestedGroupExampleSubViewModel(); } }

        [Display(GroupName = ""Top group"", Name = ""sub group 2"")]
        public NestedGroupExampleSubViewModel Model2 { get { return new NestedGroupExampleSubViewModel(); } }

    }

    [Group(""Sub group"", Order = 0, GroupType=GroupType.Logical)]
    public class NestedGroupExampleSubViewModel
    {
        [Display(GroupName = ""Sub group"", Name = ""Foo in sub group"")]
        [Bindable]
        public string Foo { get; set; }

        [Display(GroupName = ""Sub group"", Name = ""Bar in sub group"")]
        [Bindable]
        public string Bar { get; set; }

    }
")
                    ;
            }
        }

        public NestedGroupExampleViewModel NestedGroupExample
        {
            get
            {
                return new NestedGroupExampleViewModel();
            }
        }
        #endregion

    }

    #region Models for example 1
    [Group("Group example 1", Order = 0)]
    [Group("Group example 2", Order = 10)]
    [Group("Middle group", Order = 5)]
    public class GroupExampleViewModel
    {
        [Display(GroupName = "Group example 1", Name = "Foo in group 1")]
        [Bindable]
        public string FooOne { get; set; }

        [Display(GroupName = "Group example 1", Name = "Bar in group 1")]
        [Bindable]
        public string BarOne { get; set; }

        [Display(GroupName = "Group example 2", Name = "Foo in group 2", Order = 0)]
        [Bindable]
        public string FooTwo { get; set; }

        [Display(GroupName = "Group example 2", Name = "Bar in group 2", Order = -5)]
        [Bindable]
        public string BarTwo { get; set; }

        [Display(GroupName = "Middle group", Name = "Data element in the middle group")]
        [Bindable]
        public string GroupExampleBar { get; set; }

    }

    #endregion

    #region Models for Example 2
    [Group("Top group", Order = 0)]
    public class NestedGroupExampleViewModel
    {
        [Display(GroupName = "Top group", Name = "sub group 1")]
        public NestedGroupExampleSubViewModel Model1 { get { return new NestedGroupExampleSubViewModel(); } }

        [Display(GroupName = "Top group", Name = "sub group 2")]
        public NestedGroupExampleSubViewModel Model2 { get { return new NestedGroupExampleSubViewModel(); } }

    }

    [Group("Sub group", Order = 0, GroupType=GroupType.Logical)]
    public class NestedGroupExampleSubViewModel
    {
        [Display(GroupName = "Sub group", Name = "Foo in sub group")]
        [Bindable]
        public string Foo { get; set; }

        [Display(GroupName = "Sub group", Name = "Bar in sub group")]
        [Bindable]
        public string Bar { get; set; }

    }
    #endregion

}