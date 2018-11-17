
namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    public interface IBaseReportTemplate 
    { 
            /// <summary>
            /// REQUIRED for 'Title' of report.
            /// </summary>
            string PageTitle { get; set; }

            /// <summary>
            /// Name of the template which defines custom styles. These will be applied to all @media types.
            /// </summary>
            string StyleTemplateName { get; set; }

            /// <summary>
            /// Unique name for the template. Usually this will be the name of your parent template.
            /// </summary>
            string ParentTemplateName { get; set; }

            /// <summary>
            /// Template name which defines styles for @media print. The value is not mandatory.
            /// </summary>
            string PrintStyleTemplateName { get; set; }


            /// <summary>
            /// Template name which defines styles for @media screen. The value is not mandatory.
            /// </summary>
            string ScreenStyleTemplateName { get; set; }
            
    }
}
