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
    [Group("Links", Order = 10)]
    [Group("Reference", Order = 20)]
    [Button("Update dynamic links", GroupName = "Links", Order = 0)]
    // A fully specified internal link that goes back to this page
    [Link(GroupName="Links", Area="Example", Controller="BasicFormElements", Action="Links", Name="This Page", Order=10)]
    // An internal link using mainly default values that links to the "Booleans" section, and opens in a new tab
    [Link(GroupName="Links", Action="Booleans", OpensInNewTab=true, Name="Booleans, new tab", Order=20)]
    // An external link to google in a new tab
    [ExternalLink("http://www.google.com", GroupName="Links", Name="Google (static)", Order=30)]
    // A parameterised internal link. This will not link to a valid page, but pay attention to where the link is going, it uses a custom path.
    // You MUST specify the Controller and Action when using a parameterised link
    [Link(GroupName = "Links", Controller="BasicFormElements", Action="Links", Parameters = new[] { "FirstParameter", "SecondParameter" }, RouteName = "Example_parameter_route_path", Name = "Internal Dynamic (path)", Order=40)]
    // A parameterised internal link. This will not link to a valid page, but pay attention to where the link is going, it uses custom arguments
    // You MUST specify the Controller and Action when using a parameterised link
    [Link(GroupName = "Links", Controller = "BasicFormElements", Action = "Links", Parameters = new[] { "FirstParameter", "SecondParameter" }, RouteName = "Example_parameter_route_argument", Name = "Internal Dynamic (arguments)", Order = 50)]
    // A parameterised external link
    [ExternalLink("http://www.bing.com/search", GroupName = "Links", Parameters = new[] { "q" }, Name = "Search Bing (dynamic)", Order=60)]
    public class LinksViewModel
    {
        [Display(GroupName = "Links")]
        public ContentViewModel Overview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Links can be added to your page using the ""LinkAttribute"". These can be external links to other sites, or internal links for navigation/task flow to other parts of the application")
                    .AddParagraph(@"Try editing the text fields below and pressing the ""update dynamic links"" button to see how you can create dynamic links based on the contents of your view model.")
                    ;
                return content;
            }
        }

        [Display(GroupName = "Reference")]
        public ContentViewModel ReferenceOverview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"/// The LinksViewModel
[Group(""Links"", Order = 10)]
[Group(""Reference"", Order = 20)]
[Button(""Update dynamic links"", GroupName = ""Links"", Order=0)]

// A fully specified internal link that goes back to this page
[Link(GroupName=""Links"", Area=""Example"", Controller=""BasicFormElements"", Action=""Links"", Name=""This Page"", Order=10)]

// An internal link using mainly default values that links to the ""Booleans"" section, and opens in a new tab
[Link(GroupName=""Links"", Action=""Booleans"", OpensInNewTab=true, Name=""Booleans, new tab"", Order=20)]

// An external link to google in a new tab
[ExternalLink(""http://www.google.com"", GroupName=""Links"", Name=""Google (static)"", Order=30)]

// A parameterised internal link. This will not link to a valid page, but pay attention to where the link is going, it uses a custom path.
// You MUST specify the Controller and Action when using a parameterised link
[Link(GroupName = ""Links"", Controller=""BasicFormElements"", Action=""Links"", Parameters = new[] { ""FirstParameter"", ""SecondParameter"" }, RouteName = ""Example_parameter_route_path"", Name = ""Internal Dynamic (path)"", Order=40)]

// A parameterised internal link. This will not link to a valid page, but pay attention to where the link is going, it uses custom arguments
// You MUST specify the Controller and Action when using a parameterised link
[Link(GroupName = ""Links"", Controller = ""BasicFormElements"", Action = ""Links"", Parameters = new[] { ""FirstParameter"", ""SecondParameter"" }, RouteName = ""Example_parameter_route_argument"", Name = ""Internal Dynamic (arguments)"", Order = 50)]

// A parameterised external link
[ExternalLink(""http://www.bing.com/search"", GroupName = ""Links"", Parameters = new[] { ""q"" }, Name = ""Search Bing (dynamic)"", Order=60)]

public class LinksViewModel
{
        ...
        [Link(Action=""Booleans"", OpensInNewTab=true, Name=""Inline link to \""Booleans\"" page, attached to a data element"")]
        [Display(GroupName = ""Links"", Name = ""First Parameter"")]
        [Bindable]
        public string FirstParameter { get; set; }

        [ExternalLink(""http://www.google.com"", Name = ""Inline link to Google, attached to a data element"")]
        [Display(GroupName = ""Links"", Name = ""Second Parameter"")]
        [Bindable]
        public string SecondParameter { get; set; }

        [Hidden]
        public string q { get { return FirstParameter; } }
}")
      .AddParagraph("Additionally, the following custom routes are used in this example, set up in the area registration file:")
      .AddPreformatted(@"
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(""Example_parameter_route_path"",
                ""{area}/{controller}/{action}/{FirstParameter}/{SecondParameter}"",
                new { first = UrlParameter.Optional, second = UrlParameter.Optional });
            context.MapRoute(""Example_parameter_route_argument"",
                ""{area}/{controller}/{action}"",
                new { });
        }
    ")
      ;
                return content;
            }
        }

        [Link(Action="Booleans", OpensInNewTab=true, Name="Inline link to \"Booleans\" page, attached to a data element")]
        [Display(GroupName = "Links", Name = "First Parameter")]
        [Bindable]
        public string FirstParameter { get; set; }

        [ExternalLink("http://www.google.com", Name = "Inline link to Google, attached to a data element")]
        [Display(GroupName = "Links", Name = "Second Parameter")]
        [Bindable]
        public string SecondParameter { get; set; }

        [Hidden]
        public string q { get { return FirstParameter; } }
    }
}