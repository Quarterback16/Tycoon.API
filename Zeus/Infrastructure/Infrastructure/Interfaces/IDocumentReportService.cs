using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{

    /// <summary>
    /// Defines method to be implemented by DocumentReport Service.
    /// </summary>
    public interface IDocumentReportService
    {
        /// <summary>
        /// Creates the document representation.
        /// </summary>
        /// <returns></returns>
        Employment.Esc.Framework.Reporting.IDocumentRender CreateDocument();
    }

    ///// <summary>
    ///// Defines method to be implemented by HtmlToPdfConverter Service.
    ///// </summary>
    //public interface IHtmlToPdfConverterService
    //{



    //    /// <summary>
    //    /// Converts Html markup to Pdf Buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="footer">String formatted footer.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    /// <remarks>Default margins will be used.</remarks>
    //    byte[] ConvertToPdf(string templateInHtml, string header, string footer);



    //    /// <summary>
    //    /// Applies supplied header, converts html markup to Pdf buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="includeDefaultFooter">Whether to include default footer.</param>
    //    /// <param name="useDefaultMargins">Whether to include default margins.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    byte[] ConvertToPdf(string templateInHtml, string header, bool includeDefaultFooter,
    //                                      bool useDefaultMargins);

    //    /// <summary>
    //    /// Applies supplied header, footer, margins and converts html markup to Pdf buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="footer">String formatted footer.</param>
    //    /// <param name="marginsRect">Margins for Document.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    byte[] ConvertToPdf(string templateInHtml, string header, string footer, RectangleF marginsRect);

    //    /// <summary>
    //    /// Converts Html markup to Pdf Buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="useDefaultFooter">Whether to include default footer.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="footer">String formatted footer.</param>
    //    /// <param name="useDefaultMargins">Whether to include default margins.</param>
    //    /// <param name="marginsRect">Margins for Document.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    byte[] ConvertToPdf(string templateInHtml, bool useDefaultFooter, string header, string footer,
    //                                      bool useDefaultMargins, RectangleF? marginsRect = null);

        

    //    }
}
