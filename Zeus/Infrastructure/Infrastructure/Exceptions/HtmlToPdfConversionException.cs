using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EO.Pdf;

namespace Employment.Web.Mvc.Infrastructure.Exceptions
{
    /// <summary>
    /// The exception is thrown when conversion from HTML to PDF fails.
    /// </summary>
    [Serializable]
    public class HtmlToPdfConversionException : Exception
    {

        protected static string DeafultErrorMessage = "Exception occurred during conversion of HTML to PDF. ";


        public HtmlToPdfConversionException(Exception ex) : base(DeafultErrorMessage + ex.Message, ex.InnerException)
        {
             
        }


        public HtmlToPdfConversionException(HtmlToPdfException htmlToPdfException) : base( string.Format("{0} {1} {2}", DeafultErrorMessage, htmlToPdfException.ErrorCode, htmlToPdfException.Message), htmlToPdfException.InnerException)
        {
        }
    }
}
