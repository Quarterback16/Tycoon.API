using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Zeus.ViewModels
{
    public class CustomReportErrorViewModel : ILayoutOverride //: HandleErrorInfo
    {
        /// <summary>
        /// Optional. Error Message to display.
        /// </summary>
        [Hidden]
        public string ErrorToDisplay { get; set; }


        /// <summary>
        /// Content View Model for displaying error details.
        /// </summary>
        public ContentViewModel ContentViewModel
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("Error Occurred")
                    .AddParagraph("Please close this tab.")

                    .BeginParagraph()
                    .AddText("An error occurred while processing your request.")
                    .EndParagraph()



                    .BeginParagraph()
                    .AddText("For assistance, please contact the ES Help Desk on 1300 305 520 or ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                    .EndParagraph()
                    ;
            }
        }



        #region ILayoutOverride Members

        /// <summary>
        /// Hides the Left hand navigation and Required message.
        /// </summary>
        public IEnumerable<LayoutType> Hidden
        {
            get
            {
                return new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };
            }
            set
            {

            }
        }

        #endregion
    }
}