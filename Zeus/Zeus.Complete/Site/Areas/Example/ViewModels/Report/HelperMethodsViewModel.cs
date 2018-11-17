using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{

    [DisplayName("Helper Methods")]
    [Group("Helper Method Listing", Order = 1)]
    public class HelperMethodsViewModel
    {

        /// <summary>
        /// Lists Helper methods provided in ReportingTool ESS Web assembly.
        /// </summary>
        [Display( GroupName = "Helper Method Listing", Order = 1)]
        public ContentViewModel HelperMethodListing
        {
            get
            {
                return new ContentViewModel()
                .AddTitle("Helper Functions Listed.")
                .AddParagraph("Usage:  @AddBreak()  ")
                .AddPreformatted(
      @"/// <summary>
public abstract class RazorHelperExtension<T> : TemplateBase<T>
    {
        /// <summary>
        /// Capitalizes the string.
        /// </summary>
        /// <param name='stringValue'>string</param>
        /// <returns>string in upper case.</returns>
        public string ToUpperCase(string stringValue)
        {
            return stringValue.ToUpper();
        }



        /// <summary>
        /// Adds the <br/>.
        /// </summary>
        /// <returns>Html string containing <!--<br />--></returns>
        public static string AddBreak()
        {
            StringWriter stringBuilder = new StringWriter();
            using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringBuilder))
            {
                htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Br);
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// Creates the HTML label and returns it in form of string. 
        /// </summary>
        /// <param name='labelText'>label text.</param>
        /// <param name='labelForValue'>label's FOR value.</param> 
        /// <returns>Label markup.</returns>
        public static string GetLabel(string labelText, string labelForValue)
        { 
            return GetLabel(labelText, labelForValue, string.Empty);
        }


        /// <summary>
        /// Creates the HTML label and returns it in form of string. 
        /// </summary>
        /// <param name='labelText'>label text.</param>
        /// <param name='labelForValue'>label's FOR value.</param>
        /// <param name='className'>CSS Class for label.</param>
        /// <returns>Label markup.</returns>
        public static string GetLabel(string labelText, string labelForValue, string className)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
            {
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.For, labelForValue);
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Value, labelText);

                if(string.IsNullOrEmpty(className))
                {
                    htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, className);
                }

                htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label);
                htmlTextWriter.Write(labelText);
                htmlTextWriter.RenderEndTag();
            }

            return stringWriter.ToString();
        }


        /// <summary>
        /// Creates the HTML TextBox and returns it in form of string.
        /// </summary>
        /// <param name='textBoxText'>Text.</param>
        /// <param name='textBoxId'>Id to assign.</param> 
        /// <returns>TextBox markup.</returns>
        public static string GetTextBox(string textBoxText, string textBoxId)
        { 
            return GetTextBox(textBoxText, textBoxId, string.Empty); 
        }

       /// <summary>
        /// Creates the HTML TextBox and returns it in form of string.
        /// </summary>
        /// <param name='textBoxText'>Text.</param>
        /// <param name='textBoxId'>Id to assign.</param>
        /// <param name='className'>CSS class for TextBox. </param>
        /// <param name='isReadonly'> Have textbox as read-only. </param>
        /// <returns>TextBox markup.</returns>
        public static string GetTextBox(string textBoxText, string textBoxId, string className, bool isReadonly)
        {

            return GetTextBox(textBoxText, textBoxId, className, isReadonly, string.Empty);
            //string value = string.Format('<input readonly='readonly' type='text' id='{0}' value='{1}'/>', textBoxId, textBoxText);
            ////MvcHtmlString 
            //return value;
        }

        /// <summary>
        /// Creates the HTML TextBox and returns it in form of string.
        /// </summary>
        /// <param name='textBoxText'>Text.</param>
        /// <param name='textBoxId'>Id to assign.</param>
        /// <param name='className'>CSS class for TextBox. </param>
        /// <param name='isReadOnly'> Have textbox as read-only. </param>
        /// <param name='title'> Title value. </param>
        /// <returns>TextBox markup.</returns>
        public static string GetTextBox(string textBoxText, string textBoxId, string className, bool isReadOnly, string title)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
            {
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Type, 'text');
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Value, textBoxText);
                if (isReadOnly)
                {
                    htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.ReadOnly, 'readonly');
                }
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, textBoxId);

                if (!string.IsNullOrEmpty(className))
                {
                    htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, className);
                }
                if (!string.IsNullOrEmpty(className))
                {
                    htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Title, title);
                }
                htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlTextWriter.RenderEndTag();
            }

            return (stringWriter.ToString());
        }



        /// <summary>
        /// Converts the image to data URI, creates html Markup for image and then Embeds this data into it.
        /// </summary>
        /// <param name='image'>Image Path.</param> 
        /// <param name='altText'>Alt text for image.</param>
        /// <returns>Image data in html Markup.</returns>
        public static string RenderImage(string image, string altText)
        {
            return RenderImage(File.ReadAllBytes(image), altText);
        }


        /// <summary>
        /// Converts the image to data URI, creates html Markup for image and then Embeds this data into it.
        /// </summary>
        /// <param name='image'>Image Path.</param>
        /// <param name='altText'>Alt text for image.</param>
        /// <param name='height'>Height of rendered image.</param>
        /// <param name='width'>Width of rendered image.</param>
        /// <returns>Image data in html Markup.</returns>
        public static string RenderImage(string image, string altText, int height, int width)
        {
            return RenderImage(File.ReadAllBytes(image), altText, height, width);
        }



        /// <summary>
        /// Accepts image data in byte array and then converts the image to data URI, creates html Markup for image and then Embeds this data into it.
        /// </summary>
        /// <param name='imageArray'>Byte array containing image info.</param>
        /// <param name='altText'>Alt text for image.</param>
        /// <returns>Image data in html Markup.</returns>
        public static string RenderImage(byte[] imageArray, string altText)
        {
            return RenderImage(imageArray, altText, null, null);
        }


        /// <summary>
        /// Accepts image data in byte array and then converts the image to data URI, creates html Markup for image and then Embeds this data into it.
        /// </summary>
        /// <param name='imageArray'>Byte array containing image info.</param>
        /// <param name='altText'>Alt text for image.</param>
        /// <param name='height'>Height of rendered image.</param>
        /// <param name='width'>Width of rendered image.</param>
        /// <returns>Image data in html Markup.</returns>
        public static string RenderImage(byte[] imageArray, string altText, int? height, int? width)
        {
            string imageMarkup = string.Empty;

            try
            {
                // If height and width are null then calculate them from image data given.
                height = height ?? CalculateDefaultHeight(imageArray);
                width = width ?? CalculateDefaultWidth(imageArray);

                var srcValue = 'data:image/gif;base64,';
                 
                string base64DataURI = Convert.ToBase64String(imageArray);

                srcValue += base64DataURI; 

                imageMarkup = string.Format('<img width=\'{0}\' height=\'{1}\' alt=\'{2}\' src= \'{3}\' />', width, height, altText, srcValue);
            }
            catch (Exception ex)
            {
                throw new Exception('Exception occurred while Rendering image' + ex.Message);
            }

            return imageMarkup;
        }


    }
")
                ;
            }
        }
    }
}