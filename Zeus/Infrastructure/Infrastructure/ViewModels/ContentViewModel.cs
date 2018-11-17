using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines the View Model for general content.
    /// </summary>
    [Serializable]
    public class ContentViewModel : ILayoutOverride, IFluent
    {
        /// <summary>
        /// Which layout types should be hidden.
        /// </summary>
        public IEnumerable<LayoutType> Hidden { get; set; }

        /// <summary>
        /// The page title.
        /// </summary>
        public string PageTitle { get; private set; }

        private List<Content> content = new List<Content>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModel" /> class.
        /// </summary>
        public ContentViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModel" /> class.
        /// </summary>
        /// <remarks>
        /// Should only be used if the <see cref="ContentViewModel" /> is returned by a Controller Action which does not have a <see cref="MenuAttribute" /> defined. However, if the Controller Action does have a <see cref="MenuAttribute" /> defined, the name for the page title will be overriden with the supplied <paramref name="pageTitle" /> value.
        /// </remarks>
        /// <param name="pageTitle">The page title.</param>
        public ContentViewModel(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Title" /> which renders as <h2>text</h2> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddTitle(string text, params object[] args)
        {
            content.Add(new Content(ContentType.Title, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Begin a title in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginTitle()
        {
            content.Add(new Content(ContentType.BeginTitle));

            return this;
        }

        /// <summary>
        /// End a title in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndTitle()
        {
            content.Add(new Content(ContentType.EndTitle));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.SubTitle" /> which renders as <h3>text</h3> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddSubTitle(string text, params object[] args)
        {
            content.Add(new Content(ContentType.SubTitle, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Begin a sub-title in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginSubTitle()
        {
            content.Add(new Content(ContentType.BeginSubTitle));

            return this;
        }

        /// <summary>
        /// End a sub-title in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndSubTitle()
        {
            content.Add(new Content(ContentType.EndSubTitle));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Text" /> which renders as text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.Text, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.EmphasisText" /> which renders as emphasised text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddEmphasisText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.EmphasisText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.StrongText" /> which renders as strong text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddStrongText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.StrongText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Adds a <see cref="ContentType.UnderlinedText"/> which renders as Underlined text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to insert in text.</param>
        /// <returns>The current instance of <see cref="ContentViewModel"/>.</returns>
        public ContentViewModel AddUnderlinedText(string text, params object[] args)
        {
            string textToAdd = (args != null && args.Length > 0) ? string.Format(text, args) : text;
            content.Add(new Content(ContentType.UnderlinedText, textToAdd));
            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.SubscriptText" /> which renders as subscript text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddSubscriptText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.SubscriptText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.SuperscriptText" /> which renders as superscript text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddSuperscriptText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.SuperscriptText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.ObsoleteText" /> which renders as obsolete text in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddObsoleteText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.ObsoleteText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.BadgeText" /> which as a badge label in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddBadgeText(MessageType messageType, string text, params object[] args)
        {
            content.Add(new Content(ContentType.BadgeText, (args != null && args.Length > 0) ? string.Format(text, args) : text, messageType));

            return this;
        }


        /// <summary>
        /// Add a <see cref="ContentType.ScreenReaderText" /> which renders text for screen readers in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddScreenReaderText(string text, params object[] args)
        {
            content.Add(new Content(ContentType.ScreenReaderText, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Abbreviation" /> which renders as <abbr title="text">abbreviation</abbr> in the HTML output.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="text">The description of the abbreviation.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAbbreviation(string abbreviation, string text, params object[] args)
        {
            content.Add(new Content(ContentType.Abbreviation, abbreviation, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.LineBreak" /> which renders as <br /> in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLineBreak()
        {
            content.Add(new Content(ContentType.LineBreak));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Paragraph" /> which renders as <p>text</p> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddParagraph(string text, params object[] args)
        {
            content.Add(new Content(ContentType.Paragraph, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Begin a paragraph in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginParagraph()
        {
            content.Add(new Content(ContentType.BeginParagraph));

            return this;
        }

        /// <summary>
        /// End a paragraph in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndParagraph()
        {
            content.Add(new Content(ContentType.EndParagraph));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Preformatted" /> which renders as <pre>text</pre> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddPreformatted(string text, params object[] args)
        {
            content.Add(new Content(ContentType.Preformatted, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.ExternalLink" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="url">The url for the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddExternalLink(string url)
        {
            content.Add(new Content(ContentType.ExternalLink, url, url));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.ExternalLink" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="url">The url for the link.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddExternalLink(string url, string text)
        {
            content.Add(new Content(ContentType.ExternalLink, text, url));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.EmailLink" /> which renders as a mailto anchor tag in the HTML output.
        /// </summary>
        /// <param name="email">The email for the mailto link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddEmailLink(string email)
        {
            content.Add(new Content(ContentType.EmailLink, email));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string text)
        {
            return AddLink(action, null, null, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string text, IDictionary<string, object> routeValues)
        {
            return AddLink(action, null, null, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string text, IDictionary<string, object> routeValues, string routeName)
        {
            return AddLink(action, null, null, text, routeValues, routeName);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string text)
        {
            return AddLink(action, controller, null, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string text, IDictionary<string, object> routeValues)
        {
            return AddLink(action, controller, null, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string text, IDictionary<string, object> routeValues, string routeName)
        {
            return AddLink(action, controller, null, text, routeValues, routeName);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string area, string text)
        {
            return AddLink(action, controller, area, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string area, string text, IDictionary<string, object> routeValues)
        {
            return AddLink(action, controller, area, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.Link" /> which renders as an anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddLink(string action, string controller, string area, string text, IDictionary<string, object> routeValues, string routeName)
        {
            content.Add(new Content(ContentType.Link, text, action, controller, area, routeValues, routeName));

            return this;
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action)
        {
            return BeginLink(action, null, null, null, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, IDictionary<string, object> routeValues)
        {
            return BeginLink(action, null, null, routeValues, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, IDictionary<string, object> routeValues, string routeName)
        {
            return BeginLink(action, null, null, routeValues, routeName);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller)
        {
            return BeginLink(action, controller, null, null, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller, IDictionary<string, object> routeValues)
        {
            return BeginLink(action, controller, null, routeValues, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller, IDictionary<string, object> routeValues, string routeName)
        {
            return BeginLink(action, controller, null, routeValues, routeName);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller, string area)
        {
            return BeginLink(action, controller, area, null, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller, string area, IDictionary<string, object> routeValues)
        {
            return BeginLink(action, controller, area, routeValues, null);
        }

        /// <summary>
        /// Begin a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginLink(string action, string controller, string area, IDictionary<string, object> routeValues, string routeName)
        {
            content.Add(new Content(ContentType.BeginLink, null, action, controller, area, routeValues, routeName));

            return this;
        }

        /// <summary>
        /// End a link which renders an opening anchor tag in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndLink()
        {
            content.Add(new Content(ContentType.EndLink, null));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string text)
        {
            return AddAreaLink(areaLinkIcon, action, null, null, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string text, IDictionary<string, object> routeValues)
        {
            return AddAreaLink(areaLinkIcon, action, null, null, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string text, IDictionary<string, object> routeValues, string routeName)
        {
            return AddAreaLink(areaLinkIcon, action, null, null, text, routeValues, routeName);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string text)
        {
            return AddAreaLink(areaLinkIcon, action, controller, null, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string text, IDictionary<string, object> routeValues)
        {
            return AddAreaLink(areaLinkIcon, action, controller, null, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string text, IDictionary<string, object> routeValues, string routeName)
        {
            return AddAreaLink(areaLinkIcon, action, controller, null, text, routeValues, routeName);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string area, string text)
        {
            return AddAreaLink(areaLinkIcon, action, controller, area, text, null, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string area, string text, IDictionary<string, object> routeValues)
        {
            return AddAreaLink(areaLinkIcon, action, controller, area, text, routeValues, null);
        }

        /// <summary>
        /// Add a <see cref="ContentType.AreaLink" /> which renders as a specialised anchor tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Should only be used on Area landing pages.
        /// </remarks>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="text">The text to display in the link.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddAreaLink(AreaLinkIconType areaLinkIcon, string action, string controller, string area, string text, IDictionary<string, object> routeValues, string routeName)
        {
            content.Add(new Content(ContentType.AreaLink, text, action, controller, area, areaLinkIcon, routeValues, routeName));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.ListItem" /> which renders as <li>text</li> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddListItem(string text, params object[] args)
        {
            content.Add(new Content(ContentType.ListItem, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Beginning of a list item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginListItem()
        {
            content.Add(new Content(ContentType.BeginListItem));

            return this;
        }

        /// <summary>
        /// End of a list item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndListItem()
        {
            content.Add(new Content(ContentType.EndListItem));

            return this;
        }

        /// <summary>
        /// Beginning of an ordered list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginOrderedList()
        {
            content.Add(new Content(ContentType.BeginOrderedList));

            return this;
        }

        /// <summary>
        /// End of an ordered list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndOrderedList()
        {
            content.Add(new Content(ContentType.EndOrderedList));

            return this;
        }

        /// <summary>
        /// Beginning of an unordered list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginUnorderedList()
        {
            content.Add(new Content(ContentType.BeginUnorderedList));

            return this;
        }

        /// <summary>
        /// End of an unordered list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndUnorderedList()
        {
            content.Add(new Content(ContentType.EndUnorderedList));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.Image" /> which renders an <img /> tag in the HTML output.
        /// </summary>
        /// <remarks>
        /// Used for rendering images located within an Area project's Content folder.
        /// </remarks>
        /// <example>
        /// <code>
        /// // The virtual path to an image within an Area content folder should look like this:
        /// new ContentViewModel().AddImage("~/Areas/MyArea/Content/MyImage.png");
        /// 
        /// // The virtual path to an image within the Site content folder should look like this:
        /// new ContentViewModel().AddImage("~/Content/MyImage.png");
        /// </code>
        /// </example>
        /// <param name="virtualPath">The image file virtual path located in a Content folder.</param>
        /// <param name="description">The accessible description of the image.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddImage(string virtualPath, string description, params object[] args)
        {
            content.Add(new Content(ContentType.Image, (args != null && args.Length > 0) ? string.Format(description, args) : description, virtualPath));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.DescriptionName" /> which renders as <dt>text</dt> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddDescriptionName(string text, params object[] args)
        {
            content.Add(new Content(ContentType.DescriptionName, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Beginning of a description name item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginDescriptionName()
        {
            content.Add(new Content(ContentType.BeginDescriptionName));

            return this;
        }

        /// <summary>
        /// End of a description name item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndDescriptionName()
        {
            content.Add(new Content(ContentType.EndDescriptionName));

            return this;
        }

        /// <summary>
        /// Add a <see cref="ContentType.DescriptionValue" /> which renders as <dd>text</dd> in the HTML output.
        /// </summary>
        /// <param name="text">The text data.</param>
        /// <param name="args">Arguments to inject into text</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddDescriptionValue(string text, params object[] args)
        {
            content.Add(new Content(ContentType.DescriptionValue, (args != null && args.Length > 0) ? string.Format(text, args) : text));

            return this;
        }

        /// <summary>
        /// Beginning of a description value item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginDescriptionValue()
        {
            content.Add(new Content(ContentType.BeginDescriptionValue));

            return this;
        }

        /// <summary>
        /// End of a description value item in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndDescriptionValue()
        {
            content.Add(new Content(ContentType.EndDescriptionValue));

            return this;
        }

        /// <summary>
        /// Beginning of a description list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel BeginDescriptionList()
        {
            content.Add(new Content(ContentType.BeginDescriptionList));

            return this;
        }

        /// <summary>
        /// End of an description list in the HTML output.
        /// </summary>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel EndDescriptionList()
        {
            content.Add(new Content(ContentType.EndDescriptionList));

            return this;
        }

        /// <summary>
        /// Add an icon.
        /// </summary>
        /// <param name="icon">The icon name.</param>
        /// <param name="colourType">The icon colour.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel AddIcon(string icon, ColourType? colourType = null)
        {
            if (!string.IsNullOrEmpty(icon))
            {
                content.Add(new Content(ContentType.Icon, icon, colourType != null ? colourType.ToString().ToLower() : null));
            }

            return this;
        }

        /// <summary>
        /// Merge with another <see cref="ContentViewModel" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The current instance of <see cref="ContentViewModel" />.</returns>
        public ContentViewModel Merge(ContentViewModel model)
        {
            content.AddRange(model.GetContent());

            return this;
        }

        /// <summary>
        /// Returns the content data.
        /// </summary>
        /// <returns>The content data.</returns>
        public List<Content> GetContent()
        {
            return content;
        }
    }
}
