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
using Employment.Web.Mvc.Area.Example.ViewModels.Validation;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    [Security(AllowAny = true)]
    public class ValidationController : InfrastructureController
    {
        public ValidationController(IUserService userService, IAdwService adwService) : base( userService, adwService)
		{
		}

        [Menu("Validation", Order = 15)]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Validation examples")
                .AddParagraph("This area contains examples demonstrating the use of server and client side validation, including validation attributes.");

            return View(model);
        }

        #region messages
        /// <summary>
        /// Examples of the validation messages
        /// </summary>
        /// <returns></returns>
        [Menu("Validation messages", Order = 10, ParentAction = "Index")]
        public ActionResult ValidationMessages()
        {
            var model = new ValidationMessagesViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ValidationMessages(ValidationMessagesViewModel model, string submitType)
        {
            AddErrorMessage("Error --> you are on the Validation Page.");
            AddSuccessMessage("Success --> you are on the Validation Page.");
            AddWarningMessage("Warning --> you are on the Validation Page.");
            AddInformationMessage("Info --> you are on the Validation Page.");

            AddWarningMessage("FieldWithAWarning", "An error has occurred on 'Field with a warning'");
            AddInformationMessage("FieldWithInformation", "Some information about 'Field with information'");
            AddInformationMessage("FieldWithMoreInformation", "Some more information");
            AddErrorMessage("FieldWithAnError", "An error has occurred on 'Field with an error'");

            if (submitType == "success")
            {
                AddSuccessMessage("Success --> you are on the Validation Page.");
            AddSuccessMessage("FieldWithASuccess", "Success for the 'Field with a success'");
            }
            else if (submitType == "errors")
            {
                AddErrorMessage("Error --> you are on the Validation Page.");
                AddErrorMessage("FieldWithAnError", "An error has occurred on 'Field with an error'");
            AddErrorMessage("FieldWithAnotherError", "Another error on that field");
            }

            return View(model);
        }
        #endregion

        #region attributes
        /// <summary>
        /// Examples of the validation restriction attributes
        /// </summary>
        /// <returns></returns>
        [Menu("Restriction attributes", Order = 20, ParentAction = "Index")]
        public ActionResult Restrictions()
        {
            var model = new RestrictionsViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Restrictions(RestrictionsViewModel model)
        {
            return View(model);
        }
        #endregion

    }
}