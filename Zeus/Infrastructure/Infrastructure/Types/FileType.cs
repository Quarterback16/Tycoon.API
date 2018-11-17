
namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the file type.
    /// </summary>
    public enum FileType
    {
        #region Documents

        /// <summary>
        /// Microsoft Word Document (.doc).
        /// </summary>
        Doc,

        /// <summary>
        /// Rich Text Format (.rtf).
        /// </summary>
        Rtf,

        /// <summary>
        /// Portable Document Format (.pdf)
        /// </summary>
        Pdf,

        /// <summary>
        /// Microsoft Excel Format (.xls)
        /// </summary>
        Xls,

        /// <summary>
        /// (.xml)
        /// </summary>
        Xml,

        /// <summary>
        /// (.txt) document.
        /// </summary>
        Txt,

        #endregion

        #region Images

        /// <summary>
        /// Graphics Interchange Format (.gif).
        /// </summary>
        Gif,

        /// <summary>
        /// Joint Photographic Experts Group (.jpeg, .jpg, .jpe).
        /// </summary>
        Jpg,

        /// <summary>
        /// Portable Network Graphics (.png).
        /// </summary>
        Png,

        #endregion
    }
}
