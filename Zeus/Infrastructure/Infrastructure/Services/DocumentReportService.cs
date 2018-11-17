using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Employment.Esc.Framework.Reporting;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// A service for generating documents.
    /// </summary>
    public class DocumentReportService : IDocumentReportService
    {
        /// <summary>
        /// Creates the document representation.
        /// </summary>
        /// <returns></returns>
        public IDocumentRender CreateDocument()
        {
            Employment.Esc.Framework.Reporting.DocumentRenderService render = new Employment.Esc.Framework.Reporting.DocumentRenderService();
            return render.CreateDocument();
        }
    }


    ///// <summary>
    ///// Defines a service to convert HTML report template into PDF file.
    ///// </summary>
    //public class HtmlToPdfConverterService : IHtmlToPdfConverterService
    //{ 


    //    /// <summary>
    //    /// Applies supplied header, converts html markup to Pdf buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="includeDefaultFooter">Whether to include default footer.</param>
    //    /// <param name="useDefaultMargins">Whether to include default margins.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    public byte[] ConvertToPdf(string templateInHtml, string header, bool includeDefaultFooter, bool useDefaultMargins)
    //    {
    //        RectangleF emptyRect = new RectangleF();
    //        return ConvertToPdf(templateInHtml, includeDefaultFooter, header, string.Empty, useDefaultMargins, emptyRect);
    //    }


    //    /// <summary>
    //    /// Converts Html markup to Pdf Buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="footer">String formatted footer.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    /// <remarks>Default margins will be used.</remarks>
    //    public byte[] ConvertToPdf(string templateInHtml, string header, string footer)
    //    {
    //        return ConvertToPdf(templateInHtml, false, header, footer, useDefaultMargins: true);
    //    }

    //    /// <summary>
    //    /// Applies supplied header, footer, margins and converts html markup to Pdf buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <param name="header">String formatted header.</param>
    //    /// <param name="footer">String formatted footer.</param>
    //    /// <param name="marginsRect">Margins for Document.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    public byte[] ConvertToPdf(string templateInHtml, string header, string footer, RectangleF marginsRect)
    //    {
    //        return ConvertToPdf(templateInHtml, false, header, footer, false, marginsRect);
    //    }

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
    //    public byte[] ConvertToPdf(string templateInHtml, bool useDefaultFooter, string header, string footer, bool useDefaultMargins, RectangleF? marginsRect = null)
    //    {

    //        try
    //        {
    //            templateInHtml = AssignAppropriateStyles(templateInHtml);


    //            // Apply default properties
    //            HtmlToPdf.Options.PreserveHighResImages = true;

    //            // check to apply default margin
    //            if (useDefaultMargins)
    //            {
    //                EO.Pdf.HtmlToPdf.Options.OutputArea = new RectangleF(0.4f, 0.4f, 7.8f, 10f);
    //            }
    //            else
    //            {
    //                if (marginsRect != null && !marginsRect.Value.IsEmpty)
    //                {
    //                    EO.Pdf.HtmlToPdf.Options.OutputArea = marginsRect.Value;
    //                }
    //            }

    //            // check to apply default footer
    //            if (useDefaultFooter)
    //            {
    //                const string defaultFooterFormat = "<div style='font-family: Tahoma, Geneva, sans-serif;font-size:10pt'> <span style='float: right'>Page {{page_number}} of {{total_pages}}</span> </div>";
    //                //"<div><span style=\"float: right;\">Page {{page_number}} of {{total_pages}}</span></div>"
    //                HtmlToPdf.Options.FooterHtmlFormat = string.Format(defaultFooterFormat);
    //            }
    //            else
    //            {
    //                if (!string.IsNullOrEmpty(footer))
    //                {
    //                    HtmlToPdf.Options.FooterHtmlFormat = footer;
    //                }
    //            }

    //            // check to apply header
    //            if (!string.IsNullOrEmpty(header))
    //            {
    //                HtmlToPdf.Options.HeaderHtmlFormat = header;
    //            }

    //            return GeneratePdf(templateInHtml);
    //        }
    //        catch (HtmlToPdfConversionException ex)
    //        {
    //            throw new HtmlToPdfConversionException(ex);
    //        }
    //    }

    //    /// <summary>
    //    /// Converts the html markup supplied into Pdf Buffer.
    //    /// </summary>
    //    /// <param name="templateInHtml">Template content in html.</param>
    //    /// <returns>Pdf data in form of btye array.</returns>
    //    private static byte[] GeneratePdf(string templateInHtml)
    //    {
    //        try
    //        {
    //            // Buffer to store pdf data.
    //            byte[] pdfBuffer;

    //            // Add Licence Key.
    //            if (ConfigurationManager.AppSettings["EssentialObjectsPdfLicense"] != null)
    //            {
    //                string licence = ConfigurationManager.AppSettings.Get("EssentialObjectsPdfLicense");
    //                EO.Pdf.Runtime.AddLicense(licence);
    //                /*
    //                 "IcncsHWm8PoO5Kfq6doPvUaBpLHLn3Xj7fQQ7azc6c/nrqXg5/YZ8p7cwp61" +
    //                                          "n1mXpM0M66Xm+8+4iVmXpLHLn1mXwPIP41nr/QEQvFu807/745+ZpAcQ8azg" +
    //                                          "8//ooW2ltLPLrneEjrHLn1mzs/IX66juwp61n1mXpM0a8Z3c9toZ5aiX6PIf" +
    //                                          "5HaZusDgrmuntcPNn6/c9gQU7qe0psXNn2i1kZvLn1mXwAQU5qfY+AYd5Hfx" +
    //                                          "uekPxHvr0er74GS5/tPj9IDMyPYSvHazswQU5qfY+AYd5HeEjs3a66La6f8e" +
    //                                          "5HeEjnXj7fQQ7azcwp61n1mXpM0X6Jzc8gQQyJ21uMXdtG2rtg=="
    //                 */
    //            }
    //            using (MemoryStream stream = new MemoryStream())
    //            {
    //                HtmlToPdfResult pdfResult = HtmlToPdf.ConvertHtml(templateInHtml, stream);
    //                //HtmlToPdf.ConvertHtml(templateInHtml, "EOGeneratedPdf.pdf");
    //                pdfBuffer = stream.GetBuffer();
    //            }

    //            return pdfBuffer;
    //        }
    //        catch (HtmlToPdfException exception)
    //        {
    //            throw new HtmlToPdfConversionException(exception);
    //        }
    //        catch (Exception exception)
    //        {
    //            throw new HtmlToPdfConversionException(exception);
    //        }

    //    }

    //    /// <summary>
    //    /// Using placeholder applies print and common styles (common to screen and print media) to PDF.
    //    /// </summary>
    //    /// <param name="templateInHtml">template.</param>
    //    /// <returns>template with screen media styles removed.</returns>
    //    private static string AssignAppropriateStyles(string templateInHtml)
    //    {
    //        string stringToReturn = string.Empty;
    //        string startPlaceholder = "&&&";
    //        string endPlaceholder = "###";
    //        int startIndex = templateInHtml.IndexOf(startPlaceholder);
    //        int endIndex = templateInHtml.IndexOf(endPlaceholder) + endPlaceholder.Length + 2;
    //        int length = templateInHtml.Length;
    //        int combinedLength = endIndex - startIndex;


    //        string mediaScreenStyles = templateInHtml.Substring(startIndex, combinedLength);

    //        stringToReturn = templateInHtml.Replace(mediaScreenStyles, string.Empty);

    //        stringToReturn = stringToReturn.Replace("/*--*/}", string.Empty);
    //        stringToReturn = stringToReturn.Replace("/*--*/{", string.Empty);

    //        return stringToReturn;
    //    }

        
    //}
}
