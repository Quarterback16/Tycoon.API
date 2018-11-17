using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;
using System.ComponentModel;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Infrastructure.Controllers
{
    /// <summary>
    /// Defines the base infrastructure controller which handles the Dependency Injection of common services.
    /// </summary>
    public class InfrastructureController : Controller
    {
        /// <summary>
        /// Whether this is a duplicate POST submit.
        /// </summary>
        public bool IsDuplicateSubmit { get; internal set; }

        /// <summary>
        /// Override page title.
        /// </summary>
        protected string PageTitle
        {
            get { return ViewBag.PageTitle; }
            set { ViewBag.PageTitle = value; }
        }

        /// <summary>
        /// Build manager <see cref="IBuildManager"/>
        /// </summary>
        protected IBuildManager BuildManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IBuildManager>() : null;
            }
        }

        /// <summary>
        /// User service <see cref="IUserService"/>
        /// </summary>
        protected readonly IUserService UserService;
        
        /// <summary>
        /// Adw service <see cref="IAdwService"/>
        /// </summary>
        protected readonly IAdwService AdwService;


        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureController" /> class.
        /// </summary> 
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param> 
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="userService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="adwService" /> is <c>null</c>.</exception>
        public InfrastructureController(IUserService userService, IAdwService adwService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            if (adwService == null)
            {
                throw new ArgumentNullException("adwService");
            }
             
            UserService = userService;
            AdwService = adwService;
        }

        #region Validation

        /// <summary>
        /// Validates a view model.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="model">The view model instance.</param>
        /// <returns>The model state of the view model.</returns>
        protected ModelStateDictionary ValidateModel<T>(T model)
        {
            T m = model;

            var modelBinder = new ModelBindingContext
            {
                ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => m, typeof(T)),
                ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
            };

            System.Web.Mvc.ModelBinders.Binders.DefaultBinder.BindModel(ControllerContext, modelBinder);

            return modelBinder.ModelState;
        }

        #endregion

        #region Workflow

        /// <summary>
        /// Prepares the step by first checking if the current step data is valid and on a postback; otherwise, attempts to load valid saved data for the step.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="model">The current step data.</param>
        /// <param name="key">The key model used for retrieving the saved step data from the users session.</param>
        /// <param name="valid">Whether the step data is valid.</param>
        /// <returns><c>true</c> if the step is valid; otherwise, <c>false</c>.</returns>
        protected T PrepareStep<T>(T model, KeyModel key, out bool valid)
        {
            return PrepareStep(model, key, out valid, false);
        }

        /// <summary>
        /// Prepares the step by first checking if the current step data is valid and on a postback; otherwise, attempts to load valid saved data for the step.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="model">The current step data.</param>
        /// <param name="key">The key model used for retrieving the saved step data from the users session.</param>
        /// <param name="valid">Whether the step data is valid.</param>
        /// <param name="forceLoadFromSession">Force loading model from session.</param>
        /// <returns><c>true</c> if the step is valid; otherwise, <c>false</c>.</returns>
        protected T PrepareStep<T>(T model, KeyModel key, out bool valid, bool forceLoadFromSession)
        {
            valid = false;
            
            // If the model submitted for this step is already valid, return it as is
            if (!forceLoadFromSession && Request.HttpMethod == "POST" && ValidateModel(model).IsValid)
            {
                valid = true;

                return model;
            }

            T m;

            // Get data saved for current step and use if valid
            if (UserService.Session.TryGet(key, out m) && ValidateModel(m).IsValid)
            {
                // Overwrite model with saved data 
                model = m;

                valid = true;
            }

            return model;
        }

        #endregion

        #region Alerts

        /// <summary>
        /// Add an alert.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="content">The content.</param>
        protected void AddAlert(string title, ContentViewModel content)
        {
            var alerts = ViewData.Get<List<ContentViewModel>>("Alerts") ?? new List<ContentViewModel>();

            alerts.Add(new ContentViewModel().AddTitle(title).Merge(content));

            ViewData.Set<List<ContentViewModel>>("Alerts", alerts);
        }

        /// <summary>
        /// Add an alert.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="content">The content.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="colourType">The background colour of the icon.</param>
        protected void AddAlert(string title, ContentViewModel content, string icon, ColourType colourType)
        {
            var alerts = ViewData.Get<List<ContentViewModel>>("Alerts") ?? new List<ContentViewModel>();
            
            alerts.Add(new ContentViewModel().AddIcon(icon, colourType).AddTitle(title).Merge(content));

            ViewData.Set<List<ContentViewModel>>("Alerts", alerts);
        }

        #endregion

        #region Add Messages

        /// <summary>
        /// Add a success message.
        /// </summary>
        /// <param name="successMessage">The success message.</param>
        protected void AddSuccessMessage(string successMessage)
        {
            AddMessage(MessageType.Success, string.Empty, successMessage);
        }

        /// <summary>
        /// Add a success message against a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to assign the success message to.</param>
        /// <param name="successMessage">The success message.</param>
        protected void AddSuccessMessage(string propertyName, string successMessage)
        {
            AddMessage(MessageType.Success, propertyName, successMessage);
        }

        /// <summary>
        /// Add an information message.
        /// </summary>
        /// <param name="informationMessage">The information message.</param>
        protected void AddInformationMessage(string informationMessage)
        {
            AddMessage(MessageType.Information, string.Empty, informationMessage);
        }

        /// <summary>
        /// Add an information message against a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to assign the information message to.</param>
        /// <param name="informationMessage">The information message.</param>
        protected void AddInformationMessage(string propertyName, string informationMessage)
        {
            AddMessage(MessageType.Information, propertyName, informationMessage);
        }

        /// <summary>
        /// Add a warning message.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        protected void AddWarningMessage(string warningMessage)
        {
            AddMessage(MessageType.Warning, string.Empty, warningMessage);
        }

        /// <summary>
        /// Add a warning message against a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to assign the warning message to.</param>
        /// <param name="warningMessage">The warning message.</param>
        protected void AddWarningMessage(string propertyName, string warningMessage)
        {
            AddMessage(MessageType.Warning, propertyName, warningMessage);
        }

        /// <summary>
        /// Add a message to temp data.
        /// </summary>
        /// <param name="messageType">Type of message.</param>
        /// <param name="propertyName">The name of the property to assign the message to.</param>
        /// <param name="message">The message.</param>
        private void AddMessage(MessageType messageType, string propertyName, string message)
        {
            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            Dictionary<string, List<string>> messages = null;

            var key = messageType.ToString();

            if (TempData.ContainsKey(key))
            {
                messages = TempData[key] as Dictionary<string, List<string>>;
            }

            if (messages == null)
            {
                messages = new Dictionary<string, List<string>>();
            }

            if (messages.ContainsKey(propertyName))
            {
                if (!messages[propertyName].Contains(message))
                {
                    messages[propertyName].Add(message);
                }
            }
            else
            {
                messages.Add(propertyName, new List<string> { message });
            }

            TempData[key] = messages;
        }

        /// <summary>
        /// Add an error message.
        /// </summary>
        /// <remarks>
        /// Almost the same behaviour as using method <see cref="ModelStateDictionary.AddModelError(string, string)" /> on <see cref="Controller.ModelState" />.
        /// The difference is that duplicate errors will not be added.
        /// </remarks>
        /// <param name="errorMessage">The error message.</param>
        protected void AddErrorMessage(string errorMessage)
        {
            AddErrorMessage(string.Empty, errorMessage);
        }

        /// <summary>
        /// Add an error message against a property.
        /// </summary>
        /// <remarks>
        /// Almost the same behaviour as using method <see cref="ModelStateDictionary.AddModelError(string, string)" /> on <see cref="Controller.ModelState" />.
        /// The difference is that duplicate errors will not be added.
        /// </remarks>
        /// <param name="propertyName">The name of the property to assign the error to.</param>
        /// <param name="errorMessage">The error message.</param>
        protected void AddErrorMessage(string propertyName, string errorMessage)
        {
            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            if (!HasErrorMessage(propertyName, errorMessage))
            {
                ModelState.AddModelError(propertyName, errorMessage);
            }
        }

        #endregion

        #region Get Messages

        /// <summary>
        /// Get all success messages not assigned to a property.
        /// </summary>
        /// <returns>All success messages not assigned to a property.</returns>
        protected IEnumerable<string> GetSuccessMessages()
        {
            return GetMessages(MessageType.Success, string.Empty);
        }

        /// <summary>
        /// Get all success messages assigned to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the success messages for.</param>
        /// <returns>All success messages assigned to a property.</returns>
        protected IEnumerable<string> GetSuccessMessages(string propertyName)
        {
            return GetMessages(MessageType.Success, propertyName);
        }

        /// <summary>
        /// Get all information messages not assigned to a property.
        /// </summary>
        /// <returns>All information messages not assigned to a property.</returns>
        protected IEnumerable<string> GetInformationMessages()
        {
            return GetMessages(MessageType.Information, string.Empty);
        }

        /// <summary>
        /// Get all information messages assigned to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the information messages for.</param>
        /// <returns>All information messages assigned to a property.</returns>
        protected IEnumerable<string> GetInformationMessages(string propertyName)
        {
            return GetMessages(MessageType.Information, propertyName);
        }

        /// <summary>
        /// Get all warning messages not assigned to a property.
        /// </summary>
        /// <returns>All warning messages not assigned to a property.</returns>
        protected IEnumerable<string> GetWarningMessages()
        {
            return GetMessages(MessageType.Warning, string.Empty);
        }

        /// <summary>
        /// Get all warning messages assigned to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the warning messages for.</param>
        /// <returns>All warning messages assigned to a property.</returns>
        protected IEnumerable<string> GetWarningMessages(string propertyName)
        {
            return GetMessages(MessageType.Warning, propertyName);
        }

        /// <summary>
        /// Get messages from temp data assigned to a property.
        /// </summary>
        /// <param name="messageType">Type of message.</param>
        /// <param name="propertyName">The name of the property to get the messages for.</param>
        /// <returns>All messages of the specific type that are assigned to the specified property.</returns>
        private IEnumerable<string> GetMessages(MessageType messageType, string propertyName)
        {
            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            Dictionary<string, List<string>> messages = null;

            var key = messageType.ToString();

            if (TempData.ContainsKey(key))
            {
                messages = TempData[key] as Dictionary<string, List<string>>;
            }

            if (messages == null)
            {
                messages = new Dictionary<string, List<string>>();
            }

            if (messages.ContainsKey(propertyName))
            {
                return messages[propertyName];
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get all error messages not assigned to a property.
        /// </summary>
        /// <returns>All error messages not assigned to a property.</returns>
        protected IEnumerable<string> GetErrorMessages()
        {
            return GetErrorMessages(string.Empty);
        }

        /// <summary>
        /// Get all error messages assigned to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the error messages for.</param>
        /// <returns>All error messages assigned to a property.</returns>
        protected IEnumerable<string> GetErrorMessages(string propertyName)
        {
            if (propertyName == null)
            {
                propertyName = string.Empty;
            }

            var errors = ModelState.FirstOrDefault(m => string.Equals(m.Key, propertyName,StringComparison.Ordinal));

            if (errors.Value != null && errors.Value.Errors != null && errors.Value.Errors.Any(e => !string.IsNullOrEmpty(e.ErrorMessage)))
            {
                return errors.Value.Errors.Select(e => e.ErrorMessage);
            }

            return Enumerable.Empty<string>();
        }

        #endregion

        #region Has Messages

        /// <summary>
        /// Has a specific success message already been added.
        /// </summary>
        /// <param name="successMessage">The success message.</param>
        /// <returns><c>true</c> if the success message exists; otherwise, <c>false</c>.</returns>
        protected bool HasSuccessMessage(string successMessage)
        {
            return HasMessage(MessageType.Success, string.Empty, successMessage);
        }

        /// <summary>
        /// Has a specific success message already been added to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to check for the success message.</param>
        /// <param name="successMessage">The success message.</param>
        /// <returns><c>true</c> if the success message exists against the property; otherwise, <c>false</c>.</returns>
        protected bool HasSuccessMessage(string propertyName, string successMessage)
        {
            return HasMessage(MessageType.Success, propertyName, successMessage);
        }

        /// <summary>
        /// Has a specific information message already been added.
        /// </summary>
        /// <param name="informationMessage">The information message.</param>
        /// <returns><c>true</c> if the information message exists; otherwise, <c>false</c>.</returns>
        protected bool HasInformationMessage(string informationMessage)
        {
            return HasMessage(MessageType.Information, string.Empty, informationMessage);
        }

        /// <summary>
        /// Has a specific information message already been added to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to check for the information message.</param>
        /// <param name="informationMessage">The information message.</param>
        /// <returns><c>true</c> if the information message exists for the property; otherwise, <c>false</c>.</returns>
        protected bool HasInformationMessage(string propertyName, string informationMessage)
        {
            return HasMessage(MessageType.Information, propertyName, informationMessage);
        }

        /// <summary>
        /// Has a specific warning message already been added.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        /// <returns><c>true</c> if the warning message exists; otherwise, <c>false</c>.</returns>
        protected bool HasWarningMessage(string warningMessage)
        {
            return HasMessage(MessageType.Warning, string.Empty, warningMessage);
        }

        /// <summary>
        /// Has a specific warning message already been added to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to check for the warning message.</param>
        /// <param name="warningMessage">The warning message.</param>
        /// <returns><c>true</c> if the warning message exists for the property; otherwise, <c>false</c>.</returns>
        protected bool HasWarningMessage(string propertyName, string warningMessage)
        {
            return HasMessage(MessageType.Warning, propertyName, warningMessage);
        }

        /// <summary>
        /// Has a message already been added to a property in temp data.
        /// </summary>
        /// <param name="messageType">Type of message.</param>
        /// <param name="propertyName">The name of the property to check the message for.</param>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if the message exists for the property; otherwise, <c>false</c>.</returns>
        private bool HasMessage(MessageType messageType, string propertyName, string message)
        {
            return GetMessages(messageType, propertyName).Any(m => string.Equals(m, message, StringComparison.Ordinal));
        }

        /// <summary>
        /// Has a specific error message already been added.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><c>true</c> if the error message exists; Ootherwise, <c>false</c>.</returns>
        protected bool HasErrorMessage(string errorMessage)
        {
            return HasErrorMessage(string.Empty, errorMessage);
        }

        /// <summary>
        /// Has a specific error message already been added to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to check for the error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><c>true</c> if the error message exists for the property; otherwise, <c>false</c>.</returns>
        protected bool HasErrorMessage(string propertyName, string errorMessage)
        {
            return GetErrorMessages(propertyName).Any(m => string.Equals(m, errorMessage, StringComparison.Ordinal));
        }

        #endregion

        #region Action Results

        /// <summary>
        /// Returns a 401 (unauthorized) status code result.
        /// </summary>
        /// <returns>The 401 custom error page.</returns>
        protected internal HttpUnauthorizedResult HttpUnauthorized()
        {
            return HttpUnauthorized(null);
        }

        /// <summary>
        /// Returns a 401 (unauthorized) status code result.
        /// </summary>
        /// <param name="statusDescription">A custom status description to include in the result.</param>
        /// <returns>The 401 custom error page.</returns>
        protected internal virtual HttpUnauthorizedResult HttpUnauthorized(string statusDescription)
        {
            return new HttpUnauthorizedResult(statusDescription);
        }

        /// <summary>
        /// Returns a 403 (unauthorized) status code result.
        /// </summary>
        /// <returns>The 403 custom error page.</returns>
        protected internal HttpStatusCodeResult HttpForbidden()
        {
            return HttpUnauthorized(null);
        }

        /// <summary>
        /// Returns a 403 (unauthorized) status code result.
        /// </summary>
        /// <param name="statusDescription">A custom status description to include in the result.</param>
        /// <returns>The 403 custom error page.</returns>
        protected internal virtual HttpStatusCodeResult HttpForbidden(string statusDescription)
        {
            return new HttpStatusCodeResult(403, statusDescription);
        }

        /// <summary>
        /// Renders a paged view.
        /// </summary>
        /// <remarks>
        /// By default, uses the Paged Grid view for rendering the next page.
        /// </remarks>
        /// <param name="data">The pageable collection of data.</param>
        /// <returns>The render result of the next page with updated metadata.</returns>
        protected ActionResult PagedView<T>(IPageable<T> data)
        {
            return PagedView(data, "_PagedGrid");
        }

        /// <summary>
        /// Renders a paged view.
        /// </summary>
        /// <param name="data">The pageable collection of data.</param>
        /// <param name="viewName">The name of the view for rendering the next page.</param>
        /// <returns>The render result of the next page with updated metadata.</returns>
        protected ActionResult PagedView<T>(IPageable<T> data, string viewName)
        {
            var parentModel = DelegateHelper.CreateConstructorDelegate(data.Metadata.ModelType)();

            ViewData.Add("PagedMetadata", data.Metadata);
            ViewData.Add("ParentModel", parentModel);
            ViewData.Model = data;

            return new PartialViewResult
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        /// <summary>
        /// Renders an Ajax step view.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The partial view for the step.</returns>
        protected ActionResult AjaxStepView<T>(T model)
        {
            return AjaxStepView<T>(null, model);
        }

        /// <summary>
        /// Renders an Ajax step view.
        /// </summary>
        /// <param name="viewName">The name of the view for rendering the step.</param>
        /// <param name="model">The model.</param>
        /// <returns>The partial view for the step.</returns>
        protected ActionResult AjaxStepView<T>(string viewName, T model)
        {
            
            // Get parent model type and set as property value
            if (HttpContext.Request.Headers["Zeus-Parent-Type"] != null)
            {
                var parentType = HttpContext.Request.Headers["Zeus-Parent-Type"].ToString();

                if (HttpContext.Request.Headers["Zeus-Property-Name"] != null)
                {
                    var propertyName = HttpContext.Request.Headers["Zeus-Property-Name"].ToString();
                    var actualParentType = BuildManager.ResolveType(parentType);
                    var parentModel = DelegateHelper.CreateConstructorDelegate(actualParentType)();
                    var property = TypeDescriptor.GetProperties(parentModel).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName);

                    if (property != null)
                    {
                        property.SetValue(parentModel, model);

                        var view = base.PartialView(viewName, parentModel);

                        view.ViewData.Add("AjaxStepView", true);
                        view.ViewData.Add("AjaxStepViewParentType", actualParentType);
                        view.ViewData.Add("AjaxStepViewPropertyName", propertyName);

                        return view;
                    }
                }
            }

            return new HttpNotFoundResult();
        }

        public const int AjaxSelectionPageSize = 20;

        /// <summary>
        /// Renders an ajax selection view.
        /// </summary>
        /// <param name="text">The user supplied text.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="data">The data to return.</param>
        /// <returns>The next page with an indicator specifying whether there are any more pages.</returns>
        protected JsonResult AjaxSelectionView(string text, int page, IEnumerable<SelectListItem> data)
        {
            if (data == null || !data.Any())
            {
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = Enumerable.Empty<SelectListItem>(), more = false } };
            }

            if (page == 0)
            {
                data = data.Where(p => p.Value.IndexOf(text,StringComparison.OrdinalIgnoreCase)>=0).ToList();
            }

            if (!string.IsNullOrEmpty(text))
            {
                data = data.Where(p => p.Text.IndexOf(text,StringComparison.OrdinalIgnoreCase)>=0).ToList();
            }

            var totalCount = data.Count();
            var more = totalCount > 0 && (page * AjaxSelectionPageSize) < totalCount;
            data = data.Skip((page - 1) * AjaxSelectionPageSize).Take(AjaxSelectionPageSize);

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { result = data, more } };
        }

        /// <summary>
        /// Redirect the user to the previous action they were on.
        /// </summary>
        /// <returns>A redirect to the previous action if found in session; otherwise, a redirect to the main landing page.</returns>
        protected RedirectResult RedirectToPreviousAction(RouteValueDictionary routeValues = null)
        {
            // Current route details
            var currentArea = ControllerContext.RouteData.GetArea();
            var currentController = ControllerContext.RouteData.GetController();
            var currentAction = ControllerContext.RouteData.GetAction();

            if (currentAction.IndexOf('?')>=0)
            {
                currentAction = currentAction.Substring(0, currentAction.IndexOf('?'));
            }

            // Populate key with current action details
            var keyModel = new KeyModel(RememberPreviousActionAttribute.SessionKey).Add(currentArea).Add(currentController).Add(currentAction);

            Uri uri;
            if (UserService.Session.TryGet(keyModel, out uri))
            {
                UserService.Session.Remove(keyModel);

                var url = uri.ToString();

                if (routeValues != null)
                {
                    foreach (var routeValue in routeValues)
                    {
                        if (url.IndexOf('?')<0)
                        {
                            url += string.Format("?{0}={1}", routeValue.Key, routeValue.Value);
                        }
                        else
                        {
                            url += string.Format("&{0}={1}", routeValue.Key, routeValue.Value);
                        }
                    }
                }

                return new RedirectResult(url);
            }

            return new RedirectResult("/");
        }

        #endregion
    }
}
