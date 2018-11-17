//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Employment.Web.Mvc.Infrastructure.Interfaces
//{
//    /// <summary>
//    /// Represents a delegate to which a method with signature 'string GetTemplateContent(string templateName)' can be assigned.
//    /// </summary>
//    /// <param name="templateName">template name.</param>
//    /// <returns>template content.</returns>
//    public delegate string DelegateCustomTemplateResolver(string templateName);


//    /// <summary>
//    /// Defines methods that are required for HtmlReportService.
//    /// </summary>
//    public interface IHtmlReportService
//    {
//        /// <summary>
//        /// Parses the model into string, by calling GetContent() to obtain content of template and passing the model
//        /// </summary> 
//        /// <param name="model">Model. </param>
//        /// <param name="templateContent">Main template content. </param>
//        /// <returns>Parsed String.</returns>
//        string ParseMainTemplate<T>(DelegateCustomTemplateResolver getTemplateMethodResolver, T model) where T : IBaseReportTemplate;

//        ///// <summary>
//        ///// Registers sub templates which are referenced in your main Template.
//        ///// </summary>
//        ///// <param name="url"> Url string of the template. If your report is under (Views/Templates/) then Url is "~/Templates/reportName".</param>
//        ///// <param name="templateName">Name of the template referenced in @RenderPage("~/Templates/reportName", skipLayout: true).</param> 
//        ///// <returns>Exception Details if any.</returns>
//        //string RegisterSubTemplates(string url, string templateName);
//    }
//}
