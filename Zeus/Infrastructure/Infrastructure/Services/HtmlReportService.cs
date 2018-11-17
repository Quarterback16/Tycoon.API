//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Employment.Web.Mvc.Infrastructure.Interfaces;
//using Employment.ReportingTool.Implementation;
//using Employment.ReportingTool.Interfaces;
////using ReportingTool.RazorMachine.Implementation;
////using ReportingTool.RazorMachine.Interfaces;

//namespace Employment.Web.Mvc.Infrastructure.Services
//{

//    /// <summary>
//    /// Defines a service for parsing templates.
//    /// </summary>
//    public class HtmlReportService : IHtmlReportService
//    {
        
        

//        ///// <summary>
//        ///// Registers sub templates which are referenced in your main Template.
//        ///// </summary>
//        ///// <param name="url"> Url string of the template. If your report is under (Views/Templates/) then Url is "~/Templates/reportName".</param>
//        ///// <param name="templateName">Name of the template referenced in @RenderPage("~/Templates/reportName", skipLayout: true).</param> 
//        ///// <returns>Exception Details if any.</returns>
//        //public string RegisterSubTemplates(string url, string templateName)
//        //{
//        //    return string.Empty;// _templateEngine.RegisterSubTemplates(url, templateName);
//        //}
        


//        #region IHtmlReportService Members

//        /// <summary>
//        /// Parses the model into string, by calling GetContent() to obtain content of template and passing the model
//        /// </summary> 
//        /// <param name="model">Model. </param>
//        /// <returns>Parsed String.</returns>
//        public string ParseMainTemplate<T>(Interfaces.DelegateCustomTemplateResolver getTemplateMethodResolver, T model) where T: IBaseReportTemplate
//        {
//            string parsedHtml = string.Empty;
//            ITemplateEngine _templateEngine;

//            if (getTemplateMethodResolver != null)
//            {
//                Employment.ReportingTool.Implementation.DelegateCustomTemplateResolver customTemplateResolver;

//                customTemplateResolver = new Employment.ReportingTool.Implementation.DelegateCustomTemplateResolver(getTemplateMethodResolver);

//                _templateEngine = new CustomRazorTemplateEngine(customTemplateResolver);
//            }
//            else
//            {
//                throw new ArgumentNullException("getTemplateMethodResolver", "DelegateCustomTemplateResolver value cannot be null. Please specify a method that returns templateContent. ");
//            }

//            if (string.IsNullOrEmpty(model.ParentTemplateName))
//            {
//                throw new Exception("Please specify Parent (main) template name.");
//            }

//            parsedHtml = _templateEngine.Parse(model.ParentTemplateName, model, model.StyleTemplateName);

//            return parsedHtml;
//        }

//        #endregion
//    }
//}
