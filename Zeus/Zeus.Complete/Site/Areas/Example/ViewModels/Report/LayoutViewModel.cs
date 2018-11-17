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

    [DisplayName("Layout / Master View for Report templates")]
    [Group("Layout Page", Order = 1)]
    public class LayoutViewModel
    {

        [Display(GroupName="Layout Page", Order = 1)]
        public ContentViewModel LayoutPageContent
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("This is Layout View")
                    .AddStrongText("Your model must have property 'PageTitle' to set the <title></title>.")
                    .AddPreformatted(
@"
@{ 

   string pageTitle = 'Report'; 
   
   if (!string.IsNullOrEmpty(Model.PageTitle))
   {
       pageTitle = Model.PageTitle;
   } 

    string styleSection = string.Empty; 
    if (!string.IsNullOrEmpty(Model.StyleTemplateName))
    {
        styleSection = Model.StyleTemplateName;
    }

    string printStyleSection = string.Empty;      
    if (!string.IsNullOrEmpty(Model.PrintStyleTemplateName))
    {
        printStyleSection = Model.PrintStyleTemplateName;
    }
    

    string screenStyleSection = string.Empty;          
    if (!string.IsNullOrEmpty(Model.ScreenStyleTemplateName))
    {
        screenStyleSection = Model.ScreenStyleTemplateName;
    }
    
}

<!DOCTYPE html>
<html>
    <head>
       
        
        <title>@pageTitle </title>
        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
        <style  type='text/css'>

            /*  All styles go here.. Please have a look at Stylesheet-View. */

        </style>
    </head>
<body>
    <div id='content'>
        @Include(Model.ParentTemplateName) @*'bodyContent'*@
    </div>
</body>
</html>

")
                    ;
            }
        }
    }
}