using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents
{
    [Group("Overview")]
    [Group("Pie")]
    [Group("Bar")]
    public class RefreshAndDrillDownViewModel
    {
        public RefreshAndDrillDownViewModel()
        {
        }

        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Graphs can interact with the server, by refreshing their data, or drilling down into new data sets. To do this, set the 'DrillDownUrl' property and 'TopLevelUrl' property on your graph view model to the location of an AJAX action that will return the content for the new graph")
                    .AddParagraph("The AJAX provider should use the GraphViewModel partial view, and optionally can use posted back values to determine what was clicked on")
                    .AddPreformatted(@"
        public ActionResult RefreshAndDrillDown()
        {
            var model = new RefreshAndDrillDownViewModel();
            model.PieGraph = new GraphViewModel(GraphViewModel.PIE);

            fillModelData(model.PieGraph, null);
            ...
        }

        public ActionResult NewGraph(string graphType, string label, double[] dataPoint)
        {
            var model = new GraphViewModel(graphType);
            fillModelData(model, label);
            return PartialView(""EditorTemplates/GraphViewModel"", model);
        }

        private void fillModelData(GraphViewModel model, string label)
        {
            model.TopLevelUri = Url.Action(""NewGraph"", ""PowerComponents"");
            model.Title = ""Unemployment duration by state: "" + label;

            if (string.IsNullOrEmpty(label)) { // Top level
                model.SingleSeries.Add(""< 4 weeks"", 156100);
                model.SingleSeries.Add(""4 - 13 weeks"", 177900);
                model.SingleSeries.Add(""13 - 26 weeks"", 122100);
                model.SingleSeries.Add(""26 - 52 weeks"",112000);
                model.SingleSeries.Add(""52 - 104 weeks"",82000);
                model.SingleSeries.Add(""> 104 weeks"",73300);
                model.DrillDownUri = Url.Action(""NewGraph"", ""PowerComponents"");
                model.Title = ""Unemployment by duration"";
            }
            else if (label == ""< 4 weeks"") {
                model.SingleSeries.Add(""NSW"",44900);
                model.SingleSeries.Add(""VIC"",41900);
                model.SingleSeries.Add(""QLD"",32700);
                model.SingleSeries.Add(""SA"",11400);
                model.SingleSeries.Add(""WA"",16800);
                model.SingleSeries.Add(""TAS"",3000);
                model.SingleSeries.Add(""NT"",3000);
                model.SingleSeries.Add(""ACT"",2300);
            }
            ...
        }

")
                    ;
            }
        }

        [Display(GroupName="Pie")]
        public GraphViewModel PieGraph {get; set;}

        [Display(GroupName = "Bar")]
        public ContentViewModel PreBarContent
        {
            get
            {
                return new ContentViewModel().AddParagraph("You can mix regular content along with graphs into one group.");
            }
        }


        [Display(GroupName = "Bar")]
        public GraphViewModel BarGraph {get; set;}

        [Display(GroupName = "Bar")]
        public ContentViewModel PostBarContent
        {
            get
            {
                return new ContentViewModel().AddParagraph("The graph will be refreshed independently of other content.");
            }
        }


    }
}