using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Helpers;
using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using System.Collections.ObjectModel;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker; 
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="HtmlHelper"/>.
    /// </summary>
    public static class HtmlHelperExtension
    {

        private static MvcHtmlString MessageDismissButton = MvcHtmlString.Create("<span class=\"close\" data-dismiss=\"alert\">×<span class='readers'>Dismiss this message.</span></span>");

        private static IAdwService AdwService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IAdwService>() : null;
            }
        }

        private static IMenuService MenuService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IMenuService>() : null;
            }
        }

        private static IBulletinService BulletinService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IBulletinService>() : null;
            }
        }

        //private static IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        var containerProvider = DependencyResolver.Current as IContainerProvider;

        //        return (containerProvider != null) ? containerProvider.GetService<IMappingEngine>() : null;
        //    }
        //}

        private static IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        private static IConfigurationManager ConfigurationManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        /// <summary>
        /// Used in Razor Views for determining whether the build was done with DEBUG preprocessor mode.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <returns><c>true</c> if DEBUG; otherwise, <c>false</c>.</returns>
        public static bool IsDebug(this HtmlHelper html)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Environment html
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString Environment(this HtmlHelper html)
        {
            string result = string.Empty;
            var environment = ConfigurationManager.AppSettings.Get("Environment");

            if (!string.IsNullOrEmpty(environment))
            {
                var tag = new TagBuilder("div");

                switch (environment.ToUpper())
                {
                    case "WKS":
                        tag.Attributes.Add("id", "enviroWks");
                        tag.SetInnerText("Local workstation");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "DEV":
                        tag.Attributes.Add("id", "enviroDev");
                        tag.SetInnerText("Development environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "DEVFIX":
                        tag.Attributes.Add("id", "enviroDevFix");
                        tag.SetInnerText("Development fix environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "TEST":
                        tag.Attributes.Add("id", "enviroTest");
                        tag.SetInnerText("Test environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "TESTFIX":
                        tag.Attributes.Add("id", "enviroTestFix");
                        tag.SetInnerText("Test fix environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "PREPROD":
                        tag.Attributes.Add("id", "enviroPreProd");
                        tag.SetInnerText("Pre-Production environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                    case "TRAIN":
                        tag.Attributes.Add("id", "enviroTrain");
                        tag.SetInnerText("Training environment");
                        result = tag.ToString(TagRenderMode.Normal);
                        break;
                }
            }

            return new MvcHtmlString(result);
        }



        /// <summary>
        /// Get first message for property.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="messageType">Type of message.</param>
        /// <param name="propertyName">The name of the property to assign the message to.</param>
        /// <returns>A string of the message.</returns>
        public static MvcHtmlString Message(this HtmlHelper html, MessageType messageType, string propertyName)
        {
            var key = messageType.ToString();

            if (string.IsNullOrEmpty(key))
            {
                return MvcHtmlString.Empty;
            }

            Dictionary<string, List<string>> messages = null;

            if (html.ViewContext.TempData.ContainsKey(key))
            {
                messages = html.ViewContext.TempData[key] as Dictionary<string, List<string>>;
            }

            var result = string.Empty;

            if (messages != null && messages.Any())
            {
                var message = messages.FirstOrDefault(p => p.Key == propertyName);

                if (message.Value != null && message.Value.Any())
                {
                    var m = message.Value.FirstOrDefault();

                    if (!string.IsNullOrEmpty(m))
                    {
                        result = m;
                    }
                }
            }
            
            return new MvcHtmlString(result);
        }

        /// <summary>Returns an unordered list (ul element) of validation messages that are in the <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.</summary>
        /// <remarks>Custom version of <see cref="ValidationExtensions.ValidationSummary(System.Web.Mvc.HtmlHelper)"/> to use different markup and include property name shortcut anchor.</remarks>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        public static MvcHtmlString ValidationMessageSummary(this HtmlHelper html, string groupName = null)
        {
            return html.ValidationMessageSummary(false, groupName);
        }

        /// <summary>Returns an unordered list (ul element) of validation messages that are in the <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.</summary>
        /// <remarks>Custom version of <see cref="ValidationExtensions.ValidationSummary(System.Web.Mvc.HtmlHelper, bool)"/> to use different markup and include property name shortcut anchor.</remarks>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="excludePropertyErrors"><c>true</c> to have the summary display model-level errors only, or <c>false</c> to have the summary display all errors.</param>
        /// <param name="groupName">The name of the group, currently being rendered, to render appropriate messages that belong to property that resides in the same group.</param>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        public static MvcHtmlString ValidationMessageSummary(this HtmlHelper html, bool excludePropertyErrors, string groupName)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelper.ValidationMessageSummary");

            try
            {
#endif
                var nonMatchingKeys = new List<string>();

                foreach (var key in html.ViewData.ModelState.Keys)
                {
                    if (!string.IsNullOrEmpty(key) && html.ViewData.ModelMetadata.Find(key) == null)
                    {
                        // Model State error key does not have a matching property name in the View Model
                        nonMatchingKeys.Add(key);
                    }
                }

                // Set non-matching error key to empty string so the error message is displayed
                nonMatchingKeys.ForEach(k =>
                {
                    // Get errors for non-matching key
                    var errors = html.ViewData.ModelState[k];

                    // Remove non-matching key
                    html.ViewData.ModelState.Remove(k);

                    if (!html.ViewData.ModelState.ContainsKey(string.Empty))
                    {
                        // Add errors with an empty key
                        html.ViewData.ModelState.Add(string.Empty, errors);
                    }
                    else
                    {
                        foreach (var error in errors.Errors)
                        {
                            html.ViewData.ModelState[string.Empty].Errors.Add(error);
                        }
                    }
                });

                var result = new StringBuilder();

                var formContextForClientValidation = html.ViewContext.ClientValidationEnabled ? html.ViewContext.FormContext : null;

                if (html.ViewData.ModelState.IsValid)
                {                    

                    if (formContextForClientValidation == null)
                    {
                        return null;
                    }

                    if (html.ViewContext.UnobtrusiveJavaScriptEnabled && excludePropertyErrors)
                    {
                        return null;
                    }
                }
                else
                {
                    // CHANGE 
                    // We will display success messages regardless of whether error messages are on the page or not.
                    // Hence, this no longer applies --> Success summary isn't being displayed but it should still be cleared from TempData so as not to duplicate.
                    // Therefore, commenting out below.
                    //html.ViewContext.TempData.Remove(MessageType.Success.ToString());
                }

                result.AppendLine(MessageSummary(html, MessageType.Warning, groupName).ToString());
                result.AppendLine(MessageSummary(html, MessageType.Information, groupName).ToString());

                              

                // Remove ModelState stored in TempData as we are about to show it and don't want it to stick around
                html.ViewContext.TempData.Remove(PersistModelStateAttribute.TempDataKey);

                // Error summary (always include; hidden if model is valid but shown by client-side unobtrusive validation if invalid)
                var headerTag = new TagBuilder("h3");
                headerTag.SetInnerText("Error message");

                var listTag = new TagBuilder("ul");
                var stringBuilder = new StringBuilder();
                bool currentPanelContainsErrors = false; // For a check to see if current group has error messages.

                var modelStates = GetModelStates(html, excludePropertyErrors);

                foreach (KeyValuePair<string, ModelState> modelState in modelStates)
                {
                    foreach (ModelError error in modelState.Value.Errors)
                    {
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            TagBuilder listItemTag = null;

                            if (string.IsNullOrEmpty(modelState.Key))
                            {
                                if (string.IsNullOrEmpty(groupName))
                                {
                                    listItemTag = new TagBuilder("li");
                                    // Only render error messages (without key) when groupName is empty, i.e. when rendering messages on top of the page.
                                    listItemTag.SetInnerText(error.ErrorMessage);
                                }
                            }
                            else
                            {

                                var property = html.ViewData.ModelMetadata.Find(modelState.Key);
                                var displayAttribute = property.GetAttribute<DisplayAttribute>();

                                // look for the properties with supplied groupName and only get the properties that match these group names.
                                // LOGIC:
                                //      - check if the group is null then messages are displayed on top.
                                //         - proccess only the properties that don't have group name specified.
                                //      - if group is not null
                                //          - process only the properties that have group name specified and this name matches the groupName supplied.
                               /*if (string.IsNullOrEmpty(groupName) && displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.GroupName))
                                {
                                    // Ignore those properites, that have group name specified, because they will be rendered in that group.
                                    continue;
                                }
                                else*/
                                if (!string.IsNullOrEmpty(groupName) &&
                                    (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.GroupName) && !displayAttribute.GroupName.ToLower().Equals(groupName.ToLower())
                                    || (displayAttribute == null || (displayAttribute != null && string.IsNullOrEmpty(displayAttribute.GroupName)))
                                    ))
                                {
                                    // IF GroupName specified is not null, then
                                    // Ignore those properties, whose group name does not match groupName supplied.
                                    // OR Ignore those properties, which either don't have DisplayAttribute or which display Attribute but GroupName is null.
                                    continue;
                                }

                                currentPanelContainsErrors = true; // Error messages are being displayed for the group.

                                listItemTag = new TagBuilder("li");
                                
                                var displayName = property != null ? property.GetDisplayName() : modelState.Key;

                                var linkTag = new TagBuilder("a");
                                linkTag.AddCssClass("alert-link");
                                linkTag.Attributes.Add("href", string.Format("#{0}", modelState.Key.Replace('.', '_')));
                                linkTag.SetInnerText(displayName);

                                listItemTag.InnerHtml = string.Format("{0} - {1}", linkTag.ToString(TagRenderMode.Normal), error.ErrorMessage);
                            }

                            stringBuilder.AppendLine(listItemTag == null ? string.Empty : listItemTag.ToString(TagRenderMode.Normal));
                        }
                    }
                }


                listTag.InnerHtml = stringBuilder.ToString();                

                var sectionTag = new TagBuilder("section");


                if (!string.IsNullOrEmpty(stringBuilder.ToString().Trim()))
                {
                    // display message if there are errors.
                    sectionTag.AddCssClass("alert alert-danger");
                }
                else 
                {
                    // if list of errors is empty then add this class so we can hide this section from being displayed.
                    sectionTag.AddCssClass("noErrors");
                }
                sectionTag.Attributes.Add("id", string.IsNullOrEmpty(groupName) ? "validation-error-summary" : "validation-error-summary-" + groupName);
                sectionTag.AddCssClass(html.ViewData.ModelState.IsValid ? HtmlHelper.ValidationSummaryValidCssClassName : HtmlHelper.ValidationSummaryCssClassName);
                sectionTag.InnerHtml = string.Concat(MessageDismissButton, headerTag.ToString(TagRenderMode.Normal), listTag.ToString(TagRenderMode.Normal));

                if (formContextForClientValidation != null)
                {
                    if (html.ViewContext.UnobtrusiveJavaScriptEnabled)
                    {
                        if (!excludePropertyErrors)
                        {
                            sectionTag.MergeAttribute("data-valmsg-summary", "true");
                        }
                    }
                    else
                    {
                        sectionTag.GenerateId("validationSummary");
                        formContextForClientValidation.ValidationSummaryId = sectionTag.Attributes["id"];
                        formContextForClientValidation.ReplaceValidationSummary = !excludePropertyErrors;
                    }
                }

                result.AppendLine(sectionTag.ToString(TagRenderMode.Normal));


                // Success summary regardless of if model is valid or invalid
                if (string.IsNullOrEmpty(groupName))
                {
                    result.AppendLine(MessageSummary(html, MessageType.Success, groupName).ToString());
                }
                else if (! currentPanelContainsErrors)
                {
                    //TODO: check if current group has error messages, if so don't display any success message otherwise display them.
                    result.AppendLine(MessageSummary(html, MessageType.Success, groupName).ToString());
                }

                return new MvcHtmlString(result.ToString());
#if DEBUG

            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        private static MvcHtmlString MessageSummary(HtmlHelper html, MessageType messageType, string groupName)
        {
            var key = messageType.ToString();

            if (string.IsNullOrEmpty(key))
            {
                return MvcHtmlString.Empty;
            }

            Dictionary<string, List<string>> messages = null;

            if (html.ViewContext.TempData.ContainsKey(key))
            {
                messages = html.ViewContext.TempData[key] as Dictionary<string, List<string>>;
            }

            if (messages != null && messages.Any())
            {
                var headerTag = new TagBuilder("h3");
                headerTag.SetInnerText(string.Format("{0} message", key));

                var listTag = new TagBuilder("ul");
                var stringBuilder = new StringBuilder();

                foreach (var message in messages)
                {
                    foreach (var m in message.Value)
                    {
                        if (!string.IsNullOrEmpty(m))
                        {
                            TagBuilder listItemTag = null;

                            if (string.IsNullOrEmpty(message.Key))
                            {
                                if (string.IsNullOrEmpty(groupName))
                                {
                                    listItemTag = new TagBuilder("li");
                                    // Only render these messages (without key) when groupName is empty, i.e. when rendering messages on top of the page.
                                listItemTag.SetInnerText(m);
                            }
                            }
                            else
                            {
                                listItemTag = new TagBuilder("li");

                                var property = html.ViewData.ModelMetadata.Properties.FirstOrDefault(p => p.PropertyName == message.Key);
                                var displayAttribute = property.GetAttribute<DisplayAttribute>();

                                // look for the properties with supplied groupName and only get the properties that match these group names.
                                // LOGIC:
                                //      - check if the group is null then messages are displayed on top.
                                //         - proccess only the properties that don't have group name specified.
                                //      - if group is not null
                                //          - process only the properties that have group name specified and this name matches the groupName supplied.
                                if (string.IsNullOrEmpty(groupName) && displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.GroupName))
                                {
                                    // Ignore those properites, that have group name specified, because they will be rendered in that group.
                                    continue;
                                }
                                //else if (!string.IsNullOrEmpty(groupName) &&
                                //    ( displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.GroupName) && !displayAttribute.GroupName.ToLower().Equals(groupName.ToLower())
                                //    || (displayAttribute == null || (displayAttribute != null && string.IsNullOrEmpty(displayAttribute.GroupName)))
                                //    ))
                                //{
                                //    // IF GroupName specified is not null, then
                                //        // Ignore those properties, whose group name does not match groupName supplied.
                                //        // OR Ignore those properties, which either don't have DisplayAttribute or which display Attribute but GroupName is null.
                                //    continue;
                                //} 
                                else if (!string.IsNullOrEmpty(groupName) && displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.GroupName) && !displayAttribute.GroupName.ToLower().Equals(groupName.ToLower()))
                                {
                                    // Ignore those properties, whose group name does not match groupName supplied.
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(groupName) && displayAttribute == null || (displayAttribute != null && string.IsNullOrEmpty(displayAttribute.GroupName)))
                                {
                                    // Ignore those properties, which either don't have DisplayAttribute or which display Attribute but GroupName is null.
                                    continue;
                                }

                                var displayName = property != null ? property.GetDisplayName() : message.Key;

                                var linkTag = new TagBuilder("a");
                                linkTag.AddCssClass("alert-link");
                                linkTag.Attributes.Add("href", string.Format("#{0}", message.Key.Replace('.', '_')));
                                linkTag.SetInnerText(displayName);

                                listItemTag.InnerHtml = string.Format("{0} - {1}", linkTag.ToString(TagRenderMode.Normal), m);
                            }

                            stringBuilder.AppendLine(listItemTag == null ? string.Empty : listItemTag.ToString(TagRenderMode.Normal));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(stringBuilder.ToString().Trim() ))
                {
                    // only render section, if stringBuilder contains any messages.
                listTag.InnerHtml = stringBuilder.ToString();

                var sectionTag = new TagBuilder("section");
                
                    const string CommonMessageStyles = " fade in m-b-15";

                    string appropriateStyle = string.Empty;

                switch (messageType)
                {
                    case MessageType.Success:
                            appropriateStyle = "msgGood alert alert-success";
                        break;
                    case MessageType.Information:
                            appropriateStyle = "msgInfo alert alert-info";
                        break;
                    case MessageType.Warning:
                            appropriateStyle = "msgWarn alert alert-warning";
                        break;
                }

                    sectionTag.AddCssClass(!string.IsNullOrEmpty(appropriateStyle) ? appropriateStyle + CommonMessageStyles : string.Empty);

                    sectionTag.InnerHtml = string.Concat(MessageDismissButton, headerTag.ToString(TagRenderMode.Normal), listTag.ToString(TagRenderMode.Normal));

                return new MvcHtmlString(sectionTag.ToString(TagRenderMode.Normal));
            }
            }

            return MvcHtmlString.Empty;
        }

        private static Dictionary<string, ModelState> GetModelStates(HtmlHelper html, bool excludePropertyErrors)
        {
            var modelStates = new Dictionary<string, ModelState>();

            if (html.ViewData.ModelState[string.Empty] != null)
            {
                modelStates.Add(string.Empty, html.ViewData.ModelState[string.Empty]);
            }

            if (!excludePropertyErrors)
            {
                foreach (var ms in html.ViewData.ModelState.Where(m => !string.IsNullOrEmpty(m.Key)))
                {
                    modelStates.Add(ms.Key, ms.Value);
                }
            }

            return modelStates;
        }

        /// <summary>
        /// Extract inner html
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string ExtractInnerHtml(this HtmlHelper html, string tag)
        {
            var match = Regex.Match(tag, @"<(.*)\>(.*)<\/(.*)\>", RegexOptions.IgnoreCase);

            return (match.Success) ? match.Groups[2].Value : string.Empty;
        }

        /// <summary>
        /// Get user service
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IUserService GetUserService(this HtmlHelper html)
        {
            return UserService;
        }

        /// <summary>
        /// Get user organisation code
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetUserOrganisationCode(this HtmlHelper html)
        {
            return UserService.OrganisationCode;
        }

        /// <summary>
        /// Get user site code
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetUserSiteCode(this HtmlHelper html)
        {
            return UserService.SiteCode;
        }

        /// <summary>
        /// get user name
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetUsername(this HtmlHelper html)
        {
            return UserService.Username;
        }

        /// <summary>
        /// Get user full name
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString GetUserFullName(this HtmlHelper html)
        {
            var userService = UserService;
            return new MvcHtmlString(string.Format("{0} {1}", userService.FirstName, userService.LastName));
        }

        /// <summary>
        /// Get current area name
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString GetCurrentAreaName(this HtmlHelper html)
        {
            string area = html.ViewContext.RouteData.GetArea();
            string areaName = null;
            MenuService.AreaNames.TryGetValue(area, out areaName);
            return new MvcHtmlString(areaName);
            //return new MvcHtmlString(MenuService.AreaNames.FirstOrDefault(a => System.String.Equals(a.Key, area, System.StringComparison.OrdinalIgnoreCase)).Value);
        }

        /// <summary>
        /// Get current action name
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="fallbackName">The name to fallback on if there is no Menu item defined for the action.</param>
        /// <returns>The current action display name.</returns>
        public static MvcHtmlString GetCurrentActionName(this HtmlHelper html, string fallbackName)
        {
            var area = html.ViewContext.RouteData.GetArea();
            var cont = html.ViewContext.RouteData.GetController();
            var action = html.ViewContext.RouteData.GetAction();
            
            //MenuModel menuItem;
            //MenuService.MenuItemsDictionary.TryGetValue(string.Format("{0}_{1}_{2}", area, cont, action), out menuItem);

            MenuModel menuItem = null;
            foreach (MenuModel m in MenuService.MenuItems)
            {
                if ((System.String.Equals(m.Area, area, System.StringComparison.OrdinalIgnoreCase) 
                    && System.String.Equals(m.Controller, cont, System.StringComparison.OrdinalIgnoreCase) 
                    && System.String.Equals(m.Action, action, System.StringComparison.OrdinalIgnoreCase)))
                {
                    menuItem = m;
                    break;
                }
            }

            if (string.IsNullOrEmpty(fallbackName))
            {
                fallbackName = area;
            }

            return new MvcHtmlString((menuItem != null) ? menuItem.Name : fallbackName);
        }

         

        /// <summary>
        /// Execute up one level
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T ExecuteUpOneLevel<T>(this HtmlHelper html, Func<T> action)
        {
            string originalHtmlPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            html.ViewData.TemplateInfo.HtmlFieldPrefix = html.GetHtmlFieldPrefixUpOneLevel();

            T result = action();

            html.ViewData.TemplateInfo.HtmlFieldPrefix = originalHtmlPrefix;

            return result;
        }

        /// <summary>
        /// Returns HTML attributes based on the Model Metadata.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <returns>The HTML attributes.</returns>
        public static IDictionary<string, object> GetHtmlAttributes(this HtmlHelper html)
        {
            return GetHtmlAttributes(html, "readonly");
        }
        
        /// <summary>
        /// Returns HTML attributes based on the Model Metadata.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="modelMetadata">The property model metadata that the HTML attributes are for.</param>
        /// <returns>The HTML attributes.</returns>
        public static IDictionary<string, object> GetHtmlAttributes(this HtmlHelper html, ModelMetadata modelMetadata)
        {
            return GetHtmlAttributes(html, "readonly", modelMetadata);
        }

        /// <summary>
        /// Returns HTML attributes based on the Model Metadata.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="readOnlyAttribute">What to use for the the readonly attribute if the Model is read only. Use either "readonly" or "disabled". Default is "readonly".</param>
        /// <returns>The HTML attributes.</returns>
        public static IDictionary<string, object> GetHtmlAttributes(this HtmlHelper html, string readOnlyAttribute)
        {
            return GetHtmlAttributes(html, readOnlyAttribute, html.ViewData.ModelMetadata);
        }

        /// <summary>
        /// Returns HTML attributes based on the Model Metadata.
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="readOnlyAttribute">What to use for the the readonly attribute if the Model is read only. Use either "readonly" or "disabled". Default is "readonly".</param>
        /// <param name="modelMetadata">The property model metadata that the HTML attributes are for.</param>
        /// <returns>The HTML attributes.</returns>
        public static IDictionary<string, object> GetHtmlAttributes(this HtmlHelper html, string readOnlyAttribute, ModelMetadata modelMetadata)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelper.GetHtmlAttributes");

            try
            {
#endif
                var htmlAttributes = new Dictionary<string, object>();

                if (modelMetadata.IsReadOnly)
                {
                    htmlAttributes.Add(readOnlyAttribute, readOnlyAttribute);
                }

                htmlAttributes.Add(HtmlDataType.ReadOnlyType, readOnlyAttribute);

                var trigger = modelMetadata.GetAttribute<TriggerAttribute>();

                if (trigger != null)
                {
                    htmlAttributes.MergeCssClass("rhea-trigger");
                    htmlAttributes.Add(HtmlDataType.SubmitName, ButtonAttribute.SubmitTypeName);
                    htmlAttributes.Add(HtmlDataType.SubmitType, trigger.SubmitType);
                }

                if (modelMetadata.AdditionalValues.ContainsKey(AddressViewModel.AjaxPropertyModelMetadataKey))
                {
                    htmlAttributes.MergeCssClass("rhea-ajax-geospatial-address");
                    htmlAttributes.Add(HtmlDataType.Url, UrlHelper.GenerateUrl(null, "AddressSearch", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, true));

                    if (modelMetadata.AdditionalValues.ContainsKey(AddressViewModel.ReturnLatLongData))
                    {
                        htmlAttributes.Add(HtmlDataType.LatitudeLongitude, bool.TrueString.ToLower());
                    }

                    htmlAttributes.Add(HtmlDataType.FieldPrefix, html.GetHtmlFieldPrefixUpOneLevel().Replace('.', '_'));
                }
                
                if (modelMetadata.AdditionalValues.ContainsKey(JobseekerSearchViewModel.AjaxPropertyModelMetadataKey))
                {
                    htmlAttributes.MergeCssClass("rhea-ajax-jsk-search");
                    htmlAttributes.Add(HtmlDataType.Url, UrlHelper.GenerateUrl(null, "JobseekerSearch", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, includeImplicitMvcValues: true));

                    htmlAttributes.Add(HtmlDataType.FieldPrefix, html.GetHtmlFieldPrefixUpOneLevel().Replace('.', '_'));
                }

                var dataTypeAttribute = modelMetadata.GetAttribute<DataTypeAttribute>();

                var nonNullableType = modelMetadata.ModelType.GetNonNullableType();

                if (nonNullableType.IsNumeric())
                {
                    htmlAttributes.MergeCssClass("rhea-numeric");

                    if (modelMetadata.IsNullableValueType)
                    {
                        htmlAttributes.Add(HtmlDataType.IsNullable, "true");
                    }

                    if (nonNullableType == typeof(double) || nonNullableType == typeof(decimal) || nonNullableType == typeof(float))
                    {
                        htmlAttributes.Add(HtmlDataType.Decimal, 2);
                    }
                    else
                    {
                        htmlAttributes.Add(HtmlDataType.Decimal, 0);
                    }

                    var minAttribute = modelMetadata.GetAttribute<MinAttribute>();
                    var maxAttribute = modelMetadata.GetAttribute<MaxAttribute>();
                    var rangeAttribute = modelMetadata.GetAttribute<RangeAttribute>();

                    // Set min and max values based on numeric type if no [Min], [Max] or [Range] was specified
                    if (rangeAttribute == null)
                    {
                        object minValue = null;
                        object maxValue = null;

                        if (nonNullableType == typeof(decimal))
                        {
                            minValue = decimal.MinValue;
                            maxValue = decimal.MaxValue;
                        }
                        else if (nonNullableType == typeof(float))
                        {
                            minValue = float.MinValue;
                            maxValue = float.MaxValue;
                        }
                        else if (nonNullableType == typeof(double))
                        {
                            minValue = double.MinValue;
                            maxValue = double.MaxValue;
                        }
                        else if (nonNullableType == typeof(byte))
                        {
                            minValue = byte.MinValue;
                            maxValue = byte.MaxValue;
                        }
                        else if (nonNullableType == typeof(sbyte))
                        {
                            minValue = sbyte.MinValue;
                            maxValue = sbyte.MaxValue;
                        }
                        else if (nonNullableType == typeof(short))
                        {
                            minValue = short.MinValue;
                            maxValue = short.MaxValue;
                        }
                        else if (nonNullableType == typeof(ushort))
                        {
                            minValue = ushort.MinValue;
                            maxValue = ushort.MaxValue;
                        }
                        else if (nonNullableType == typeof(int))
                        {
                            minValue = int.MinValue;
                            maxValue = int.MaxValue;
                        }
                        else if (nonNullableType == typeof(uint))
                        {
                            minValue = uint.MinValue;
                            maxValue = uint.MaxValue;
                        }
                        else if (nonNullableType == typeof(long))
                        {
                            minValue = long.MinValue;
                            maxValue = long.MaxValue;
                        }
                        else if (nonNullableType == typeof(ulong))
                        {
                            minValue = ulong.MinValue;
                            maxValue = ulong.MaxValue;
                        }

                        var added = false;

                        if (minAttribute == null && minValue != null)
                        {
                            htmlAttributes.Add("data-val-range-min", minValue);
                            added = true;
                        }
                        else if (minAttribute != null)
                        {
                            minValue = minAttribute.Minimum;
                        }

                        if (maxAttribute == null && maxValue != null)
                        {
                            htmlAttributes.Add("data-val-range-max", maxValue);
                            added = true;
                        }
                        else if (maxAttribute != null)
                        {
                            maxValue = maxAttribute.Maximum;
                        }
                        if (minValue != null && maxValue != null)
                        {
                            var minTemp = minValue.ToString();
                            var maxTemp = maxValue.ToString();
                            // Don't include client-side error checking if the number is so large it is an exponential number
                            if (added && minTemp.IndexOf('E') < 0 && minTemp.IndexOf('e') < 0 && maxTemp.IndexOf('E') < 0 && maxTemp.IndexOf('e') < 0)
                            {
                                // Include client-side error message which will cause the range checking to take place
                                htmlAttributes.Add("data-val-range", string.Format(DataAnnotationsResources.RangeAttribute_Invalid, modelMetadata.DisplayName, minValue, maxValue));
                            }
                        }
                    }

                    // Apply numeric max length
                    if (string.IsNullOrEmpty(modelMetadata.TemplateHint) || modelMetadata.TemplateHint == "Number")
                    {
                        var numericLength = modelMetadata.GetAttribute<NumericLengthAttribute>();

                        if (numericLength != null && !htmlAttributes.ContainsKey("maxlength"))
                        {
                            htmlAttributes.Add("maxlength", numericLength.MaximumLength);
                        }
                    }
                }

                MergeContingentIf(modelMetadata, htmlAttributes, "VisibleIf");
                MergeContingentIf(modelMetadata, htmlAttributes, "ReadOnlyIf");
                MergeContingentIf(modelMetadata, htmlAttributes, "EditableIf");
                MergeContingentIf(modelMetadata, htmlAttributes, "ClearIf");
                MergeContingentIf(modelMetadata, htmlAttributes, "Gst");
                MergeContingentIf(modelMetadata, htmlAttributes, "Age");
                MergeContingentIf(modelMetadata, htmlAttributes, "Copy");

                // Add the multiple tag to a multi select list, to enable the client side javascript to behave appropriately
                if (modelMetadata.ModelType == typeof(MultiSelectList) || modelMetadata.ModelType == typeof(IEnumerable<string>))
                {
                    htmlAttributes.MergeCssClass("rhea-multiple");
                }

                var ajax = modelMetadata.GetAttribute<AjaxSelectionAttribute>();

                if (ajax != null)
                {
                    htmlAttributes.MergeCssClass("rhea-ajax");

                    if (modelMetadata.IsComplexType)
                        htmlAttributes.Add(HtmlDataType.IsComplexType, "true");
                    else
                        htmlAttributes.Add(HtmlDataType.IsComplexType, "false");

                    var routeName = ajax.RouteName;

                    if (string.IsNullOrEmpty(routeName))
                    {
                        routeName = (ajax.Area == string.Empty) ? null : html.ViewContext.RouteData.GetRouteName();
                    }

                    var routeArea = new RouteValueDictionary { { "Area", ajax.Area ?? html.ViewContext.RouteData.GetArea() } };

                    var url = UrlHelper.GenerateUrl(routeName, ajax.Action, (!string.IsNullOrEmpty(ajax.Controller) ? ajax.Controller : html.ViewContext.RouteData.GetController()), routeArea, html.RouteCollection, html.ViewContext.RequestContext, true);

                    if (string.IsNullOrEmpty(url))
                    {
                        url = UrlHelper.GenerateUrl(null, ajax.Action, (!string.IsNullOrEmpty(ajax.Controller) ? ajax.Controller : html.ViewContext.RouteData.GetController()), routeArea, html.RouteCollection, html.ViewContext.RequestContext, true);
                    }

                    htmlAttributes.Add(HtmlDataType.Url, url);

                    htmlAttributes.Add(HtmlDataType.FieldPrefix, html.GetHtmlFieldPrefixUpOneLevel().Replace('.', '_'));

                    htmlAttributes.Add(HtmlDataType.Parameters, ajax.Parameters != null ? string.Join(",", ajax.Parameters) : string.Empty);

                }


                var ajaxLoad = modelMetadata.GetAttribute<AjaxLoadAttribute>();

                // Apply Ajax Load only if model is null or an actual InheritanceViewModel object
                if (ajaxLoad != null && modelMetadata.ModelType == typeof(InheritanceViewModel) && (modelMetadata.Model == null || modelMetadata.Model.GetType() == typeof(InheritanceViewModel)))
                {
                    htmlAttributes.MergeCssClass("rhea-ajaxload");

                    var routeName = ajaxLoad.RouteName;

                    if (string.IsNullOrEmpty(routeName))
                    {
                        routeName = (ajaxLoad.Area == string.Empty) ? null : html.ViewContext.RouteData.GetRouteName();
                    }

                    var routeArea = new RouteValueDictionary { { "Area", ajaxLoad.Area ?? html.ViewContext.RouteData.GetArea() } };

                    var url = UrlHelper.GenerateUrl(routeName, ajaxLoad.Action, (!string.IsNullOrEmpty(ajaxLoad.Controller) ? ajaxLoad.Controller : html.ViewContext.RouteData.GetController()), routeArea, html.RouteCollection, html.ViewContext.RequestContext, true);

                    if (string.IsNullOrEmpty(url))
                    {
                        url = UrlHelper.GenerateUrl(null, ajaxLoad.Action, (!string.IsNullOrEmpty(ajaxLoad.Controller) ? ajaxLoad.Controller : html.ViewContext.RouteData.GetController()), routeArea, html.RouteCollection, html.ViewContext.RequestContext, true);
                    }

                    htmlAttributes.Add(HtmlDataType.Url, url);

                    htmlAttributes.Add(HtmlDataType.FieldPrefix, html.GetHtmlFieldPrefixUpOneLevel().Replace('.', '_'));

                    htmlAttributes.Add(HtmlDataType.Parameters, ajaxLoad.Parameters != null ? string.Join(",", ajaxLoad.Parameters) : string.Empty);

                    htmlAttributes.Add(HtmlDataType.PropertyNameForAjax, modelMetadata.PropertyName);
                }


                htmlAttributes.Add(HtmlDataType.DisplayName, modelMetadata.GetDisplayName());

                

                if (string.IsNullOrEmpty(modelMetadata.TemplateHint)
                    || modelMetadata.TemplateHint == "String"
                    || modelMetadata.TemplateHint == "MultilineText"
                    || modelMetadata.TemplateHint == "EmailAddress"
                    || modelMetadata.TemplateHint == "PhoneNumber"
                    || modelMetadata.TemplateHint == "Password")
                {

                     
                    var stringLength = modelMetadata.GetAttribute<StringLengthAttribute>();

                    if (stringLength != null && !htmlAttributes.ContainsKey("maxlength"))
                    {
                        htmlAttributes.Add("maxlength", stringLength.MaximumLength);
                    }
                }

                if (CustomDataType.Grid.Equals(modelMetadata.DataTypeName, StringComparison.Ordinal))
                {
                    htmlAttributes.Add(HtmlDataType.SkipLinkTable, true);
                }

                //IEnumerable<SkipLinkAttribute> skipLink = modelMetadata.GetAttributes<SkipLinkAttribute>();
                if (/*skipLink.Any() && */!(CustomDataType.Grid.Equals(modelMetadata.DataTypeName, StringComparison.Ordinal)) && modelMetadata.GetAttributes<SkipLinkAttribute>().Any())
                {
                    htmlAttributes.Add(HtmlDataType.SkipLink, true);
                }

                var countdown = modelMetadata.GetAttribute<CountdownAttribute>();

                if (countdown != null)
                {
                    htmlAttributes.Add(HtmlDataType.Countdown, true);
                }

                if (nonNullableType == typeof(DateTime))
                {
                    var dataType = DataType.DateTime;
                     

                    if (dataTypeAttribute != null && (dataTypeAttribute.DataType == DataType.Date || dataTypeAttribute.DataType == DataType.Time))
                    {
                        dataType = dataTypeAttribute.DataType;
                    }

                    var dataTypeDescription = dataType == DataType.DateTime ? "date and time" : dataType.ToString().ToLower();

                    htmlAttributes.Add("data-val-date", string.Format("{0} must be a valid {1}.", modelMetadata.GetDisplayName(), dataTypeDescription));

                    if (!htmlAttributes.ContainsKey("data-val"))
                    {
                        htmlAttributes.Add("data-val", "true");
                    }
                }

                var smartAutocompleteAttribute = modelMetadata.GetAttribute<SmartAutocompleteAttribute>();
                if (smartAutocompleteAttribute != null)
                {
                    htmlAttributes.Add("data-autocomplete", "true");
                    htmlAttributes.Add("class", "textBoxAutocomplete");
                    htmlAttributes.Add(HtmlDataType.Url, UrlHelper.GenerateUrl(null, "GetData", "Report", new RouteValueDictionary{ {"Area", "Example"} }, html.RouteCollection, html.ViewContext.RequestContext, true));
                }

                return htmlAttributes;
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        private static void MergeContingentIf(ModelMetadata modelMetadata, Dictionary<string, object> htmlAttributes, string contingentType)
        {
            contingentType = contingentType.ToLower();

            if (modelMetadata.AdditionalValues.ContainsKey(contingentType))
            {
                htmlAttributes.MergeCssClass(string.Format("rhea-{0}", contingentType));

                var data = (Dictionary<string, object>)modelMetadata.AdditionalValues[contingentType];

                foreach (var kvp in data)
                {
                    htmlAttributes.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Returns an HTML select element for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <returns>An HTML select element for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            return ZeusEnumDropDownListFor(html, expression, null);
        }

        /// <summary>
        /// Returns an HTML select element for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>An HTML select element for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            return ZeusEnumDropDownListFor(html, expression, null, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>An HTML select element for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
        {
            return ZeusEnumDropDownListFor(html, expression, null, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="defaultValue">The default value if no selection has already been made.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>An HTML select element for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, Enum defaultValue, object htmlAttributes)
        {
            return ZeusEnumDropDownListFor(html, expression, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns an HTML select element for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="defaultValue">The default value if no selection has already been made.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>An HTML select element for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, Enum defaultValue, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var isEnumerable = metadata.ModelType.IsEnumerableType();
            var type = metadata.ModelType.GetNonNullableType();
            IEnumerable<object> modelAsEnumerable = Enumerable.Empty<object>();

            if (isEnumerable)
            {
                type = metadata.ModelType.IsEnumerableType() && metadata.ModelType.IsGenericType && metadata.ModelType.HasElementType ? metadata.ModelType.GetElementType() : metadata.ModelType.GetGenericArguments().FirstOrDefault();

                if (metadata.Model != null)
                {
                    modelAsEnumerable = ((IEnumerable)metadata.Model).Cast<object>();
                }
            }

            // Get default value from enum if no View Model default value is supplied
            if (defaultValue == null && ((isEnumerable && !modelAsEnumerable.Any()) || metadata.Model == null))
            {
                var defaultValueAttribute = type.GetAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null)
                {
                    defaultValue = defaultValueAttribute.Value as Enum;
                }
            }

            var values = Enum.GetValues(type).Cast<TEnum>().Distinct();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = type.GetEnumDescription(value),
                            Value = value.ToString(),
                            Selected = isEnumerable ? (!modelAsEnumerable.Any() && defaultValue != null ? value.Equals(defaultValue) : modelAsEnumerable.Any(value.Equals)) : ((metadata.Model == null || (int)metadata.Model == 0) && defaultValue != null ? value.Equals(defaultValue) : value.Equals(metadata.Model))
                        };

            if (isEnumerable)
            {
                // Get all items
                if (!items.Any())
                {
                    htmlAttributes.Add("disabled", "disabled");
                }

                if (html.ViewData.ModelMetadata.DataTypeName == CustomDataType.CheckBoxList)
                {
                    return html.CheckBoxList(items, htmlAttributes);
                }

                htmlAttributes.Add("multiple", "multiple");

                // Include JSON of selected items 
                var selectedListItems = new List<object>();

                foreach (var item in items)
                {
                    if (item.Selected)
                    {
                        selectedListItems.Add(new { Value = item.Value, Text = item.Text });
                    }
                }

                htmlAttributes.Add(HtmlDataType.MultiSelect, Json.Encode(selectedListItems));
            }
            else
            {
                // Add an empty selection for single select
                items = new[] { new SelectListItem { Value = string.Empty, Text = " " } }.Concat(items);
            }

            var fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(string.Empty);

            var stringBuilder = new StringBuilder();

            foreach (SelectListItem item in items)
            {
                var option = new TagBuilder("option")
                {
                    InnerHtml = html.Encode(item.Text)
                };

                if (item.Value != null)
                {
                    option.Attributes["value"] = item.Value;
                }

                if (item.Selected)
                {
                    option.Attributes["selected"] = "selected";
                }

                stringBuilder.AppendLine(option.ToString(TagRenderMode.Normal));
            }
            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = stringBuilder.ToString()
            };

            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);
            tagBuilder.GenerateId(fullHtmlFieldName);

            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) && modelState.Errors.Count > 0)
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            tagBuilder.MergeAttributes(html.GetUnobtrusiveValidationAttributes(html.ViewData.ModelMetadata.PropertyName, html.ViewData.ModelMetadata));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Returns a HTML radio button group for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <returns>A HTML radio button group for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumRadioButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            return ZeusEnumRadioButtonGroupFor(html, expression, null);
        }

        /// <summary>
        /// Returns a HTML radio button group for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>A HTML radio button group for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumRadioButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            return ZeusEnumRadioButtonGroupFor(html, expression, null, htmlAttributes);
        }

        /// <summary>
        /// Returns a HTML radio button group for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>A HTML radio button group for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumRadioButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
        {
            return ZeusEnumRadioButtonGroupFor(html, expression, null, htmlAttributes);
        }

        /// <summary>
        /// Returns a HTML radio button group for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="defaultValue">The default value if no selection has already been made.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>A HTML radio button group for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumRadioButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, Enum defaultValue, object htmlAttributes)
        {
            return ZeusEnumRadioButtonGroupFor(html, expression, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns a HTML radio button group for the enum in the object that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the enum to display.</param>
        /// <param name="defaultValue">The default value if no selection has already been made.</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the HTML select element.</param>
        /// <returns>A HTML radio button group for the enum in the object that is represented by the expression.</returns>
        public static MvcHtmlString ZeusEnumRadioButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, Enum defaultValue, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var isEnumerable = metadata.ModelType.IsEnumerableType();

            if (isEnumerable)
            {
                return html.EnumDropDownListFor(expression, defaultValue.ToString(), htmlAttributes);
            }

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            var isHorizontal = metadata.DataTypeName == CustomDataType.RadioButtonGroupHorizontal;

            var type = metadata.ModelType.GetNonNullableType();
            IEnumerable<object> modelAsEnumerable = Enumerable.Empty<object>();

            // Get default value from enum if no View Model default value is supplied
            if (defaultValue == null && ((isEnumerable && !modelAsEnumerable.Any()) || metadata.Model == null))
            {
                var defaultValueAttribute = type.GetAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null)
                {
                    defaultValue = defaultValueAttribute.Value as Enum;
                }
            }

            var values = Enum.GetValues(type).Cast<TEnum>().Distinct();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = type.GetEnumDescription(value),
                            Value = value.ToString(),
                            Selected = isEnumerable ? (!modelAsEnumerable.Any() && defaultValue != null ? value.Equals(defaultValue) : modelAsEnumerable.Any(value.Equals)) : (metadata.Model == null && defaultValue != null ? value.Equals(defaultValue) : value.Equals(metadata.Model))
                        };

            // If the enum is nullable, add an empty selection
            if (metadata.IsNullableValueType)
            {
                items = new[] { new SelectListItem { Value = string.Empty, Text = " " } }.Concat(items);
            }

            htmlAttributes.Add(HtmlDataType.RadioButtonGroup, true);

            html.GetUnobtrusiveValidationAttributes(html.ViewData.ModelMetadata.PropertyName, html.ViewData.ModelMetadata).ForEach(a => { if (!htmlAttributes.ContainsKey(a.Key)) { htmlAttributes.Add(a.Key, a.Value); } });

            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append("<br>");

            int i = 0;
            int total = items.Count();
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    total--;
                    continue;
                }

                // Generate an id to be given to the radio button field (first id matches main label "for" target id)
                var id = i == 0 ? metadata.PropertyName : string.Format("{0}-{1}", metadata.PropertyName, i);
                i++;

                if (htmlAttributes.ContainsKey("id"))
                {
                    htmlAttributes["id"] = html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id));
                }
                else
                {
                    htmlAttributes.Add("id", html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id)));
                }

                var itemAccessor = item; // Prevent access to modified closure
                stringBuilder.Append(html.ExecuteUpOneLevel(() => html.RadioButton(metadata.PropertyName, itemAccessor.Value, itemAccessor.Selected, htmlAttributes)));
                stringBuilder.Append(html.ExecuteUpOneLevel(() => html.Label(id, itemAccessor.Text)));

                if (!isHorizontal && i < total)
                {
                    stringBuilder.Append("<br>");
                }
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>Returns HTML markup for each property in the object that is represented by the <see cref="T:System.Linq.Expressions.Expression" /> expression.</summary>
        /// <returns>The HTML markup for each property in the object that is represented by the expression.</returns>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the value.</typeparam>
        public static MvcHtmlString ZeusEnumDisplayFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            return ZeusEnumDisplayFor(html, expression, null);
        }

        /// <summary>Returns HTML markup for each property in the object that is represented by the <see cref="T:System.Linq.Expressions.Expression" /> expression.</summary>
        /// <returns>The HTML markup for each property in the object that is represented by the expression.</returns>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <param name="defaultValue"> </param>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the value.</typeparam>
        public static MvcHtmlString ZeusEnumDisplayFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, Enum defaultValue)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var type = metadata.ModelType.GetNonNullableType();

            // Get default value from enum if no View Model default value is supplied
            if (defaultValue == null && metadata.Model == null)
            {
                var defaultValueAttribute = type.GetAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null)
                {
                    defaultValue = defaultValueAttribute.Value as Enum;
                }
            }

            return new MvcHtmlString(metadata.Model == null && defaultValue != null ? defaultValue.GetDescription() : metadata.ModelType.GetEnumDescription(metadata.Model));
        }

        /// <summary>
        /// Returns the HtmlFieldPrefix moved up one level.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        /// <returns>The HtmlFieldPrefix moved up one level.</returns>
        public static string GetHtmlFieldPrefixUpOneLevel(this HtmlHelper html)
        {
            if (string.IsNullOrEmpty(html.ViewData.TemplateInfo.HtmlFieldPrefix) || html.ViewData.TemplateInfo.HtmlFieldPrefix.IndexOf('.') < 0)
            {
                return string.Empty;
            }

            return html.ViewData.TemplateInfo.HtmlFieldPrefix.Substring(0, html.ViewData.TemplateInfo.HtmlFieldPrefix.LastIndexOf('.'));
        }
        
        /// <summary>
        /// Shows the menu items.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        public static MvcHtmlString ShowMenu(this HtmlHelper html)
        {
            return MenuService.ShowMenu(html);
        }

        /// <summary>
        /// Returns whether the user can see a menu item.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="area">The area.</param>
        /// <returns><c>true</c> if the user can see the menu item; otherwise, <c>false</c>.</returns>
        public static bool CanSeeMenuItem(this HtmlHelper html, string action, string controller, string area)
        {
            var identity = UserService.Identity;
            return MenuService.MenuItems.Any(m => string.Equals(m.Action, action, StringComparison.Ordinal) && string.Equals(m.Controller, controller, StringComparison.Ordinal) && string.Equals(m.Area, area, StringComparison.Ordinal) && m.IsAuthorized(identity));
        }

        /// <summary>
        /// Renders the bulletin items up to a specified limit. 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString ShowBulletins(this HtmlHelper html)
        {
            int bulletinLimit;

            if (int.TryParse(ConfigurationManager.AppSettings.Get("BulletinListLimit"), out bulletinLimit))
            {
                return ShowBulletins(html, bulletinLimit);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Show bulletins.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="limit">Limit the number of bulletins shown.</param>
        /// <returns>The rendered bulletins.</returns>
        public static MvcHtmlString ShowBulletins(this HtmlHelper html, int limit)
        {
            var bulletins = BulletinService.List(BulletinType.RJCP, limit);
            var list = new TagBuilder("ul");
            
            foreach (var bulletinItem in bulletins)
            {
                var item = new TagBuilder("li");
                var span = new TagBuilder("span");
                var anchor = new TagBuilder("a");

                span.AddCssClass("title");
                span.SetInnerText(string.Format("{0} ", bulletinItem.LiveDate.ToShortDateString()));
                
                anchor.SetInnerText(bulletinItem.Title);
                anchor.Attributes.Add("title", string.Format("{0} {1}", bulletinItem.LiveDate.ToShortDateString(), bulletinItem.Title));
                anchor.Attributes.Add("href", UrlHelper.GenerateUrl(html.ViewContext.RouteData.GetRouteName(), "Bulletin", "Default", new RouteValueDictionary { { "Area", string.Empty }, { "id", bulletinItem.PageId } }, html.RouteCollection, html.ViewContext.RequestContext, true));

                span.InnerHtml += anchor.ToString(TagRenderMode.Normal);
                item.InnerHtml = span.ToString(TagRenderMode.Normal);
                list.InnerHtml += item.ToString(TagRenderMode.Normal);
            }

            return MvcHtmlString.Create(list.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders the users history items.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        /// <param name="historyType">The history type.</param>
        /// <returns>Returns HTML markup for the users history.</returns>
        public static MvcHtmlString ShowHistory(this HtmlHelper html, HistoryType historyType)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("HtmlHelper.ShowHistory ({0})", historyType));

            try
            {
#endif
                var historyTypeDescription = historyType.GetDescription();
                var control = new StringBuilder();
                control.Append("<div class=\"history\">");
                control.Append("<h2>Last accessed</h2>");

                var data = UserService.History.Get(historyType);
                if (data == null || !data.Any())
                {
                    control.Append("No recent " + html.Encode(historyTypeDescription) + " records");
                }
                else
                {
                    int pageSize = 10;
                    var historySection = ConfigurationManager.GetSection<HistorySection>("history");

                    if (historySection != null && historySection.PageSize > 0)
                    {
                        pageSize = historySection.PageSize;
                    }

                    HistoryPageMetadata metadata = new HistoryPageMetadata
                    {
                        HistoryType = historyType,
                        PageSize = pageSize,
                        PageNumber = 1,
                        Total = data.Count()
                    };

                    var recentHistory = data.ToHistoryModelList(new Pageable<HistoryModel>(metadata));

                    var list = new TagBuilder("ul");
                    string id = ("history_" + html.Encode(historyTypeDescription)).Replace(' ', '_'); // spaces aren't allowed in id attributes
                    list.MergeAttribute("id", id);
                    list.MergeAttribute(HtmlDataType.PinnedCount, data.Count(d => d.IsPinned).ToString());
                    list.InnerHtml = GetHistoryItems(html, historyType, recentHistory).ToString();

                    control.Append(list.ToString(TagRenderMode.Normal));

                    //setup metadata for next page;
                    recentHistory.Metadata.PageNumber++;
                    if (recentHistory.Metadata.HasMorePages())
                    {
                        var more = new TagBuilder("a");
                        more.AddCssClass("requestMore rhea-paged");
                        more.MergeAttribute("href", "#");
                        more.MergeAttribute("id", string.Format("{0}_more", id));
                        more.MergeAttribute(HtmlDataType.PropertyIdGrid, id);
                        more.MergeAttribute(HtmlDataType.PagedMetadata, recentHistory.Metadata.Serialize());
                        more.MergeAttribute(HtmlDataType.Url, UrlHelper.GenerateUrl("Default", "HistoryNextPage", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, false));

                        var moreSpan = new TagBuilder("span");
                        moreSpan.InnerHtml = string.Format("load {0} more <span class=\"readers\">recently accessed {1} records</span>", recentHistory.Metadata.PageSize, historyTypeDescription);
                        more.InnerHtml = moreSpan.ToString(TagRenderMode.Normal);

                        control.Append(more.ToString(TagRenderMode.Normal));
                    }
                }
                control.Append("</div>");
                return MvcHtmlString.Create(control.ToString());
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Renders the inner list items for the recent history menu. It is IPageable context sensitive.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="historyType">Type of the history.</param>
        /// <param name="recentHistory">The recent history.</param>
        /// <returns>Returns the current page of inner list items html for the recent history menu.</returns>
        public static MvcHtmlString GetHistoryItems(this HtmlHelper html, HistoryType historyType, IPageable<HistoryModel> recentHistory)
        {
            var historySection = ConfigurationManager.GetSection<HistorySection>("history");

            if (historySection != null)
            {
                RouteData routeData = html.ViewContext.RouteData;

                StringBuilder items = new StringBuilder();
                foreach (HistoryModel history in recentHistory.GetCurrentPage())
                {
                    string valuesAsQueryString = history.Values.ToQueryString();
                    TagBuilder item = new TagBuilder("li");
                    item.MergeAttribute(HtmlDataType.ObjectValues, valuesAsQueryString);
                    TagBuilder anchor = new TagBuilder("a");

                    // Grab a copy of the current route value collection to modify
                    RouteValueDictionary routeValues = new RouteValueDictionary { };
                    foreach (string key in routeData.Values.Keys)
                    {
                        routeValues[key] = routeData.Values[key];
                    }

                    if (history.Values != null)
                    {
                        history.Values.ForEach(v => routeValues[v.Key] = v.Value);
                    }

                    string href = UrlHelper.GenerateUrl(routeValues.GetRouteName(), routeData.GetAction(), routeData.GetController(), routeValues, html.RouteCollection, html.ViewContext.RequestContext, false);

                    anchor.Attributes.Add("href", href);
                    anchor.SetInnerText(history.DisplayName);

                    TagBuilder pin = new TagBuilder("a");
                    TagBuilder icon = new TagBuilder("i");
                    TagBuilder readerSpan = new TagBuilder("span");
                    readerSpan.AddCssClass("readers");
                    icon.AddCssClass("fa");
                    icon.AddCssClass("fa-2x");
                    pin.AddCssClass("history-pin");

                    pin.MergeAttribute("href", "#");
                    pin.MergeAttribute(HtmlDataType.HistoryType, historyType.ToString());
                    pin.MergeAttribute(HtmlDataType.HistoryDescription, historyType.GetDescription());
                    pin.MergeAttribute(HtmlDataType.ObjectValues, history.Values.ToQueryString());
                    if (history.IsPinned)
                    {
                        pin.AddCssClass("pinned");
                        readerSpan.InnerHtml = "Unpin from list";
                        icon.AddCssClass("fa-bookmark");
                        pin.MergeAttribute(HtmlDataType.Url, UrlHelper.GenerateUrl(null, "UnpinHistory", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, true));
                    }
                    else
                    {
                        readerSpan.InnerHtml = "Pin to list";
                        icon.AddCssClass("fa-bookmark-o");
                        pin.MergeAttribute(HtmlDataType.Url, UrlHelper.GenerateUrl(null, "PinHistory", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, true));
                    }
                    pin.InnerHtml = readerSpan.ToString() + icon.ToString();

                    item.InnerHtml = anchor.ToString(TagRenderMode.Normal) + pin.ToString();

                    items.Append(item.ToString(TagRenderMode.Normal));
                }

                return MvcHtmlString.Create(items.ToString());
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Render a button.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="name">Display name of button.</param>
        /// <returns>The rendered button.</returns>
        public static MvcHtmlString Button(this HtmlHelper html, string name)
        {
            return Button(html, name, name);
        }

        /// <summary>
        /// Render a button.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="name">Display name of button.</param>
        /// <param name="value">Value to submit for button.</param>
        /// <returns>The rendered button.</returns>
        public static MvcHtmlString Button(this HtmlHelper html, string name, string value)
        {
            return Button(html, name, value, false, false, false, false, false, false, null, null, false);
        }


        /// <summary>
        /// Render a button.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="button">The button.</param>
        /// <returns>The rendered button.</returns>
        public static MvcHtmlString Button(this HtmlHelper html, ButtonAttribute button)
        {
            return Button(html, button.Name, button.GetValue(), !string.IsNullOrEmpty(button.SplitButtonParent), button.Reset, button.Clear, button.Primary, button.Cancel, button.SkipClientSideValidation, null, button.Roles, button.ResultsInDownload);
        }

        /// <summary>
        /// Render a button.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="button">The button.</param>
        /// <param name="htmlAttributes">The HTML attributes to give the button.</param>
        /// <returns>The rendered button.</returns>
        public static MvcHtmlString Button(this HtmlHelper html, ButtonAttribute button, object htmlAttributes)
        {
            return Button(html, button.Name, button.GetValue(), !string.IsNullOrEmpty(button.SplitButtonParent), button.Reset, button.Clear, button.Primary, button.Cancel, button.SkipClientSideValidation, htmlAttributes, button.Roles, button.ResultsInDownload);
        }

        /// <summary>
        /// Render a button.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="name">Display name of button.</param>
        /// <param name="value">Value to submit for button.</param>
        /// <param name="imitateAnchor">Whether it should wrap itself in an a tag to imitate anchor styling</param>
        /// <param name="reset">Whether it is a reset button.</param>
        /// <param name="clear">Whether the button will clear the form fields along with any pre-set values.</param>
        /// <param name="primary">Whether it is a primary button.</param>
        /// <param name="cancel">Whether it is a cancel button.</param>
        /// <param name="skipClientSideValidation">Whether the client-side validation should be skipped when the button is selected.</param>
        /// <param name="htmlAttributes">The HTML attributes to give the button.</param>
        /// <param name="roles">If specified, the roles the user must have to see the button.</param>
        /// <param name="resultsInDownload">Whether this button results in the downloading of a file.</param>
        /// <returns>The rendered button.</returns>
        public static MvcHtmlString Button(this HtmlHelper html, string name, string value, bool imitateAnchor, bool reset, bool clear, bool primary, bool cancel, bool skipClientSideValidation, object htmlAttributes, string[] roles, bool resultsInDownload)
        {
            // Don't render button it it requires roles and the user is not in one of them
            if (roles != null && !UserService.IsInRole(roles))
            {
                return MvcHtmlString.Empty;
            }

            var att = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            MvcHtmlString result;
            

            if (clear)
            {
                // Build button for Clear and set name to 'Clear' (overrides the name of button to maintain consistency)
                name = "Clear";
                // <input type = 'button' name = {ButtonHandlerAttribute.Name} value = "Clear" class = "button" />
                var inputTag = new TagBuilder("button");
                inputTag.Attributes.Add("type", "submit");
                inputTag.Attributes.Add("name", ButtonAttribute.SubmitTypeName);
                inputTag.Attributes.Add("value", value);
                // add unique attribute to identify and retrieve button from jQuery.
                inputTag.Attributes.Add(HtmlDataType.ButtonClear, "true");
                if (!imitateAnchor) inputTag.AddCssClass("btn btn-default");
                inputTag.SetInnerText(name);
                if (htmlAttributes != null && htmlAttributes.GetType().IsGenericType && htmlAttributes.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var hA = (Dictionary<string, object>)htmlAttributes; // casts the htmlAttributes to Dictionary<string,ojbect>
                    if (hA.ContainsKey("class"))
                    {
                        inputTag.AddCssClass(hA["class"].ToString());
                        hA.Remove("class");
                    }
                    inputTag.MergeAttributes(hA);
                }
                else
                {
                    inputTag.MergeAttributes(att);
                }
                if (skipClientSideValidation)
                {
                    inputTag.AddCssClass("cancel");
                }
               
                result = MvcHtmlString.Create(inputTag.ToString(TagRenderMode.Normal));

            }
            else if (reset)
            {
                //Set the default name to button 
                value = "Reset";

                // <input type="reset" name="{ButtonHandlerAttribute.Name}" value="{'Reset'}" class="button" />
                var inputTag = new TagBuilder("input");

                inputTag.Attributes.Add("type", "reset");
                inputTag.Attributes.Add("name", ButtonAttribute.SubmitTypeName);
                inputTag.Attributes.Add("value", value);
                if (!imitateAnchor) inputTag.AddCssClass("btn btn-default reset");

                if (htmlAttributes != null && htmlAttributes.GetType().IsGenericType && htmlAttributes.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var hA = (Dictionary<string, object>)htmlAttributes;

                    if (hA.ContainsKey("class"))
                    {
                        inputTag.AddCssClass(hA["class"].ToString());
                        hA.Remove("class");
                    }

                    inputTag.MergeAttributes(hA);
                }
                else
                {
                    inputTag.MergeAttributes(att);
                }

                if (skipClientSideValidation)
                {
                    inputTag.AddCssClass("cancel");
                }

                result = MvcHtmlString.Create(inputTag.ToString(TagRenderMode.SelfClosing));
            }
            else
            {
                // Produces: <button type="submit" name="{ButtonAttribute.SubmitTypeName}" value="{value}" {disabled="disabled"}>{name}</button>
                var tag = new TagBuilder("button");

                tag.Attributes.Add("type", "submit");
                tag.Attributes.Add("name", ButtonAttribute.SubmitTypeName);
                tag.Attributes.Add("value", value);
                tag.SetInnerText(name);

                if (!imitateAnchor)
                {
                    tag.AddCssClass("btn");

                    if (primary)
                    {
                        tag.AddCssClass("btn-primary");
                    }
                    else
                    {
                        tag.AddCssClass("btn-inverse");
                    }

                    if (cancel)
                    {
                        tag.AddCssClass("cancel");
                    }
                }
                if (resultsInDownload)
                {
                    tag.Attributes.Add(HtmlDataType.IsDownload, "true");
                }

                if (htmlAttributes != null && htmlAttributes.GetType().IsGenericType && htmlAttributes.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var hA = (Dictionary<string, object>)htmlAttributes;

                    if (hA.ContainsKey("class"))
                    {
                        tag.AddCssClass(hA["class"].ToString());
                        hA.Remove("class");
                    }

                    tag.MergeAttributes(hA);
                }
                else
                {
                    tag.MergeAttributes(att);
                }

                if (skipClientSideValidation)
                {
                    tag.AddCssClass("cancel");
                }

                result = MvcHtmlString.Create(tag.ToString());
            }

            if (imitateAnchor)
            {
                return new MvcHtmlString("<a>" + result + "</a>");
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Get an Adw Code description
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string AdwGetCodeDescription(this HtmlHelper html, string codeType, string code)
        {
            return AdwService.GetListCodeDescription(codeType, code);
        }

        /// <summary>
        /// Adw List code in select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType)
        {
            return AdwListCode(html, codeType, string.Empty);
        }

        /// <summary>
        /// Adw list code in select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, string selectedCode)
        {
            return AdwListCode(html, codeType, selectedCode, string.Empty);
        }

        /// <summary>
        /// Adw list code in seleact list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCode"></param>
        /// <param name="startingCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, string selectedCode, string startingCode)
        {
            return AdwListCode(html, codeType, selectedCode, startingCode, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list code in seleact list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCode"></param>
        /// <param name="startingCode"></param>
        /// <param name="allowEmpty"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, string selectedCode, string startingCode, bool allowEmpty, SelectionType selectionType)
        {
            var response = AdwService.GetListCodes(codeType, startingCode);

            var list = new List<SelectListItem>();

            if (allowEmpty && selectionType != SelectionType.Default)
            {
                list.Add(new SelectListItem { Value = string.Empty, Text = " " });
            }

            foreach (var item in response)
            {
                list.Add(new SelectListItem { Text = item.Description, Value = item.Code, Selected = (item.Code == selectedCode) });
            }

            return list;
        }

        /// <summary>
        /// Adw list code in seleact list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCodes"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, IEnumerable<string> selectedCodes)
        {
            return AdwListCode(html, codeType, selectedCodes, string.Empty, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list code in seleact list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCodes"></param>
        /// <param name="startingCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, IEnumerable<string> selectedCodes, string startingCode)
        {
            return AdwListCode(html, codeType, selectedCodes, startingCode, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list code in seleact list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="codeType"></param>
        /// <param name="selectedCodes"></param>
        /// <param name="startingCode"></param>
        /// <param name="allowEmpty"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListCode(this HtmlHelper html, string codeType, IEnumerable<string> selectedCodes, string startingCode, bool allowEmpty, SelectionType selectionType)
        {
            var response = AdwService.GetListCodes(codeType, startingCode);

            var list = new List<SelectListItem>();

            if (allowEmpty && selectionType != SelectionType.Default)
            {
                list.Add(new SelectListItem { Value = string.Empty, Text = " " });
            }

            foreach (var item in response)
            {
                list.Add(new SelectListItem { Text = item.Description, Value = item.Code, Selected = (selectedCodes.Contains(item.Code)) });
            }

            return list;
        }


        /// <summary>
        /// Adw related code description
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string AdwGetRelatedCodeDescription(this HtmlHelper html, string relatedCodeType, string searchCode, string code)
        {
            return AdwGetRelatedCodeDescription(html, relatedCodeType, searchCode, code, true);
        }

        /// <summary>
        /// Adw related code description
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="code"></param>
        /// <param name="dominantSearch"></param>
        /// <returns></returns>
        public static string AdwGetRelatedCodeDescription(this HtmlHelper html, string relatedCodeType, string searchCode, string code, bool dominantSearch)
        {
            return AdwService.GetRelatedCodeDescription(relatedCodeType, searchCode, code, dominantSearch);
        }

        /// <summary>
        /// Adw list related code as list of selectable
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, true);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="selectedCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, string selectedCode)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, true, selectedCode);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, dominantSearch, string.Empty, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <param name="selectedCode"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch, string selectedCode)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, dominantSearch, selectedCode, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <param name="allowEmpty"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch, bool allowEmpty)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, dominantSearch, string.Empty, allowEmpty, SelectionType.Default);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <param name="selectedCode"></param>
        /// <param name="allowEmpty"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch, string selectedCode, bool allowEmpty, SelectionType selectionType)
        {
            var response = AdwService.GetRelatedCodes(relatedCodeType, searchCode, dominantSearch).ToCodeModelList();

            var list = new List<SelectListItem>();

            if (allowEmpty && selectionType != SelectionType.Default)
            {
                list.Add(new SelectListItem { Value = string.Empty, Text = " " });
            }

            foreach (var item in response)
            {
                list.Add(new SelectListItem { Text = item.Description, Value = item.Code, Selected = (item.Code == selectedCode) });
            }

            return list;
        }


        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <param name="selectedCodes"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch, IEnumerable<string> selectedCodes)
        {
            return AdwListRelatedCode(html, relatedCodeType, searchCode, dominantSearch, selectedCodes, false, SelectionType.Default);
        }

        /// <summary>
        /// Adw list related code as select list
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relatedCodeType"></param>
        /// <param name="searchCode"></param>
        /// <param name="dominantSearch"></param>
        /// <param name="selectedCodes"></param>
        /// <param name="allowEmpty"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public static List<SelectListItem> AdwListRelatedCode(this HtmlHelper html, string relatedCodeType, string searchCode, bool dominantSearch, IEnumerable<string> selectedCodes, bool allowEmpty, SelectionType selectionType)
        {
            var response = AdwService.GetRelatedCodes(relatedCodeType, searchCode, dominantSearch).ToCodeModelList();

            var list = new List<SelectListItem>();

            if (allowEmpty && selectionType != SelectionType.Default)
            {
                list.Add(new SelectListItem { Value = string.Empty, Text = " " });
            }

            foreach (var item in response)
            {
                list.Add(new SelectListItem { Text = item.Description, Value = item.Code, Selected = (selectedCodes.Contains(item.Code)) });
            }

            return list;
        }

        /// <summary>
        /// Render link at class level.
        /// </summary>
        public static MvcHtmlString RenderLink(this HtmlHelper html, LinkAttribute link, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, bool suppressKey)
        {
            return RenderLink(html, link, modelPropertiesMetadata, link.IsConditionMet(model), html.ViewData.TemplateInfo.HtmlFieldPrefix, suppressKey);
        }

        /// <summary>
        /// Render link at property level.
        /// </summary>
        public static MvcHtmlString RenderLink(this HtmlHelper html, LinkAttribute link, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, string propertyName, object propertyValue, bool suppressKey)
        {
            return RenderLink(html, link, modelPropertiesMetadata, link.IsConditionMet(propertyName, propertyValue, model), html.ViewData.TemplateInfo.HtmlFieldPrefix, suppressKey);
        }

        /// <summary>
        /// Render link at property level.
        /// </summary>
        public static MvcHtmlString RenderLink(this HtmlHelper html, LinkAttribute link, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, string propertyName, object propertyValue, string fieldPrefix, bool suppressKey)
        {
            return RenderLink(html, link, modelPropertiesMetadata, link.IsConditionMet(propertyName, propertyValue, model), fieldPrefix, suppressKey);
        }

        /// <summary>
        /// Render link.
        /// </summary>
        internal static MvcHtmlString RenderLink(this HtmlHelper html, LinkAttribute link, IEnumerable<ModelMetadata> modelPropertiesMetadata, bool conditionMet, string fieldPrefix, bool suppressKey)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelperExtension.RenderLink");

            try
            {
#endif
            // Return empty string if link is not configured correctly or the link requires the user has a particular role but the user doesn't belong to it
            if (link == null || string.IsNullOrEmpty(link.Name) || (link.Roles != null && !UserService.IsInRole(link.Roles)))
            {
                return MvcHtmlString.Empty;
            }

            var routes = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(link.Area))
            {
                routes.Add("area", link.Area);
            }

            if (link.Parameters != null)
            {
                for (int i = 0; i < link.Parameters.Length; i++)
                {
                    var parameter = link.Parameters[i];
                   // ModelMetadata dependentProperty = null;
                    foreach (ModelMetadata p in modelPropertiesMetadata)
                    {
                        if ((String.Equals(p.PropertyName, parameter, StringComparison.Ordinal)))
                        {
                            routes.Add(parameter, p.Model);
                            break;
                        }
                    }

                    //if (dependentProperty != null)
                    //{
                    //    routes.Add(parameter, dependentProperty.Model);
                    //}
                }
            }
            else
            {
                if (!suppressKey)
                {
                    var keyProperty = GridHelper.GetKeyMetadata(modelPropertiesMetadata);

                    if (keyProperty != null)
                    {
                        routes.Add(keyProperty.PropertyName, keyProperty.Model);
                    }
                }
            }

            var htmlAttributes = new Dictionary<string, object>();

            

            if (link.SkipClientSideValidation)
            {
                htmlAttributes.MergeCssClass("cancel");
            }

            if (!string.IsNullOrEmpty(link.RouteName))
            {
                routes.Add("action", link.Action);
                routes.Add("controller", link.Controller);
            }


            if (!string.IsNullOrEmpty(link.PropertyNameForAjax))
            {
                htmlAttributes.Add(HtmlDataType.PropertyNameForAjax, link.PropertyNameForAjax);
            }
            

            // Return link as normal if no dependency check is needed
            if (link.DependencyType == ActionForDependencyType.None)
            {
                if (link.OpensInNewTab)
                {
                    htmlAttributes.Add("target", "_blank");
                }

                if (link.Cancel)
                {
                    htmlAttributes.MergeCssClass("floatLeft");
                }

                if (!string.IsNullOrEmpty(link.RouteName))
                {
                    return html.ExecuteUpOneLevel(() => html.RouteLink(link.Name, link.RouteName, routes, htmlAttributes));
                }

                return html.ExecuteUpOneLevel(() => html.ActionLink(link.Name, link.Action, link.Controller, routes, htmlAttributes));
            }

            

            htmlAttributes.MergeCssClass("rhea-actionif");
            htmlAttributes.Add(HtmlDataType.FieldPrefix, fieldPrefix != null ? fieldPrefix.Replace('.', '_') : string.Empty);
            htmlAttributes.Add(HtmlDataType.ActionForDependencyType, link.DependencyType);
            htmlAttributes.Add(HtmlDataType.DependentProperty, link.DependentProperty);
            htmlAttributes.Add(HtmlDataType.ComparisonType, link.ComparisonType);
            htmlAttributes.Add(HtmlDataType.DependentValue, link.DependentValue);
            htmlAttributes.Add(HtmlDataType.Type, "link");

            var wrapper = new TagBuilder("span");

            wrapper.MergeAttributes(htmlAttributes);

            htmlAttributes = new Dictionary<string, object>();

            if (link.OpensInNewTab)
            {
                htmlAttributes.Add("target", "_blank");
            }

            if (link.Cancel)
            {
                htmlAttributes.MergeCssClass("floatLeft");
            }


            switch (link.DependencyType)
            {
                case ActionForDependencyType.Enabled:
                    TagBuilder spanTag1 = new TagBuilder("span");
                    spanTag1.AddCssClass("input-button-disabled");

                    if (conditionMet)
                    {
                        spanTag1.AddCssClass("hidden");
                    }
                    else
                    {
                        htmlAttributes["class"] = "hidden";
                    }

                    spanTag1.SetInnerText(link.Name);

                    //Renders a link.
                        wrapper.InnerHtml = spanTag1.ToString(TagRenderMode.Normal) + html.ExecuteUpOneLevel(() => (!string.IsNullOrEmpty(link.RouteName)) ? html.RouteLink(link.Name, link.RouteName, routes, htmlAttributes) : html.ActionLink(link.Name, link.Action, link.Controller, routes, htmlAttributes));
                    break;
                case ActionForDependencyType.Disabled:
                    TagBuilder spanTag = new TagBuilder("span");
                    spanTag.AddCssClass("input-button-disabled");

                    if (!conditionMet)
                    {
                        spanTag.AddCssClass("hidden");
                    }
                    else
                    {
                        htmlAttributes["class"] = "hidden";
                    }

                    spanTag.SetInnerText(link.Name);

                        wrapper.InnerHtml = spanTag.ToString(TagRenderMode.Normal) + html.ExecuteUpOneLevel(() => (!string.IsNullOrEmpty(link.RouteName)) ? html.RouteLink(link.Name, link.RouteName, routes, htmlAttributes) : html.ActionLink(link.Name, link.Action, link.Controller, routes, htmlAttributes));

                    break;
                case ActionForDependencyType.Visible:
                    if (!conditionMet)
                    {
                        wrapper.AddCssClass("hidden");
                    }
                    wrapper.InnerHtml = html.ExecuteUpOneLevel(() => (!string.IsNullOrEmpty(link.RouteName)) ? html.RouteLink(link.Name, link.RouteName, routes, htmlAttributes) : html.ActionLink(link.Name, link.Action, link.Controller, routes, htmlAttributes)).ToString();
                    break;
                case ActionForDependencyType.Hidden:
                    if (conditionMet)
                    {
                        wrapper.AddCssClass("hidden");
                    }
                    wrapper.InnerHtml = html.ExecuteUpOneLevel(() => (!string.IsNullOrEmpty(link.RouteName)) ? html.RouteLink(link.Name, link.RouteName, routes, htmlAttributes) : html.ActionLink(link.Name, link.Action, link.Controller, routes, htmlAttributes)).ToString();
                    break;
            }

                return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));

#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Render external link at class level.
        /// </summary>
        public static MvcHtmlString RenderExternalLink(this HtmlHelper html, ExternalLinkAttribute externalLink, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model)
        {
            return RenderExternalLink(html, externalLink, modelPropertiesMetadata, externalLink.IsConditionMet(model), html.ViewData.TemplateInfo.HtmlFieldPrefix, null);
        }

        /// <summary>
        /// Render external link at property level.
        /// </summary>
        public static MvcHtmlString RenderExternalLink(this HtmlHelper html, ExternalLinkAttribute externalLink, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, string propertyName, object propertyValue)
        {
            return RenderExternalLink(html, externalLink, modelPropertiesMetadata, externalLink.IsConditionMet(propertyName, propertyValue, model), html.ViewData.TemplateInfo.HtmlFieldPrefix, null);
        }

        /// <summary>
        /// Render external link at property level.
        /// </summary>
        public static MvcHtmlString RenderExternalLink(this HtmlHelper html, ExternalLinkAttribute externalLink, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, string propertyName, object propertyValue, string fieldPrefix)
        {
            return RenderExternalLink(html, externalLink, modelPropertiesMetadata, externalLink.IsConditionMet(propertyName, propertyValue, model), fieldPrefix, null);
        }

        /// <summary>
        /// Render external link at property level.
        /// </summary>
        public static MvcHtmlString RenderExternalLink(this HtmlHelper html, ExternalLinkAttribute externalLink, IEnumerable<ModelMetadata> modelPropertiesMetadata, object model, string propertyName, object propertyValue, string fieldPrefix, string overrideTitle)
        {
            return RenderExternalLink(html, externalLink, modelPropertiesMetadata, externalLink.IsConditionMet(propertyName, propertyValue, model), fieldPrefix, overrideTitle);
        }

        /// <summary>
        /// Render external link.
        /// </summary>
        internal static MvcHtmlString RenderExternalLink(this HtmlHelper html, ExternalLinkAttribute externalLink, IEnumerable<ModelMetadata> modelPropertiesMetadata, bool conditionMet, string fieldPrefix, string overrideTitle)
        {
            // Return empty string is link is not configured correctly or the link requires the user has a particular role but the user doesn't belong to it
            if (externalLink == null || string.IsNullOrEmpty(externalLink.Url) || string.IsNullOrEmpty(externalLink.Name) || (externalLink.Roles != null && !UserService.IsInRole(externalLink.Roles)))
            {
                return MvcHtmlString.Empty;
            }

            var externalUrl = externalLink.Url;

            var routes = new RouteValueDictionary();

            if (externalLink.Parameters != null)
            {
                foreach (var parameter in externalLink.Parameters)
                {
                    var dependentProperty = modelPropertiesMetadata.FirstOrDefault(p => p.PropertyName == parameter);

                    if (dependentProperty != null)
                    {
                        routes.Add(parameter, dependentProperty.Model);
                    }
                }
            }
            
            // Include additional parameters in URL
            if (routes.Any())
            {
                if (externalUrl.IndexOf('?') < 0)
                {
                    externalUrl += "?";
                }
                else
                {
                    externalUrl += "&";
                }

                externalUrl += routes.ToQueryString();
            }
            
            var externalLinkTag = new TagBuilder("a");

            externalLinkTag.Attributes.Add("href", externalUrl);
            externalLinkTag.Attributes.Add("target", "_blank");
            externalLinkTag.Attributes.Add("title", string.Format("{0} (opens in a new window)", overrideTitle ?? externalLink.Name));

            externalLinkTag.SetInnerText(externalLink.Name);

            // Return external link as normal if no dependency check is needed
            if (externalLink.DependencyType == ActionForDependencyType.None)
            {
                return new MvcHtmlString(externalLinkTag.ToString(TagRenderMode.Normal));
            }

            var htmlAttributes = new Dictionary<string, object>();

            htmlAttributes.MergeCssClass("rhea-actionif");
            htmlAttributes.Add(HtmlDataType.FieldPrefix, fieldPrefix != null ? fieldPrefix.Replace('.', '_') : string.Empty);
            htmlAttributes.Add(HtmlDataType.ActionForDependencyType, externalLink.DependencyType);
            htmlAttributes.Add(HtmlDataType.DependentProperty, externalLink.DependentProperty);
            htmlAttributes.Add(HtmlDataType.ComparisonType, externalLink.ComparisonType);
            htmlAttributes.Add(HtmlDataType.DependentValue, externalLink.DependentValue);
            htmlAttributes.Add(HtmlDataType.Type, "link");

            var wrapper = new TagBuilder("span");

            wrapper.MergeAttributes(htmlAttributes);

            htmlAttributes = new Dictionary<string, object>();

            var spanTag = new TagBuilder("span");

            switch (externalLink.DependencyType)
            {
                case ActionForDependencyType.Enabled:
                    spanTag = new TagBuilder("span");
                    spanTag.AddCssClass("input-button-disabled");

                    if (conditionMet)
                    {
                        spanTag.AddCssClass("hidden");
                    }
                    else
                    {
                        htmlAttributes["class"] = "hidden";
                    }

                    spanTag.SetInnerText(externalLink.Name);

                    wrapper.InnerHtml = spanTag.ToString(TagRenderMode.Normal);
                    externalLinkTag.MergeAttributes(htmlAttributes);
                    wrapper.InnerHtml += externalLinkTag.ToString(TagRenderMode.Normal);

                    break;
                case ActionForDependencyType.Disabled:
                    spanTag = new TagBuilder("span");
                    spanTag.AddCssClass("input-button-disabled");

                    if (!conditionMet)
                    {
                        spanTag.AddCssClass("hidden");
                    }
                    else
                    {
                        htmlAttributes["class"] = "hidden";
                    }

                    spanTag.SetInnerText(externalLink.Name);

                    wrapper.InnerHtml = spanTag.ToString(TagRenderMode.Normal);
                    externalLinkTag.MergeAttributes(htmlAttributes);
                    wrapper.InnerHtml += externalLinkTag.ToString(TagRenderMode.Normal);

                    break;
                case ActionForDependencyType.Visible:
                    if (!conditionMet)
                    {
                        wrapper.AddCssClass("hidden");
                    }

                    externalLinkTag.MergeAttributes(htmlAttributes);
                    wrapper.InnerHtml += externalLinkTag.ToString(TagRenderMode.Normal);

                    break;
                case ActionForDependencyType.Hidden:
                    if (conditionMet)
                    {
                        wrapper.AddCssClass("hidden");
                    }

                    externalLinkTag.MergeAttributes(htmlAttributes);
                    wrapper.InnerHtml += externalLinkTag.ToString(TagRenderMode.Normal);

                    break;
            }

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders items as a Radio Button Group.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="items">The items to render in the Radio Button Group.</param>
        /// <param name="htmlAttributes">HTML attributes to apply to property.</param>
        /// <returns>The rendered Radio Button Group.</returns>
        public static MvcHtmlString RadioButtonGroup(this HtmlHelper html, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes)
        {
            var result = new StringBuilder();

            bool isHorizontal = html.ViewData.ModelMetadata.DataTypeName == CustomDataType.RadioButtonGroupHorizontal;

            htmlAttributes.Add(HtmlDataType.RadioButtonGroup, true);
            
            html.GetUnobtrusiveValidationAttributes(html.ViewData.ModelMetadata.PropertyName, html.ViewData.ModelMetadata).ForEach(a => { if (!htmlAttributes.ContainsKey(a.Key)) { htmlAttributes.Add(a.Key, a.Value); } });

            //result.Append("<br>");

            int i = 0;
            int total = items.Count();
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    total--;
                    continue;
                }

                // Generate an id to be given to the radio button field (first id matches main label "for" target id)
                var id = i == 0 ? html.ViewData.ModelMetadata.PropertyName : string.Format("{0}-{1}", html.ViewData.ModelMetadata.PropertyName, i);
                i++;

                if (htmlAttributes.ContainsKey("id"))
                {
                    htmlAttributes["id"] = html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id));
                }
                else
                {
                    htmlAttributes.Add("id", html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id)));
                }

                var itemAccessor = item; // Prevent access to modified closure
                result.Append(html.ExecuteUpOneLevel(() => html.RadioButton(html.ViewData.ModelMetadata.PropertyName, itemAccessor.Value, itemAccessor.Selected, htmlAttributes)));
                result.Append(html.ExecuteUpOneLevel(() => html.Label(id, itemAccessor.Text)));

                if (!isHorizontal && i < total)
                {
                    result.Append("<br>");
                }
            }

            return new MvcHtmlString(result.ToString());
        }

        /// <summary>
        /// Renders items as a Check Box List.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="items">The items to render in the Check Box List.</param>
        /// <param name="htmlAttributes">HTML attributes to apply to property.</param>
        /// <returns>The rendered Check Box List.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper html, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes)
        {
            return CheckBoxList(html, items, htmlAttributes, html.ViewData.ModelMetadata.PropertyName);
        }

        /// <summary>
        /// Renders items as a Check Box List.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="items">The items to render in the Check Box List.</param>
        /// <param name="htmlAttributes">HTML attributes to apply to property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The rendered Check Box List.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper html, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes, string propertyName)
        {
            htmlAttributes.Add(HtmlDataType.CheckboxList, true);

            html.GetUnobtrusiveValidationAttributes(html.ViewData.ModelMetadata.PropertyName, html.ViewData.ModelMetadata).ForEach(a => { if (!htmlAttributes.ContainsKey(a.Key)) { htmlAttributes.Add(a.Key, a.Value); } });

            StringBuilder result = new StringBuilder();
            result.Append("<ul class=\"check-box-list\">");
            int i = 0;
            foreach (SelectListItem item in items)
            {
                // Ignore items with no value
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }
                
                // Generate an id to be given to the check box field (first id matches main label "for" target id)
                string id = i == 0 ? propertyName : string.Format("{0}-{1}", propertyName, i);
                i++;

                if (htmlAttributes.ContainsKey("id"))
                {
                    htmlAttributes["id"] = html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id));
                }
                else
                {
                    htmlAttributes.Add("id", html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(id)));
                }

                if (htmlAttributes.ContainsKey("name"))
                {
                    htmlAttributes["name"] = html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName));
                }
                else
                {
                    htmlAttributes.Add("name", html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName)));
                }

                result.Append("<li>");
                var input = new TagBuilder("input");
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("value", item.Value);

                if (item.Selected)
                {
                    input.Attributes.Add("checked", "checked");
                }

                input.MergeAttributes(htmlAttributes);

                result.Append("<div class=\"checkbox\">");
                result.Append(input.ToString(TagRenderMode.SelfClosing));
                result.AppendFormat("<label for=\"{0}\">{1}</label>", htmlAttributes["id"], item.Text);
                result.Append("</div>");
                result.Append("</li>");
            }
            result.Append("</ul>");

            //var builder = new TagBuilder("ul");

            //builder.AddCssClass("check-box-list");

            //builder.InnerHtml = result.ToString();

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Get a list of links and buttons for a property.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="property">The property with links.</param>
        /// <param name="inline">Whether the link or button is inline with the property.</param>
        /// <returns>The list of links and buttons for the property.</returns>
        public static List<MvcHtmlString> GetPropertyLinksAndButtons(this HtmlHelper html, ModelMetadata property, bool inline)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelper.GetPropertyLinksAndButtons");

            try
            {
#endif
                var startTag = string.Format("<span class=\"{0}\">", inline ? "button" : "input-button");
                var endTag = "</span>";

                // These should be links not assigned to a group
                var ungroupedLinks = new List<KeyValuePair<int, string>>();
                html.GetUngroupedLinks(property).ForEach(l => ungroupedLinks.Add(new KeyValuePair<int, string>(l.Key, string.Format("{0}{1}{2}", startTag, l.Value, endTag))));

                // These should be buttons not assigned to a group
                if (!inline)
                {
                    startTag = string.Empty;
                    endTag = string.Empty;
                }

                var ungroupedButtons = new List<KeyValuePair<int, string>>();
                html.GetUngroupedButtons(property).ForEach(b => ungroupedButtons.Add(new KeyValuePair<int, string>(b.Key, string.Format("{0}{1}{2}", startTag, b.Value, endTag))));

                return ungroupedLinks.Concat(ungroupedButtons).OrderBy(p => p.Key).ThenBy(p => p.Value).Select(p => new MvcHtmlString(p.Value)).ToList();
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Get a list of links that are not assigned to a group.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="property">The property with links.</param>
        /// <returns>The list of ungrouped links for the property.</returns>
        public static List<KeyValuePair<int, string>> GetUngroupedLinks(this HtmlHelper html, ModelMetadata property)
        {
            // These should be links not assigned to a group
            var links = property.GetAttributes<LinkAttribute>().Where(l => string.IsNullOrEmpty(l.GroupName)).ToList();
            var externalLinks = property.GetAttributes<ExternalLinkAttribute>().Where(e => string.IsNullOrEmpty(e.GroupName)).ToList();
            var renderedLinks = new List<KeyValuePair<int, string>>();

            if (links.Any())
            {
                foreach (LinkAttribute link in links.OrderBy(l => l.Order))
                {
                    string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;

                    var parentModel = property.AdditionalValues["ParentModel"];

                    string formattedLink = null;

                    var dependentProperty = link.DependentProperty;

                    // Use property if it is supplied and no dependent property is set
                    if (string.IsNullOrEmpty(dependentProperty))
                    {
                        dependentProperty = property.PropertyName;
                    }

                    // If link has a dependency, ensure field prefix is correct by checking the dependent property exists in the parent model and have the field prefix correspond to the one that does
                    if (link.DependencyType == ActionForDependencyType.None || (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.PropertyName == dependentProperty)))
                    {
                        formattedLink = html.RenderLink(link, html.ViewData.ModelMetadata.Properties, parentModel, property.PropertyName, property.Model, fieldPrefix, true).ToString();
                    }
                    else if (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.Properties.Any(pp => pp.PropertyName == dependentProperty)))
                    {
                        // Html Field Prefix was incorrect, correct here
                        fieldPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName);
                        parentModel = property.Model;

                        formattedLink = html.RenderLink(link, html.ViewData.ModelMetadata.Properties, parentModel, property.PropertyName, property.Model, fieldPrefix, true).ToString();
                    }

                    if (!string.IsNullOrEmpty(formattedLink))
                    {
                        renderedLinks.Add(new KeyValuePair<int, string>(link.Order, formattedLink));
                    }
                }
            }

            if (externalLinks.Any())
            {
                foreach (ExternalLinkAttribute externalLink in externalLinks.OrderBy(e => e.Order))
                {
                    string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;

                    var parentModel = property.AdditionalValues["ParentModel"];

                    string formattedExternalLink = null;

                    var dependentProperty = externalLink.DependentProperty;

                    // Use property if it is supplied and no dependent property is set
                    if (string.IsNullOrEmpty(dependentProperty))
                    {
                        dependentProperty = property.PropertyName;
                    }

                    // If external link has a dependency, ensure field prefix is correct by checking the dependent property exists in the parent model and have the field prefix correspond to the one that does
                    if (externalLink.DependencyType == ActionForDependencyType.None || (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.PropertyName == dependentProperty)))
                    {
                        formattedExternalLink = html.RenderExternalLink(externalLink, html.ViewData.ModelMetadata.Properties, parentModel, property.PropertyName, property.Model, fieldPrefix).ToString();
                    }
                    else if (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.Properties.Any(pp => pp.PropertyName == dependentProperty)))
                    {
                        // Html Field Prefix was incorrect, correct here
                        fieldPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName);
                        parentModel = property.Model;

                        formattedExternalLink = html.RenderExternalLink(externalLink, html.ViewData.ModelMetadata.Properties, parentModel, property.PropertyName, property.Model, fieldPrefix).ToString();
                    }

                    if (!string.IsNullOrEmpty(formattedExternalLink))
                    {
                        renderedLinks.Add(new KeyValuePair<int, string>(externalLink.Order, formattedExternalLink));
                    }
                }
            }

            return renderedLinks.OrderBy(r => r.Key).ToList();
        }

        /// <summary>
        /// Get a list of buttons that are not assigned to a group.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="property">The property with buttons.</param>
        /// <returns>The list of ungrouped buttons for the property.</returns>
        public static List<KeyValuePair<int, string>> GetUngroupedButtons(this HtmlHelper html, ModelMetadata property)
        {
            // These should be buttons not assigned to a group
            var buttons = property.GetAttributes<ButtonAttribute>().Where(b => string.IsNullOrEmpty(b.GroupName)).ToList();
            var renderedButtons = new List<KeyValuePair<int, string>>();

            if (buttons.Any())
            {
                foreach (ButtonAttribute button in buttons.OrderBy(b => b.Order))
                {
                    string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;

                    var parentModel = property.AdditionalValues["ParentModel"];

                    string formattedButton = null;

                    var dependentProperty = button.DependentProperty;

                    // Use property if it is supplied and no dependent property is set
                    if (string.IsNullOrEmpty(dependentProperty))
                    {
                        dependentProperty = property.PropertyName;
                    }

                    // If button has a dependency, ensure field prefix is correct by checking the dependent property exists in the parent model and have the field prefix correspond to the one that does
                    if (button.DependencyType == ActionForDependencyType.None || (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.PropertyName == dependentProperty)))
                    {
                        formattedButton = button.Render(html, parentModel, fieldPrefix, property.PropertyName, property.Model).ToString();
                    }
                    else if (parentModel != null && html.ViewData.ModelMetadata.Properties.Any(p => p.Properties.Any(pp => pp.PropertyName == dependentProperty)))
                    {
                        // Html Field Prefix was incorrect, correct here
                        fieldPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName);
                        parentModel = property.Model;

                        formattedButton = button.Render(html, parentModel, fieldPrefix, property.PropertyName, property.Model).ToString();
                    }

                    if (!string.IsNullOrEmpty(formattedButton))
                    {
                        renderedButtons.Add(new KeyValuePair<int, string>(button.Order, formattedButton));
                    }
                }
            }

            return renderedButtons;
        }

        /// <summary>
        /// Generate a label for a property.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="property">The property to generate a label for.</param>
        /// <param name="parent"></param>
        /// <returns>The generated label.</returns>
        public static MvcHtmlString GetLabel(this HtmlHelper html, ModelMetadata property, ModelMetadata parent)
        {
            return html.GetLabel(property, parent, null);
        }


        private const string getLabelRegexPattern = @"<(.*)\>(.*)<\/(.*)\>";
        private static readonly Regex getLabelRegex = new Regex(getLabelRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Generate a label for a property.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="property">The property to generate a label for.</param>
        /// <param name="parent">The parent of the property.</param>
        /// <param name="overridePropertyName">A specific display name to use instead of the property's default label.</param>
        /// <returns>The generated label.</returns>
        public static MvcHtmlString GetLabel(this HtmlHelper html, ModelMetadata property, ModelMetadata parent, string overridePropertyName)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelper.GetLabel");

            try
            {
#endif
                /*
                string colSize = "col-md-2"; // default is 2.
                RowAttribute rowAttribute = property.GetAttribute<RowAttribute>();
                if (rowAttribute != null)
                {
                    switch (rowAttribute.RowType)
                    {
                        case RowType.Default:
                            colSize = "col-md-2";
                            break;
                        case RowType.Dynamic:
                            colSize = "col-md-6";
                            break;
                        case RowType.Half:
                            colSize = "col-md-2"; // input gets 4 cols
                            break;
                        case RowType.Third:
                            colSize = "col-md-2";
                            break;
                        case RowType.Quarter:
                            colSize = "col-md-1";
                            break;

                    }
                }*/

                // Pattern for getting parts of HTML tag

                //string label = html.Label(property.PropertyName, new { @class = string.Format("control-label {0}", colSize) }).ToString();
                //string label; // = "<label class=\"control-label " + colSize + "\" for=\"" + property.PropertyName + "\">" + property.PropertyName + "</label>";
                //var labelMatch = Regex.Match(label, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                // Match labelMatch = getLabelRegex.Match(label);

                // System.Diagnostics.Debug.WriteLine("labelMatch.Groups[1].Value " + labelMatch.Groups[1].Value);
                // System.Diagnostics.Debug.WriteLine("labelMatch.Groups[2].Value " + labelMatch.Groups[2].Value);
                //System.Diagnostics.Debug.WriteLine("labelMatch.Groups[3].Value " + labelMatch.Groups[3].Value);

                //if (labelMatch.Success)
                //{
                HiddenAttribute hiddenAttribute = property.GetAttribute<HiddenAttribute>();

                string hidden = (hiddenAttribute != null && hiddenAttribute.LabelOnly) ? " class=\"hidden\"" : string.Empty;
                string labelText = !string.IsNullOrEmpty(overridePropertyName) ? overridePropertyName : property.DisplayName ?? property.PropertyName; //labelMatch.Groups[2].Value;

                // Make label text a link for a property that should be a read only hyperlink
                if (property.IsReadOnlyHyperlink())
                {
                    LinkAttribute link = property.GetAttribute<LinkAttribute>();
                    ExternalLinkAttribute externalLink = property.GetAttribute<ExternalLinkAttribute>();

                    if (link != null)
                    {
                        link.Name = labelText;
                        labelText = RenderLink(html, link, parent.Properties, parent.Model, property.PropertyName, property.Model, true).ToString();
                    }
                    else if (externalLink != null)
                    {
                        externalLink.Name = labelText;
                        labelText = RenderExternalLink(html, externalLink, parent.Properties, parent.Model, property.PropertyName, property.Model, html.ViewData.TemplateInfo.HtmlFieldPrefix, labelText).ToString();
                    }
                }

                // Prepare access key
                string accessKey = string.Empty;
                AccessKeyAttribute accessKeyAttribute = property.GetAttribute<AccessKeyAttribute>();

                if (accessKeyAttribute != null)
                {
                    // Check that access key character exists in the label text
                    int position = labelText.IndexOf(accessKeyAttribute.Key);

                    if (position != -1)
                    {
                        // Set the access key
                        accessKey = string.Format(" accesskey=\"{0}\"", char.ToLowerInvariant(accessKeyAttribute.Key));

                        // Underline the access key character in the label
                        labelText = labelText.Insert(position + 1, "</u>");
                        labelText = labelText.Insert(position, "<u>");
                    }
                }

                string additionalForText = string.Empty;
                AdwSelectionAttribute adwSelectionAttribute = property.GetAttribute<AdwSelectionAttribute>();
                SelectionAttribute selectionAttribute = property.GetAttribute<SelectionAttribute>();

                // Retarget label "for" to DisplayText for AdwSelection and Selection with none selection type
                if ((adwSelectionAttribute != null && adwSelectionAttribute.SelectionType == SelectionType.None) || (selectionAttribute != null && selectionAttribute.SelectionType == SelectionType.None))
                {
                    additionalForText = "_DisplayText";
                    //labelFor = labelFor.Insert(labelFor.LastIndexOf('"'), "_DisplayText");
                }
                // Adjustment for split DateTime control (renders as a datepicker input + timepicker input)
                else if ((property.ModelType == typeof(DateTime) || property.ModelType == typeof(DateTime?)) && property.DataTypeName == DataType.DateTime.ToString())
                {
                    additionalForText = "_Date";
                    labelText += " <span class=\"readers\">(Date)</span>";
                }

                //string labelFor = "label class=\"control-label " + colSize + "\" for=\"" + property.PropertyName + additionalForText + "\"";//labelMatch.Groups[1].Value;
                StringBuilder sb = new StringBuilder();
                // Open tag
                sb.Append("<label class=\"control-label ");
                //sb.Append(colSize);
                sb.Append("\" for=\"");
                sb.Append(html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName));
                sb.Append(additionalForText);
                sb.Append("\"");
                //sb.Append(labelFor);
                sb.Append(accessKey);
                sb.Append(hidden);
                sb.Append('>');

                // Inject items into label

                // Inner text
                sb.Append(labelText);

                bool isRequired = property.IsRequired;

                // Override IsRequired for bool to only be required if bool is explicitly decorated with [Required]
                if (isRequired && property.ModelType == typeof(bool))
                {
                    isRequired = property.GetAttribute<RequiredAttribute>() != null;
                }

                // [Required] indicator
                if (isRequired)
                {
                    sb.Append(" <abbr class=\"req\" title=\"required\">*</abbr>");
                }
                    // [RequiredIf] based indicator
                else if (property.HasRequiredIf())
                {
                    // Include indicator but with hidden if currently not required
                    sb.Append(" <abbr class=\"req");
                    sb.Append(property.IsRequired() ? string.Empty : " hidden");
                    sb.Append("\" title=\"required\">*</abbr>");
                }

                // Close tag
                sb.Append("</label>");

                // Error message
                string error = html.ValidationMessage(property.PropertyName).ToString();

                //var errorMatch = Regex.Match(error, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match errorMatch = getLabelRegex.Match(error);

                string showOrHideErrorTip = errorMatch.Success && !string.IsNullOrEmpty(errorMatch.Groups[2].Value) ? string.Empty : " style=\"display:none\"";

                sb.AppendFormat("<a class=\"errorTip\" href=\"javascript:;\" {0}=\"{1}\"{2}>{3}</a> ", HtmlDataType.ErrorTipFor, html.ViewData.TemplateInfo.GetFullHtmlFieldName(property.PropertyName), showOrHideErrorTip, html.ValidationMessage(property.PropertyName));

                // Information message
                DescriptionAttribute description = property.GetAttribute<DescriptionAttribute>();

                if (description != null)
                {
                    sb.AppendFormat("<a class=\"hintTip\" href=\"javascript:;\"><span>Help tip: {0} {1}</span></a> ", property.GetDisplayName(), description.Description);
                }
                else
                {
                    string informationMessage = html.Message(MessageType.Information, property.PropertyName).ToString();

                    if (!string.IsNullOrEmpty(informationMessage))
                    {
                        sb.Append("<a class=\"hintTip\" href=\"javascript:;\"><span>Help tip: ");
                        sb.Append(informationMessage);
                        sb.Append("</span></a> ");
                    }
                }
                //label = sb.ToString();
                //}

                return new MvcHtmlString(sb.ToString());
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Creates the HTML for the buttons, split buttons, links and external links attached via attributes to a ViewModel group
        /// </summary>
        /// <param name="html"></param>
        /// <param name="model"></param>
        /// <param name="allActions">A list of Attributes describing the buttons and links to render</param>
        /// <returns></returns>
        public static List<MvcHtmlString> GetButtonArea(this HtmlHelper html, IEnumerable<ISplitButtonChild> allActions)
        {
            IEnumerable<ISplitButtonChild> topLevels = allActions.Where(a => string.IsNullOrEmpty(a.SplitButtonParent)).OrderBy(a => a.Order);
            List<MvcHtmlString> actionsToRender = new List<MvcHtmlString>();

            foreach (ISplitButtonChild action in topLevels)
            {
                ButtonAttribute buttonAttr = action as ButtonAttribute;
                List<ISplitButtonChild> subActions = (buttonAttr == null) ? null : allActions.Where(a => a.SplitButtonParent == buttonAttr.Name).OrderBy(a => a.Order).ToList();
                if (subActions == null || subActions.Count == 0)
                {
                    // Just render the action itself
                    actionsToRender.Add(action.Render(html));
                }
                else
                {
                    bool isADropDownOnly = string.IsNullOrEmpty(buttonAttr.SubmitType);

                    // Render a button group
                    TagBuilder buttonGroup = new TagBuilder("div");
                    buttonGroup.AddCssClass("btn-group");

                    TagBuilder toggleButton = new TagBuilder("button");
                    toggleButton.AddCssClass("btn");
                    toggleButton.AddCssClass("dropdown-toggle");
                    if (buttonAttr.Primary)
                    {
                        toggleButton.AddCssClass("btn-primary");
                    }
                    else
                    {
                        toggleButton.AddCssClass("btn-inverse");
                    }
                    toggleButton.MergeAttribute("type", "button");
                    toggleButton.MergeAttribute(HtmlDataType.Toggle, "dropdown");
                    if (isADropDownOnly)
                    {
                        toggleButton.InnerHtml = HttpUtility.HtmlEncode(buttonAttr.Name) + @" <span class=""caret""></span>";
                    }
                    else
                    {
                        toggleButton.InnerHtml = @"<span class=""caret""></span><span class=""sr-only"">Toggle dropdown</span>";
                    }

                    TagBuilder list = new TagBuilder("ul");
                    list.AddCssClass("dropdown-menu");
                    list.MergeAttribute("role", "menu");
                    StringBuilder sb = new StringBuilder();
                    foreach (ISplitButtonChild subAction in subActions)
                    {
                        TagBuilder li = new TagBuilder("li");
                        li.InnerHtml = subAction.Render(html).ToString();
                        sb.Append(li.ToString());
                    }
                    list.InnerHtml = sb.ToString();

                    if (isADropDownOnly)
                    {
                        buttonGroup.InnerHtml = toggleButton.ToString() + list.ToString();
                    }
                    else
                    {
                        buttonGroup.InnerHtml = action.Render(html) + toggleButton.ToString() + list.ToString();
                    }

                    actionsToRender.Add(new MvcHtmlString(buttonGroup.ToString()));
                }
            }

            return actionsToRender;
        }

        /// <summary>
        /// Generate Area quick links.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <returns>The generated label.</returns>
        public static MvcHtmlString ShowAreaQuicklinks(this HtmlHelper html)
        {
            return ShowAreaQuicklinks(html, false);
        }

        /// <summary>
        /// Generate Area quick links.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <param name="excludeExtras">Whether to exclude Bulletins and Noticeboard.</param>
        /// <returns>The generated label.</returns>
        public static MvcHtmlString ShowAreaQuicklinks(this HtmlHelper html, bool excludeExtras)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("HtmlHelper.ShowAreaQuicklinks");

            try
            {
#endif
                int perRow = 15;
                int totalShown = 0;


                var areas = new List<MenuTile>();

                // Include tile for Home
                areas.Add(new MenuTile { AreaName = "Home", DisplayName = "Home", RouteName = "Default" });

                if (!excludeExtras)
                {
                    areas.Add(new MenuTile { AreaName = "Bulletins", DisplayName = "Bulletins", RouteName = "Default_Bulletins" });

                    //// Only show noticeboard if the user has a Diary role
                    //if (UserService.IsInRole(new[] { "DIA", "DIU", "DIV" }))
                    //{
                    //    areas.Add(new MenuTile { AreaName = "Notification", DisplayName = "Noticeboard", RouteName = "Notification_noticeboard" });
                    //}
                }

                areas = areas.Concat(MenuService.VisibleAreaTiles).ToList();
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class=\"nav navbar-nav navbar-left\">");
                var urlHelper = new UrlHelper(new RequestContext(html.ViewContext.RequestContext.HttpContext, new RouteData()));
                while (totalShown < areas.Count)
                {
                    var areasInRow = areas.Skip(totalShown).Take(perRow);

                    foreach (var area in areasInRow)
                    {
                        var listItem = new TagBuilder("li");
                        var link = new TagBuilder("a");
                        var span = new TagBuilder("span");
                        span.AddCssClass("toolTip");
                        var routeName = string.IsNullOrEmpty(area.RouteName) ? string.Format("{0}_default", area.AreaName) : area.RouteName;
                        var href = urlHelper.RouteUrl(routeName, new { });
                        link.Attributes.Add("href", href);
                        span.SetInnerText(area.DisplayName);
                        link.InnerHtml = span.ToString();
                        listItem.InnerHtml = link.ToString();

                        sb.Append(listItem.ToString());
                        totalShown++;
                    }
                }
                sb.Append("</ul>");

                return new MvcHtmlString(sb.ToString());
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Get all area tiles.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <returns>The all area tiles.</returns>
        public static ReadOnlyCollection<MenuTile> GetAllAreaTiles(this HtmlHelper html)
        {
            return MenuService.AreaTiles;
        }

        /// <summary>
        /// Get visible area tiles.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <returns>The visible area tiles.</returns>
        public static ReadOnlyCollection<MenuTile> GetVisibleAreaTiles(this HtmlHelper html)
        {
            return MenuService.VisibleAreaTiles;
        }

        /// <summary>
        /// Render a hidden Anti-Duplicate Submit Token for use in preventing duplicate submits by comparing against the token in Session.
        /// </summary>
        /// <param name="html">The HTML hlper instance.</param>
        /// <returns>The hidden Anti-Duplicate Submit Token input.</returns>
        public static MvcHtmlString AntiDuplicateSubmitToken(this HtmlHelper html)
        {
            // Generate random token
            var token = Guid.NewGuid().ToString();

            // Temporarily store token in TempData
            html.ViewContext.TempData[ValidateAntiDuplicateSubmitTokenAttribute.GetKey(html.ViewContext.RouteData)] = token;

            // Build hidden input
            var tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes["type"] = "hidden";
            tagBuilder.Attributes["name"] = ValidateAntiDuplicateSubmitTokenAttribute.FormFieldName;
            tagBuilder.Attributes["value"] = token;

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Generate beginning tag of main form.
        /// </summary>
        /// <param name="html">The HTML helper instance.</param>
        /// <returns>The generated main form opening tag.</returns>
        public static MvcHtmlString BeginMainForm(this HtmlHelper html)
        {
            var supportUpload = string.Empty;

            // Include support for file uploads if allowed
            if (html.ViewData.ContainsKey(AllowFileUploadAttribute.ViewDataKey) && (bool)html.ViewData[AllowFileUploadAttribute.ViewDataKey])
            {
                supportUpload = "enctype=\"multipart/form-data\"";
            }

            var result = string.Format("<form action=\"{0}\" id=\"main_form\" method=\"{1}\" autocomplete=\"off\" {2}>",
                                   HttpUtility.HtmlEncode(html.ViewContext.HttpContext.Request.RawUrl), FormMethod.Post,
                                   supportUpload);

            // Do not display property errors on top.
            return new MvcHtmlString(string.Concat(result, html.AntiForgeryToken(), html.AntiDuplicateSubmitToken(), html.ValidationMessageSummary()));
        }

        /// <summary>
        /// Renders a partial view to a string for use in multi component rendering
        /// </summary>
        /// <param name="html"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="viewData">Additional data to pass to the view. May be null</param>
        /// <returns></returns>
        public static MvcHtmlString RenderPartialViewToString(this HtmlHelper html, string viewName, object model, ViewDataDictionary viewData)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                ControllerContext controllerContext = html.ViewContext.Controller.ControllerContext;
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                TempDataDictionary tempData = new TempDataDictionary();
                viewData = viewData ?? new ViewDataDictionary();
                viewData.Model = model;
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                return new MvcHtmlString(stringWriter.ToString());
            }
        }

        /// <summary>
        /// Renders any widgets that are supposed to be currently open for the given context
        /// </summary>
        /// <param name="html"></param>
        public static MvcHtmlString RenderWidgets(this HtmlHelper html)
        {
            WidgetsAttribute widgetsAttribute = html.ViewData.ModelMetadata.GetAttribute<WidgetsAttribute>();
            if (widgetsAttribute != null)
            {
                TagBuilder tag = new TagBuilder("div");
                tag.AddCssClass("row");
                tag.AddCssClass("zeus-widget-container");
                tag.Attributes.Add(HtmlDataType.WidgetContext, widgetsAttribute.WidgetContext);

                StringBuilder sb = new StringBuilder();
                var openWidgets = UserService.Dashboard.GetOpenWidgetNames(widgetsAttribute.WidgetContext);
                foreach (string name in openWidgets)
                {
                    WidgetViewModel widget = WidgetViewModel.GetWidget(name, widgetsAttribute.WidgetContext);
                    if (widget != null)
                    {
                        var viewData = new ViewDataDictionary();
                        viewData.Add("WidgetContext", widgetsAttribute.WidgetContext);
                        sb.Append(html.RenderPartialViewToString("EditorTemplates/WidgetViewModel", widget, viewData));
                    }
                }
                tag.InnerHtml = sb.ToString();
                return new MvcHtmlString(tag.ToString());
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }

        /// <summary>
        /// Calculates the right sidebar content and renders it to a string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderRightSidebar(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();

            // Render widget selector if required
            WidgetsAttribute widgetsAttribute = html.ViewData.ModelMetadata.GetAttribute<WidgetsAttribute>();
            if (widgetsAttribute != null)
            {
                WidgetSelectorViewModel model = new WidgetSelectorViewModel();
                model.WidgetDataContexts.Add("Office"); // TODO: make this specifyable
                model.WidgetDataContexts.Add("Mine");// TODO: make this specifyable
                model.CurrentWidgetDataContext = UserService.Dashboard.GetDataContext(widgetsAttribute.WidgetContext) ?? "Office";
                model.WidgetContext = widgetsAttribute.WidgetContext;
                IEnumerable<string> openWidgets = UserService.Dashboard.GetOpenWidgetNames(model.WidgetContext);
                model.AvailableWidgetNames = WidgetViewModel.GetWidgetsForContext(model.WidgetContext).Where(w => !openWidgets.Contains(w.UniqueName)).Select(w => w.UniqueName).ToList();
                sb.Append(html.RenderPartialViewToString("EditorTemplates/WidgetSelectorViewModel", model, null));
            }

            // Render context components if required
            var contextComponents = html.ViewData.ModelMetadata.GetAttributes<ContextComponentAttribute>();
            foreach (ContextComponentAttribute ccAttribute in contextComponents) {
                sb.Append(ContextComponentAttribute.RegisteredRenderers[ccAttribute.KeyString](html, html.ViewData.Model));
            }

            return new MvcHtmlString(sb.ToString());
        }


        /// <summary>
        /// Shows the content.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MvcHtmlString ShowContent(this HtmlHelper html, ContentViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            ContentType? lastContentType = null;
            List<Content> areaLinks = null;
            var contents = model.GetContent();
            var processed = 0;

            foreach (var content in contents)
            {
                // Starting to add area links
                if (lastContentType != ContentType.AreaLink && content.Type == ContentType.AreaLink)
                {
                    areaLinks = new List<Content>();
                }

                // Add area link to collection
                if (content.Type == ContentType.AreaLink && areaLinks != null)
                {
                    areaLinks.Add(content);

                    lastContentType = content.Type;
                    processed++;

                    if (processed != contents.Count)
                    {
                        continue;
                    }
                }

                // Finished adding area links
                if (lastContentType == ContentType.AreaLink && (content.Type != ContentType.AreaLink || processed == contents.Count) && areaLinks != null && areaLinks.Any())
                {
                    sb.Append("<nav class=\"areaNav\">");

                    int PerRow = 3;
                    const int TotalCol = 12;

                    int counter = 0;
                    foreach (var areaLink in areaLinks)
                    {
                        if (counter % PerRow == 0)
                        {
                            sb.Append("<div class= \"row\">");
                        }
                        if (string.IsNullOrEmpty(areaLink.Controller))
                        {
                            areaLink.Controller = html.ViewContext.RouteData.GetController();
                        }

                        areaLink.Area = html.ViewContext.RouteData.DataTokens["area"] != null ? html.ViewContext.RouteData.DataTokens["area"].ToString() : html.ViewContext.RouteData.GetArea();

                        if (areaLink.AreaLinkIcon.HasValue && areaLink.AreaLinkIcon != AreaLinkIconType.None)
                        {
                            sb.AppendFormat("<div class=\"areaLinkIcon-{0}\" class='col-md-{1}'>", areaLink.AreaLinkIcon, TotalCol / PerRow);
                        }
                        else
                        {
                            sb.Append(string.Format("<div class='col-md-{0}'>", TotalCol / PerRow));
                        }

                        var routeValues = new RouteValueDictionary { { "Area", areaLink.Area } };

                        if (areaLink.RouteValues != null)
                        {
                            routeValues = new RouteValueDictionary(routeValues.Concat(areaLink.RouteValues).ToDictionary(r => r.Key, r => r.Value));
                        }

                        var linkTag = new TagBuilder("a");
                        var href = UrlHelper.GenerateUrl(content.RouteName, areaLink.Action, areaLink.Controller, routeValues, html.RouteCollection, html.ViewContext.RequestContext, false);
                        linkTag.Attributes.Add("href", href);
                        linkTag.AddCssClass("btn btn-lg fa-2x");
                        linkTag.InnerHtml = string.Format("<span class=\"pull-left\">{0}</span>", areaLink.Text);
                        sb.Append(linkTag.ToString(TagRenderMode.Normal));

                        sb.Append("</div>");

                        counter++;
                        if (counter % PerRow == 0 || counter == areaLinks.Count)
                        {
                            sb.Append("</div>"); // Row div.
                        }
                    }

                    sb.Append("</nav>");
                    sb.Append("<div class=\"clearBoth\"></div>");
                }

                //TODO: should content.Text be html encoded?
                switch (content.Type)
                {
                    case ContentType.Title:
                        sb.AppendFormat("<h2>{0}</h2>", content.Text);
                        break;

                    case ContentType.BeginTitle:
                        sb.Append("<h2>");
                        break;

                    case ContentType.EndTitle:
                        sb.Append("</h2>");
                        break;

                    case ContentType.SubTitle:
                        sb.AppendFormat("<h3>{0}</h3>", content.Text);
                        break;

                    case ContentType.BeginSubTitle:
                        sb.Append("<h3>");
                        break;

                    case ContentType.EndSubTitle:
                        sb.Append("</h3>");
                        break;

                    case ContentType.Text:
                        sb.Append(content.Text);
                        break;

                    case ContentType.EmphasisText:
                        sb.AppendFormat("<em>{0}</em>", content.Text);
                        break;

                    case ContentType.StrongText:
                        sb.AppendFormat("<strong>{0}</strong>", content.Text);
                        break;

                    case ContentType.UnderlinedText:
                        sb.AppendFormat("<u>{0}</u>", content.Text);
                        break;

                    case ContentType.SubscriptText:
                        sb.AppendFormat("<sub>{0}</sub>", content.Text);
                        break;

                    case ContentType.SuperscriptText:
                        sb.AppendFormat("<sup>{0}</sup>", content.Text);
                        break;

                    case ContentType.ObsoleteText:
                        sb.AppendFormat("<s>{0}</s>", content.Text);
                        break;

                    case ContentType.BadgeText:
                        sb.AppendFormat("<span class=\"{0}\">{1}</span>", content.BadgeType.GetLabelClasses(), content.Text);
                        break;

                    case ContentType.ScreenReaderText:
                        sb.AppendFormat("<span class=\"readers\">{0}</span>", content.Text);
                        break;

                    case ContentType.Abbreviation:
                        sb.AppendFormat("<abbr title=\"{0}\">{1}</abbr>", content.Value, content.Text);
                        break;

                    case ContentType.LineBreak:
                        sb.Append("<br />");
                        break;

                    case ContentType.Paragraph:
                        sb.AppendFormat("<p>{0}</p>", content.Text);
                        break;

                    case ContentType.BeginParagraph:
                        sb.Append("<p>");
                        break;

                    case ContentType.EndParagraph:
                        sb.Append("</p>");
                        break;

                    case ContentType.Preformatted:
                        sb.AppendFormat("<pre>{0}</pre>", content.Text);
                        break;

                    case ContentType.BeginOrderedList:
                        sb.Append("<ol>");
                        break;

                    case ContentType.EndOrderedList:
                        sb.Append("</ol>");
                        break;

                    case ContentType.BeginUnorderedList:
                        sb.Append("<ul>");
                        break;

                    case ContentType.EndUnorderedList:
                        sb.Append("</ul>");
                        break;

                    case ContentType.ListItem:
                        sb.AppendFormat("<li>{0}</li>", content.Text);
                        break;

                    case ContentType.BeginListItem:
                        sb.Append("<li>");
                        break;

                    case ContentType.EndListItem:
                        sb.Append("</li>");
                        break;

                    case ContentType.ExternalLink:

                        var externalLinkTag = new TagBuilder("a");
                        externalLinkTag.Attributes.Add("href", content.Value);
                        externalLinkTag.Attributes.Add("target", "_blank");
                        externalLinkTag.Attributes.Add("title", string.Format("{0} (opens in a new window)", content.Text));
                        externalLinkTag.SetInnerText(content.Text);
                        sb.Append(externalLinkTag.ToString(TagRenderMode.Normal));
                        break;

                    case ContentType.EmailLink:
                        var emailLinkTag = new TagBuilder("a");
                        emailLinkTag.Attributes.Add("href", string.Format("mailto:{0}", content.Text));
                        emailLinkTag.SetInnerText(content.Text);
                        sb.Append(emailLinkTag.ToString(TagRenderMode.Normal));
                        break;

                    case ContentType.Link:

                        if (string.IsNullOrEmpty(content.Controller))
                        {
                            content.Controller = html.ViewContext.RouteData.GetController();
                        }

                        if (string.IsNullOrEmpty(content.Area))
                        {
                            content.Area = html.ViewContext.RouteData.DataTokens["area"] != null ? html.ViewContext.RouteData.DataTokens["area"].ToString() : html.ViewContext.RouteData.GetArea();
                        }

                        var routeValues = new RouteValueDictionary { { "Area", content.Area } };

                        if (content.RouteValues != null)
                        {
                            routeValues = new RouteValueDictionary(routeValues.Concat(content.RouteValues).ToDictionary(r => r.Key, r => r.Value));
                        }

                        var linkTag = new TagBuilder("a");
                        var href = UrlHelper.GenerateUrl(content.RouteName, content.Action, content.Controller, routeValues, html.RouteCollection, html.ViewContext.RequestContext, false);
                        linkTag.Attributes.Add("href", href);
                        linkTag.SetInnerText(content.Text);
                        sb.Append(linkTag.ToString(TagRenderMode.Normal));

                        break;

                    case ContentType.BeginLink:

                        if (string.IsNullOrEmpty(content.Controller))
                        {
                            content.Controller = html.ViewContext.RouteData.GetController();
                        }

                        if (string.IsNullOrEmpty(content.Area))
                        {
                            content.Area = html.ViewContext.RouteData.DataTokens["area"] != null ? html.ViewContext.RouteData.DataTokens["area"].ToString() : html.ViewContext.RouteData.GetArea();
                        }

                        var beginLinkTag = new TagBuilder("a");
                        var beginHref = UrlHelper.GenerateUrl(content.RouteName, content.Action, content.Controller, new RouteValueDictionary { { "Area", content.Area } }, html.RouteCollection, html.ViewContext.RequestContext, false);
                        beginLinkTag.Attributes.Add("href", beginHref);
                        sb.Append(beginLinkTag.ToString(TagRenderMode.StartTag));

                        break;

                    case ContentType.EndLink:
                        sb.Append("</a>");
                        break;

                    case ContentType.Image:
                        var imageTag = new TagBuilder("img");
                        imageTag.Attributes.Add("alt", content.Text);
                        var imagePath = UrlHelper.GenerateContentUrl(content.Value, html.ViewContext.HttpContext);
                        imageTag.Attributes.Add("src", imagePath);
                        sb.Append(imageTag.ToString(TagRenderMode.SelfClosing));

                        break;

                    case ContentType.BeginDescriptionList:
                        sb.Append("<dl>");
                        break;

                    case ContentType.EndDescriptionList:
                        sb.Append("</dl>");
                        break;

                    case ContentType.DescriptionName:
                        sb.AppendFormat("<dt>{0}</dt>", content.Text);
                        break;

                    case ContentType.BeginDescriptionName:
                        sb.Append("<dt>");
                        break;

                    case ContentType.EndDescriptionName:
                        sb.Append("</dt>");
                        break;

                    case ContentType.DescriptionValue:
                        sb.AppendFormat("<dd>{0}</dd>", content.Text);
                        break;

                    case ContentType.BeginDescriptionValue:
                        sb.Append("<dd>");
                        break;

                    case ContentType.EndDescriptionValue:
                        sb.Append("</dd>");
                        break;

                    case ContentType.Icon:
                        if (content.Value != null)
                        {
                            sb.Append("<span class=\"fa-stack fa-2x\">");
                            sb.AppendFormat("<i class=\"fa fa-circle fa-stack-2x fg-{0}\"></i>", content.Value);
                            sb.AppendFormat("<i class=\"fa {0} fa-stack-1x fa-inverse\"></i>", content.Text);
                            sb.Append("</span>");
                        }
                        else
                        {
                            sb.AppendFormat("<i class=\"fa {0} fa-2x\"></i>", content.Text);
                        }
                        break;
                }

                lastContentType = content.Type;
                processed++;
            }

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Whether there are any alerts.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns><c>true</c> if there are alerts; otherwise, <c>false</c>.</returns>
        public static bool HasAlerts(this HtmlHelper html)
        {
            var alerts = html.ViewContext.ViewData.Get<List<ContentViewModel>>("Alerts");

            return alerts != null && alerts.Any();
        }

        /// <summary>
        /// Include a DIV with a HTML data attribute containing JSON data of alerts.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns>A DIV with a HTML data attribute containing JSON data of alerts.</returns>
        public static MvcHtmlString Alerts(this HtmlHelper html)
        {
            var alerts = html.ViewContext.ViewData.Get<List<ContentViewModel>>("Alerts");

            if (alerts == null || !alerts.Any())
            {
                return MvcHtmlString.Empty;
            }

            var div = new TagBuilder("div");

            div.MergeAttribute("id", "zeus-alert");

            var data = new List<object>();

            foreach (var alert in alerts)
            {
                var content = html.ShowContent(alert).ToString();

                data.Add(new { Text = content });
            }

            div.MergeAttribute("data-zeus-alerts", Json.Encode(data));

            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        /*
        public static MvcHtmlString Object(this HtmlHelper html, object model)
        {   
            if (html.ViewData.Model == null)
            {
                return MvcHtmlString.Create(html.ViewData.ModelMetadata.NullDisplayText);
            }

            var response = MvcHtmlString.Empty;
            var listOfGroupsRendered = new Dictionary<string, List<GroupAttribute>>();
            var propertyGroups = html.ViewData.GetGroupedAndOrderedMetadata();
            var groupContainers = 0;

            var groupRowDivTag = new TagBuilder("div");
        
            foreach (var propertyGroup in propertyGroups)
            {
                IEnumerable<GroupAttribute> otherGroupsInSameRow = null;
            
                response = response.Concat(html.ObjectBeginGroup(model, propertyGroups, propertyGroup, listOfGroupsRendered, groupContainers, otherGroupsInSameRow, groupRowDivTag))
                    .Concat(html.ObjectProperties(model, propertyGroups, propertyGroup))
                    .Concat(html.ObjectEndGroup(model, propertyGroups, propertyGroup, listOfGroupsRendered, groupContainers, otherGroupsInSameRow, groupRowDivTag));
            }

            return response;
        }


        public static MvcHtmlString ObjectBeginGroup(this HtmlHelper html, object model,
                                                     IOrderedEnumerable<IGrouping<string, ModelMetadata>> propertyGroups,
                                                     IGrouping<string, ModelMetadata> propertyGroup,
                                                     Dictionary<string, List<GroupAttribute>> listOfGroupsRendered,
                                                     int groupContainers,
                                                     IEnumerable<GroupAttribute> otherGroupsInSameRow,
                                                     TagBuilder groupRowDivTag)
        {
            var sb = new StringBuilder();
            var groups = html.ViewData.ModelMetadata.GetAttributes<GroupAttribute>().ToList();
            var rowStyleForGroup = string.Empty;

            // Remove "row" class for groupRowDivTag
            groupRowDivTag.Attributes.Remove("class");
            // Get current group
            var group = groups.SingleOrDefault(a => string.Equals(a.Name, propertyGroup.Key, StringComparison.Ordinal));
            var groupConditionMet = group != null ? group.IsConditionMet(model) : true;



            var pG = propertyGroup; // Prevent access to modified closure

            // Get type of current group
            GroupType? groupType =
                groups.Where(g => string.Equals(g.Name, propertyGroup.Key, StringComparison.Ordinal))
                      .Select(g => g.GroupType)
                      .FirstOrDefault();
            GroupRowType? groupRowType = null;

            if (group != null)
            {
                // Get row type for current group
                groupRowType = group.RowType;
                    // groups.Where(grp => grp.Name == pG.Key).Select(grp => grp.RowType).FirstOrDefault();
                // Get rowName
                string groupRowName = group.RowName;
                    //groups.Where(grp => grp.Name == pG.Key).Select(grp => grp.RowName).FirstOrDefault();
                //otherGroupsInSameRow = string.IsNullOrEmpty(groupRowName) ? null : Enumerable.Empty<GroupAttribute>();


                // Find other groups in same row
                otherGroupsInSameRow = groups.Where(m =>
                    {
                        // Ignore current property
                        if (string.Equals(m.Name, propertyGroup.Key, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        return (!string.IsNullOrEmpty(groupRowName) && m.RowName == groupRowName);
                    }).ToList();



                if (otherGroupsInSameRow.Any())
                {
                    // if this is the first group being rendered, then add the 'row' class.
                    if (string.IsNullOrEmpty(groupRowName) ||
                        (!string.IsNullOrEmpty(groupRowName) && !listOfGroupsRendered.ContainsKey(groupRowName)))
                    {
                        groupRowDivTag.AddCssClass("row");
                        sb.Append(groupRowDivTag.ToString(TagRenderMode.StartTag));
                    }

                }
                else
                {
                    groupRowDivTag.AddCssClass("row");
                    sb.Append(groupRowDivTag.ToString(TagRenderMode.StartTag));
                }
                if (!string.IsNullOrEmpty(groupRowName))
                {
                    if (listOfGroupsRendered.ContainsKey(groupRowName))
                    {
                        listOfGroupsRendered[groupRowName].Add(group);
                    }
                    else
                    {
                        listOfGroupsRendered.Add(groupRowName, new List<GroupAttribute>() {group});
                    }
                }
            }



            if (!groupType.HasValue)
            {
                groupType = GroupType.Default;
            }

            // Process FieldSet and Logical group types and group has at least one property that has HideSurroundingHtml set to false.
            if ((groupType == GroupType.FieldSet || groupType == GroupType.Logical) && propertyGroup.Any(g =>
                {
                    // Only render group container if it contains a visible property
                    var a = g.GetAttribute<DataTypeAttribute>();

                    if (a != null && string.Equals(a.CustomDataType, CustomDataType.Grid, StringComparison.Ordinal))
                    {
                        // Only display grid if there are results
                        var enumerable = g.Model as IEnumerable<dynamic>;

                        return (enumerable != null && enumerable.Any());
                    }

                    return !g.HideSurroundingHtml;
                }))
            {
                // Only add to FieldSet if it is an actual FieldSet item (will have key name)
                if (!string.IsNullOrEmpty(propertyGroup.Key))
                {
                    var divTag = new TagBuilder("div");
                    var groupName = propertyGroup.Key;

                    // Apply settings from [Group]
                    if (group != null)
                    {
                        // Check for group name override
                        if (!string.IsNullOrEmpty(group.OverrideNameWithPropertyValue))
                        {
                            var overrideProperty =
                                propertyGroups.SelectMany(g => g)
                                              .FirstOrDefault(
                                                  p =>
                                                  string.Equals(p.PropertyName, group.OverrideNameWithPropertyValue,
                                                                StringComparison.Ordinal));

                            // Use override if set with a string value
                            if (overrideProperty != null && overrideProperty.Model is string &&
                                !string.IsNullOrEmpty(overrideProperty.Model as string))
                            {
                                groupName = overrideProperty.Model as string;
                            }
                        }

                        // Hide group and apply ContainerFor if the condition requires it be visible/hidden
                        if (group.DependencyType == ActionForDependencyType.Visible ||
                            group.DependencyType == ActionForDependencyType.Hidden)
                        {
                            if ((group.DependencyType == ActionForDependencyType.Visible && !groupConditionMet)
                                || (group.DependencyType == ActionForDependencyType.Hidden && groupConditionMet))
                            {
                                divTag.AddCssClass("hidden");
                            }

                            divTag.AddCssClass("rhea-visibleif");

                            var htmlAttributes = new Dictionary<string, object>();

                            htmlAttributes.Add(HtmlDataType.DependentPropertyVisibleIf, group.DependentProperty);
                            htmlAttributes.Add(HtmlDataType.ComparisonTypeVisibleIf, group.ComparisonType);
                            htmlAttributes.Add(HtmlDataType.PassOnNullVisibleIf,
                                               group.PassOnNull.ToString().ToLowerInvariant());
                            htmlAttributes.Add(HtmlDataType.FailOnNullVisibleIf,
                                               group.FailOnNull.ToString().ToLowerInvariant());

                            var values = group.DependentValue as object[];

                            if (values != null)
                            {
                                htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf,
                                                   string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                            }
                            else
                            {
                                htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf, group.DependentValue);
                            }

                            divTag.MergeAttributes(htmlAttributes);

                            divTag.Attributes.Add("id",
                                                  string.Format("ContainerFor-{0}Group{1}",
                                                                html.ViewData.TemplateInfo.GetFullHtmlFieldId(
                                                                    Regex.Replace(group.Name, "[^a-zA-Z0-9]",
                                                                                  string.Empty)), ++groupContainers));
                        }
                    }

                    //var rowStyleForGroup = string.Empty;
                    if (groupRowType != null && groupRowType != GroupRowType.Default)
                    {
                        switch (groupRowType)
                        {
                            case GroupRowType.Full:
                                rowStyleForGroup = "col-md-12";
                                break;
                            case GroupRowType.Default:
                                //rowStyleForGroup = "col-md-12";
                                break;
                            case GroupRowType.Half:
                                rowStyleForGroup = "col-md-6";
                                break;
                            case GroupRowType.OneThird:
                                rowStyleForGroup = "col-md-4";
                                break;
                            case GroupRowType.TwoThird:
                                rowStyleForGroup = "col-md-8";
                                break;
                        }

                    }
                    else if (groupRowType != null && groupRowType == GroupRowType.Default)
                    {
                        // TODO: look for the groups that are in the same row (with same row-name) and find their row types.
                        if (otherGroupsInSameRow != null) //&& otherGroupsWithSameRowName.Count() <= 2
                        {
                            int totalColsOccupiedByOtherGroups = otherGroupsInSameRow.Sum(m => (int) m.RowType);

                            switch (totalColsOccupiedByOtherGroups)
                            {
                                case 0:
                                    // no other groups with same name.
                                    rowStyleForGroup = "col-md-12";
                                    break;
                                case 4:
                                    rowStyleForGroup = "col-md-8";
                                    break;
                                case 6:
                                    rowStyleForGroup = "col-md-6";
                                    break;
                                case 8:
                                    rowStyleForGroup = "col-md-4";
                                    break;
                                case 12:
                                    break;
                            }

                        }
                    }


                    if (divTag.Attributes.ContainsKey("class"))
                    {
                        divTag.Attributes["class"] += " " + rowStyleForGroup;
                    }
                    else
                    {
                        divTag.Attributes.Add("class", rowStyleForGroup);
                    }

                    // Opening DIV tag for Visible/Hidden group fieldset
                    sb.Append(divTag.ToString(TagRenderMode.StartTag));

                    // Render FieldSet if appropriate
                    if (groupType == GroupType.FieldSet)
                    {
                        sb.Append(
                            @"<div class=""panel panel-inverse""><div class=""panel-heading""><h4 class=""panel-title"">");
                        sb.Append(groupName);
                        sb.Append(@"</h4></div>");
                        var groupNameID = groupName.Replace(" ", "_");
                        sb.Append(@"<div class=""panel-body"" id=""group-");
                        sb.Append(groupNameID);
                        sb.Append(@""">");
                        sb.Append(html.ValidationMessageSummary(groupName));
                    }
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ObjectEndGroup(this HtmlHelper html, object model,
                                                   IOrderedEnumerable<IGrouping<string, ModelMetadata>> propertyGroups,
                                                   IGrouping<string, ModelMetadata> propertyGroup,
                                                   Dictionary<string, List<GroupAttribute>> listOfGroupsRendered,
                                                   int groupContainers, IEnumerable<GroupAttribute> otherGroupsInSameRow,
                                                   TagBuilder groupRowDivTag)
        {
            var sb = new StringBuilder();
            var groupType = html.GetGroupType(propertyGroup.Key);
            var groups = html.ViewData.ModelMetadata.GetAttributes<GroupAttribute>().ToList();
            var group = groups.SingleOrDefault(a => string.Equals(a.Name, propertyGroup.Key, StringComparison.Ordinal));

            if ((groupType == GroupType.FieldSet || groupType == GroupType.Logical) && propertyGroup.Any(g =>
                {
                    var a = g.GetAttribute<DataTypeAttribute>();

                    if (a != null && string.Equals(a.CustomDataType, CustomDataType.Grid, StringComparison.Ordinal))
                    {
                        // Only display grid if there are results
                        var enumerable = g.Model as IEnumerable<dynamic>;

                        return (enumerable != null && enumerable.Any());

                    }

                    return !g.HideSurroundingHtml;
                }))
            {
                Type typeToUse = model != null ? model.GetType() : html.ViewData.ModelMetadata.ModelType;
                // Get buttons for current group
                var groupButtons =
                    typeToUse.GetAttributes<ButtonAttribute>(
                        b =>
                        !string.IsNullOrEmpty(b.GroupName) &&
                        string.Equals(b.GroupName, propertyGroup.Key, StringComparison.Ordinal));

                // Get links for current group
                var groupLinks =
                    typeToUse.GetAttributes<LinkAttribute>(
                        l =>
                        !string.IsNullOrEmpty(l.GroupName) &&
                        string.Equals(l.GroupName, propertyGroup.Key, StringComparison.Ordinal));
                var groupExternalLinks =
                    typeToUse.GetAttributes<ExternalLinkAttribute>(
                        e =>
                        !string.IsNullOrEmpty(e.GroupName) &&
                        string.Equals(e.GroupName, propertyGroup.Key, StringComparison.Ordinal));
                IEnumerable<ISplitButtonChild> allActions =
                    groupButtons.Cast<ISplitButtonChild>()
                                .Concat(groupLinks.Cast<ISplitButtonChild>())
                                .Concat(groupExternalLinks.Cast<ISplitButtonChild>());
                List<MvcHtmlString> actionsToRender = html.GetButtonArea(allActions);

                //if ((groupType == GroupType.FieldSet || groupType == GroupType.Logical) && !string.IsNullOrEmpty(propertyGroup.Key))
                if ((groupType == GroupType.FieldSet) && !string.IsNullOrEmpty(propertyGroup.Key))
                {
                    sb.Append(@"</div>"); // <!--panel-body -->   
                }
                if (actionsToRender.Any())
                {
                    sb.Append(
                        @"<div class=""panel-footer""><div class=""form-group""><div class=""colB txtR noPad nestedButtons"">");

                    foreach (var item in actionsToRender)
                    {
                        // buttons and links are rendered here.
                        sb.Append(item);
                    }

                    sb.Append(@"</div></div></div>"); //<!--panel-footer -->   
                }

                // Only add to fieldset if an actual field set item (will have key name)
                if (!string.IsNullOrEmpty(propertyGroup.Key))
                {
                    //if (groupType == GroupType.FieldSet || groupType == GroupType.Logical)
                    if (groupType == GroupType.FieldSet)
                    {


                        sb.Append(@"</div>"); // <!--panel-inverse-->

                    }



                    sb.Append(@"</div>"); // <!--Closing DIV for Visible/Hidden for group fieldset-->


                }
            }
            if (group != null)
            {
                // if no other groups in this row, then close Row.
                if (string.IsNullOrEmpty(group.RowName) || (otherGroupsInSameRow == null) ||
                    (otherGroupsInSameRow != null && !otherGroupsInSameRow.Any()))
                {
                    sb.Append(groupRowDivTag.ToString(TagRenderMode.EndTag)); // <!--Row Div-->

                }
                else if (otherGroupsInSameRow != null && !string.IsNullOrEmpty(group.RowName) &&
                         listOfGroupsRendered[group.RowName] != null &&
                         (otherGroupsInSameRow.Count() + 1) == listOfGroupsRendered[group.RowName].Count)
                {
                    // Close the Row div tag only when the number of groups rendered exceeds the number of groups in same row. 
                    sb.Append(groupRowDivTag.ToString(TagRenderMode.EndTag)); // <!--Row Div-->
                    listOfGroupsRendered[group.RowName].Clear();
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }



        public static MvcHtmlString ObjectProperties(this HtmlHelper html, object model,
                                                     IOrderedEnumerable<IGrouping<string, ModelMetadata>> propertyGroups,
                                                     IGrouping<string, ModelMetadata> propertyGroup)
        {
            var sb = new StringBuilder();
            var propertiesShownInRow = new List<string>();
            var isCalendarViewModel = false;
            var groupType = html.GetGroupType(propertyGroup.Key);
            var groups = html.ViewData.ModelMetadata.GetAttributes<GroupAttribute>().ToList();
            var group = groups.SingleOrDefault(a => string.Equals(a.Name, propertyGroup.Key, StringComparison.Ordinal));
            int currentPropertyIndex = 0;
            int totalProperties = propertyGroup.Count();
            foreach (var property in propertyGroup)
            {
                if (property.PropertyName == "ParentType" || property.PropertyName == "PropertyNameInParent")
                {
                    var parentModel = html.ViewData.ModelMetadata.GetParentModel();

                    if (parentModel != null)
                    {
                        string value = string.Empty;

                        // Check for ParentType in an InheritanceViewModel
                        if (property.PropertyName == "ParentType")
                        {
                            var parentType = parentModel.GetType();

                            value = string.Format("{0}, {1}", parentType.FullName,
                                                  parentType.Assembly.FullName.Substring(0,
                                                                                         parentType.Assembly.FullName
                                                                                                   .IndexOf(',')));
                        }
                        else
                        {
                            // Property name in parent
                            value = html.ViewData.ModelMetadata.PropertyName;
                        }

                        sb.Append(html.Hidden(property.PropertyName, value));

                        continue;
                    }
                }

                var hiddenAttribute = property.GetAttribute<HiddenAttribute>();
                var containerForID = string.Format("ContainerFor-{0}",
                                                   html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName));
                var visible = property.IsVisible() &&
                              (hiddenAttribute == null || (hiddenAttribute != null && hiddenAttribute.LabelOnly));
                bool isNestedViewModel = property.IsViewModel();
                bool isGrid = property.DataTypeName == CustomDataType.Grid ||
                              property.DataTypeName == CustomDataType.GridEditable;
                bool isCheckBox = property.ModelType == typeof (bool) &&
                                  (string.IsNullOrEmpty(property.TemplateHint) ||
                                   string.Equals(property.TemplateHint, "Boolean", StringComparison.Ordinal));
                bool isReadOnlyHyperlink = property.IsReadOnlyHyperlink();
                bool isContentViewModel = property.ModelType ==
                                          typeof (Employment.Web.Mvc.Infrastructure.ViewModels.ContentViewModel);


                // Check if the property is to be excluded from the view entirely
                if (hiddenAttribute != null && hiddenAttribute.ExcludeFromView)
                {
                    continue;
                }
                if (property.ModelType ==
                    typeof (Employment.Web.Mvc.Infrastructure.ViewModels.Calendar.CalendarViewModel) ||
                    property.Model is Employment.Web.Mvc.Infrastructure.ViewModels.Calendar.CalendarViewModel)
                {
                    isCalendarViewModel = true;
                }

                if (currentPropertyIndex == 0 && (!isNestedViewModel || isContentViewModel) &&
                    !string.IsNullOrEmpty(propertyGroup.Key))
                {
                    sb.Append(@"<div class=""form-horizontal"">");
                }

                if (property.ModelType == typeof (IndicatorType?) ||
                    property.ModelType == typeof (IEnumerable<LayoutType>))
                {
                    continue;
                }

                var selector = property.GetAttribute<SelectorAttribute>();

                if (selector != null)
                {
                    continue;
                }

                var row = property.GetAttribute<RowAttribute>();
                IEnumerable<ModelMetadata> otherPropertiesInSameRow = null;

                // Get other properties within the current property group that are in the same row
                if (row != null)
                {
                    otherPropertiesInSameRow = propertyGroup.Where(m =>
                        {
                            // Ignore current property
                            if (string.Equals(m.PropertyName, property.PropertyName, StringComparison.Ordinal))
                            {
                                return false;
                            }

                            var r = m.GetAttribute<RowAttribute>();

                            return (r != null && string.Equals(r.Name, row.Name, StringComparison.Ordinal));
                        });

                    if (otherPropertiesInSameRow != null)
                    {
                        if (!propertiesShownInRow.Any() && (!isNestedViewModel || isContentViewModel))
                            // Dont show row div if this is a nested view model
                        {
                            sb.Append(@"<div class=""form-group"">"); //@*row*@
                        }

                        propertiesShownInRow.Add(property.PropertyName);
                    }
                }
                else if (!isNestedViewModel || isContentViewModel) // Dont show row div if this is a nested view model
                {
                    sb.Append(@"<div class=""form-group"">"); //@*row*@
                }

                var containerDivTag = new TagBuilder("div");
                    // div for ContainerFor- id (the container div needs to contain both the label and input in order to hide/show both correctly)
                containerDivTag.Attributes.Add("id", containerForID);
                //containerDivTag.AddCssClass(rowStyleForGroup);

                var divTag = new TagBuilder("div"); // col size div tag for property
                divTag.Attributes.Add("id", string.Format("Inner{0}", containerForID));

                int cols = 12; //10
                int defaultLabelCols = 0; //2

                if (row != null)
                {
                    var otherPropCount = otherPropertiesInSameRow.Count();
                    switch (row.RowType)
                    {
                        case RowType.Default:
                            if (otherPropertiesInSameRow != null && otherPropCount == 1)
                            {
                                cols = 4 + 2; // total properties in same group = 2
                            }
                            else if (otherPropertiesInSameRow != null && otherPropCount == 2)
                            {
                                cols = 2 + 2; // total properties in same group = 3
                            }
                            else if (otherPropertiesInSameRow != null && otherPropCount == 3)
                            {
                                cols = 2 + 1; // total properties in same group = 4 RowType.Quarter
                            }
                            break;
                        case RowType.Dynamic:
                            if (otherPropertiesInSameRow != null && otherPropCount == 0)
                            {
                                cols = 6 + 6; // because label type dynamic also has cols =6.
                            }
                            else if (otherPropertiesInSameRow != null && otherPropCount == 1)
                            {
                                cols = 4 + 2; // total properties in same group = 2
                            }
                            else if (otherPropertiesInSameRow != null && otherPropCount == 2)
                            {
                                cols = 2 + 1; // total properties in same group = 3
                            }
                            break;
                        case RowType.Half:
                            cols = 4 + 2;
                            break;
                        case RowType.Third:
                            cols = 2 + 2;
                            break;
                        case RowType.Quarter:
                            cols = 2 + 1; // Label cols are set to 1 when RowType is Quarter.
                            break;

                    }

                }
                if ((property.ModelType != null && isContentViewModel) || isGrid)
                {
                    cols += defaultLabelCols;
                }
                string colSize = "col-md-" + cols;

                // Don't need to apply 'colB' formatting to nested view model
                if (!isNestedViewModel || isContentViewModel)
                {

                    if (row == null || row != null && row.RowType != RowType.Dynamic)
                        // Don't include 'colB' if row type is dynamic
                    {
                        divTag.AddCssClass("colB");
                    }

                    containerDivTag.AddCssClass(colSize);
                    //divTag.AddCssClass(colSize);
                }
                else
                {
                    // Apply contingent html attributes to nested view model
                    var htmlAttributes = html.GetHtmlAttributes("readonly", property);

                    // Remove readonly setting when applying to DIV to validate against W3C
                    htmlAttributes.Remove("readonly");
                    htmlAttributes.Remove(HtmlDataType.ReadOnlyType);
                    // Add col size to div
                    //if(htmlAttributes.ContainsKey("class"))
                    //{
                    //    htmlAttributes["class"] += " " + colSize;
                    //}
                    //else
                    //{
                    //    htmlAttributes.Add("class", colSize);
                    //}
                    divTag.MergeAttributes(htmlAttributes);
                }

                if (!visible)
                {
                    containerDivTag.AddCssClass("hidden");
                }

                if (row != null && row.RowType != RowType.Flow)
                {
                    //divTag.AddCssClass(row.RowType.ToString().ToLower());
                }

                if (html.ViewData.ModelState != null && html.ViewData.ModelState.ContainsKey(property.PropertyName) &&
                    html.ViewData.ModelState[property.PropertyName].Errors.Any())
                {
                    divTag.AddCssClass("error");
                }

                // Div must start before label so the ContainerFor- includes the label and form control ([VisibleIf] hides/shows the ContainerFor- so we need the label inside to hide/show it as well)
                sb.Append(containerDivTag.ToString(TagRenderMode.StartTag));

                if (property.HideSurroundingHtml)
                {
                    sb.Append(divTag.ToString(TagRenderMode.StartTag));
                    sb.Append(html.Editor(property.PropertyName, new {ParentModel = model}));
                    sb.Append("</div>");
                }
                else
                {
                    // Get property level links and buttons
                    var propertyLinksAndButtons = html.GetPropertyLinksAndButtons(property, false);

                    // Just need to make it so if not nested view model or grid, still output buttons/links, just not inside the label
                    if (!isNestedViewModel && !isGrid)
                    {
                        if (!isCheckBox)
                        {
                            sb.Append(html.GetLabel(property, html.ViewData.ModelMetadata));
                        }

                        if (propertyLinksAndButtons.Any())
                        {

                        }
                    }

                    // Render label after control for checkboxes
                    if (isCheckBox)
                    {
                        sb.Append(html.GetLabel(property, html.ViewData.ModelMetadata));
                    }

                    sb.Append(divTag.ToString(TagRenderMode.StartTag));


                    // This div must appear before the form control in order for the float right to interact properly with the BFC.
                    if (!isNestedViewModel && !isGrid && !isReadOnlyHyperlink)
                    {
                        foreach (var linkOrButton in html.GetPropertyLinksAndButtons(property, true))
                        {
                            sb.Append(@"<div class=""form-control-appendix"">");
                            sb.Append(linkOrButton);
                            sb.Append(@"</div>");
                        }
                    }

                    if (propertyLinksAndButtons.Any())
                    {
                        sb.Append(@"<div class=""form-control-with-appendix"">");
                    }

                    sb.Append(html.Editor(property.PropertyName,
                                          new {ParentModel = model, PropertyNameInParentModel = property.PropertyName}));

                    if (propertyLinksAndButtons.Any())
                    {
                        sb.Append(@"</div>");
                    }

                    if (isNestedViewModel && !isGrid)
                    {
                        foreach (var l in propertyLinksAndButtons)
                        {
                            sb.Append(l);
                        }
                    }
                    sb.Append(divTag.ToString(TagRenderMode.EndTag)); //<!--column size div tag for property-->
                }

                sb.Append(containerDivTag.ToString(TagRenderMode.EndTag));

                if (!isNestedViewModel || isContentViewModel)
                {
                    if (row != null)
                    {
                        if (otherPropertiesInSameRow != null &&
                            (otherPropertiesInSameRow.Count() + 1) == propertiesShownInRow.Count())
                        {
                            propertiesShownInRow.Clear();
                            sb.Append(@"</div>"); //<!--form-group div for property (with RowType) is closed here-->
                        }
                    }
                    else
                    {
                        propertiesShownInRow.Clear();
                        sb.Append(@"</div>"); // <!--form-group div for property (without RowType) is closed here-->
                    }
                }


                // Buttons assigned to a group should go at end of group

                currentPropertyIndex++;

                if (currentPropertyIndex == totalProperties && (!isNestedViewModel || isContentViewModel) &&
                    !string.IsNullOrEmpty(propertyGroup.Key))
                {
                    sb.Append(@"</div>"); // <!--div class="form-horizontal"-->
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        */
        }
}
