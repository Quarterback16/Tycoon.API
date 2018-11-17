using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
	/// <summary>
	/// Defines the Grid controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class PagedGridController : InfrastructureController
    {
	    private readonly IDummyService DummyService;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="DefaultController" /> class.
	    /// </summary>
	    /// <param name="dummyService"> </param>
	    /// <param name="userService">User service for retrieving user data.</param>
	    /// <param name="adwService">Adw service for retrieving ADW data.</param>
	    public PagedGridController(IDummyService dummyService,  IUserService userService, IAdwService adwService) : base( userService, adwService)
        {
            if (dummyService == null)
            {
                throw new ArgumentNullException("dummyService");
            }

            DummyService = dummyService;
        }

        #region Paging (full set)

        /// <summary>
        /// Demonstrates how to do paging in a grid for a full set of data.
        /// </summary>
        [Menu("Paging (full set)", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult DummiesAll(long? DummyID)
        {

            // Create view model with search form
            var model = new DummiesAllViewModel();

            if( DummyID != null)
            {
                AddSuccessMessage("id{0} successful.", DummyID.ToString());
            }
            else
            {
                AddWarningMessage(" Warning.");
                AddWarningMessage(" Warning.");
                AddErrorMessage(" Error.");
                AddErrorMessage(" Warning.");
                AddInformationMessage(" Warning.");
                AddSuccessMessage(" Warning.");
            }

            return View(model);
        }
         
         
        /// <summary>
        /// Demonstrates how to do paging in a grid for a full set of data.
        /// </summary>
        /// <remarks>
        /// Remember to take a look at the mappings in <see cref="ExampleMapper" /> to see how they're setup for use by this action.
        /// </remarks>
        [HttpPost]
        public ActionResult DummiesAll(DummiesAllViewModel model, string submitType)
        {
            if (ModelState.IsValid)
            {
                // Execute search only when that action is selected
                if (String.Compare(submitType, "Search", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // Get page metadata from view model
                    var metadata = ExampleMapper.MapToDummiesAllPageMetadata(model);//   MappingEngine.Map<DummiesAllPageMetadata>(model);

                    // Use page metadata to retrieve data via service
                    var domainModel = DummyService.FindAll(metadata.StartsWith);

                    // Populate grid property with data and page metadata
                    // Note that in the mapping, both the source and destination types are specified,
                    // and a new Pageable instance (with page metadata) is created for use as the destination object (so page metadata is kept)
                    
                    model.Dummies = ExampleMapper.ToPageableDummyViewModel(domainModel, new Pageable<DummyViewModel>(metadata));// MappingEngine.Map<IEnumerable<DummyModel>, IPageable<DummyViewModel>>(domainModel, new Pageable<DummyViewModel>(metadata));
                }
                else
                {
                    // Get the selected item
                    var selected = model.Dummies.FirstOrDefault(m => m.DummyID == model.SelectedKey);

                    if (selected != null)
                    {
                        // Display its information
                        model.Content = new ContentViewModel()
                            .AddTitle(selected.Name)
                            .AddParagraph(selected.Description);
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Demonstrates how to do paging in a grid for a full set of data.
        /// </summary>
        /// <remarks>
        /// This action shows the use of the page metadata for retrieving and returning the next page.
        /// Note that this is decorated with the <see cref="AjaxOnlyAttribute" />.
        /// Remember to take a look at the mappings in <see cref="ExampleMapper" /> to see how they're setup for use by this action.
        /// </remarks>
        /// <param name="metadata">The page metadata required for retrieving the next page.</param>
        /// <returns>The next page.</returns>
        [AjaxOnly]
        public ActionResult DummiesAllNextPage(DummiesAllPageMetadata metadata)
        {
            // Use page metadata to retrieve data via service
            var domainModel = DummyService.FindAll(metadata.StartsWith);

            // Populate grid property with data and page metadata
            // Note that in the mapping, both the source and destination types are specified,
            // and a new Pageable instance (with page metadata) is created for use as the destination object (so page metadata is kept)
            var data = ExampleMapper.ToPageableDummyViewModel(domainModel, new Pageable<DummyViewModel>(metadata)); // MappingEngine.Map<IEnumerable<DummyModel>, IPageable<DummyViewModel>>(domainModel, new Pageable<DummyViewModel>(metadata));
            
            // Note that we are returning PagedView instead of View
            return PagedView(data);
        }

        #endregion

        #region Paging (mainframe)

        /// <summary>
        /// Demonstrates how to do paging in a grid for data paged by mainframe.
        /// </summary>
        [Menu("Paging (mainframe)", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult DummiesMainframe()
        {
            // Create view model with search form
            var model = new DummiesMainframeViewModel();

            return View(model);
        }

        /// <summary>
        /// Demonstrates how to do paging in a grid for data paged by mainframe.
        /// </summary>
        /// <remarks>
        /// Remember to take a look at the mappings in <see cref="ExampleMapper" /> to see how they're setup for use by this action.
        /// </remarks>
        [HttpPost]
        public ActionResult DummiesMainframe(DummiesMainframeViewModel model, string submitType)
        {
            if (ModelState.IsValid)
            {
                // Execute search only when that action is selected
                if (String.Compare(submitType, "Search", StringComparison.OrdinalIgnoreCase)==0)
                {
                    // Get page metadata from view model
                    var metadata = ExampleMapper.MapToDummiesMainframePageMetadata(model); // MappingEngine.Map<DummiesMainframePageMetadata>(model);

                    // Use page metadata to retrieve data via service
                    var domainModel = DummyService.Find(metadata.StartsWith, metadata.NextSequenceID);

                    // Note when mainframe is paging, we need to update the page metadata that is specifically used by mainframe for paging
                    ExampleMapper.Map(domainModel, metadata);
                    //metadata = //   MappingEngine.Map<DummiesModel, DummiesMainframePageMetadata>(domainModel, metadata);

                    // Populate grid property with data and page metadata
                    // Note that in the mapping, both the source and destination types are specified,
                    // and a new Pageable instance (with page metadata) is created for use as the destination object (so page metadata is kept)
                    model.Dummies = ExampleMapper.ToPageableDummyViewModel(domainModel.Dummies, new Pageable<DummyViewModel>(metadata));//  MappingEngine.Map<IEnumerable<DummyModel>, IPageable<DummyViewModel>>(domainModel.Dummies, new Pageable<DummyViewModel>(metadata));
                }
                else
                {
                    // Get the selected item
                    var selected = model.Dummies.FirstOrDefault(m => m.DummyID == model.SelectedKey);

                    if (selected != null)
                    {
                        // Display its information
                        model.Content = new ContentViewModel()
                            .AddTitle(selected.Name)
                            .AddParagraph(selected.Description);
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Demonstrates how to do paging in a grid for a full set of data.
        /// </summary>
        /// <remarks>
        /// This action shows the use of the page metadata for retrieving and returning the next page.
        /// Note that this is decorated with the <see cref="AjaxOnlyAttribute" />.
        /// Remember to take a look at the mappings in <see cref="ExampleMapper" /> to see how they're setup for use by this action.
        /// </remarks>
        /// <param name="metadata">The page metadata required for retrieving the next page.</param>
        /// <returns>The next page.</returns>
        [AjaxOnly]
        public ActionResult DummiesMainframeNextPage(DummiesMainframePageMetadata metadata)
        {
            // Use page metadata to retrieve data via service
            var domainModel = DummyService.Find(metadata.StartsWith, metadata.NextSequenceID);

            // Note when mainframe is paging, we need to update the page metadata that is specifically used by mainframe for paging
            //metadata = ExampleMapper.MapToDummiesMainframePageMetadata(domainModel, metadata);// MappingEngine.Map<DummiesModel, DummiesMainframePageMetadata>(domainModel, metadata);
            ExampleMapper.Map(domainModel, metadata);

            // Populate grid property with data and page metadata
            // Note that in the mapping, both the source and destination types are specified,
            // and a new Pageable instance (with page metadata) is created for use as the destination object (so page metadata is kept)
            var data = ExampleMapper.ToPageableDummyViewModel(domainModel.Dummies, new Pageable<DummyViewModel>(metadata));// MappingEngine.Map<IEnumerable<DummyModel>, IPageable<DummyViewModel>>(domainModel.Dummies, new Pageable<DummyViewModel>(metadata));

            // Note that we are returning PagedView instead of View
            return PagedView(data);
        }

        #endregion


        #region SORTING Grid with Paging


        /// <summary>
        /// Demonstrates sorting function for Grid / search results.
        /// </summary>
        /// <returns></returns>
        [Menu("Grid Sorting", ParentAction="Index", ParentController = "Tables")]
        public ActionResult GridSorting()
        {

            PageTitle = "Sorts the Grid.";

            var model = new SortingViewModel();

            return View(model);
        }



        [HttpPost]
        public ActionResult GridSorting(SortingViewModel model, string submitType)
        {
            //if (model.DummyID.HasValue)
            //    model.Content = new ContentViewModel().AddTitle(dummyID.Value.ToString()).AddText("you clicked " + dummyID.Value.ToString());

            if(ModelState.IsValid)
            {

                if("Search".Equals(submitType, StringComparison.OrdinalIgnoreCase))
                {
                    // Get PageMetadata from view model.
                    SortingGridMetadata metadata = ExampleMapper.MapToSortingGridMetadata(model);//   MappingEngine.Map<SortingGridMetadata>(model);

                    // Use this page metadata to retrieve data via service.
                    IEnumerable<SortModel> results = DummyService.GetAllForSorting(metadata.Name);

                    // Map these results for paging.
                    // Populate Grid property with data and pageMetata.
                    // NOTE: Mapping contains both Source and Destination types. New Pageable instance is created with page Metadata for use with the destination object, to keep pageMetadata.
                    model.Results = results.ToGridSortingViewModelList(new Pageable<GridSortingViewModel>(metadata));// MappingEngine.Map<IEnumerable<SortModel>, IPageable<GridSortingViewModel>>(results, new Pageable<GridSortingViewModel>(metadata));
                }
                else
                {
                    var selectedValues = model.Results.FirstOrDefault(x => x.SortingID == model.SelectedKeys);
                    if(selectedValues != null)
                    {
                        model.SelectedContent = new ContentViewModel().AddTitle("Selected").AddParagraph(selectedValues.Address);
                    }
                }
                
            }
            return View(model);
        }


        #endregion


        [Menu("Post-Redirect-Get")]
        public ActionResult PRG()
        {
            return RedirectToAction("WithPRG");
        }

        #region With Post-Redirect-Get

        [Menu("Following pattern (route)", ParentAction = "PRG")]
        public ActionResult WithPRG(long? id)
        {
            var model = new PRGViewModel();

            if (id.HasValue)
            {
                var dummy = DummyService.Get(id.Value);

                model.DummyID = dummy.DummyID;
                model.Name = dummy.Name;
                model.Date = dummy.Date;
                model.EmailAddress = dummy.EmailAddress;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult WithPRG(PRGViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform update
                if (model.DummyID.HasValue)
                {
                    DummyService.Edit(model.DummyID.Value, model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Edited successfully");
                }
                else
                {
                    model.DummyID = DummyService.Add(model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Added successfully");
                }

                // Follow Post-Redirect-Get pattern by redirecting to GET after successful POST update
                // Pass reload details via route for reloading in GET (this is always the preferred approach)
                return RedirectToAction("WithPRG", new { id = model.DummyID });
            }

            return View(model);
        }

        [Menu("Following pattern (TempData)", ParentAction = "PRG")]
        public ActionResult WithPRGandTempData(long? id)
        {
            var model = new PRGViewModel();

            if (!id.HasValue)
            {
                id = TempData.Get<long?>("PRG");
            }

            if (id.HasValue)
            {
                var dummy = DummyService.Get(id.Value);

                model.DummyID = dummy.DummyID;
                model.Name = dummy.Name;
                model.Date = dummy.Date;
                model.EmailAddress = dummy.EmailAddress;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult WithPRGandTempData(PRGViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform update
                if (model.DummyID.HasValue)
                {
                    DummyService.Edit(model.DummyID.Value, model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Edited successfully");
                }
                else
                {
                    model.DummyID = DummyService.Add(model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Added successfully");
                }

                // Follow Post-Redirect-Get pattern by redirecting to GET after successful POST update
                // Passing reload details via TempData.Set<T> for reloading in GET
                // (since the reload details is just an id, you should actually do this via the route approach - using TempData for the sake of the example) 
                // NOTE: You should only use TempData when there are a lot of reload details to pass that aren't suitable for passing via route
                // NOTE: Only store the data necessary for reloading the object in TempData after the redirect - Don't store the object itself that you plan to reload
                TempData.Set<long?>("PRG", model.DummyID);

                return RedirectToAction("WithPRGandTempData");
            }

            return View(model);
        }

        #endregion

        #region Without Post-Redirect-Get

        [Menu("Not following pattern", ParentAction = "PRG")]
        public ActionResult WithoutPRG(long? id)
        {
            var model = new PRGViewModel();

            if (id.HasValue)
            {
                var dummy = DummyService.Get(id.Value);

                model.DummyID = dummy.DummyID;
                model.Name = dummy.Name;
                model.Date = dummy.Date;
                model.EmailAddress = dummy.EmailAddress;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult WithoutPRG(PRGViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform update
                if (model.DummyID.HasValue)
                {
                    DummyService.Edit(model.DummyID.Value, model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Edited successfully");
                }
                else
                {
                    model.DummyID = DummyService.Add(model.Name, model.Date, model.EmailAddress);
                    AddSuccessMessage("Added successfully");
                }

                // Not following Post-Redirect-Get often requires modifying the model state
                ModelState.Remove("DummyID");
            }

            return View(model);
        }

        #endregion
    }
}
