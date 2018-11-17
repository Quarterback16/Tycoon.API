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
    [Group("Pie", "first", GroupRowType.Half)]
    [Group("Bar", "first", GroupRowType.Half)]
    [Group("Line", "mez", GroupRowType.Half)]
    [Group("MultiOverview")]
    [Group("MultiPie", "second", GroupRowType.Half)]
    [Group("MultiBar", "second", GroupRowType.Half)]
    [Group("MultiLine", "third", GroupRowType.Half)]
    public class GraphTypesViewModel
    {
        public GraphTypesViewModel()
        {
        }

        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Graphs can be easily added to your pages using the 'GraphViewModel'. Each graph will automatically be rendered as a table as well for accessibility.")
                    .AddParagraph("Graphs can be constructed from simple data series, and can be pie, bar, or line graphs. You can also plot multiple pies, bars and lines on the same graph. These will each display slightly differently due to the inherent differences between these graph types. For multi graphs, the user will be able to toggle on and off which series they wish to see on the plots.")
                    .AddParagraph("Using the graphs is as simple as creating the model and filling the data series.")
                    .AddPreformatted(@"
        public GraphTypesViewModel()
        {
            [Display(GroupName=""Pie"")]
            public GraphViewModel PieGraph {get; set;}

            [Display(GroupName = ""Bar"")]
            public GraphViewModel BarGraph {get; set;}

            [Display(GroupName = ""Line"")]
            public GraphViewModel LineGraph { get; set; }
        }

        public GraphsController()
        {
            ...
            public ActionResult GraphTypes()
            {
                model.PieGraph = new GraphViewModel(GraphViewModel.PIE);
                model.PieGraph.SingleSeries.Add(""PR's to Resubmit"", 20);
                model.PieGraph.SingleSeries.Add(""Outstanding re-engagment results"", 9);
                model.PieGraph.SingleSeries.Add(""Outstanding Activity Attendance results"", 15);
                ...

                model.BarGraph = new GraphViewModel(GraphViewModel.BAR);
                model.BarGraph.SingleSeries = model.PieGraph.SingleSeries;

                model.LineGraph = new GraphViewModel(GraphViewModel.LINE);
                model.LineGraph.SingleSeries.Add(0, 2);
                model.LineGraph.SingleSeries.Add(1, 2.5);
                model.LineGraph.SingleSeries.Add(2.3, 4.7);
                ...
            }
        }
")
                    ;
            }
        }

        [Display(GroupName="Pie")]
        public GraphViewModel PieGraph {get; set;}

        [Display(GroupName = "Bar")]
        public GraphViewModel BarGraph {get; set;}

        [Display(GroupName = "Line")]
        public ContentViewModel PreLineContent
        {
            get
            {
                return new ContentViewModel().AddParagraph("You can mix regular content along with graphs into one group");
            }
        }

        [Display(GroupName = "Line")]
        public GraphViewModel LineGraph { get; set; }

        [Display(GroupName = "Line")]
        public ContentViewModel PostLineContent
        {
            get
            {
                return new ContentViewModel().AddParagraph("Don't forget that the capabilities of the graphs can be extended in many different ways. Contact the Framework team to discuss possibilities if you wish to be able to do something with graphs that isn't explained in these examples.");
            }
        }

        [Display(GroupName = "MultiOverview")]
        public ContentViewModel MultiOverview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Graphs with multiple series are a little more complicated to set up. Each different series must be given a name. Since each multi graph is also rendered as a multi column table, it's important to only graph data on the same chart if they can be represented using the same X (key) axis.")
                    .AddPreformatted(@"
        public GraphTypesViewModel()
        {
            [Display(GroupName = ""MultiPie"")]
            public GraphViewModel MultiPieGraph { get; set; }

            [Display(GroupName = ""MultiBar"")]
            public GraphViewModel MultiBarGraph { get; set; }

            [Display(GroupName = ""MultiLine"")]
            public GraphViewModel MultiLineGraph { get; set; }
        }

        public GraphsController()
        {
            ...
            public ActionResult GraphTypes()
            {
                ...
                model.MultiBarGraph = new GraphViewModel(GraphViewModel.BAR);
                Dictionary&lt;object, double&gt; lastMonth = model.MultiBarGraph.AddDataSet(""Last Month"");
                lastMonth.Add(""Outstanding re-engagment results"", 43);
                lastMonth.Add(""Outstanding Activity Attendance results"", 23);
                ...

                Dictionary&lt;object, double&gt; lastWeek = model.MultiBarGraph.AddDataSet(""Last Week"");
                lastWeek.Add(""PR's to Resubmit"", 20);
                lastWeek.Add(""Outstanding re-engagment results"", 9);
                ...

                Dictionary&lt;object, double&gt; thisWeek = model.MultiBarGraph.AddDataSet(""This Week"");
                thisWeek.Add(""Outstanding re-engagment results"", 4);
                thisWeek.Add(""Outstanding Activity Attendance results"", 0);
                ...

                model.MultiPieGraph = new GraphViewModel(GraphViewModel.PIE);
                model.MultiPieGraph.Values = model.MultiBarGraph.Values;

                model.MultiLineGraph = new GraphViewModel(GraphViewModel.LINE);
                Dictionary&lt;object, double&gt; seriesOfNumbers = model.MultiLineGraph.AddDataSet(""Series of numbers"");
                seriesOfNumbers.Add(0, 2);
                seriesOfNumbers.Add(1, 2.5);
                seriesOfNumbers.Add(2.3, 4.7);
                ...
                Dictionary&lt;object, double&gt; dollarsIn = model.MultiLineGraph.AddDataSet(""Dollars in"");
                dollarsIn.Add(0, 15);
                dollarsIn.Add(1, 14);
                dollarsIn.Add(2.3, 8);
                ...
                ...
            }
            ...
        }
")
                    ;
            }
        }

        [Display(GroupName = "MultiPie")]
        public GraphViewModel MultiPieGraph { get; set; }

        [Display(GroupName = "MultiBar")]
        public GraphViewModel MultiBarGraph { get; set; }

        [Display(GroupName = "MultiLine")]
        public GraphViewModel MultiLineGraph { get; set; }
    }
}