using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    public class GraphViewModel
    {
        public const string PIE = "pie";
        public const string BAR = "bar";
        public const string LINE = "line";

        const string DefaultChartName = "Chart";

        /// <summary>
        /// The collection of datasets in this graph. Each named dataset is itself a collection of object to double mappings
        /// For bar graphs and pie graphs, the keys of the inner collections should be strings. For line graphs, they should be
        /// numeric or time values.
        /// You should only use multiple datasets witin the same graph if they have the same key axis. That is, the same X axis for numerical data,
        /// or a similar set of categories for categorical data
        /// </summary>
        public Dictionary<string, Dictionary<object, double>> Values { get; set; }

        /// <summary>
        /// Shortcut for using GraphModels that will only ever have one data series.
        /// When this Property is first used, a default dat series named "Chart" will be created and placed in the Values collection
        /// </summary>
        public Dictionary<object, double> SingleSeries
        {
            get
            {
                if (!Values.ContainsKey(DefaultChartName)) {
                    Values[DefaultChartName] = new Dictionary<object,double>();
                }
                return Values[DefaultChartName];
            }
            set
            {
                Values[DefaultChartName] = value;
            }
        }

        public string GraphType { get; set; }

        /// <summary>
        /// Text to tdisplay below the graph
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Url to fetch rendered HTML from, when a user drill down into this graph.
        /// This would typically be set to an AjaxOnly method that will return a Partial View.
        /// </summary>
        public string DrillDownUrl { get; set; }

        /// <summary>
        /// The Url to render top level data for this model. Useful when providing drill down capability
        /// This would typically be set to an AjaxOnly method that will return a Partial View.
        /// Setting this will create a top level button in the rendered graph.
        /// </summary>
        public string TopLevelUrl { get; set; }

        public GraphViewModel(string type)
        {
            Values = new Dictionary<string, Dictionary<object, double>>();
            GraphType = type;
        }

        /// <summary>
        /// Adds a named dataset to the graph collection, and returns it so that the caller can add values to it
        /// </summary>
        public Dictionary<object, double> AddDataSet(string name)
        {
            var newDict = new Dictionary<object, double>();
            Values.Add(name, newDict);
            return newDict;
        }

        static int uniqueIdCounter = 0;
        public static string GetUniqueId()
        {
            int nextId = uniqueIdCounter++;
            return "zeus-graph-" + nextId;
        }
   }
}