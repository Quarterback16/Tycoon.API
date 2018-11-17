using System;

namespace Employment.Web.Mvc.Service.Interfaces.Common
{
    /// <summary>
    /// Generic Comment Model
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// Gets or sets the comment ID.
        /// </summary>
        /// <value>The comment ID.</value>
        public Int64 CommentID { get; set; }
        /// <summary>
        /// Gets or sets the type of the comment.
        /// </summary>
        /// <value>The type of the comment.</value>
        public String CommentType { get; set; }
        /// <summary>
        /// Gets or sets the organisation code.
        /// </summary>
        /// <value>The organisation code.</value>
        public String OrganisationCode { get; set; }
        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        /// <value>The sequence number.</value>
        public Int64 SequenceNumber { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public String Status { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public String Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see>
        ///                                                  <cref>Comment</cref>
        ///                                              </see>
        ///     is public.
        /// </summary>
        /// <value><c>true</c> if public; otherwise, <c>false</c>.</value>
        public Boolean Public { get; set; }
        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>The topic.</value>
        public String Topic { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public Boolean ReadOnly { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [long text exists].
        /// </summary>
        /// <value><c>true</c> if [long text exists]; otherwise, <c>false</c>.</value>
        public Boolean LongTextExists { get; set; }
        /// <summary>
        /// Gets or sets the update user ID.
        /// </summary>
        /// <value>The update user ID.</value>
        public String UpdateUserID { get; set; }
        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        /// <value>The update time.</value>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Gets or sets the create user ID.
        /// </summary>
        /// <value>The create user ID.</value>
        public String CreateUserID { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Gets or sets the integrity control number.
        /// </summary>
        /// <value>The integrity control number.</value>
        public Int64 IntegrityControlNumber { get; set; }
    }
}
