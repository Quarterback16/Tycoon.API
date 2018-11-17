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
    [Group("Split buttons", Order=10)]
    [Group("Reference", Order=20)]

    // A split button with a primary button, a child submit button, a link, and an external link
    [Button("Split", "Split buttons", SubmitType="Custom", Primary=true)]
    [Button("Sub button", "Split buttons", SplitButtonParent="Split")]
    [Link("Link to this page", "Split buttons", Action="SplitButtons", SplitButtonParent="Split")]
    [ExternalLink("http://google.com.au", "Google", GroupName="Split buttons", SplitButtonParent="Split")]

    // A drop down button with a child submit button, a link, and an external link
    [Button("Drop down", "Split buttons")]
    [Button("Sub button", "Split buttons", SplitButtonParent = "Drop down")]
    [Link("Link to this page", "Split buttons", Action = "SplitButtons", SplitButtonParent = "Drop down")]
    [ExternalLink("http://google.com.au", "Google", GroupName = "Split buttons", SplitButtonParent = "Drop down")]
    public class SplitButtonsViewModel
    {
        [Display(GroupName="Split buttons")]
        public ContentViewModel Overview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Split buttons are used to group related functions into a single button with a drop down list. The two variants are: the split button, and the drop down button.")
                    .AddParagraph(@"The split button contains a primary action that can be clicked, along with a drop down for less frequently used options. To use this, make sure your primary button has a SubmitType defined.")
                    .AddParagraph(@"The drop down has no primary action, and clicking on the button always reveals the drop down menu. To use this, make sure that your primary buttons DOES NOT have a SubmitType defined")
                    .AddParagraph(@"The items that appear in the drop down list may be buttons, links, or external links. Use the ""SplitButtonParent"" attribute to specifcy which button they should appear below. Regardless of the type of child element, they will be styled in the same way.")
                    ;
                return content;
            }
        }

        [Display(GroupName = "Split buttons")]
        [Bindable]
        public string SomeData { get; set; }

        [Display(GroupName="Reference")]
        public ContentViewModel Reference
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"    /// The SplitButtonsViewModel
    [Group(""Split buttons"", Order=10)]
    [Group(""Reference"", Order=20)]

    // A split button with a primary button, a child submit button, a link, and an external link
    [Button(""Split"", ""Split buttons"", SubmitType=""Custom"", Primary=true)]
    [Button(""Sub button"", ""Split buttons"", SplitButtonParent=""Split"")]
    [Link(""Link to this page"", ""Split buttons"", Action=""SplitButtons"", SplitButtonParent=""Split"")]
    [ExternalLink(""http://google.com.au"", ""Google"", GroupName=""Split buttons"", SplitButtonParent=""Split"")]

    // A drop down button with a child submit button, a link, and an external link
    [Button(""Drop down"", ""Split buttons"")]
    [Button(""Sub button"", ""Split buttons"", SplitButtonParent = ""Drop down"")]
    [Link(""Link to this page"", ""Split buttons"", Action = ""SplitButtons"", SplitButtonParent = ""Drop down"")]
    [ExternalLink(""http://google.com.au"", ""Google"", GroupName = ""Split buttons"", SplitButtonParent = ""Drop down"")]
    public class SplitButtonsViewModel
    {
        ....
    }
    ");
                return content;
            }
        }
    }
}