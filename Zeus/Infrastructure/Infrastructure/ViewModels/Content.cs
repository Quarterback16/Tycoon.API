using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines an individual content item.
    /// </summary>
    [Serializable]
    public class Content
    {
        /// <summary>
        /// The type of content.
        /// </summary>
        public ContentType Type { get; set; }

        /// <summary>
        /// Text data.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The area link icon.
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.AreaLink" />.
        /// </remarks>
        public AreaLinkIconType? AreaLinkIcon { get; set; }

        /// <summary>
        /// A value that goes along with <see cref="Text" />.
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.ExternalLink" />, <see cref="ContentType.EmailLink" /> or <see cref="ContentType.Abbreviation" />..
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        /// An message type for this piece of content (e.g. error, warning, success, etc.)
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.BadgeText" />
        /// </remarks>
        public MessageType BadgeType { get; set; }

        /// <summary>
        /// The controller action.
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.AreaLink" /> or <see cref="ContentType.Link" />.
        /// </remarks>
        public string Action { get; set; }

        /// <summary>
        /// The controller (defaults to controller in current context).
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.AreaLink" /> or <see cref="ContentType.Link" />.
        /// </remarks>
        public string Controller { get; set; }

        /// <summary>
        /// The area (defaults to area in current context).
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type" /> is <see cref="ContentType.AreaLink" /> or <see cref="ContentType.Link" />.
        /// </remarks>
        public string Area { get; set; }

        /// <summary>
        /// The route values to include in a <see cref="ContentType.AreaLink" /> or <see cref="ContentType.Link" />.
        /// </summary>
        public IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// The route name to use when generating a link for <see cref="ContentType.AreaLink" /> or <see cref="ContentType.Link" />.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <param name="type">The content type of text.</param>
        public Content(ContentType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <param name="type">The content type of text.</param>
        /// <param name="text">The text data.</param>
        public Content(ContentType type, string text)
        {
            Type = type;
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <remarks>
        /// Should only use when the <see cref="Type" /> is <see cref="ContentType.ExternalLink" />, <see cref="ContentType.EmailLink" /> or <see cref="ContentType.Abbreviation" />.
        /// </remarks>
        /// <param name="type">The content type of text.</param>
        /// <param name="text">The text data.</param>
        /// <param name="value">Value that goes along with the text.</param>
        public Content(ContentType type, string text, string value)
        {
            Type = type;
            Text = text;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <remarks>
        /// Should only use when the <see cref="Type" /> is <see cref="ContentType.Link" />.
        /// </remarks>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="type">The content type of text.</param>
        /// <param name="text">The text data.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        public Content(ContentType type, string text, string action, string controller, string area, IDictionary<string, object> routeValues, string routeName)
        {
            Type = type;
            Text = text;
            Action = action;
            Controller = controller;
            Area = area;

            if (routeValues != null)
            {
                routeValues.Remove("area");
            }

            RouteValues = routeValues;
            RouteName = routeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <remarks>
        /// Should only use when the <see cref="Type" /> is <see cref="ContentType.AreaLink" />.
        /// </remarks>
        /// <param name="action">The controller action.</param>
        /// <param name="controller">The controller (defaults to controller in current context).</param>
        /// <param name="area">The area (defaults to area in current context).</param>
        /// <param name="areaLinkIcon">The type of area link icon to use in the link.</param>
        /// <param name="type">The content type of text.</param>
        /// <param name="text">The text data.</param>
        /// <param name="routeValues">The route values to include in a link.</param>
        /// <param name="routeName">The name of the route to use when generating the link.</param>
        public Content(ContentType type, string text, string action, string controller, string area, AreaLinkIconType areaLinkIcon, IDictionary<string, object> routeValues, string routeName)
        {
            Type = type;
            Text = text;
            Action = action;
            Controller = controller;
            Area = area;
            AreaLinkIcon = areaLinkIcon;

            if (routeValues != null)
            {
                routeValues.Remove("area");
            }

            RouteValues = routeValues;
            RouteName = routeName;
        }

        /// <summary>
        /// For creating BadgeTexts
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <param name="messageType"></param>
        public Content(ContentType type, string text, MessageType messageType)
        {
            Type = type;
            Text = text;
            BadgeType = messageType;
        }
    }
}