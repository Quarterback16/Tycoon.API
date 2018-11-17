using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Area.Example.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels.Calendar;
using Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents;
using Employment.Web.Mvc.Area.Example.ViewModels.Calendar;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    [Security(AllowAny=true)]
    public class PowerComponentsController : InfrastructureController
    {
        public PowerComponentsController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }
        [Menu("Power components", Order = 100)]

        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Power components examples")
                .AddParagraph("This area contains examples demonstrating the use of various complex controls, including widgets/dashboards, graphs, calendar and date controls and more.");

            return View(model);
        }


        #region graph types
        [Menu("Graphs", Order = 10, ParentAction = "Index")]
        public ActionResult GraphTypes()
        {
            var model = new GraphTypesViewModel();

            model.PieGraph = new GraphViewModel(GraphViewModel.PIE);
            model.PieGraph.SingleSeries.Add("PR's to Resubmit", 20);
            model.PieGraph.SingleSeries.Add("Outstanding re-engagment results", 9);
            model.PieGraph.SingleSeries.Add("Outstanding Activity Attendance results", 15);
            model.PieGraph.SingleSeries.Add("# of job seekers transferred to your caseload", 2);
            model.PieGraph.SingleSeries.Add("# of EPP pending", 2);
            model.PieGraph.SingleSeries.Add("# of job seekers with <40 jobs applied", 8);

            model.BarGraph = new GraphViewModel(GraphViewModel.BAR);
            model.BarGraph.SingleSeries = model.PieGraph.SingleSeries;

            model.MultiBarGraph = new GraphViewModel(GraphViewModel.BAR);
            Dictionary<object, double> lastMonth = model.MultiBarGraph.AddDataSet("Last Month");
            lastMonth.Add("Outstanding re-engagment results", 43);
            lastMonth.Add("Outstanding Activity Attendance results", 23);
            //lastMonth.Add("# of job seekers transferred to your caseload", 6);
            lastMonth.Add("# of EPP pending", 9);
            //lastMonth.Add("# of job seekers with <40 jobs applied", 14);
            Dictionary<object, double> lastWeek = model.MultiBarGraph.AddDataSet("Last Week");
            lastWeek.Add("PR's to Resubmit", 20);
            lastWeek.Add("Outstanding re-engagment results", 9);
            lastWeek.Add("Outstanding Activity Attendance results", 15);
            //lastWeek.Add("# of job seekers transferred to your caseload", 2);
            lastWeek.Add("# of EPP pending", 2);
            //lastWeek.Add("# of job seekers with <40 jobs applied", 8);
            Dictionary<object, double> thisWeek = model.MultiBarGraph.AddDataSet("This Week");
            thisWeek.Add("Outstanding re-engagment results", 4);
            thisWeek.Add("Outstanding Activity Attendance results", 0);
            thisWeek.Add("# of EPP pending", 8);
            //thisWeek.Add("# of job seekers with <40 jobs applied", 3);

            model.MultiPieGraph = new GraphViewModel(GraphViewModel.PIE);
            model.MultiPieGraph.Values = model.MultiBarGraph.Values;

            model.LineGraph = new GraphViewModel(GraphViewModel.LINE);
            model.LineGraph.SingleSeries.Add(0, 2);
            model.LineGraph.SingleSeries.Add(1, 2.5);
            model.LineGraph.SingleSeries.Add(2.3, 4.7);
            model.LineGraph.SingleSeries.Add(3.2, 4.98);
            model.LineGraph.SingleSeries.Add(6.1, 3.0123);
            model.LineGraph.SingleSeries.Add(7.4, 4.2);
            model.LineGraph.SingleSeries.Add(8.4, 5.8);
            model.LineGraph.SingleSeries.Add(8.9, 11.2);

            model.MultiLineGraph = new GraphViewModel(GraphViewModel.LINE);
            Dictionary<object, double> seriesOfNumbers = model.MultiLineGraph.AddDataSet("Series of numbers");
            seriesOfNumbers.Add(0, 2);
            seriesOfNumbers.Add(1, 2.5);
            seriesOfNumbers.Add(2.3, 4.7);
            seriesOfNumbers.Add(3.2, 4.98);
            seriesOfNumbers.Add(6.1, 3.0123);
            seriesOfNumbers.Add(7.4, 4.2);
            seriesOfNumbers.Add(8.4, 5.8);
            seriesOfNumbers.Add(8.9, 11.2);
            Dictionary<object, double> dollarsIn = model.MultiLineGraph.AddDataSet("Dollars in");
            dollarsIn.Add(0, 15);
            dollarsIn.Add(1, 14);
            dollarsIn.Add(2.3, 8);
            dollarsIn.Add(3.2, 7);
            dollarsIn.Add(6.1, 6.5);
            dollarsIn.Add(7.4, 6.2);
            dollarsIn.Add(8.4, 6.5);
            dollarsIn.Add(8.9, 7.5);
            Dictionary<object, double> housePrices = model.MultiLineGraph.AddDataSet("House prices");
            housePrices.Add(0, 0.5);
            housePrices.Add(1, 0.75);
            housePrices.Add(2.3, 1.25);
            housePrices.Add(3.2, 1.75);
            housePrices.Add(6.1, 8);
            housePrices.Add(7.4, 12);
            housePrices.Add(8.4, 16);
            housePrices.Add(8.9, 17);
            Dictionary<object, double> publicOpinion = model.MultiLineGraph.AddDataSet("Public opinion");
            publicOpinion.Add(0, 10);
            publicOpinion.Add(1, 6);
            publicOpinion.Add(2.3, 2);
            publicOpinion.Add(3.2, 3);
            publicOpinion.Add(6.1, 12.0123);
            publicOpinion.Add(7.4, 11.5);
            publicOpinion.Add(8.4, 5.8);
            publicOpinion.Add(8.9, 4.2);

            return View(model);
        }
        #endregion

        #region refresh / drill down
        [Menu("Graph drill down", Order = 20, ParentAction = "Index")]
        public ActionResult RefreshAndDrillDown()
        {
            var model = new RefreshAndDrillDownViewModel();
            model.PieGraph = new GraphViewModel(GraphViewModel.PIE);
            fillModelData(model.PieGraph, null);

            model.BarGraph = new GraphViewModel(GraphViewModel.BAR);
            fillModelData(model.BarGraph, null);

            return View(model);
        }

        public ActionResult NewGraph(string graphType, string label, double[] dataPoint)
        {
            var model = new GraphViewModel(graphType);
            fillModelData(model, label);
            return PartialView("EditorTemplates/GraphViewModel", model);
        }

        private void fillModelData(GraphViewModel model, string label)
        {
            model.TopLevelUrl = Url.Action("NewGraph", "PowerComponents");
            model.Title = "Unemployment duration by state: " + label;

            if (string.IsNullOrEmpty(label))
            { // Top level
                model.SingleSeries.Add("< 4 weeks", 156100);
                model.SingleSeries.Add("4 - 13 weeks", 177900);
                model.SingleSeries.Add("13 - 26 weeks", 122100);
                model.SingleSeries.Add("26 - 52 weeks", 112000);
                model.SingleSeries.Add("52 - 104 weeks", 82000);
                model.SingleSeries.Add("> 104 weeks", 73300);
                model.DrillDownUrl = Url.Action("NewGraph", "PowerComponents");
                model.Title = "Unemployment by duration";
            }
            else if (label == "< 4 weeks")
            {
                model.SingleSeries.Add("NSW", 44900);
                model.SingleSeries.Add("VIC", 41900);
                model.SingleSeries.Add("QLD", 32700);
                model.SingleSeries.Add("SA", 11400);
                model.SingleSeries.Add("WA", 16800);
                model.SingleSeries.Add("TAS", 3000);
                model.SingleSeries.Add("NT", 3000);
                model.SingleSeries.Add("ACT", 2300);
            }
            else if (label == "4 - 13 weeks")
            {
                model.SingleSeries.Add("NSW", 51700);
                model.SingleSeries.Add("VIC", 49900);
                model.SingleSeries.Add("QLD", 35600);
                model.SingleSeries.Add("SA", 13200);
                model.SingleSeries.Add("WA", 20000);
                model.SingleSeries.Add("TAS", 1900);
                model.SingleSeries.Add("NT", 1300);
                model.SingleSeries.Add("ACT", 2200);
            }
            else if (label == "13 - 26 weeks")
            {
                model.SingleSeries.Add("NSW", 35400);
                model.SingleSeries.Add("VIC", 32900);
                model.SingleSeries.Add("QLD", 26200);
                model.SingleSeries.Add("SA", 10200);
                model.SingleSeries.Add("WA", 12300);
                model.SingleSeries.Add("TAS", 2900);
                model.SingleSeries.Add("NT", 700);
                model.SingleSeries.Add("ACT", 1500);
            }
            else if (label == "26 - 52 weeks")
            {
                model.SingleSeries.Add("NSW", 33600);
                model.SingleSeries.Add("VIC", 29900);
                model.SingleSeries.Add("QLD", 25000);
                model.SingleSeries.Add("SA", 9100);
                model.SingleSeries.Add("WA", 9500);
                model.SingleSeries.Add("TAS", 3200);
                model.SingleSeries.Add("NT", 500);
                model.SingleSeries.Add("ACT", 1100);
            }
            else if (label == "52 - 104 weeks")
            {
                model.SingleSeries.Add("NSW", 25000);
                model.SingleSeries.Add("VIC", 20600);
                model.SingleSeries.Add("QLD", 18500);
                model.SingleSeries.Add("SA", 7700);
                model.SingleSeries.Add("WA", 5800);
                model.SingleSeries.Add("TAS", 3000);
                model.SingleSeries.Add("NT", 300);
                model.SingleSeries.Add("ACT", 1000);
            }
            else if (label == "> 104 weeks")
            {
                model.SingleSeries.Add("NSW", 27400);
                model.SingleSeries.Add("VIC", 17400);
                model.SingleSeries.Add("QLD", 13700);
                model.SingleSeries.Add("SA", 6300);
                model.SingleSeries.Add("WA", 4800);
                model.SingleSeries.Add("TAS", 3000);
                model.SingleSeries.Add("NT", 200);
                model.SingleSeries.Add("ACT", 400);
            }
        }
        #endregion

        #region Widgets
        /// <summary>
        /// Examples of widgets
        /// </summary>
        /// <returns></returns>
        [Menu("Dashboard widgets", Order = 30, ParentAction = "Index")]
        public ActionResult Widgets()
        {
            var model = new WidgetsViewModel();
            UserService.Dashboard.SetWidgetLayout("Unique title for the widget", "Single widget example");
            return View(model);
        }

        #endregion

        #region Widget content Actions

        [AjaxOnly]
        public ActionResult WidgetContentOne()
        {
            System.Threading.Thread.Sleep(1000); // Fake waiting time to show javascript spinner
            var model = new ContentViewModel();
            model.AddSubTitle("A content based block");
            model.AddParagraph("A content based widget with lots of information.");
            model.AddListItem("A list with items");
            model.AddListItem("Another list item");
            model.AddParagraph("Some further text after the list contains even more words, and punctuation marks as well.");
            return PartialView("EditorTemplates/ContentViewModel", model);
        }

        [AjaxOnly]
        public ActionResult WidgetContentTwo()
        {
            System.Threading.Thread.Sleep(1000); // Fake waiting time to show javascript spinner
            var model = new GraphViewModel(GraphViewModel.PIE);
            model.SingleSeries.Add("pencils", 37.5);
            model.SingleSeries.Add("pens", 62.5);
            return PartialView("EditorTemplates/GraphViewModel", model);
        }

        [AjaxOnly]
        public ActionResult WidgetContentThree()
        {
            System.Threading.Thread.Sleep(1000); // Fake waiting time to show javascript spinner
            var model = new AViewModel();
            return PartialView("EditorTemplates/Object", model);
        }

        [AjaxOnly]
        public ActionResult WidgetContentFour()
        {
            var model = new GraphViewModel(GraphViewModel.PIE);
            model.SingleSeries.Add("Employed full-time",8055300);
            model.SingleSeries.Add("Employed part-time",3540400);
            model.SingleSeries.Add("Unemployed",728500);
            model.SingleSeries.Add("Not in the labour force",6717700);
            model.DrillDownUrl = Url.Action("DrillDownLabour", "PowerComponents");
            return PartialView("EditorTemplates/GraphViewModel", model);
        }

        [AjaxOnly]
        [HttpPost]

        public ActionResult DrillDownLabour(string graphType, string label, double[] dataPoint)
        {
            var model = new GraphViewModel(graphType);
            model.TopLevelUrl = Url.Action("WidgetContentFour", "PowerComponents");
            if (label == "Employed full-time")
            {
                model.SingleSeries.Add("Managers (FT)",1293700);
                model.SingleSeries.Add("Professionals (FT)",1925800);
                model.SingleSeries.Add("Technicians and trade workers (FT)",1439400);
                model.SingleSeries.Add("Community and personal service workers (FT)",542400);
                model.SingleSeries.Add("Clerical and administrative workers (FT)",1090700);
                model.SingleSeries.Add("Sales workers (FT)",465600);
                model.SingleSeries.Add("Machinary operators and drivers (FT)",647000);
                model.SingleSeries.Add("Labourers (FT)",616900);
            }
            else if (label == "Employed part-time")
            {
                model.SingleSeries.Add("Managers (PT)", 185600);
                model.SingleSeries.Add("Professionals (PT)", 624300);
                model.SingleSeries.Add("Technicians and trade workers (PT)", 237100);
                model.SingleSeries.Add("Community and personal service workers (PT)", 600400);
                model.SingleSeries.Add("Clerical and administrative workers (PT)", 565900);
                model.SingleSeries.Add("Sales workers (PT)", 617900);
                model.SingleSeries.Add("Machinary operators and drivers (PT)", 114600);
                model.SingleSeries.Add("Labourers (PT)", 514700);
            }
            else
            {
                model.SingleSeries.Add("Employed full-time", 8055300);
                model.SingleSeries.Add("Employed part-time", 3540400);
                model.SingleSeries.Add("Unemployed", 728500);
                model.SingleSeries.Add("Not in the labour force", 6717700);
                model.DrillDownUrl = Url.Action("DrillDownLabour", "PowerComponents");
                model.TopLevelUrl = null;
            }
            return PartialView("EditorTemplates/GraphViewModel", model);
        }

        class AViewModel : ILayoutOverride {
            public ContentViewModel FirstBit
            {
                get
                {
                    return new ContentViewModel().AddParagraph("A mixed model with text and form elements, this widget has been registered as full width");
                }
            }

            public string AnEntryField { get; set; }

            public ContentViewModel SecondBit {
                get {
                    return new ContentViewModel().AddParagraph("Some further parts within the complex view model");
                }
            }

            public IEnumerable<LayoutType> Hidden
            {
                get
                {
                    return new LayoutType[] { LayoutType.TitleAndBreadcrumbs, LayoutType.RequiredFieldsMessage, LayoutType.LeftHandNavigation };
                }
                set
                {

                }
            }

        }

        public ActionResult WidgetContentFive(string widgetContext)
        {
            System.Threading.Thread.Sleep(1000); // Fake waiting time to show javascript spinner
            var model = new ContentViewModel();
            model.AddSubTitle("Data context: " + (UserService.Dashboard.GetDataContext(widgetContext) ?? "not set"));
            model.AddParagraph("This model changes its information based on the data context, set in the right hand sidebar.");
            model.AddParagraph("It is useful for making a set of widgets display a different kind of information as a whole.");
            model.AddParagraph("A user selected data context is saved for that user across all pages/widgets");
            model.AddParagraph("You can access the Data Context in your widget controllers using the dashboard service:");
            model.AddPreformatted(@"
        public ActionResult WidgetContentFive(string widgetContext)
        {
            ...
            string dataContext = UserService.Dashboard.GetDataContext(widgetContext);
            ...
        }
");
            return PartialView("EditorTemplates/ContentViewModel", model);
        }


        #endregion 
       
        #region Dashboard
        [Menu("Dashboard example", Order = 40, ParentAction = "Index")]
        public ActionResult Dashboard()
        {
            var model = new DashboardViewModel();

            AddInformationMessage(@"This dashboard page shows an example of a dashboard with several widgets to choose from. It is intended to display the interactions available to all widgets, rather than demonstrate any particular code.
Use the controls on the right hand sidebar and the widget title bars to: Add widgets to the page, Remove widgets from the page, Expand/Collapse the content of the widget, Refresh the content of the widget, Use the arrow buttons to change the widget order, Use the mouse to drag the widgets around and change their order, Visit another page and then return here to see the preservation of your widget choices");

            return View(model);
        }
        #endregion

        #region right sidebar
        [Menu("Right sidebar", Order = 50, ParentAction = "Index")]
        public ActionResult RightSidebar()
        {
            var model = new RightSidebarViewModel();

            return View(model);
        }

        #endregion

        #region History
        [Menu("Last accessed", Order = 60, ParentAction = "Index")]
        [History(Infrastructure.Types.HistoryType.JobSeeker)]
        [History(Infrastructure.Types.HistoryType.Contract)]
        public ActionResult History(string jobseekerId, string contractId)
        {
            if (jobseekerId == null || contractId == null)
            {
                return RedirectToAction("History", new { jobseekerId = "(not set)", contractId = "(not set)" });
            }

            // Check history and add elements if none
            IEnumerable<HistoryModel> jobSeekers = UserService.History.Get(HistoryType.JobSeeker);
            IEnumerable<HistoryModel> contracts = UserService.History.Get(HistoryType.Contract);

            //UserService.History.SetJobSeeker((new Random()).Next(100000), (new Random()).Next(100) + " - jobseeker");

            if (!jobSeekers.Any())
            {
                UserService.History.SetJobSeeker(123456, "A fake jobseeker");
                UserService.History.SetJobSeeker(654321, "John Smith");
            }

            if (!contracts.Any())
            {
                UserService.History.SetContract("CB222222", "A big contract");
                UserService.History.SetContract("CL111111", "A little contract");
            }

            var model = new HistoryViewModel(jobseekerId, contractId);

            return View(model);
        }

        #endregion

        #region date based content
        /// <summary>
        /// An example of using <see cref="DateBasedContentAttribute" />.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Menu("Date based content", Order = 70, ParentAction = "Index")]
        public ActionResult DateBasedContent()
        {
            var model = new DateBasedContentViewModel();

            return View(model);
        }

        [AjaxOnly]
        public ActionResult HandlerForDateBasedContent(DateTime selectedDate)
        {
            var model = new ContentViewModel()
                    .AddText("You have selected the date:")
                    .AddTitle(selectedDate.ToString("dd/MM/yyyy"))
            ;

            return PartialView("EditorTemplates/ContentViewModel", model);
        }

        #endregion


        
        #region Calendar Area

        
        /// <summary>
        /// Events list of 'Meeting with Jobseeker' type.
        /// </summary>
        private static IList<CategoryItemViewModel> meetingCategoryEventsList = new List<CategoryItemViewModel>()
        {
            new CategoryItemViewModel()
            {
                Id = "1",
                EventDescription = new ContentViewModel().AddText( "Next month event."),
                Title = "Meeting with jQuery",
                Start = DateTime.Now.AddMonths(1),
                End = DateTime.Now.AddMonths(1).AddHours(2)
            },
            new CategoryItemViewModel()
            {
                Id = "2",
                EventDescription = new ContentViewModel().AddText("Next Week's ALL DAY event."),
                Title = "Meeting with JSON",
                Start = DateTime.Now.AddDays(7),
                End = DateTime.Now.AddDays(7),
                AllDay = true
            }

        };

        /// <summary>
        /// Events list for 'Events' type.
        /// </summary>
        private static IList<CategoryItemViewModel> eventsCategoryList = new List<CategoryItemViewModel>()
        {
           
            new CategoryItemViewModel() {
                AllDay = true, 
                Id = "3", 
                EventDescription = new ContentViewModel().AddText("Bind an event handler to the \"click\" JavaScript event."), 
                Title = "Click Event", 
                Start = DateTime.Now.AddDays(3), 
                End = DateTime.Now.AddDays(3) ,
                HoverDescription = new ContentViewModel().AddUnderlinedText("This is icon for Click event").AddIcon(IconType.AngleUp)
            }, 
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "4", 
                EventDescription = new ContentViewModel().AddStrongText("Keyup event.").AddIcon(IconType.Android), 
                Title = "keyup pause event", 
                Start = DateTime.Now.AddDays(2), 
                End = DateTime.Now.AddDays(2).AddHours(1) 
            },
            new CategoryItemViewModel() {
                AllDay = true, 
                Id = "5", 
                EventDescription = new ContentViewModel().AddText("Bind an event handler to the \"click\" ."), 
                Title = "Click Event", 
                Start = DateTime.Now.AddDays(3), 
                End = DateTime.Now.AddDays(3) 
            },
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "6", 
                EventDescription = new ContentViewModel().AddUnderlinedText("Trigger that event on an element."), 
                Title = "keyup pause event", 
                Start = DateTime.Now.AddDays(8), 
                End = DateTime.Now.AddDays(8).AddHours(1) 
            },
            new CategoryItemViewModel() {
                AllDay = true, 
                Id = "7", 
                EventDescription = new ContentViewModel().AddEmphasisText("This is emphasis text."), 
                Title = "Pause Event", 
                Start = DateTime.Now.AddDays(8), 
                End = DateTime.Now.AddDays(8) ,
                IsEditable = false
            },
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "8", 
                EventDescription = new ContentViewModel().AddText("EVENT WITH ICON.").AddIcon(IconType.Android), 
                Title = "Iconic event", 
                Start = DateTime.Now.AddDays(8), 
                End = DateTime.Now.AddDays(8).AddHours(3) 
            }
            
        };


        /// <summary>
        /// List containing items for 'Q&A' category.
        /// </summary>
        private static IList<CategoryItemViewModel> qAndaCategoryList = new List<CategoryItemViewModel>()
        {
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "9", 
                EventDescription = new ContentViewModel().AddText("One Hour Q&A with Jobseekers"), 
                Title = "Q&A with Jobseekers", 
                Start = DateTime.Now.AddDays(1), 
                End = DateTime.Now.AddDays(1).AddHours(1) 
            },
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "10", 
                EventDescription = new ContentViewModel().AddText("Next YEar's event. Q&A with Employers Q&A with Employers Q&A with Employers"), 
                Title = "Q&A with Employers", 
                Start = DateTime.Now.AddYears(1), 
                End = DateTime.Now.AddYears(1).AddHours(2) 
            },
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "11", 
                EventDescription = new ContentViewModel().AddText("2 Hour Q&A Jobseekers"), 
                Title = "Q&A", 
                Start = DateTime.Now.AddDays(9), 
                End = DateTime.Now.AddDays(9).AddHours(2) 
            },
            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "12", 
                EventDescription = new ContentViewModel().AddText("Discussion"), 
                Title = "Chat with Employers", 
                Start = DateTime.Now.AddDays(1), 
                End = DateTime.Now.AddDays(1).AddHours(3) 
            },

            new CategoryItemViewModel() {
                AllDay = false, 
                Id = "13", 
                EventDescription = new ContentViewModel().AddText("4 Hour Q&A with Jobseekers"), 
                Title = "Q&A with Jobseekers", 
                Start = DateTime.Now.AddDays(2), 
                End = DateTime.Now.AddDays(2).AddHours(4) 
            } 
        };

        

        /// <summary>
        /// Index Menu to return the calendar view with events populated on it.
        /// </summary>
        /// <returns></returns>
        [Menu("Calendar", Order = 1, ParentAction = "Index")]
        public ActionResult CalendarIndex()
        {
            CalendarIndexViewModel model = new CalendarIndexViewModel();

            model.Calendar = new Infrastructure.ViewModels.Calendar.CalendarViewModel(Infrastructure.Types.Calendar.DefaultView.Month) { CalendarName = "Appointments" };
            model.Calendar.Categories = new List<CategoryViewModel>();


            var meetingCategory = new CategoryViewModel("Meetings with JobSeeker", "DisplayForm")
            {
                Description = new Infrastructure.ViewModels.ContentViewModel().AddText("This category is for something...."), 
                Area = "Example", 
                Color = Infrastructure.Types.ColourType.Blue,
                IconType = Infrastructure.Types.IconType.User
            };
            meetingCategory.Items = meetingCategoryEventsList;
            model.Calendar.Categories.Add(meetingCategory);

            var eventCategory = new CategoryViewModel("Event", "DisplayFormEvent")
            {
                Description = new Infrastructure.ViewModels.ContentViewModel().AddText("This is description of category."),
                Area = "Example",
                DragResizeAction = "UpdateEventTimes",
                Color = Infrastructure.Types.ColourType.Green,
                IconType = Infrastructure.Types.IconType.CalendarO
            };
            eventCategory.Items = eventsCategoryList;
            model.Calendar.Categories.Add(eventCategory);

            var qAndaCategory = new CategoryViewModel("Q and A", "DisplayFormQA")
            {
                Description = new Infrastructure.ViewModels.ContentViewModel().AddText("Q & A"), 
                Area = "Example", 
                Color = Infrastructure.Types.ColourType.Orange,
                IconType = Infrastructure.Types.IconType.Question
            };
            qAndaCategory.Items = qAndaCategoryList;
            model.Calendar.Categories.Add(qAndaCategory);

            return View(model);
        }


        /// <summary>
        /// Initiates a view model for creating new event of type 'MeetingWithJObseeker'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        public ActionResult DisplayForm(string id)
        {
            var model = new AppointmentViewModel(); // inherits CategoryItemViewModel or an interface 
            model.Heading = new ContentViewModel().AddTitle("Add new 'Appointment with Jobseeker' event.");
            if (!string.IsNullOrEmpty(id))
            {
                // populate event details here for this event ID.
                var categoryItemViewModel = GetAppointment(id);
                if (categoryItemViewModel != null)
                {
                    model = categoryItemViewModel.ToAppointmentViewModel();
                    model.Heading = new ContentViewModel().AddTitle("Edit 'Appointment with Jobseeker' event ID: " + model.Id);
                }                
            }
            return PartialView("EditorTemplates/Object", model);
        }


        /// <summary>
        /// Display Form Post action ajax.
        /// </summary>
        /// <param name="model"></param>
        /// If <paramref name="model"/> is empty or null, returns empty content so user will be redirected to Index action from javascript.
        /// If ModelState is invalid, or valid then returns PartialView("EditorTemplates/Object", model).
        [HttpPost]
        [AjaxOnly]
        public ActionResult DisplayForm(AppointmentViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                // TODO: If everything is valid, then add the event in database then redirect to Index() action which will populate this newly created event inside of calendar.
                // else if something has failed then redirect this partial view.

                if (string.IsNullOrEmpty(model.Id))
                {
                    // Add new event.
                    meetingCategoryEventsList.Add
                        (new CategoryItemViewModel()
                            {
                                Id = new Random().Next(20, 1000).ToString(),
                                EventDescription = new ContentViewModel().AddText(model.Description),
                                Title = model.Title,
                                Start = model.Start,
                                End = model.End,
                            }
                        );
                }
                else
                {
                    // Edit existing event. 
                    var eventToUpdate = GetAppointment(model.Id);
                    if (eventToUpdate != null)
                    {
                        eventToUpdate = model.ToCategoryItemViewModel(eventToUpdate);
                        meetingCategoryEventsList.Remove(eventToUpdate);
                        meetingCategoryEventsList.Add(eventToUpdate);
                    }
                }

                model.ModelStateIsValid = true; 
                 
            }
            else if (model == null)
            {
                AddErrorMessage("Model is null.");
                return Content(string.Empty);
            }
            return PartialView("EditorTemplates/Object", model);
            
        }


        /// <summary>
        /// Initiates the <see cref="EventViewModel"/> to be displayed for event category.
        /// </summary>
        /// <param name="id">ID of the event, if present.</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult DisplayFormEvent(string id)
        {

            var model = new EventViewModel(); // inherits CategoryItemViewModel or an interface
            model.Heading = new ContentViewModel().AddTitle("Add new 'Event'.");
            if (!string.IsNullOrEmpty(id))
            {
                // populate event details here for this event ID.
                var categoryItemViewModel = GetAppointment(id);
                if (categoryItemViewModel != null)
                {
                    model = categoryItemViewModel.ToEventViewModel();
                    model.Heading = new ContentViewModel().AddTitle("Edit 'Event' {0}.", id);
                }
            }
            return PartialView("EditorTemplates/Object", model);

        }


        /// <summary>
        /// Post event for category Event.
        /// </summary>
        /// <remarks>
        /// Date validation occurs at client side.
        /// </remarks>
        /// <param name="model"><see cref="EventViewModel"/> instance.</param>
        /// <returns>
        /// If <paramref name="model"/> is empty or null, returns empty content so user will be redirected to Index action from javascript.
        /// If ModelState is invalid, or valid then returns PartialView("EditorTemplates/Object", model).
        /// </returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DisplayFormEvent(EventViewModel model)
        {
            // If everything is valid, then set ModelStateIsValid = true; so javaScript will redirect to Index() action which will populate this newly created event inside of calendar.
            // else if something has failed, Add error message 
            // In both cases redirect to partial view.

            if (model != null && ModelState.IsValid)
            { 
                if (model.End.HasValue && model.Start.HasValue && DateTime.Compare(model.Start.Value,  model.End.Value) <= 0)
                {

                    if (string.IsNullOrEmpty(model.Id))
                    {
                        // Add new event.
                        eventsCategoryList.Add
                            (new CategoryItemViewModel()
                            {
                                Id = new Random().Next(20, 1000).ToString(),
                                EventDescription = new ContentViewModel().AddText(model.Description),
                                Title = model.Title,
                                Start = model.Start,
                                End = model.End,
                                AllDay = model.AllDay
                            }
                            );
                    }
                    else
                    {
                        // Edit existing event. 

                        var eventToUpdate = GetAppointment(model.Id);
                        if (eventToUpdate != null)
                        {
                            eventToUpdate = model.ToCategoryItemViewModel(eventToUpdate);
                            eventsCategoryList.Remove(eventToUpdate);
                            eventsCategoryList.Add(eventToUpdate);
                        }
                    }
                    model.ModelStateIsValid = true;
                }
                else
                {
                    AddErrorMessage("End", "End Session must occur later than Start Session.");
                }               
            }
            else if (model == null)
            {
                AddErrorMessage("Model is null.");
                return Content(string.Empty);
            }
            return PartialView("EditorTemplates/Object", model);
        }



        /// <summary>
        /// Initiates the <see cref="QAViewModel"/> to be displayed for Q&A category.
        /// </summary>
        /// <param name="id">ID of the event, if present.</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult DisplayFormQA(string id)
        {

            var model = new QAViewModel(); // inherits CategoryItemViewModel or an interface
            model.Heading = new ContentViewModel().AddTitle("Add new 'Q & A' event.");
            if (!string.IsNullOrEmpty(id))
            {
                // populate event details here for this event ID.
                var categoryItemViewModel = GetAppointment(id);
                if (categoryItemViewModel != null)
                {
                    model = categoryItemViewModel.ToQAViewModel();
                    model.Heading = new ContentViewModel().AddTitle("Edit 'Q & A' {0}.", id);
                }
            }
            return PartialView("EditorTemplates/Object", model);

        }


        /// <summary>
        /// Post event for category Event.
        /// </summary>
        /// <remarks>
        /// Date validation occurs at server side.
        /// </remarks>
        /// <param name="model"><see cref="Q&AViewModel"/> instance.</param>
        /// If <paramref name="model"/> is empty or null, returns empty content so user will be redirected to Index action from javascript.
        /// If ModelState is invalid, or valid then returns PartialView("EditorTemplates/Object", model).
        [HttpPost]
        [AjaxOnly]
        public ActionResult DisplayFormQA(QAViewModel model, string submitType)
        {
            // If everything is valid, then set ModelStateIsValid = true; so javaScript will redirect to Index() action which will populate this newly created event inside of calendar.
            // else if something has failed, Add error message 
            // In both cases redirect to partial view.

            if (model != null && ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(model.Id) && !string.IsNullOrEmpty(submitType) && submitType.Equals(BasicSubmitType.DeleteEvent))
                {
                    // TODO: process deletion of event here...
                    qAndaCategoryList.Remove(qAndaCategoryList.FirstOrDefault(x=>x.Id.Equals(model.Id)));
                    model.ModelStateIsValid = true;
                    AddSuccessMessage("Event deleted.");
                }

                else if (model.End.HasValue && model.Start.HasValue && DateTime.Compare(model.Start.Value, model.End.Value) <= 0)
                {

                    if (string.IsNullOrEmpty(model.Id))
                    {
                        // Add new event.
                        qAndaCategoryList.Add
                            (new CategoryItemViewModel()
                            {
                                Id = new Random().Next(20, 1000).ToString(),
                                EventDescription = new ContentViewModel().AddText(model.Description),
                                Title = model.Title,
                                Start = model.Start,
                                End = model.End,
                                AllDay = model.AllDay
                            }
                            );
                    }
                    else
                    {
                        // Edit existing event. 

                        var eventToUpdate = GetAppointment(model.Id);
                        if (eventToUpdate != null)
                        {
                            eventToUpdate = model.ToCategoryItemViewModel(eventToUpdate);
                            qAndaCategoryList.Remove(eventToUpdate);
                            qAndaCategoryList.Add(eventToUpdate);
                        }
                        AddSuccessMessage("Event updated.");
                    }
                    model.ModelStateIsValid = true;
                }
                else
                {
                    AddErrorMessage("End", "End Session must occur later than Start Session.");
                }
                
            }
            else if (model == null)
            {
                AddErrorMessage("Model is null.");
                return Content(string.Empty);
            }
            
            return PartialView("EditorTemplates/Object", model);
        }



        /// <summary>
        /// This action is responsible for handling the Resize and Drag Drop events of calendar. Only for category: 'Event'.
        /// </summary>
        /// <param name="id">ID of the event on which the drag/resize has been performed.</param>
        /// <param name="start">New Start time.</param>
        /// <param name="end">New End time.</param>
        /// <returns><c>True</c> if the backend updated was successful, otherwise <c>false</c>.</returns>
        [HttpPost]
        [AjaxOnly]        
        public bool UpdateEventTimes(string id, DateTime? start, DateTime? end)
        {
            var isValid = false;

            var eventInContext = eventsCategoryList.FirstOrDefault(x => x.Id.Equals(id));

            if(eventInContext != null && (start.HasValue || end.HasValue))
            {
                eventInContext.Start = start.HasValue ? start : eventInContext.Start;
                eventInContext.End = end.HasValue ? end : eventInContext.End;

                isValid = true;
            }
            return isValid;             
        }


        private CategoryItemViewModel GetAppointment(string id)
        {
            var eventToUpdate = meetingCategoryEventsList.FirstOrDefault(x => x.Id.Equals(id));
            if (eventToUpdate == null)
            {
                eventToUpdate = eventsCategoryList.FirstOrDefault(x => x.Id.Equals(id));
            }
            if (eventToUpdate == null)
            {
                eventToUpdate = qAndaCategoryList.FirstOrDefault(x => x.Id.Equals(id));
            }

            return eventToUpdate;
        }



        #endregion

    }
}