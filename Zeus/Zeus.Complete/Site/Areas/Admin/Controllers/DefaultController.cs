using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Admin.Mappers;
using Employment.Web.Mvc.Area.Admin.ViewModels;
using Employment.Web.Mvc.Area.Admin.ViewModels.User;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Provisioner;

namespace Employment.Web.Mvc.Area.Admin.Controllers
{
	/// <summary>
	/// Defines the Default controller.
	/// </summary>
    [Security(Roles = new[] { "DAD", "SPS", "SPC", "SPN", "EMU", "DES", "DEU","DVO" })]
    public class DefaultController : InfrastructureController
	{
	    protected readonly IProvisionerService ProvisionerService;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="DefaultController" /> class.
	    /// </summary>
	    /// <param name="provisioner">Provisioner service for emulating user</param>
	    /// <param name="userService">User service for retrieving user data.</param>
	    /// <param name="adwService">Adw service for retrieving ADW data.</param>
	    public DefaultController(IProvisionerService provisioner,  IUserService userService, IAdwService adwService) : base( userService, adwService)
	    {
	        ProvisionerService = provisioner;

            if (provisioner == null)
            {
                throw new ArgumentNullException("provisioner");
            }
	    }

        /// <summary>
        /// Index action.
        /// </summary>
        [Menu("Show Security Claims")]
        [Security(AllowAny = true)]
        public ActionResult Index()
        {
            var model = new DisplayClaimsViewModel
                {
                    Claims = UserService.Identity.Claims.AsEnumerable().ToClaimViewModelList()//  MappingEngine.Map<IEnumerable<ClaimViewModel>>(UserService.Identity.Claims.AsEnumerable())
                };

            return View(model);
        }

        [Menu("Date Changer")]
        [Security(Roles = new[] { "DAD", "SPS", "SPC", "SPN", "DES", "DEU","DVO" }, AllowInProduction = false)]
	    public ActionResult DateChanger()
	    {
	        var model = new DateTimeContext {Current = UserService.DateTime.Date};
	        return View(model);
	    }

	    [HttpPost]
        [Security(Roles = new[] { "DAD", "SPS", "SPC", "SPN", "DES", "DEU","DVO" }, AllowInProduction = false)]
        public ActionResult DateChanger(DateTimeContext model)
	    {
	        if (ModelState.IsValid)
	        {
	            if (model.Current.Date.Equals(UserService.DateTime.Date))
	            {
	                AddWarningMessage("Date not been updated. Select a new date and Submit");
	                return View(model);
	            }

	            UserService.DateTime = model.Current;
                    AddInformationMessage("Date updated Successfully");
	                return RedirectToAction("DateChanger");
	        }
            AddErrorMessage("The date entered was not recognised.  The date entered must be in the form DD/MM/YYYY");
	        return View(model);
	    }

        [Security(Roles = new []{"EMU"})]
        [Menu("Emulate")]
	    public ActionResult DepartmentEmulateUser()
	    {
	        var model = new DepartmentEmulateUser();
	        return View(model);
	    }

        [Security(Roles = new []{"EMU"})]
	    [HttpPost]
	    public ActionResult DepartmentEmulateUser(DepartmentEmulateUser emulateUser)
	    {
	        if (ModelState.IsValid)
	        {
	            ProvisionerModel emulate = emulateUser.ToProvisionerModel();//   MappingEngine.Map<ProvisionerModel>(emulateUser);
	            try
	            {
                    var result = ProvisionerService.EmulateAtNextLogon(emulate);
                    if (result)
                    {
                        AddInformationMessage("User will be emulated at next logon. Restart your browser");
                        return View(emulateUser);
                    }

	            }
                catch (ServiceValidationException )
	            {
                    AddErrorMessage("User Emulate Failed.");
                    return View(emulateUser);
	            }
	        }

            AddErrorMessage("User Emulate Failed.");
	        return View(emulateUser);
	    }
	}
}