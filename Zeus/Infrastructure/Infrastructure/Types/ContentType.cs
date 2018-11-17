namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the content type.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// A title or heading (<h2></h2>).
        /// </summary>
        Title,

        /// <summary>
        /// The beginning of a title.
        /// </summary>
        BeginTitle,

        /// <summary>
        /// The end of a title.
        /// </summary>
        EndTitle,

        /// <summary>
        /// A sub-title or sub-heading (<h3></h3>).
        /// </summary>
        SubTitle,

        /// <summary>
        /// The beginning of a sub-title or sub-heading.
        /// </summary>
        BeginSubTitle,

        /// <summary>
        /// The end of a sub-title or sub-heading.
        /// </summary>
        EndSubTitle,

        /// <summary>
        /// Plain text.
        /// </summary>
        Text,

        /// <summary>
        /// Emphasised text.
        /// </summary>
        EmphasisText,

        /// <summary>
        /// Strong text.
        /// </summary>
        StrongText,

        /// <summary>
        /// Underlined text.
        /// </summary>
        UnderlinedText,


        /// <summary>
        /// Subscript text.
        /// </summary>
        SubscriptText,

        /// <summary>
        /// Superscript text.
        /// </summary>
        SuperscriptText,

        /// <summary>
        /// Obsolete text.
        /// </summary>
        ObsoleteText,

        /// <summary>
        /// Screen reader text.
        /// </summary>
        ScreenReaderText,

        /// <summary>
        /// Abbreviation text.
        /// </summary>
        Abbreviation,

        /// <summary>
        /// A line break.
        /// </summary>
        LineBreak,

        /// <summary>
        /// A paragraph.
        /// </summary>
        Paragraph,

        /// <summary>
        /// The beginning of a paragraph.
        /// </summary>
        BeginParagraph,

        /// <summary>
        /// The end of a paragraph.
        /// </summary>
        EndParagraph,
        
        /// <summary>
        /// Text that has been preformatted with line breaks, tabs and whitespace.
        /// </summary>
        Preformatted,

        /// <summary>
        /// An external hyperlink.
        /// </summary>
        ExternalLink,

        /// <summary>
        /// An email address hyperlink.
        /// </summary>
        EmailLink,

        /// <summary>
        /// A normal hyperlink.
        /// </summary>
        Link,

        /// <summary>
        /// The beginning of a normal hyperlink.
        /// </summary>
        BeginLink,

        /// <summary>
        /// The end of a normal hyperlink.
        /// </summary>
        EndLink,

        /// <summary>
        /// A specialised hyperlink specifically for an Area landing page.
        /// </summary>
        AreaLink,

        /// <summary>
        /// Beginning of an ordered list.
        /// </summary>
        BeginOrderedList,

        /// <summary>
        /// End of an ordered list.
        /// </summary>
        EndOrderedList,

        /// <summary>
        /// Beginning of an unordered list.
        /// </summary>
        BeginUnorderedList,

        /// <summary>
        /// End of an unordered list.
        /// </summary>
        EndUnorderedList,

        /// <summary>
        /// An item in a list.
        /// </summary>
        ListItem,

        /// <summary>
        /// Beginning of an item in a list.
        /// </summary>
        BeginListItem,

        /// <summary>
        /// End of an item in a list.
        /// </summary>
        EndListItem,

        /// <summary>
        /// An image.
        /// </summary>
        Image,

        /// <summary>
        /// Beginning of a description list.
        /// </summary>
        BeginDescriptionList,

        /// <summary>
        /// A description name item in a description list.
        /// </summary>
        DescriptionName,

        /// <summary>
        /// Beginning of a description name item in a description list.
        /// </summary>
        BeginDescriptionName,

        /// <summary>
        /// End of a description name item in a description list.
        /// </summary>
        EndDescriptionName,

        /// <summary>
        /// A description value item in a description list.
        /// </summary>
        DescriptionValue,

        /// <summary>
        /// Beginning of a description value item in a description list.
        /// </summary>
        BeginDescriptionValue,

        /// <summary>
        /// End of a description name item in a description list.
        /// </summary>
        EndDescriptionValue,

        /// <summary>
        /// End of a description list.
        /// </summary>
        EndDescriptionList,

        /// <summary>
        /// An icon.
        /// </summary>
        Icon,

        /// <summary>
        /// Text styled as a badge with display options for error, warning, success, etc.
        /// </summary>
        BadgeText
    }
}
