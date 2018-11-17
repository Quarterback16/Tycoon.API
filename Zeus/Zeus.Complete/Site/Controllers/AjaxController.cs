using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Interfaces.Geospatial;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using Employment.Web.Mvc.Infrastructure.Interfaces.JobSeeker;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Zeus.Controllers
{
    [Security(AllowAny = true)]
    public class AjaxController : InfrastructureController
    {
        private readonly IAddressService AddressService;

        private readonly IJobseekerSearchService JobseekerSearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AjaxController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        /// <param name="addressService"></param>
        public AjaxController(IUserService userService, IAdwService adwService, IAddressService addressService, IJobseekerSearchService jobseekerSearchService)
            : base( userService, adwService)
        {
            AddressService = addressService;
            JobseekerSearchService = jobseekerSearchService;
        }

        /// <summary>
        /// Gets the next page of Adw list codes, optionally filtering to only those containing the text supplied by the user (if supplied).
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="code">Adw code.</param>
        /// <param name="orderType">The order type to display with.</param>
        /// <param name="displayType">The display type to indicate what Adw property to use for the display text.</param>
        /// <param name="excludeValues">The values to exclude from the Adw selection options.</param>
        /// <returns>The next page of Adw list codes.</returns>
        [AjaxOnly]
        [OutputCache(Duration=3600, VaryByParam="*")]
        public ActionResult ListCode(string text, int page, string code, AdwOrderType orderType, AdwDisplayType displayType, IEnumerable<string> excludeValues)
        {
            var result = Enumerable.Empty<SelectListItem>();

            if (!string.IsNullOrEmpty(code))
            {
                result = AdwService.GetListCodes(code).ToOrderedSelectListItem(orderType, displayType).Where(m => excludeValues == null || !excludeValues.Contains(m.Value));
            }

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Gets the next page of Adw related codes, optionally filtering to only those containing the text supplied by the user (if supplied).
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="code">Adw code.</param>
        /// <param name="dependentValue">The dependent value.</param>
        /// <param name="dominant">Whether this is a dominant lookup.</param>
        /// <param name="orderType">The order type to display with.</param>
        /// <param name="displayType">The display type to indicate what Adw property to use for the display text.</param>
        /// <param name="excludeValues">The values to exclude from the Adw selection options.</param>
        /// <returns>The next page of Adw related codes.</returns>
        [AjaxOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult RelatedCode(string text, int page, string code, string dependentValue, bool dominant, AdwOrderType orderType, AdwDisplayType displayType, IEnumerable<string> excludeValues)
        {
            var result = Enumerable.Empty<SelectListItem>();

            if (!string.IsNullOrEmpty(code))
            {
                result = AdwService.GetRelatedCodes(code, dependentValue, dominant).ToOrderedSelectListItem(orderType, displayType).Where(m => excludeValues == null || !excludeValues.Contains(m.Value));
            }

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Pins object to history.
        /// </summary>
        /// <param name="historyType">Type of the history.</param>
        /// <param name="values">The values necessary for loading the object as a query string.</param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult PinHistory(HistoryType historyType, string values)
        {
            UserService.History.Pin(historyType, values.FromQueryStringToDictionary());

            return null;
        }

        /// <summary>
        /// Unpins the recent history.
        /// </summary>
        /// <param name="historyType">Type of the history.</param>
        /// <param name="values">The values necessary for loading the object as a query string.</param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult UnpinHistory(HistoryType historyType, string values)
        {
            UserService.History.Unpin(historyType, values.FromQueryStringToDictionary());

            return null;
        }

        /// <summary>
        /// Retrieves the next recent history page
        /// </summary>
        /// <param name="metadata">The paging metadata.</param>
        /// <returns>Returns the html markup for the next page of recent history list items.</returns>
        [AjaxOnly]
        public ActionResult HistoryNextPage(HistoryPageMetadata metadata)
        {
            IEnumerable<HistoryModel> model = UserService.History.Get(metadata.HistoryType);

            Pageable<HistoryModel> data = model.ToHistoryModelList(new Pageable<HistoryModel>(metadata));

            return View(data);
        }

        /// <summary>
        /// Gets the possible matching addresses for the user supplied text.
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <returns>The possible matching address data.</returns>
        [AjaxOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult AddressSearch(string text, int page, bool returnLatLong)
        {
            var model = AddressService.Validate(text, returnLatLong);
            var data = model.ToAjaxAddressViewModelList();//  MappingEngine.Map<IEnumerable<AddressModel>, IEnumerable<AjaxAddressViewModel>>(model);

            // Return empty result if no data matches were found that contain a line 1
            if (data == null || !data.Any(d => !string.IsNullOrWhiteSpace(d.Line1)))
            {
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = Enumerable.Empty<AjaxAddressViewModel>(), more = false } };
            }

            var ajaxSelectionPageSize = 10;
            var totalCount = data.Count();
            var more = totalCount > 0 && (page * ajaxSelectionPageSize) < totalCount;
            data = data.Skip((page - 1) * ajaxSelectionPageSize).Take(ajaxSelectionPageSize);

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = data, more } };
        }

        /// <summary>
        /// Gets the Jobseeker records based on the supplied text.
        /// </summary>
        /// <param name="text">Search text.</param>
        /// <param name="page">Page number.</param>
        /// <returns>Jobseeker matching records.</returns>
        [AjaxOnly]
        public ActionResult JobseekerSearch(string text, int page)
        {
            var model = JobseekerSearchService.SearchJobSeeker(text);
            var data = model.ToAjaxJobseekerSearchViewModelList(); // map to AjaxJobseekerSearchViewModel.

            // Return empty result if no data matches were found
            if(data == null || !data.Any(m => !string.IsNullOrWhiteSpace(m.FirstName)))
            {
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = Enumerable.Empty<AjaxJobseekerSearchViewModel>(), more = false } };
            }

            var ajaxSelectionPageSize = 10;
            var total = data.Count();
            var more = total > 0 && (page * ajaxSelectionPageSize) < total;

            // page value starts at 1. so if page = 1, then we need to return first 10 records.
            data = data.Skip((page - 1) * ajaxSelectionPageSize).Take(ajaxSelectionPageSize);

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = data, more } };
        }

        /// <summary>
        /// Gets the State data.
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <returns>The state codes.</returns>
        [AjaxOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public JsonResult GetState(string text, int page)
        {
            var result = AdwService.GetListCodes("STT").ToSelectList(m => m.Code, m => m.Code);

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Gets the locality data based on Postcode.
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="postcode">The user supplied Postcode.</param>
        /// <returns>The locality data.</returns>
        [AjaxOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public JsonResult GetLocality(string text, int page, string postcode)
        {
            var result = AdwService.GetRelatedCodes("LPCC", postcode, true).ToCodeModelList().ToSelectList(m => m.Description, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        /// <summary>
        /// Gets the Postcode data based on State.
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="state">The user supplied State.</param>
        /// <returns>The postcode data.</returns>
        [AjaxOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public JsonResult GetPostcode(string text, int page, string state)
        {
            var result = AdwService.GetRelatedCodes("STPC", state, true).ToCodeModelList().ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }


        #region Widget handling
        [AjaxOnly]
        [HttpPost]
        public ActionResult AddWidget(string widgetName, string widgetContext)
        {
            WidgetViewModel model = WidgetViewModel.GetWidget(widgetName, widgetContext);
            if (model == null)
            {
                return HttpNotFound("The requested widget was not found");
            }
            UserService.Dashboard.AddWidgetName(widgetName, widgetContext);
            ViewData["WidgetContext"] = widgetContext;
            return PartialView("EditorTemplates/WidgetViewModel", model);
        }


        [AjaxOnly]
        [HttpPost]
        public ActionResult RemoveWidget(string widgetName, string widgetContext)
        {
            UserService.Dashboard.RemoveWidgetName(widgetName, widgetContext);
            return null;
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult SetWidgetLayout(string widgetLayout, string widgetContext)
        {
            UserService.Dashboard.SetWidgetLayout(widgetLayout, widgetContext);
            return null;
        }


        [AjaxOnly]
        [HttpPost]
        public ActionResult SetWidgetDataContext(string dataContext, string widgetContext)
        {
            UserService.Dashboard.SetDataContext(dataContext, widgetContext);
            return null;
        }

        #endregion

    }
}