using Employment.Web.Mvc.Area.Example.Controllers;

namespace Employment.Web.Mvc.Area.Example.Types
{
    /// <summary>
    /// Defines the available basic action types for <see cref="ButtonController.Basic(ButtonViewModel, string)" />.
    /// </summary>
    public class BasicSubmitType
    {
        /// <summary>
        /// Submit.
        /// </summary>
        public const string Submit = "Basic Submit";

        /// <summary>
        /// Submit.
        /// </summary>
        public const string SubmitSytle = "Submit with style";

        /// <summary>
        /// Submit.
        /// </summary>
        public const string SubmitFlair = "Submit with flair";

        /// <summary>
        /// Edit.
        /// </summary>
        public const string Edit = "Basic Edit";

        /// <summary>
        /// Edit.
        /// </summary>
        public const string EditPanache = "Edit with panache";

        /// <summary>
        /// Edit.
        /// </summary>
        public const string EditGumption = "Edit with gumption";


        /// <summary>
        /// Calendar, 'Delete Event' submit type.
        /// </summary>
        public const string DeleteEvent = "Delete";
    }
}