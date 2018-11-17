using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Area.Example.ViewModels.Workflow;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    /// <summary>
    /// Defines the WorkflowController controller.
    /// </summary>
    [Security(AllowAny = true)]
    public class WorkflowController : InfrastructureController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowController" /> class.
        /// </summary>
        /// <param name="mappingEngine">AutoMapper mapping engine for mapping between types.</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public WorkflowController( IUserService userService, IAdwService adwService) : base( userService, adwService) { }

        public ActionResult Index()
        {
            return RedirectToAction("Workflow");
        }

        /// <summary>
        /// Load workflow.
        /// </summary>
        [Menu("Workflow")]
        public ActionResult Workflow(long? id, int? step)
        {
            // Default right panel with information
            var model = new WorkflowViewModel() { ID = id, Details = new InheritanceViewModel() };

            if (id.HasValue)
            {
                model.Summary.AddTitle("Editing ID {0}", id.Value);
            }
            else
            {
                model.Summary.AddTitle("Creating a new record");
            }

            if (step.HasValue)
            {
                switch (step.Value)
                {
                    case 1:
                        // Prepopulate step if we're editing (have an ID), otherwise it will be set wuth an empty Step1ViewModel
                        model.Details = GetStep1(id);                        
                        break;
                    case 2:
                        // Prepopulate step if we're editing (have an ID), otherwise it will be set with an empty Step2ViewModel
                        model.Details = GetStep2(id);
                        break;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Handle submit of workflow step.
        /// </summary>
        /// <param name="model">The workflow model.</param>
        /// <param name="submitType">The submit type.</param>
        [HttpPost]
        public ActionResult Workflow(WorkflowViewModel model, string submitType)
        {
            if (ModelState.IsValid)
            {
                if (model.Details is Step1ViewModel)
                {
                    var m = model.Details as Step1ViewModel;

                    if (m != null)
                    {
                        // Update backend
                        // ...

                        // Follow Post-Redirect-Get
                        return RedirectToAction("Workflow", new { id = model.ID, step = 1 });
                    }
                }
                else if (model.Details is Step2ViewModel)
                {
                    var m = model.Details as Step2ViewModel;

                    if (m != null)
                    {
                        // Update backend
                        // ...

                        // Follow Post-Redirect-Get
                        return RedirectToAction("Workflow", new { id = model.ID, step = 2 });
                    }
                }

                AddInformationMessage(string.Format("Submitted '{0}' for ID '{1}'.", submitType, model.ID));
            }

            return View(model);
        }

        /// <summary>
        /// Load Information Step.
        /// </summary>
        /// <param name="id">The ID.</param>
        //[AjaxOnly]
        public ActionResult StepInfo(long? id)
        {
            var model = new StepInfoViewModel();
            System.Threading.Thread.Sleep(2000);
            return AjaxStepView("Workflow", model);
        }

        /// <summary>
        /// Load Step 1 based on ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        //[AjaxOnly]
        public ActionResult Step1(long? id)
        {
            var model = GetStep1(id);
            System.Threading.Thread.Sleep(2000);
            return AjaxStepView("Workflow", model);
        }

        /// <summary>
        /// Load Step 2 based on ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        //[AjaxOnly]
        public ActionResult Step2(long? id)
        {
            var model = GetStep2(id);
            System.Threading.Thread.Sleep(2000);
            return AjaxStepView("Workflow", model);
        }

        private Step1ViewModel GetStep1(long? id)
        {
            var model = new Step1ViewModel();

            if (id.HasValue)
            {
                // Load existing data from a Service via id
                // For the example, just populating directly here...
                model.Name = "Foo";
                model.OccupationLevel1 = "8";
                model.OccupationLevel2 = "82";
                model.OccupationLevel3 = "8212";
            }

            return model;
        }

        private Step2ViewModel GetStep2(long? id)
        {
            var model = new Step2ViewModel();

            if (id.HasValue)
            {
                // Load existing data from a Service via id
                // For the example, just populating directly here...
                model.EmailAddress = "foo@bar.com";
            }

            return model;
        }
    }
}
