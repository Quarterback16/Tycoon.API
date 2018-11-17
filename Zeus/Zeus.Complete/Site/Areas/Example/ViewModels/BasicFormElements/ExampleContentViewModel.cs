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
    public class ExampleContentViewModel
    {
        [Display(GroupName="Overview")]
        public ContentViewModel Example
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Basic content is created through the 'ContentViewModel'. It has many different types of content that can be added.")
                    .AddTitle("Title text")
                    .AddSubTitle("Subtitle - Types of text")
                    .AddText("Absolutely normal text")
                    .AddLineBreak()
                    .AddStrongText("Strong text")
                    .AddLineBreak()
                    .AddUnderlinedText("Underlined text")
                    .AddLineBreak()
                    .AddEmphasisText("Emphasised text")
                    .AddLineBreak()
                    .AddObsoleteText("This text is obsolete")
                    .AddLineBreak()
                    .AddAbbreviation("abbr.", "text to abbreviate")
                    .AddLineBreak()
                    .AddText("Some text followed by").AddSuperscriptText("Superscript text")
                    .AddLineBreak()
                    .AddText("Some text followed by").AddSubscriptText("Subscript Text")
                    .AddLineBreak()
                    .AddScreenReaderText("Text only visible to screen readers")
                    .AddLineBreak()
                    .AddEmailLink("email-link@email.com")
                    .AddLineBreak()
                    .AddExternalLink("http://www.google.com", "Link to Google")
                    .AddLineBreak()
                    .AddLink("ExampleContent", "BasicFormElements", "An internal link")
                    .AddLineBreak()
                    .AddParagraph("A regular (new) paragraph")
                    .AddLineBreak()
                    .AddDescriptionName("Description name")
                    .AddLineBreak()
                    .AddDescriptionValue("Description value")
                    .BeginParagraph()
                    .AddBadgeText(MessageType.Error, "Error Badge")
                    .AddBadgeText(MessageType.Information, "Information Badge")
                    .AddBadgeText(MessageType.Success, "Success Badge")
                    .AddBadgeText(MessageType.Warning, "Warning Badge")
                    .EndParagraph()
                    .BeginUnorderedList()
                    .AddListItem("List item 1")
                    .AddListItem("List item 2")
                    .AddListItem("List item 3")
                    .EndUnorderedList()
                    .AddPreformatted("Some preformatted text")
                    .AddLineBreak()
                    .AddText("Font awesome icon: ").AddIcon("fa-comment", ColourType.Red)
                    .AddLineBreak()
                    .AddText("Font awesome icon: ").AddIcon("fa-envelope", ColourType.Green)
                    .AddLineBreak()
                    .AddText("Font awesome icon: ").AddIcon("fa-signal")
                    .AddLineBreak()
                    .AddText("Image: ").AddImage("~/Content/layout/spinner.gif", "An example image")
                    .AddLineBreak()
                    .AddAreaLink(AreaLinkIconType.ActivityAdd, "ExampleContent", "BasicFormElements", "An area link")
                ;

            }
        }

        [Display(GroupName="Code")]
        public ContentViewModel Code
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("This is how the above example page was created:")
                    .AddPreformatted(@"
        [Display(GroupName=""Overview"")]
        public ContentViewModel Example
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(""Basic content is created through the 'ContentViewModel'. It has many different types of content that can be added."")
                    .AddTitle(""Title text"")
                    .AddSubTitle(""Subtitle - Types of text"")
                    .AddText(""Absolutely normal text"")
                    .AddLineBreak()
                    .AddStrongText(""Strong text"")
                    .AddLineBreak()
                    .AddUnderlinedText(""Underlined text"")
                    .AddLineBreak()
                    .AddEmphasisText(""Emphasised text"")
                    .AddLineBreak()
                    .AddObsoleteText(""This text is obsolete"")
                    .AddLineBreak()
                    .AddAbbreviation(""abbr."", ""text to abbreviate"")
                    .AddLineBreak()
                    .AddText(""Some text followed by"").AddSuperscriptText(""Superscript text"")
                    .AddLineBreak()
                    .AddText(""Some text followed by"").AddSubscriptText(""Subscript Text"")
                    .AddLineBreak()
                    .AddScreenReaderText(""Text only visible to screen readers"")
                    .AddLineBreak()
                    .AddEmailLink(""email-link@email.com"")
                    .AddLineBreak()
                    .AddExternalLink(""http://www.google.com"", ""Link to Google"")
                    .AddLineBreak()
                    .AddLink(""ExampleContent"", ""BasicFormElements"", ""An internal link"")
                    .AddLineBreak()
                    .AddParagraph(""A regular (new) paragraph"")
                    .AddLineBreak()
                    .AddDescriptionName(""Description name"")
                    .AddLineBreak()
                    .AddDescriptionValue(""Description value"")
                    .BeginParagraph()
                    .AddBadgeText(MessageType.Error, ""Error Badge"")
                    .AddBadgeText(MessageType.Information, ""Information Badge"")
                    .AddBadgeText(MessageType.Success, ""Success Badge"")
                    .AddBadgeText(MessageType.Warning, ""Warning Badge"")
                    .EndParagraph()
                    .BeginUnorderedList()
                    .AddListItem(""List item 1"")
                    .AddListItem(""List item 2"")
                    .AddListItem(""List item 3"")
                    .EndUnorderedList()
                    .AddPreformatted(""Some preformatted text"")
                    .AddLineBreak()
                    .AddText(""Font awesome icon: "").AddIcon(""fa-comment"", ColourType.Red)
                    .AddLineBreak()
                    .AddText(""Font awesome icon: "").AddIcon(""fa-envelope"", ColourType.Green)
                    .AddLineBreak()
                    .AddText(""Font awesome icon: "").AddIcon(""fa-signal"")
                    .AddLineBreak()
                    .AddText(""Image: "").AddImage(""~/Content/layout/spinner.gif"", ""An example image"")
                    .AddLineBreak()
                    .AddAreaLink(AreaLinkIconType.ActivityAdd, ""ExampleContent"", ""BasicFormElements"", ""An area link"")
                ;
            }
        }
")
                ;
            }
        }
    }
}