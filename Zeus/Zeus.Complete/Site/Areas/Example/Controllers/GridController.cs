using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Employment.Web.Mvc.Area.Example.ViewModels.Grid;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Area.Example.Mappers;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
	/// <summary>
	/// Defines the Grid controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class GridController : InfrastructureController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public GridController( IUserService userService, IAdwService adwService) : base( userService, adwService) { }

        /// <summary>
        /// Display action with a read only Grid.
        /// </summary>
        /// <returns></returns>
        [Menu("Display only", ParentAction = "Index", ParentController="Tables")]
        public ActionResult Display()
        {
            var model = new DisplayViewModel();

            model.Claims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());//MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());

             

            return View(model);
        }

        /// <summary>
        /// Single action demonstrating single selection in a Grid.
        /// </summary>
        [Menu("Single selection", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult Single()
        {
            var model = new SingleViewModel();

            // Populate
            model.Claims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());//MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());
            
            return View(model);
        }

        /// <summary>
        /// Single post action demonstrating single selection in a Grid.
        /// </summary>
        [HttpPost]
        public ActionResult Single(SingleViewModel model)
        {
            // Repopulate grid
            model.Claims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());

            if (model.SelectedKey != null)
            {
                // Reapply row selections based on matching [Key]
                model.Claims.ForEach(c => { if (model.SelectedKey == c.HashKey) { c.Selected = true; } });

                // Get selected row
                var claim = model.Claims.FirstOrDefault(c => c.HashKey == model.SelectedKey);

                if (claim != null)
                {
                    model.Selections = new ContentViewModel().AddParagraph("You selected claim type '{0}' which has a value of '{1}'.", claim.ClaimType, claim.Value);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Multiple action demonstrating single selection in a Grid.
        /// </summary>
        [Menu("Multiple selection", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult Multiple()
        {
            var model = new MultipleViewModel();

            // Populate
            model.Claims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());//MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());
            
            return View(model);
        }

        /// <summary>
        /// Multiple post action demonstrating single selection in a Grid.
        /// </summary>
        [HttpPost]
        public ActionResult Multiple(MultipleViewModel model)
        {
            // Get selected rows
            var selected = model.Claims.Where(m => m.Selected);

            // Repopulate grid
            model.Claims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());

            // Reapply row selections based on matching [Key]
            model.Claims.ForEach(c => { if (selected.Any(s => s.HashKey == c.HashKey)) { c.Selected = true; } });

            // Process selected items
            if (selected.Any())
            {
                model.Selections = new ContentViewModel();

                foreach (var claim in model.Claims.Where(m => m.Selected))
                {
                    model.Selections.AddParagraph("You selected claim type '{0}' which has a value of '{1}'.", claim.ClaimType, claim.Value);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Combination action demonstrating different selection type grids together.
        /// </summary>
        [Menu("Combination selection", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult Combination()
        {
            var model = new CombinationViewModel();
            
            // Populate
            model.MultipleClaims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());//  MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());
            model.SingleClaims1 = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());//MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());
            model.SingleClaims2 = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());// MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());
            model.DisplayClaims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());// MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable());

            return View(model);
        }

        /// <summary>
        /// Combination action demonstrating different selection type grids together.
        /// </summary>
        [HttpPost]
        public ActionResult Combination(CombinationViewModel model)
        {
            // Process selected items
            model.Selections = new ContentViewModel();

            model.Selections.AddTitle("Selections from grid allowing multiple selection.");

            // Get selected rows
            var selectedMultipleClaims = model.MultipleClaims.Where(m => m.Selected);

            // Repopulate grid
            model.MultipleClaims = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());

            // Reapply row selections based on matching [Key]
            model.MultipleClaims.ForEach(c => { if (selectedMultipleClaims.Any(s => s.HashKey == c.HashKey)) { c.Selected = true; } });

            if (model.MultipleClaims.Any(m => m.Selected))
            {
                foreach (var claim in model.MultipleClaims.Where(m => m.Selected))
                {
                    model.Selections.AddParagraph("You selected claim type '{0}' which has a value of '{1}'.", claim.ClaimType, claim.Value);
                }
            }
            else
            {
                model.Selections.AddParagraph("None.");
            }

            model.Selections.AddTitle("Selections from first grid allowing single selection.");

            // Repopulate grid
            model.SingleClaims1 = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());

            if (model.SelectedSingleClaims1Key != null)
            {
                // Reapply row selections based on matching [Key]
                model.SingleClaims1.ForEach(c => { if (model.SelectedSingleClaims1Key == c.HashKey) { c.Selected = true; } });

                // Get selected row
                var singleClaim1 = model.SingleClaims1.SingleOrDefault(m => m.HashKey == model.SelectedSingleClaims1Key);

                if (singleClaim1 != null)
                {
                    model.Selections.AddParagraph("You selected claim type '{0}' which has a value of '{1}'.", singleClaim1.ClaimType, singleClaim1.Value);
                }
            }
            else
            {
                model.Selections.AddParagraph("None.");
            }

            model.Selections.AddTitle("Selections from second grid allowing single selection.");

            // Repopulate grid
            model.SingleClaims2 = ExampleMapper.ToClaimViewModelList(UserService.Identity.Claims.AsEnumerable());

            if (model.SelectedSingleClaims2Key != null)
            {
                // Reapply row selections based on matching [Key]
                model.SingleClaims2.ForEach(c => { if (model.SelectedSingleClaims2Key == c.HashKey) { c.Selected = true; } });

                // Get selected row
                var singleClaim2 = model.SingleClaims2.SingleOrDefault(m => m.HashKey == model.SelectedSingleClaims2Key);

                if (singleClaim2 != null)
                {
                    model.Selections.AddParagraph("You selected claim type '{0}' which has a value of '{1}'.", singleClaim2.ClaimType, singleClaim2.Value);
                }
            }
            else
            {
                model.Selections.AddParagraph("None.");
            }

            return View(model);
        }

        /// <summary>
        /// Button action demonstrating inline button selection in a Grid.
        /// </summary>
        [Menu("Inline button selection", ParentAction = "Index", ParentController = "Tables")]
        public ActionResult Button()
        {
            var model = new ButtonViewModel();

            // Populate
            model.Claims = ExampleMapper.ToClaimWithButtonsViewModelList(UserService.Identity.Claims.AsEnumerable());//MappingEngine.Map<IEnumerable<ClaimWithButtonsViewModel>>(UserService.Identity.Claims.AsEnumerable());

            return View(model);
        }

        /// <summary>
        /// Button action demonstrating inline button selection in a Grid.
        /// </summary>
        [HttpPost]
        public ActionResult Button(ButtonViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Button action demonstrating inline button selection in a Grid.
        /// </summary>
        public ActionResult ButtonEdit(ButtonEditViewModel model)
        {
            if (model == null)
            {
                model = new ButtonEditViewModel();
                model.Message = new ContentViewModel().AddTitle("You haven't selected a claim to edit.");
            }
            else
            {
                var message = new ContentViewModel().AddTitle("You selected to edit hash key {0}.", model.HashKey);

                //ClaimWithButtonsViewModel claim = MappingEngine.Map<IEnumerable<ClaimWithButtonsViewModel>>(UserService.Identity.Claims.AsEnumerable()).FirstOrDefault(m => m.HashKey == model.HashKey);
                ClaimWithButtonsViewModel claim = (ExampleMapper.ToClaimWithButtonsViewModelList(UserService.Identity.Claims.AsEnumerable())).FirstOrDefault(m => m.HashKey == model.HashKey);

                if (claim != null)
                {
                    model = ExampleMapper.ToButtonEditViewModel(claim); //MappingEngine.Map<ButtonEditViewModel>(claim);
                }

                model.Message = message;
            }

            return View(model);
        }
    }
}
