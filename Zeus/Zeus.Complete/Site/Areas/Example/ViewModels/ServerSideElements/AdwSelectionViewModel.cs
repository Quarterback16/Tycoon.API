using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{
    [DisplayName("[AdwSelection] attribute example")]
    [Group("Adw selection (List Code)", Order = 1)]
    [Group("Adw selection (Related Code)", Order = 2)]
    [Button("Submit", "Adw selection (List Code)" )]
    [Button("Clear Button", "Adw selection (List Code)", Clear = true)]
    [Button("Submit", "Adw selection (Related Code)")]
    [Button("Clear Button", "Adw selection (Related Code)", Clear = true)]
    public class AdwSelectionViewModel
    {
        [Display(GroupName = "Adw selection (List Code)", Order = 0)]
        public ContentViewModel Introduction
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"The AdwSelection attribute can be used as a short cut to create select lists and multi select lists that are populated from the ADW service. These choices are determined at the SERVER and returned via an ajax call. If your select must be quick on the client side, consider using a basic SelectList and populating it yourself.")
                    .AddLink("SingleSelects", "BasicFormElements", "More about single selects")
                    .AddLineBreak()
                    .AddLink("MultiSelects", "BasicFormElements", "More about multi selects")
                    .AddLineBreak();
                return content;
            }
        }

        #region Group "Adw selection (List Code)"

        [Display(GroupName = "Adw selection (List Code)", Order = 1)]
        public ContentViewModel ContentForListCodeAge
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A single selection example of an Adw List Code.
        [Display(GroupName = ""Adw selection (List Code)"", Order = 2, Name = ""Age (single selection)"")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, ""AGE"")]
        [Bindable]
        public string ListCodeAge { get; set; }");

                return content;
            }
        }
        
        /// <summary>
        /// A single selection example of an Adw List Code.
        /// </summary>
        [Display(GroupName = "Adw selection (List Code)", Order = 3, Name = "Age (single selection)")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "AGE")]
        [Bindable]
        public string ListCodeAge { get; set; }

        [Display(GroupName = "Adw selection (List Code)", Order = 4)]
        public ContentViewModel ContentForListCodeAges
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection example of an Adw List Code.
        [Display(GroupName = ""Adw selection (List Code)"", Order = 4, Name = ""Ages (multiple selection)"")]
        [AdwSelection(SelectionType.Multiple, AdwType.ListCode, ""AGE"")]
        [Bindable]
        public IEnumerable&lt;string&gt; ListCodeAges { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A multiple selection example of an Adw List Code.
        /// </summary>
        [Display(GroupName = "Adw selection (List Code)", Order = 6, Name = "Ages (multiple selection)")]
        [AdwSelection(SelectionType.Multiple, AdwType.ListCode, "AGE")]
        [Bindable]
        public IEnumerable<string> ListCodeAges { get; set; }

        [Display(GroupName = "Adw selection (List Code)", Order = 7)]
        public ContentViewModel ContentForListCodeAgeNone
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// An example of an Adw List Code with no selection allowed.
        [Display(GroupName = ""Adw selection (List Code)"", Order = 2, Name = ""Age (no selection)"")]
        [AdwSelection(SelectionType.None, AdwType.ListCode, ""AGE"")]
        [DefaultValue(""<21"")]
        [Bindable]
        public string ListCodeAgeNone { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// An example of an Adw List Code with no selection allowed.
        /// </summary>
        [Display(GroupName = "Adw selection (List Code)", Order = 8, Name = "Age (no selection)")]
        [AdwSelection(SelectionType.None, AdwType.ListCode, "AGE")]
        [DefaultValue("<21")]
        [Bindable]
        public string ListCodeAgeNone { get; set; }

        [Display(GroupName = "Adw selection (List Code)", Order = 9)]
        public ContentViewModel ContentForListCodeAgesNone
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection example of an Adw List Code.
        [Display(GroupName = ""Adw selection (List Code)"", Order = 4, Name = ""Ages (no selection)"")]
        [AdwSelection(SelectionType.None, AdwType.ListCode, ""AGE"")]
        [DefaultValue(new [] { ""<21"", ""18"" })]
        [Bindable]
        public IEnumerable&lt;string&gt; ListCodeAgesNone { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// An example of an Adw List Code with no selection allowed.
        /// </summary>
        [Display(GroupName = "Adw selection (List Code)", Order = 10, Name = "Ages (no selection)")]
        [AdwSelection(SelectionType.None, AdwType.ListCode, "AGE")]
        [DefaultValue(new [] { "<21", "18" })]
        [Bindable]
        public IEnumerable<string> ListCodeAgesNone { get; set; }

        #endregion

        #region Group "Adw selection (Related Code)"

        [Display(GroupName = "Adw selection (Related Code)", Order = 1)]
        public ContentViewModel ContentForRelatedCodeAge
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A single selection example of an Adw Related Code.
        [Display(GroupName = ""Adw selection (Related Code)"", Order = 2, Name = ""Age (single selection)"")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, ""AGRA"", DependentValue = ""Y"")]
        [Bindable]
        public string RelatedCodeAge { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A single selection example of an Adw Related Code.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 3, Name = "Age (single selection)")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AGRA", DependentValue = "Y")]
        [Bindable]
        public string RelatedCodeAge { get; set; }

        /// <summary>
        /// Content for describing <see cref="RelatedCodeAges" />.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 4)]
        public ContentViewModel ContentForRelatedCodeAges
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection example of an Adw Related Code.
        [Display(GroupName = ""Adw selection (Related Code)"", Order = 4, Name = ""Ages (multiple selection)"")]
        [AdwSelection(SelectionType.Multiple, AdwType.RelatedCode, ""AGRA"", DependentValue = ""Y"")]
        [Bindable]
        public IEnumerable&lt;string&gt; RelatedCodeAges { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A multiple selection example of an Adw Related Code.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 6, Name = "Ages (multiple selection)")]
        [AdwSelection(SelectionType.Multiple, AdwType.RelatedCode, "AGRA", DependentValue = "Y")]
        [Bindable]
        public IEnumerable<string> RelatedCodeAges { get; set; }

        [Display(GroupName = "Adw selection (Related Code)", Order = 7)]
        public ContentViewModel ContentForRelatedCodeAgeNone
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// An example of an Adw Related Code with no selection allowed.
        [Display(GroupName = ""Adw selection (Related Code)"", Order = 2, Name = ""Age (no selection)"")]
        [AdwSelection(SelectionType.None, AdwType.RelatedCode, ""AGRA"", DependentValue = ""Y"")]
        [DefaultValue(""55+"")]
        [Bindable]
        public string RelatedCodeAgeNone { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// An example of an Adw Related Code with no selection allowed.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 8, Name = "Age (no selection)")]
        [AdwSelection(SelectionType.None, AdwType.RelatedCode, "AGRA", DependentValue = "Y")]
        [DefaultValue("55+")]
        [Bindable]
        public string RelatedCodeAgeNone { get; set; }

        /// <summary>
        /// Content for describing <see cref="RelatedCodeAges" />.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 9)]
        public ContentViewModel ContentForRelatedCodeAgesNone
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiple selection example of an Adw Related Code.
        [Display(GroupName = ""Adw selection (Related Code)"", Order = 4, Name = ""Ages (no selection)"")]
        [AdwSelection(SelectionType.None, AdwType.RelatedCode, ""AGRA"", DependentValue = ""Y"")]
        [DefaultValue(new [] { ""50-54"", ""55+"" })]
        [Bindable]
        public IEnumerable&lt;string&gt; RelatedCodeAgesNone { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A multiple selection example of an Adw Related Code.
        /// </summary>
        [Display(GroupName = "Adw selection (Related Code)", Order = 10, Name = "Ages (no selection)")]
        [AdwSelection(SelectionType.None, AdwType.RelatedCode, "AGRA", DependentValue = "Y")]
        [DefaultValue(new[] { "50-54", "55+" })]
        [Bindable]
        public IEnumerable<string> RelatedCodeAgesNone { get; set; }

        #endregion
    }
}