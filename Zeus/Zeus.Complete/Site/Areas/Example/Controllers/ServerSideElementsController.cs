using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements;
using Department.AddressValidation;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;
using Employment.Web.Mvc.Infrastructure.Interfaces.JobSeeker; 
using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Area.Example.Models;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    [Security(AllowAny = true)]
    public class ServerSideElementsController : InfrastructureController
    {
        protected readonly IDummyService DummyService;

        protected readonly IJobseekerSearchService JobseekerSearchService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dummyService">Dummy service for retrieving example data</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public ServerSideElementsController(IDummyService dummyService, IUserService userService, IAdwService adwService, IJobseekerSearchService jobseekerSearchService)
            : base(userService, adwService)
		{
            if (dummyService == null)
            {
                throw new ArgumentNullException("dummyService");
            }

		    DummyService = dummyService;
            JobseekerSearchService = jobseekerSearchService;
		}

        [Menu("Server side controls", Order = 20)]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Server side controls examples")
                .AddParagraph("This area contains examples demonstrating how to use elements that obtain their data from the server after a page has loaded.");

            return View(model);
        }

        #region Adw selection
        /// <summary>
        /// Examples of selections automatically populated with choices on the server side using the AdwSelectionAttribute
        /// </summary>
        /// <returns></returns>
        [Menu("Adw selection", Order = 10, ParentAction = "Index")]
        public ActionResult AdwSelection()
        {
            var model = new AdwSelectionViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdwSelection(AdwSelectionViewModel model)
        {
            return View(model);
        }
        #endregion

        #region hierarchical adw selection

        /// <summary>
        /// An example of using the <see cref="AdwSelectionAttribute" />.
        /// </summary>
        [Menu("Hierarchical ADW selection", Order=20, ParentAction = "Index")]
        public ActionResult HierarchicalAdwSelection()
        {
            var model = new HierarchicalAdwSelectionViewModel();

            return View(model);
        }

        /// <summary>
        /// An example of using the <see cref="AdwSelectionAttribute" />.
        /// </summary>
        [HttpPost]
        public ActionResult HierarchicalAdwSelection(HierarchicalAdwSelectionViewModel model)
        {
            model.Selection = new ContentViewModel();
            if (!string.IsNullOrEmpty(model.OccupationLevel3))
            {
                model.Selection.AddParagraph("You selected occupation level 3 {0}.", model.OccupationLevel3);
            }
            else
            {
                model.Selection.AddParagraph("Please select an item in occupation level 3.");
            }

            return View(model);
        }

        /// <summary>
        /// Ajax method for <see cref="AjaxSelectionAttribute" /> property.
        /// </summary>
        /// <remarks>
        /// Although this Ajax method is using the <see cref="IAdwService" /> as its data source, it is just for the sake of the example. You would replace it with a call to your own RHEA Service. If you needed Adw data then you would use <see cref="AdwSelectionAttribute" /> instead.
        /// </remarks>
        [AjaxOnly]
        public JsonResult GetOccupationLevel1(string text, int page)
        {
            var result = AdwService.GetListCodes("AZ1").ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Ajax method for <see cref="AjaxSelectionAttribute" /> property.
        /// </summary>
        /// <remarks>
        /// Although this Ajax method is using the <see cref="IAdwService" /> as its data source, it is just for the sake of the example. You would replace it with a call to your own RHEA Service. If you needed Adw data then you would use <see cref="AdwSelectionAttribute" /> instead.
        /// </remarks>
        [AjaxOnly]
        public JsonResult GetOccupationLevel2(string text, int page, string occupationLevel1)
        {
            var result = AdwService.GetRelatedCodes("AZ12", occupationLevel1, true).ToCodeModelList().ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Ajax method for <see cref="AjaxSelectionAttribute" /> property.
        /// </summary>
        /// <remarks>
        /// Although this Ajax method is using the <see cref="IAdwService" /> as its data source, it is just for the sake of the example. You would replace it with a call to your own RHEA Service. If you needed Adw data then you would use <see cref="AdwSelectionAttribute" /> instead.
        /// </remarks>
        [AjaxOnly]
        public JsonResult GetOccupationLevel3(string text, int page, string occupationLevel2)
        {
            var result = AdwService.GetRelatedCodes("AZ24", occupationLevel2, true).ToCodeModelList().ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        #endregion

        #region ajax selection
        /// <summary>
        /// An example of using <see cref="AjaxSelectionAttribute" />.
        /// </summary>
        /// <remarks>
        /// Although the example is using the <see cref="IAdwService" /> as its data source, it is just for the sake of the example. You would replace it with a call to your own RHEA Service. If you needed Adw data then you would use <see cref="AdwSelectionAttribute" /> instead.
        /// </remarks>
        [Menu("Ajax selection", Order = 30, ParentAction = "Index")]
        public ActionResult AjaxSelection()
        {
            var model = new AjaxSelectionViewModel();

            model.OccupationLevel1 = AdwService.GetListCodes("AZ1").ToSelectList(m => m.Code, m => m.Description);

            return View(model);
        }

        /// <summary>
        /// An example of using <see cref="AjaxSelectionAttribute" />.
        /// </summary>
        [HttpPost]
        public ActionResult AjaxSelection(AjaxSelectionViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Ajax method for <see cref="AjaxSelectionAttribute" /> property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [AjaxOnly]
        public JsonResult GetAutoCompleteItems(string text, int page)
        {
            var result = AdwService.GetListCodes("AZ1").ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Ajax method for <see cref="AjaxSelectionAttribute" /> property.
        /// </summary>
        public JsonResult GetDummyID(string text, int page)
        {
            var result = DummyService.FindAll(string.Empty).ToSelectList(m => m.DummyID.ToString(), m => string.Format("{0} - {1}", m.DummyID.ToString(), m.Name));

            return AjaxSelectionView(text, page, result);
        }

        #endregion

        #region Address autocomplete

        [Menu("Address autocomplete", Order = 40, ParentAction = "Index")]
        public ActionResult AddressSelection()
        {
            var model = new AddressSelectionViewModel();
            model.ResidentialAddress.ReturnLatLongDetails = true;

            //var result = new JobSeekerSearchService().SearchJobSeeker();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddressSelection(AddressSelectionViewModel model)
        {
            model.AddressInfo.AddLineBreak()
                             .AddStrongText("Latitude: ").AddText(model.ResidentialAddress.Latitude)
                             .AddStrongText("Longitude: ").AddText(model.ResidentialAddress.Longitude)
                             .AddStrongText("Locality: ").AddText(model.ResidentialAddress.Locality)
                             .AddLineBreak()
                             .AddStrongText("State: ").AddText(model.ResidentialAddress.State)
                             .AddStrongText("Suburb: ").AddText(model.ResidentialAddress.Locality)
                             .AddStrongText("Postcode: ").AddText(model.ResidentialAddress.Postcode);
           

            return View(model);
        }

        [AjaxOnly]
        public JsonResult ValidateAddress(string text, int page)
        {
            if (text.Length < 7)
            {
                return AjaxSelectionView(text, page, null);
            }
            var addr = new Address(text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            var addrValidator = new AddressValidator();
            var validAddresses = addrValidator.ValidateAddress(addr);
            var data = validAddresses.ToSelectListItem(m => string.Format("{0}, {1}, {2}, {3}, {4}", m.AddressLine1, m.Suburb, m.State, m.Postcode, m.ReliabilityLevel), m => string.Format("{0}, {1}, {2}, {3}, {4}", m.AddressLine1, m.Suburb, m.State, m.Postcode, m.ReliabilityLevel));
            return AjaxSelectionView(string.Empty, page, data);
        }
        
        #endregion

        #region File download
        [Menu("File download", Order = 50, ParentAction = "Index")]
        public ActionResult FileDownload()
        {
            var model = new FileDownloadViewModel();
            return View(model);
        }

        [HttpPost]
        public FileStreamResult FileDownload(FileDownloadViewModel model)
        {
            string[] streets = new string[] { "Flower", "McManus", "Kingston", "Queen", "High", "Anzac", "Berrington", "Newberry" };
            string[] streetTypes = new string[] { "Street", "Place", "Crescent", "Parade", "Avenue" };
            string[] suburbs = new string[] { "Bonner", "Chifley", "Pyrmont", "Ryde", "Oxley", "Toorak", "Newtown" };
            string[] states = new string[] { "NSW", "ACT", "VIC", "QLD" };
            Random random = new Random();
            var addressList = new List<Address>();
            for (int i = 1; i < 10; i++)
            {
                var a1 = (random.Next(100) + 1).ToString();
                var a2 = streets[random.Next(streets.Length)];
                var a3 = streetTypes[random.Next(streetTypes.Length)];
                var suburb = suburbs[random.Next(suburbs.Length)];
                var state = states[random.Next(states.Length)];
                var postcode = (random.Next(9000) + 1000).ToString();
                addressList.Add(new Address(a1, a2, a3, suburb, state, postcode));
            }

            var fileStream = new MemoryStream();
            var sw = new StreamWriter(fileStream);
            var csv = new CsvWriter(sw);
            csv.WriteRecords(addressList);
            sw.Flush();
            fileStream.Seek(0, SeekOrigin.Begin);
            return File(fileStream, "text/csv", "Randomly generated addresses.csv");
            //return File(fileStream, "application/vnd.ms-excel", "Randomly generated addresses.xls"); // excel
        }
        #endregion

        #region job seeker single line

        /// <summary>
        /// Single-line search for jobseeker.
        /// </summary>
        /// <returns></returns>
        [Menu("Jobseeker single-line search", ParentAction = "Index", Order = 60)]
        [HttpGet]
        public ActionResult JobSeekerSearch()
        {
            var jobseekerSearchViewModel = new JobSeekerSearchExampleViewModel();

            return View(jobseekerSearchViewModel);

        }



        [HttpPost]
        public ActionResult JobSeekerSearch(JobSeekerSearchExampleViewModel model)
        {
            if (model != null && model.JobseekerSearch != null)
            {
                var list = JobseekerSearchService.SearchJobSeeker(ExampleMapper.MapToJobSeekerSearchModel(model.JobseekerSearch));
                if (list != null && list.Any())
                {

                    model.JobSeekerResults = new List<JobSeekerModelList>();

                    foreach (var record in list)
                    {
                        model.JobSeekerResults.Add(ExampleMapper.MapToJobSeekerModelList(record));
                    }
                }
                else
                {
                    AddInformationMessage("No records found.");
                }

            }
            return View(model);

        }

        #endregion

    }
}