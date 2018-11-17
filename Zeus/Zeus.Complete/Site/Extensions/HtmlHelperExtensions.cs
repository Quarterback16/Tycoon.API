using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.Web.Helpers;
using Employment.Web.Mvc.Infrastructure.Controllers;
using System.Text.RegularExpressions;
using System.Web;

namespace Employment.Web.Mvc.Zeus.Extensions
{
    public static class HtmlHelperExtension
    {
        private static IAdwService AdwService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IAdwService>() : null;
            }
        }

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
        /// Shows a bulletin list as HTML.
        /// </summary>
        /// <param name="html">The Html Helper.</param>
        /// <param name="model">The Bulletin View Model.</param>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Pageable collection must be type of IPageable.</exception>
        public static MvcHtmlString ShowBulletinList(this HtmlHelper html, BulletinsViewModel model, ModelMetadata modelMetadata, UrlHelper url)
        {
            
            const string propertyNameInParentModel = "Bulletins";
            ModelMetadata propertyModelMetadata = modelMetadata.Properties.FirstOrDefault(p => String.Equals( p.PropertyName, propertyNameInParentModel,StringComparison.Ordinal));
            IEnumerable<BulletinViewModel> models = model.Bulletins.AsEnumerable();

            // Default paging
            int page = 1;
            int pageSize = 50;

            // Determine paging metadata
            PageMetadata pagedMetadata = null;
            var pagedAttribute = propertyModelMetadata.GetAttribute<PagedAttribute>();
            if (pagedAttribute != null)
            {
                //check if Page Size property based
                if (pagedAttribute.Size is string)
                {
                    //check if property exists in the model
                    var property = modelMetadata.Properties.FirstOrDefault(x => String.Equals(x.PropertyName, pagedAttribute.Size as string, StringComparison.Ordinal));
                    if (property == null)
                    {
                        throw new InvalidOperationException(string.Format("Dependent Page Size property {0} does not exist.", pagedAttribute.Size as string));
                    }
                    if (property.Model is int)
                    {
                        pagedAttribute.Size = property.Model;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Dependent Page Size property {0} must be type of integer.", pagedAttribute.Size as string));
                    }
                }

                // Use paged attribute size if set
                pageSize = (pagedAttribute.Size is int && (int) pagedAttribute.Size > 0) ? (int) pagedAttribute.Size : pageSize;

                var pageable = model.Bulletins as IPageable;

                if (pageable == null)
                {
                    throw new InvalidOperationException("Pageable collection must be type of IPageable.");
                }

                pagedMetadata = pageable.Metadata;

                if (pagedMetadata == null)
                {
                    pagedMetadata = new PageMetadata();
                }

                // Use model count for total if the paged metadata does not have a total
                if (pagedMetadata.Total == 0 && model.Bulletins.Count() > pagedMetadata.Total)
                {
                    pagedMetadata.Total = model.Bulletins.Count();
                }

                // Use page number from paged metadata if set
                if (pagedMetadata.PageNumber > 1)
                {
                    // Minus one as paged metadata are the details for the next page, not the current page
                    page = pagedMetadata.PageNumber - 1;
                }
                else
                {
                    // Page model if it has more items than the allowed page size
                    models = model.Bulletins.Count() > pageSize ? model.Bulletins.AsEnumerable().Skip((page - 1)*pageSize).Take(pageSize) : model.Bulletins;
                }
            }
            else
            {
                // No paging if no paged attribute
                page = 1;
                pageSize = model.Bulletins.Count();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<section id=\"content\" class=\"content\"><div class=\"row\"><ul id=\"bulletins\">");

            foreach (BulletinViewModel bulletin in models)
            {
                var date = bulletin.LiveDate.ToShortDateString();
                sb.AppendFormat("<li><span>{0}</span>{1}</li>", date, html.ActionLink(bulletin.Title, "Bulletin", "Default", new { Area = string.Empty, id = bulletin.PageId }, new { title = string.Format("{0} {1}", date, bulletin.Title) }));
            }
            sb.Append("</ul></div></section>");

            if (pagedMetadata != null)
            {
                // Setup paged metadata of next page
                pagedMetadata.ModelType = typeof (Employment.Web.Mvc.Infrastructure.ViewModels.BulletinViewModel);
                pagedMetadata.PropertyName = propertyNameInParentModel;
                pagedMetadata.PageNumber = page + 1; // Next page
                pagedMetadata.PageSize = pageSize;

                // Show load more link if there are more pages
                if (pagedMetadata.HasMorePages())
                {
                    TagBuilder pagedLink = new TagBuilder("a");

                    pagedLink.AddCssClass("requestMore");
                    pagedLink.AddCssClass("rhea-paged");

                    var htmlAttributes = new Dictionary<string, object>();

                    htmlAttributes.Add(HtmlDataType.PropertyIdGrid, "bulletins");
                    htmlAttributes.Add(HtmlDataType.PagedMetadata, pagedMetadata.Serialize());

                    var routes = new RouteValueDictionary();

                    if (!string.IsNullOrEmpty(pagedAttribute.Area))
                    {
                        routes.Add("Area", pagedAttribute.Area);
                    }

                    htmlAttributes.Add(HtmlDataType.Url, url.Action(pagedAttribute.Action, pagedAttribute.Controller, routes));

                    pagedLink.MergeAttributes(htmlAttributes);

                    pagedLink.InnerHtml = "<span>Load More <span class=\"readers\">bulletins</span></span>";

                    pagedLink.MergeAttribute("href", "#");

                    // Output link
                    sb.Append(pagedLink.ToString(TagRenderMode.Normal));
                }
            }
            return new MvcHtmlString(sb.ToString());
        }


        /// <summary>
        /// Shows a bulletin as HTML.
        /// </summary>
        /// <param name="html">The Html Helper.</param>
        /// <param name="model">The Bulletin View Model.</param>
        /// <returns></returns>
        public static MvcHtmlString ShowBulletin(this HtmlHelper html, BulletinViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<section id=\"content\" class=\"content\">");
            sb.AppendFormat("<h2>{0}</h2>", model.Title);
            sb.Append("<div class=\"row\">");
            sb.Append(model.Html);
            sb.Append("</div>");
            sb.Append("<div class=\"clearBoth\">&nbsp;</div>");
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"colB txtR noPad\">");
            sb.AppendFormat("<span class=\"input-button\">{0}</span>", html.ActionLink("View All Bulletins", "Bulletins", "Default", null, new {@class = "button"}));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</section>");
            return new MvcHtmlString(sb.ToString());
        }



        /// <summary>
        /// Shows the number.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static MvcHtmlString ShowNumber(this HtmlHelper html, object model, ViewContext context)
        {
            IDictionary<string, object> htmlAttributes = html.GetHtmlAttributes("readonly");
            bool isCurrency = false;

            var value = context.ViewData.ModelMetadata.Model;
            object defaultValue = null;

            // Check for default value
            if (html.ViewData.ModelMetadata.TryGetDefaultValue(out defaultValue))
            {
                // Only use default value if model value is nullable and null or model value is zero
                if ((context.ViewData.ModelMetadata.ModelType.IsNullableType() && value == null) || Convert.ToString(value) == "0")
                {
                    context.ViewData.TemplateInfo.FormattedModelValue = value = defaultValue;
                }
            }

            if (DataType.Currency.ToString().Equals(context.ViewData.ModelMetadata.DataTypeName, StringComparison.Ordinal))
            {
                isCurrency = true;

                if (htmlAttributes.ContainsKey(HtmlDataType.Decimal))
                {
                    htmlAttributes[HtmlDataType.Decimal] = 2;
                }
                context.ViewData.TemplateInfo.FormattedModelValue = string.Format(CultureInfo.CurrentCulture, "{0:C}", new[] { value });
            }
            else if (htmlAttributes.ContainsKey(HtmlDataType.Decimal) && "2".Equals(htmlAttributes[HtmlDataType.Decimal].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (context.ViewData.TemplateInfo.FormattedModelValue == context.ViewData.ModelMetadata.Model)
                {
                    context.ViewData.TemplateInfo.FormattedModelValue = string.Format(CultureInfo.CurrentCulture, "{0:0.00}", new[] { value });
                }
            }

            // Right align numeric values unless it is an ID
            if (context.ViewData.ModelMetadata.GetAttribute<IdAttribute>() == null)
            {
                htmlAttributes.MergeCssClass("txtR");
            }

            htmlAttributes.MergeCssClass("form-control");
            MvcHtmlString result = html.TextBox(string.Empty, context.ViewData.TemplateInfo.FormattedModelValue, htmlAttributes);

            // Replace 'data-val-number' with 'data-val-currency' for currency to be correctly validated client-side
            if (isCurrency)
            {
                result = new MvcHtmlString(result.ToString().Replace("data-val-number", "data-val-currency"));
            }

            return result;
        }


        /// <summary>
        /// Shows the select list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="modelList">The model list.</param>
        /// <returns></returns>
        public static MvcHtmlString ShowSelectList(this HtmlHelper html, IEnumerable<SelectListItem> modelList, ModelMetadata modelMetadata)
        {
            return html.ShowSelectList(modelList, modelMetadata, html.GetHtmlAttributes("disabled"));
        }

        /// <summary>
        /// Shows the select list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="modelList">The model list.</param>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <param name="htmlAttributes">A preprepared dictionary of HTML attributes, such as that returned by GetHtmlAttributes</param>
        /// <returns></returns>
        public static MvcHtmlString ShowSelectList(this HtmlHelper html, IEnumerable<SelectListItem> modelList, ModelMetadata modelMetadata, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();

            var model = modelList != null ? modelList.ToList() : Enumerable.Empty<SelectListItem>().ToList();

            List<SelectListItem> selectionItems = new List<SelectListItem>();

            var selectionType = SelectionType.Single;
            var selectionTypeAttribute = modelMetadata.GetAttribute<SelectionTypeAttribute>();
            if (selectionTypeAttribute != null)
            {
                selectionType = selectionTypeAttribute.SelectionType;
            }
            else if (modelMetadata.ModelType == typeof(MultiSelectList))
            {
                selectionType = SelectionType.Multiple;
            }

            // Disable the control if no selection is being allowed
            if (selectionType == SelectionType.None)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            // Determine if this select list should be displayed using radios or checkboxes instead of as a drop down
            bool isRadioButtonGroup = (selectionType == SelectionType.Single && (modelMetadata.DataTypeName == CustomDataType.RadioButtonGroupVertical || modelMetadata.DataTypeName == CustomDataType.RadioButtonGroupHorizontal));
            bool isCheckBoxList = (selectionType == SelectionType.Multiple && modelMetadata.DataTypeName == CustomDataType.CheckBoxList);

            var emptySelection = model.FirstOrDefault(p => String.Equals(p.Value, string.Empty, StringComparison.Ordinal));

            if (emptySelection != null)
            {
                model.Remove(emptySelection);
            }

            var defaultValueAttribute = modelMetadata.GetAttribute<DefaultValueAttribute>();

            var defaultValues = Enumerable.Empty<string>();

            if (defaultValueAttribute != null && defaultValueAttribute.Value != null)
            {
                if (defaultValueAttribute.Value.GetType().IsArray)
                {
                    defaultValues = defaultValueAttribute.Value as IEnumerable<string>;
                }
                else
                {
                    defaultValues = new[] {defaultValueAttribute.Value as string}.AsEnumerable();
                }
            }

            var ajax = modelMetadata.GetAttribute<AjaxSelectionAttribute>();
            var adw = modelMetadata.GetAttribute<AdwSelectionAttribute>();
            bool isUsingAjax = (ajax != null || adw != null);

            if (!model.Any() && !htmlAttributes.ContainsKey("disabled"))
            {
                // Disable if there are no items to choose from
                if (!isUsingAjax)
                {
                    htmlAttributes.Add("disabled", "disabled");
                }
                else if (ajax != null && ajax.Parameters != null)
                {
                    // Disable if using Ajax that depends on other parameters and one or more of those parameters are empty
                    // TODO: Performance tune
                    var parentModel = modelMetadata.AdditionalValues["ParentModel"];
                    var parentModelPropertiesMetadata = ModelMetadataProviders.Current.GetMetadataForProperties(parentModel, parentModel.GetType());

                    foreach (var parameter in ajax.Parameters)
                    {
                        var isEmpty = false;
                        var parameterMetadata = parentModelPropertiesMetadata.FirstOrDefault(m => String.Equals(m.PropertyName, parameter,StringComparison.Ordinal));

                        if (parameterMetadata != null)
                        {
                            if (parameterMetadata.Model == null)
                            {
                                isEmpty = true;
                            }
                            else if (parameterMetadata.ModelType == typeof (string))
                            {
                                if (string.IsNullOrEmpty(parameterMetadata.Model as string))
                                {
                                    isEmpty = true;
                                }
                            }
                            else
                            {
                                var selectList = parameterMetadata.Model as IEnumerable<SelectListItem>;

                                if (selectList != null)
                                {
                                    if (!selectList.Any(p => p.Selected && !string.IsNullOrEmpty(p.Value)))
                                    {
                                        isEmpty = true;
                                    }
                                }
                                else if (parameterMetadata.ModelType.GetConstructor(Type.EmptyTypes) != null && ComparisonType.EqualTo.Compare(parameterMetadata.Model, parameterMetadata.ModelType.GetDefaultValue()))
                                {
                                    isEmpty = true;
                                }
                            }
                        }

                        if (isEmpty)
                        {
                            htmlAttributes.Add("disabled", "disabled");
                            break;
                        }
                    }
                }
            }

            var emptyMessage = " ";

            htmlAttributes.Add(HtmlDataType.EmptyMessage, emptyMessage);

            if (selectionType != SelectionType.Multiple)
            {
                emptySelection = new SelectListItem { Value = string.Empty, Text = emptyMessage };
                selectionItems.Add(emptySelection);
                model.Add(emptySelection);
            }

            if (isUsingAjax)
            {

                // Only include empty item and selected/default item or first item as the rest will be retrieved by Ajax on request
                var selectedItem = model.Any(p => p.Selected) ? model.FirstOrDefault(p => p.Selected) : model.FirstOrDefault();

                if (selectedItem != null)
                {
                    // Only add if not already added
                    if (!selectionItems.Any(p => String.Equals(p.Value, selectedItem.Value, StringComparison.Ordinal)))
                    {
                        selectionItems.Add(selectedItem);
                    }

                    string name = string.Format("{0}[0]", modelMetadata.PropertyName);

                    // Necessary for model binding of IEnumerable<SelectListItem> (just doing selected item only)
                    sb.Append(html.ExecuteUpOneLevel(() => html.Hidden(string.Format("{0}.Value", name), selectedItem.Value)));
                    sb.Append(html.ExecuteUpOneLevel(() => html.Hidden(string.Format("{0}.Text", name), selectedItem.Text)));
                }
            }
            else
            {
                // Include all items
                selectionItems.AddRange(model);

                int rowNumber = 0;
                for (int i = 0; i < model.Count; i++)
                {
                    var m = model[i];
                    string name = string.Format("{0}[{1}]", modelMetadata.PropertyName, rowNumber);

                    // Necessary for model binding of IEnumerable<SelectListItem>
                    sb.Append(html.ExecuteUpOneLevel(() => html.Hidden(string.Format("{0}.Value", name), m.Value)));
                    sb.Append(html.ExecuteUpOneLevel(() => html.Hidden(string.Format("{0}.Text", name), m.Text)));

                    rowNumber++;
                }
            }

            // Do default selection if none are selected
            if (!selectionItems.Any(s => s.Selected))
            {
                if (defaultValues.Any())
                {
                    // Set to value specified by DefaultValue attribute
                    foreach (var defaultValue in defaultValues)
                    {
                        var selectionItem = selectionItems.FirstOrDefault(s => String.Equals(s.Value, defaultValue, StringComparison.Ordinal));

                        if (selectionItem != null)
                        {
                            selectionItem.Selected = true;
                        }
                    }
                }
                else if (selectionType != SelectionType.Multiple)
                {
                    // Otherwise just select the first item, but not if this is a multi select
                    var selectionItem = selectionItems.FirstOrDefault();

                    if (selectionItem != null)
                    {
                        selectionItem.Selected = true;
                    }
                }
            }

            // Cannot render as radio button group or check box list using ajax
            if (isRadioButtonGroup && !isUsingAjax)
            {
                 sb.Append(html.RadioButtonGroup(selectionItems, htmlAttributes));
            }
            else if (isCheckBoxList && !isUsingAjax)
            {
                sb.Append(html.CheckBoxList(selectionItems, htmlAttributes));
            }
            else
            {
                // Include unobtrusive validation attributes
                html.GetUnobtrusiveValidationAttributes(modelMetadata.PropertyName, modelMetadata).ForEach(a =>
                {
                    if (!htmlAttributes.ContainsKey(a.Key))
                    {
                        htmlAttributes.Add(a.Key, a.Value);
                    }
                });

                if (selectionType == SelectionType.Multiple) htmlAttributes.Add("multiple", "multiple");
                sb.Append(html.ExecuteUpOneLevel(() => html.DropDownList(modelMetadata.PropertyName, selectionItems, htmlAttributes)));
            }
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Shows the boolean.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewData">The view data.</param>
        /// <returns></returns>
        public static MvcHtmlString ShowBoolean(this HtmlHelper html, bool? model, ViewDataDictionary<bool?> viewData)
        {
            if (viewData.ModelMetadata.IsNullableValueType)
            {
                return html.DropDownList(string.Empty, new List<SelectListItem> {new SelectListItem {Text = " ", Value = String.Empty, Selected = !model.HasValue}, new SelectListItem {Text = "Yes", Value = "true", Selected = model.HasValue && model.Value}, new SelectListItem {Text = "No", Value = "false", Selected = model.HasValue && !model.Value},}, html.GetHtmlAttributes("disabled"));
            }
            else
            {
                TagBuilder checkbox = new TagBuilder("input");

                // Include unobtrusive validation attributes
                checkbox.MergeAttributes(html.GetUnobtrusiveValidationAttributes(viewData.ModelMetadata.PropertyName, viewData.ModelMetadata));

                // But exclude the one based on [Required] if the property was not explicity decorated with [Required]
                if (viewData.ModelMetadata.GetAttribute<RequiredAttribute>() == null)
                {
                    checkbox.Attributes.Remove("data-val-required");
                }

                // If removing [Required] results in there only being the attribute that indicates there are validation attributes, then remove the indicator as well
                if (checkbox.Attributes.Count == 1 && checkbox.Attributes.ContainsKey("data-val") )
                {
                    checkbox.Attributes.Remove("data-val");
                }
                
                checkbox.Attributes.Add("id", viewData.TemplateInfo.GetFullHtmlFieldId(string.Empty));
                checkbox.Attributes.Add("name", viewData.TemplateInfo.GetFullHtmlFieldName(string.Empty));
                checkbox.Attributes.Add("type", "checkbox");
                checkbox.Attributes.Add("value", "true");

                if (model ?? false)
                {
                    checkbox.Attributes.Add("checked", "checked");
                }

                checkbox.MergeAttributes(html.GetHtmlAttributes("disabled"));

                SwitcherAttribute switcher = viewData.ModelMetadata.GetAttribute<SwitcherAttribute>() ?? new SwitcherAttribute();
                if (viewData.ModelMetadata.DataTypeName != CustomDataType.CheckBoxList)
                {
                    checkbox.MergeAttribute(HtmlDataType.Switcher, "true");
                    checkbox.MergeAttribute(HtmlDataType.SwitcherChecked, switcher.CheckedText);
                    checkbox.MergeAttribute(HtmlDataType.SwitcherUnchecked, switcher.UncheckedText);
                }
                
                TagBuilder hidden = new TagBuilder("input");
                hidden.Attributes.Add("name", viewData.TemplateInfo.GetFullHtmlFieldName(string.Empty));
                hidden.Attributes.Add("type", "hidden");
                hidden.Attributes.Add("value", "false");
                
                return new MvcHtmlString(checkbox.ToString(TagRenderMode.SelfClosing) + hidden.ToString(TagRenderMode.SelfClosing));
            }
        }

        /// <summary>
        /// Show ADW selection supporting single selection for properties decorated with <see cref="AdwSelectionAttribute"/>.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The ADW selection.</returns>
        public static MvcHtmlString AdwSelection(this HtmlHelper html, string model)
        {
            var adwSelection = html.ViewData.ModelMetadata.GetAttribute<AdwSelectionAttribute>();

            if (adwSelection == null)
            {
                throw new InvalidOperationException("Model is missing the AdwSelectionAttribute.");
            }

            var parentModel = html.ViewData.ModelMetadata.GetParentModel();

            // Try to use default value when model is not set
            if (string.IsNullOrWhiteSpace(model))
            {
                var defaultValue = html.ViewData.ModelMetadata.GetAttribute<DefaultValueAttribute>();

                if (defaultValue != null)
                {
                    model = defaultValue.Value as string;
                }
            }

            if (adwSelection.SelectionType == SelectionType.None)
            {
                var selectedListItem = adwSelection.GetSelectListItem(parentModel, model).FirstOrDefault(s => s.Selected);

                // When no selection is allowed, render selected description as a readonly textbox and the actual model value as a hidden input
                return html.TextBox("DisplayText", (selectedListItem != null) ? selectedListItem.Text : null, new Dictionary<string, object> { { "readonly", "readonly" }, { "class", "form-control" } }).Concat(html.Hidden(string.Empty, model));
            }

            // Get all items
            var selectListItems = adwSelection.GetSelectListItems(parentModel, model);

            // Setup ADW data attributes
            var htmlAttributes = html.GetHtmlAttributes("disabled");

            if (!string.IsNullOrEmpty(adwSelection.DependentProperty))
            {
                htmlAttributes.MergeCssClass("rhea-hierarchy");
                htmlAttributes.Add(HtmlDataType.DependentPropertyId, html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(adwSelection.DependentProperty)));
            }
            else if (!string.IsNullOrEmpty(adwSelection.DependentValue))
            {
                htmlAttributes.Add(HtmlDataType.DependentValueAdw, adwSelection.DependentValue);
            }

            if (adwSelection.ExcludeValues != null)
            {
                htmlAttributes.Add(HtmlDataType.ExcludeValues, string.Join(",", adwSelection.ExcludeValues));
            }

            htmlAttributes.Add(HtmlDataType.EmptyMessage, " ");
            
            bool isRadioButtonGroup = (html.ViewData.ModelMetadata.DataTypeName == CustomDataType.RadioButtonGroupVertical || html.ViewData.ModelMetadata.DataTypeName == CustomDataType.RadioButtonGroupHorizontal);

            if (isRadioButtonGroup)
            {
                if (selectListItems == null || !selectListItems.Any())
                {
                    htmlAttributes["disabled"] = "disabled";
                }

                return html.RadioButtonGroup(selectListItems, htmlAttributes);
            }
            else
            {
                // Activate Ajax for dropdowns with a dependency on another property or more than X items
                if (!string.IsNullOrEmpty(adwSelection.DependentProperty) || selectListItems.Count() > InfrastructureController.AjaxSelectionPageSize)
                {
                    htmlAttributes.Add(HtmlDataType.Url, UrlHelper.GenerateUrl(null, (adwSelection.AdwType == AdwType.RelatedCode) ? "RelatedCode" : "ListCode", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, false));
                    htmlAttributes.Add(HtmlDataType.Code, adwSelection.Code);
                    htmlAttributes.Add(HtmlDataType.Dominant, adwSelection.Dominant.ToString().ToLower());
                    htmlAttributes.Add(HtmlDataType.OrderType, adwSelection.OrderType);
                    htmlAttributes.Add(HtmlDataType.DisplayType, adwSelection.DisplayType);
                    htmlAttributes.MergeCssClass("form-control", "rhea-adw");

                    if (selectListItems.Any())
                    {
                        // Only include selected item for Ajax dropdown (the rest will be retrieved on open so no need to include full list on initial render)
                        selectListItems = selectListItems.Where(s => s.Selected);
                    }
                }

                // Disable if no items to select from
                if (!selectListItems.Any() && !htmlAttributes.ContainsKey("disabled"))
                {
                    htmlAttributes.Add("disabled", "disabled");
                }

                // Include an empty option to allow clearing of the dropdown
                selectListItems = new List<SelectListItem> { new SelectListItem { Value = string.Empty, Text = @" " } }.Concat(selectListItems);
                
                return html.DropDownList(string.Empty, selectListItems, htmlAttributes);
            }
        }

        /// <summary>
        /// Show ADW selection supporting multiple selections for properties decorated with <see cref="AdwSelectionAttribute"/>.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The ADW selection.</returns>
        public static MvcHtmlString AdwSelection(this HtmlHelper html, IEnumerable<string> model)
        {
            var adwSelection = html.ViewData.ModelMetadata.GetAttribute<AdwSelectionAttribute>();

            if (adwSelection == null)
            {
                throw new InvalidOperationException("Model is missing the AdwSelectionAttribute.");
            }

            var parentModel = html.ViewData.ModelMetadata.GetParentModel();

            // Try to use default values when model is not set
            if (model == null || !model.Any())
            {
                var defaultValueAttribute = html.ViewData.ModelMetadata.GetAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null && defaultValueAttribute.Value != null)
                {
                    if (defaultValueAttribute.Value is IEnumerable<string>)
                    {
                        model = defaultValueAttribute.Value as IEnumerable<string>;
                    }
                    else if (defaultValueAttribute.Value is string)
                    {
                        model = new string [] { defaultValueAttribute.Value as string }.AsEnumerable();
                    }
                }
            }

            // Setup ADW data attributes
            var htmlAttributes = html.GetHtmlAttributes("disabled");

            // Apply ADW class for client-side transform
            htmlAttributes.MergeCssClass("rhea-adw");

            htmlAttributes.Add(HtmlDataType.Url, UrlHelper.GenerateUrl(null, (adwSelection.AdwType == AdwType.RelatedCode) ? "RelatedCode" : "ListCode", "Ajax", new RouteValueDictionary { { "Area", string.Empty } }, html.RouteCollection, html.ViewContext.RequestContext, false));
            htmlAttributes.Add(HtmlDataType.Code, adwSelection.Code);
            htmlAttributes.Add(HtmlDataType.Dominant, adwSelection.Dominant.ToString().ToLower());
            htmlAttributes.Add(HtmlDataType.OrderType, adwSelection.OrderType);
            htmlAttributes.Add(HtmlDataType.DisplayType, adwSelection.DisplayType);

            if (!string.IsNullOrEmpty(adwSelection.DependentProperty))
            {
                htmlAttributes.MergeCssClass("rhea-hierarchy");

                htmlAttributes.Add(HtmlDataType.DependentPropertyId, html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldId(adwSelection.DependentProperty)));
            }
            else if (!string.IsNullOrEmpty(adwSelection.DependentValue))
            {
                htmlAttributes.Add(HtmlDataType.DependentValueAdw, adwSelection.DependentValue);
            }

            if (adwSelection.ExcludeValues != null)
            {
                htmlAttributes.Add(HtmlDataType.ExcludeValues, string.Join(",", adwSelection.ExcludeValues));
            }

            htmlAttributes.Add(HtmlDataType.EmptyMessage, " ");

            // Get all items
            var selectListItems = adwSelection.GetSelectListItems(parentModel, model);

            if (adwSelection.SelectionType == SelectionType.None || !selectListItems.Any())
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            if (html.ViewData.ModelMetadata.DataTypeName == CustomDataType.CheckBoxList)
            {
                return html.CheckBoxList(selectListItems, htmlAttributes);
            }

            // Apply CSS class for styling
            htmlAttributes.MergeCssClass("form-control");
            
            // Include JSON of selected items 
            var selectedListItems = new List<object>();

            foreach (var item in selectListItems)
            {
                if (item.Selected)
                {
                    selectedListItems.Add(new { Value = item.Value, Text = item.Text });
                }
            }

            htmlAttributes.Add(HtmlDataType.MultiSelect, Json.Encode(selectedListItems));

            return html.TextBox(string.Empty, null, htmlAttributes);
        }

        /// <summary>
        /// Show selection supporting single selection for properties decorated with <see cref="SelectionAttribute"/>.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The selection.</returns>
        public static MvcHtmlString Selection(this HtmlHelper html, string model)
        {
            var selection = html.ViewData.ModelMetadata.GetAttribute<SelectionAttribute>();

            if (selection == null)
            {
                throw new InvalidOperationException("Model is missing the SelectionAttribute.");
            }

            var parentModel = html.ViewData.ModelMetadata.GetParentModel();

            // Try to use default value when model is not set
            if (string.IsNullOrWhiteSpace(model))
            {
                var defaultValue = html.ViewData.ModelMetadata.GetAttribute<DefaultValueAttribute>();

                if (defaultValue != null)
                {
                    model = defaultValue.Value as string;
                }
            }

            var selectListItems = selection.GetSelectListItems(model);

            if (selection.SelectionType == SelectionType.None)
            {
                var selectedListItem = selectListItems.FirstOrDefault(s => s.Selected);

                // When no selection is allowed, render selected description as a readonly textbox and the actual model value as a hidden input
                return html.TextBox("DisplayText", (selectedListItem != null) ? selectedListItem.Text : null, new Dictionary<string, object> { { "readonly", "readonly" }, { "class", "form-control" } }).Concat(html.Hidden(string.Empty, model));
            }

            // Setup data attributes
            var htmlAttributes = html.GetHtmlAttributes("disabled");

            if (selectListItems == null || !selectListItems.Any())
            {
                htmlAttributes["disabled"] = "disabled";
            }

            htmlAttributes.Add(HtmlDataType.EmptyMessage, " ");

            bool isRadioButtonGroup = (html.ViewData.ModelMetadata.DataTypeName == CustomDataType.RadioButtonGroupVertical || html.ViewData.ModelMetadata.DataTypeName == CustomDataType.RadioButtonGroupHorizontal);

            if (isRadioButtonGroup)
            {
                return html.RadioButtonGroup(selectListItems, htmlAttributes);
            }
            else
            {
                // Include an empty option to allow clearing of the dropdown
                selectListItems = new List<SelectListItem> { new SelectListItem { Value = string.Empty, Text = @" " } }.Concat(selectListItems);

                return html.DropDownList(string.Empty, selectListItems, htmlAttributes);
            }
        }

        /// <summary>
        /// Show selection supporting multiple selections for properties decorated with <see cref="SelectionAttribute"/>.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The selection.</returns>
        public static MvcHtmlString Selection(this HtmlHelper html, IEnumerable<string> model)
        {
            var selection = html.ViewData.ModelMetadata.GetAttribute<SelectionAttribute>();

            if (selection == null)
            {
                throw new InvalidOperationException("Model is missing the SelectionAttribute.");
            }

            var parentModel = html.ViewData.ModelMetadata.GetParentModel();

            // Try to use default values when model is not set
            if (model == null || !model.Any())
            {
                var defaultValueAttribute = html.ViewData.ModelMetadata.GetAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null && defaultValueAttribute.Value != null)
                {
                    if (defaultValueAttribute.Value is IEnumerable<string>)
                    {
                        model = defaultValueAttribute.Value as IEnumerable<string>;
                    }
                    else if (defaultValueAttribute.Value is string)
                    {
                        model = new string[] { defaultValueAttribute.Value as string }.AsEnumerable();
                    }
                }
            }

            // Setup ADW data attributes
            var htmlAttributes = html.GetHtmlAttributes("disabled");

            htmlAttributes.Add(HtmlDataType.EmptyMessage, " ");

            // Get all items
            var selectListItems = selection.GetSelectListItems(model);

            if (selection.SelectionType == SelectionType.None || !selectListItems.Any())
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            if (html.ViewData.ModelMetadata.DataTypeName == CustomDataType.CheckBoxList)
            {
                return html.CheckBoxList(selectListItems, htmlAttributes);
            }

            htmlAttributes.Add("multiple", "multiple");

            // Include JSON of selected items 
            var selectedListItems = new List<object>();

            foreach (var item in selectListItems)
            {
                if (item.Selected)
                {
                    selectedListItems.Add(new { Value = item.Value, Text = item.Text });
                }
            }

            htmlAttributes.Add(HtmlDataType.MultiSelect, Json.Encode(selectedListItems));

            return html.DropDownList(string.Empty, selectListItems, htmlAttributes);
        }

        /// <summary>
        /// Show date and time inputs.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The date and time inputs.</returns>
        public static MvcHtmlString DateTime(this HtmlHelper html, DateTime? model)
        {
            var sb = new StringBuilder();

            // Get all HTML attributes
            var htmlAttributes = html.GetHtmlAttributes("readonly");
    
            var datePart = string.Empty;
            var timePart = string.Empty;

            var value = (model.HasValue ? model.Value.ToString("d/MM/yyyy hh:mm tt") : string.Empty);
            var values = value.Split(' ');

            if (values.Length == 3)
            {
                datePart = values[0];
                timePart = string.Format("{0} {1}", values[1], values[2]);
            }
    
            // Move data validation attributes to input
            var inputHtmlAttributes = htmlAttributes.Where(a => a.Key.StartsWith("data-val")).ToDictionary(a => a.Key, a => a.Value);

            foreach (var kvp in inputHtmlAttributes)
            {
                htmlAttributes.Remove(kvp.Key);
            }

            inputHtmlAttributes.Add(HtmlDataType.DisplayName, htmlAttributes[HtmlDataType.DisplayName]);
            inputHtmlAttributes.MergeCssClass("form-control");
            inputHtmlAttributes.Add(HtmlDataType.DateTimePicker, true);
    
            var dateInput = html.ExecuteUpOneLevel(() => html.TextBox(string.Format("{0}.Date", html.ViewData.ModelMetadata.PropertyName), datePart, inputHtmlAttributes));
            var timeInput = html.ExecuteUpOneLevel(() => html.TextBox(string.Format("{0}.Time", html.ViewData.ModelMetadata.PropertyName), timePart, inputHtmlAttributes));
            var timeLabel = html.Label("Time", html.ViewData.ModelMetadata.GetDisplayName(), new { @class = "hidden" }).ToString().Replace("</label>", " <span class=\"readers\">(Time)</span></label>");

            sb.Append(@"<div class=""input-group datetime"">");
            sb.Append(dateInput);
            sb.Append(@"<span class=""input-group-addon fordate""><i class=""fa fa-calendar""></i></span>");
            sb.Append(timeLabel);
            sb.Append(timeInput);
            sb.Append(@"<span class=""input-group-addon fortime""><i class=""fa fa-clock-o""></i></span>");
            sb.Append(html.Hidden(string.Empty, value, htmlAttributes));
            sb.Append(@"</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Show date input.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The date input.</returns>
        public static MvcHtmlString Date(this HtmlHelper html, DateTime? model)
        {
            var sb = new StringBuilder();

            // Get all HTML attributes
            var htmlAttributes = html.GetHtmlAttributes("readonly");

            htmlAttributes.MergeCssClass("form-control");
            htmlAttributes.Add(HtmlDataType.DatePicker, true);

            sb.Append(@"<div class=""input-group date"">");
            sb.Append(html.TextBox(string.Empty, model.HasValue ? model.Value.ToString("d/MM/yyyy") : string.Empty, htmlAttributes));
            sb.Append(@"<span class=""input-group-addon""><i class=""fa fa-calendar""></i></span></div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Show time input.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The time input.</returns>
        public static MvcHtmlString Time(this HtmlHelper html, DateTime? model)
        {
            var sb = new StringBuilder();

            // Get all HTML attributes
            var htmlAttributes = html.GetHtmlAttributes("readonly");
    
            htmlAttributes.MergeCssClass("form-control");
            htmlAttributes.Add(HtmlDataType.TimePicker, true);

            sb.Append(@"<div class=""input-group time"">");
            sb.Append(html.TextBox(string.Empty, model.HasValue ? model.Value.ToString("hh:mm tt") : string.Empty, htmlAttributes));
            sb.Append(@"<span class=""input-group-addon""><i class=""fa fa-clock-o""></i></span></div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Show email address input.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>The email address input.</returns>
        public static MvcHtmlString EmailAddress(this HtmlHelper html, object model)
        {
            var htmlAttributes = html.GetHtmlAttributes("readonly");

            htmlAttributes.Add("type", "email");
            htmlAttributes.MergeCssClass("form-control");

            return html.TextBox(string.Empty, model, htmlAttributes);
        }

        /// <summary>
        /// Show file input.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns>The file input.</returns>
        public static MvcHtmlString File(this HtmlHelper html, HttpPostedFileBase model)
        {
            var input = new TagBuilder("input");

            input.Attributes.Add("type", "file");
            input.Attributes.Add("name", html.ExecuteUpOneLevel(() => html.ViewData.TemplateInfo.GetFullHtmlFieldName(html.ViewData.ModelMetadata.PropertyName)));
            input.MergeAttributes(html.GetHtmlAttributes("readonly"));
            
            var file = html.ViewData.ModelMetadata.GetAttribute<Employment.Web.Mvc.Infrastructure.DataAnnotations.FileAttribute>();

            if (file != null)
            {
                input.Attributes.Add("accept", file.GetAcceptedFileExtensions());
            }
    
            return MvcHtmlString.Create(input.ToString(TagRenderMode.SelfClosing));
        }

        public static string GetActionDisplayName(this HtmlHelper html)
        {
            var actionName = html.GetCurrentActionName(html.ViewData.ModelMetadata.GetDisplayName()).ToString();

            // Allow override of page title on Content View Model
            if (html.ViewData.ModelMetadata.ModelType == typeof(Employment.Web.Mvc.Infrastructure.ViewModels.ContentViewModel))
            {
                var contentViewModel = html.ViewData.Model as Employment.Web.Mvc.Infrastructure.ViewModels.ContentViewModel;

                if (contentViewModel != null && !string.IsNullOrEmpty(contentViewModel.PageTitle))
                {
                    actionName = contentViewModel.PageTitle;
                }
            }

            // Final override of page title
            if (!string.IsNullOrEmpty(html.ViewBag.PageTitle))
            {
                actionName = html.ViewBag.PageTitle;
            }

            return actionName;
        }



        /*


        private static MvcHtmlString ObjectBeginForm(this HtmlHelper html)
        {
            var sb = new StringBuilder();

            // Will only output for root template depth
            if (html.ViewData.TemplateInfo.TemplateDepth <= 1)
            {
                var area = html.ViewContext.RouteData.GetArea();
                area = string.IsNullOrEmpty(area) ? "Home" : area;
                var areaName = html.GetCurrentAreaName().ToString();
                var actionName = html.GetActionDisplayName();

                var layoutOverride = html.ViewData.ModelMetadata.Model as ILayoutOverride;
                var layoutHidden = (layoutOverride != null && layoutOverride.Hidden != null) ? layoutOverride.Hidden : Enumerable.Empty<LayoutType>();

                var contentSection = new TagBuilder("div");

                contentSection.MergeAttribute("id", "content");
                contentSection.AddCssClass("content");

                if (html.ViewData.ModelMetadata.SkipClientSideUnsavedChanges())
                {
                    contentSection.MergeAttribute(HtmlDataType.SkipUnsavedChanges, "true");
                }

                if (html.ViewData.ModelMetadata.SkipClientSideValidation())
                {
                    contentSection.MergeAttribute(HtmlDataType.SkipValidation, "true");
                }

                var menu = html.ShowMenu();

                if (MvcHtmlString.IsNullOrEmpty(menu) || layoutHidden.Contains(LayoutType.LeftHandNavigation))
                {
                    sb.Append(contentSection.ToString(TagRenderMode.StartTag)); // Content section
                    sb.Append(@"<a id=""pagenav""></a>"); // Anchor for readers
                }
                else
                {
                    // Left sidebar
                    sb.Append(@"<div id=""sidebar"" class=""sidebar""><div data-scrollbar=""true"" data-height=""100%"">");
                    sb.Append(@"<a id=""pagenav""></a>"); // Anchor for readers
                    sb.Append(menu.ToString());
                    //sb.Append(html.ShowHistory());
                    sb.Append(@"</div></div><div class=""sidebar-bg""></div>");

                    // Right sidebar
                    sb.Append(@"<div id=""sidebar-right"" class=""sidebar sidebar-right""><div data-scrollbar=""true"" data-height=""100%"">");
                    sb.Append(html.RenderRightSidebar().ToString());
                    sb.Append("</div></div>");

                    sb.Append(contentSection.ToString(TagRenderMode.StartTag)); // Content section
                }

                if (!layoutHidden.Contains(LayoutType.TitleAndBreadcrumbs))
                {
                    sb.Append(@"<ol class=""breadcrumb pull-right""><li class=""areaIcon floatLeft"">");

                    if (area != "Home")
                    {
                        var hiddenTextTag = new TagBuilder("span");

                        hiddenTextTag.AddCssClass("readers");
                        hiddenTextTag.SetInnerText("Parent page:");

                        sb.Append(hiddenTextTag.ToString());

                        var areaLinkTag = new TagBuilder("a");
                        var areaTile = html.GetAllAreaTiles().FirstOrDefault(t => t.AreaName == area);
                        var routeName = (areaTile != null && !string.IsNullOrEmpty(areaTile.RouteName)) ? areaTile.RouteName : string.Format("{0}_default", area);
                        var areaHref = new UrlHelper(new RequestContext(html.ViewContext.RequestContext.HttpContext, new RouteData())).RouteUrl(routeName, new { });

                        areaLinkTag.Attributes.Add("href", areaHref);
                        areaLinkTag.SetInnerText(areaName);

                        sb.Append(areaLinkTag.ToString(TagRenderMode.Normal));
                    }

                    sb.Append(@"</li><li class=""active"">");
                    sb.Append(actionName);
                    sb.Append(@"</li></ol><h1 class=""page-header"">");
                    sb.Append(actionName);
                    sb.Append("</h1>");
                }

                sb.Append(html.BeginMainForm().ToString());

                if (!layoutHidden.Contains(LayoutType.RequiredFieldsMessage))
                {
                    sb.Append(@"<div id=""actionsMenu"" class=""group row""><strong>Note:</strong> Required fields are marked with an asterisk <abbr class=""req"" title=""required"">*</abbr>");
                    sb.Append(@"<a id=""skipnav"">&nbsp;</a>"); // Anchor for readers
                    sb.Append(@"</div>");
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        private static MvcHtmlString ObjectEndForm(this HtmlHelper html)
        {
            var sb = new StringBuilder();

            if (html.ViewData.TemplateInfo.TemplateDepth <= 1)
            {
                Type typeToUse = html.ViewData.Model != null ? html.ViewData.Model.GetType() : html.ViewData.ModelMetadata.ModelType;

                // Ungrouped buttons
                var buttons = typeToUse.GetAttributes<ButtonAttribute>(b => string.IsNullOrEmpty(b.GroupName));
                var links = typeToUse.GetAttributes<LinkAttribute>(l => string.IsNullOrEmpty(l.GroupName));
                var externalLinks = typeToUse.GetAttributes<ExternalLinkAttribute>(e => string.IsNullOrEmpty(e.GroupName));

                IEnumerable<ISplitButtonChild> allActions = buttons.Cast<ISplitButtonChild>().Concat(links.Cast<ISplitButtonChild>()).Concat(externalLinks.Cast<ISplitButtonChild>());
                List<MvcHtmlString> actionsToRender = html.GetButtonArea(allActions);

                if (actionsToRender.Any())
                {
                    sb.Append(@"<div class=""form-group""><div class=""colB txtR noPad nestedButtons"">");

                    foreach (var item in actionsToRender)
                    {
                        sb.Append(item.ToString());
                    }

                    sb.Append(@"</div></div>");
                }

                
                // Reproducing html.EndForm()
                sb.Append(@"</form>");

                var formContextForClientValidation = html.ViewContext.ClientValidationEnabled ? html.ViewContext.FormContext : null;

                if (formContextForClientValidation != null && html.ViewContext.UnobtrusiveJavaScriptEnabled)
                {
                    string format = "<script type=\"text/javascript\">\r\n//<![CDATA[\r\nif (!window.mvcClientValidationMetadata) {{ window.mvcClientValidationMetadata = []; }}\r\nwindow.mvcClientValidationMetadata.push({0});\r\n//]]>\r\n</script>".Replace("\r\n", Environment.NewLine);
                    string jsonValidationMetadata = formContextForClientValidation.GetJsonValidationMetadata();
                    string value = string.Format(CultureInfo.InvariantCulture, format, new object[]
    	            {
		                jsonValidationMetadata
	                });

                    sb.Append(value);
                }

                sb.Append(@"</div>"); // Content section
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Produces HTML for a View Model object (including all nested properties).
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns>The HTML for the View Model object.</returns>
        public static MvcHtmlString Object(this HtmlHelper html)
        {
            if (html.ViewContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                return html.ObjectForAjax();
            }

            var groups = html.ViewData.ModelMetadata.GetAttributes<GroupAttribute>().ToList();
            bool isWorkflow = groups.Any(g => g.GroupType == GroupType.Workflow);

            if (isWorkflow)
            {
                // TODO: Separate helper for wizard style workflow
                return MvcHtmlString.Empty;
            }

            var sb = new StringBuilder();
            //var htmlStrings = new List<MvcHtmlString>();
            var output = MvcHtmlString.Empty;


            var Model = html.ViewData.Model;
           // var IsAjax = html.ViewContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            var groupContainers = 0;

            sb.Append(html.ObjectBeginForm());

            if (html.ViewData.Model == null)
            {
                sb.Append(html.ViewData.ModelMetadata.NullDisplayText);
                sb.Append(html.ObjectEndForm());

                return MvcHtmlString.Create(sb.ToString());
            }

            var isCalendarViewModel = false;

            if (html.ViewData.ModelMetadata.ModelType == typeof(Employment.Web.Mvc.Infrastructure.ViewModels.Calendar.CalendarViewModel) || html.ViewData.Model is Employment.Web.Mvc.Infrastructure.ViewModels.Calendar.CalendarViewModel)
            {
                isCalendarViewModel = true; 
            }

            if(isCalendarViewModel)
            {
                sb.Append(html.Partial("_CalendarGridPartial").ToString());
            }

            var groupRowDivTag = new TagBuilder("div");
            string previousGroupRenderedStyle = string.Empty;
            var listOfGroupsRendered = new Dictionary<string, List<GroupAttribute>>();

            var propertyGroups = html.ViewData.GetGroupedAndOrderedMetadata();

            foreach (var propertyGroup in propertyGroups)
            {
                var rowStyleForGroup = string.Empty;

                // Remove "row" class for groupRowDivTag
                groupRowDivTag.Attributes.Remove("class");

                // Get current group
                var group = groups.SingleOrDefault(a => string.Equals(a.Name, propertyGroup.Key, StringComparison.Ordinal));
                var groupConditionMet = group != null ? group.IsConditionMet(html.ViewData.Model) : true;

                IEnumerable<GroupAttribute> otherGroupsInSameRow = null;

                var pG = propertyGroup; // Prevent access to modified closure

                // Get type of current group
                GroupType? groupType = groups.Where(g => string.Equals(g.Name, pG.Key, StringComparison.Ordinal)).Select(g => g.GroupType).FirstOrDefault();
                GroupRowType? groupRowType = null;

                if (group != null)
                {
                    // Row type
                    groupRowType = group.RowType;

                    // Row name
                    string groupRowName = group.RowName;

                    // Initialise as empty if we have a row name
                    otherGroupsInSameRow = !string.IsNullOrEmpty(groupRowName) ? Enumerable.Empty<GroupAttribute>() : null;

                    // Find other groups in same row
                    otherGroupsInSameRow = groups.Where(g=>
                    {
                        // Ignore current group
                        if (string.Equals(g.Name, pG.Key, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        return (!string.IsNullOrEmpty(groupRowName) && g.RowName == groupRowName);
                    });

                    if (otherGroupsInSameRow.Any())
                    {
                        // Render for only the first group in the row
                        if (string.IsNullOrEmpty(groupRowName) || (!string.IsNullOrEmpty(groupRowName) && !listOfGroupsRendered.ContainsKey(groupRowName)))
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

                    // Keep log of rendered groups
                    if (!string.IsNullOrEmpty(groupRowName))
                    {
                        if (listOfGroupsRendered.ContainsKey(groupRowName))
                        {
                            listOfGroupsRendered[groupRowName].Add(group);
                        }
                        else
                        {
                            listOfGroupsRendered.Add(groupRowName, new List<GroupAttribute>() { group });
                        }
                    }
                }

                if (!groupType.HasValue)
                {
                    groupType = GroupType.Default;
                }

                // Process FieldSet and Logical group types that have at least one property with HideSurroundingHtml set to false
                if ((groupType == GroupType.FieldSet || groupType == GroupType.Logical) && propertyGroup.Any(g =>
                {
                    // Only render group container if it contains a visible property
                    var dataType = g.GetAttribute<DataTypeAttribute>();

                    if (dataType != null && string.Equals(dataType.CustomDataType, CustomDataType.Grid, StringComparison.Ordinal))
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
                                var overrideProperty = propertyGroups.SelectMany(g => g).FirstOrDefault(p => string.Equals(p.PropertyName, group.OverrideNameWithPropertyValue, StringComparison.Ordinal));

                                // Use override if set with a string value
                                if (overrideProperty != null && overrideProperty.Model is string && !string.IsNullOrEmpty(overrideProperty.Model as string))
                                {
                                    groupName = overrideProperty.Model as string;
                                }
                            }

                            // Hide group and apply ContainerFor if the condition requires it be visible/hidden
                            if (group.DependencyType == ActionForDependencyType.Visible || group.DependencyType == ActionForDependencyType.Hidden)
                            {
                                if ((group.DependencyType == ActionForDependencyType.Visible && !groupConditionMet) || (group.DependencyType == ActionForDependencyType.Hidden && groupConditionMet))
                                {
                                    divTag.AddCssClass("hidden");
                                }

                                divTag.AddCssClass("rhea-visibleif");

                                var htmlAttributes = new Dictionary<string, object>();

                                htmlAttributes.Add(HtmlDataType.DependentPropertyVisibleIf, group.DependentProperty);
                                htmlAttributes.Add(HtmlDataType.ComparisonTypeVisibleIf, group.ComparisonType);
                                htmlAttributes.Add(HtmlDataType.PassOnNullVisibleIf, group.PassOnNull.ToString().ToLowerInvariant());
                                htmlAttributes.Add(HtmlDataType.FailOnNullVisibleIf, group.FailOnNull.ToString().ToLowerInvariant());

                                var values = group.DependentValue as object[];

                                if (values != null)
                                {
                                    htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                                }
                                else
                                {
                                    htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf, group.DependentValue);
                                }

                                divTag.MergeAttributes(htmlAttributes);

                                divTag.Attributes.Add("id", string.Format("ContainerFor-{0}Group{1}", html.ViewData.TemplateInfo.GetFullHtmlFieldId(Regex.Replace(group.Name, "[^a-zA-Z0-9]", string.Empty)), ++groupContainers));
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
                                int totalColsOccupiedByOtherGroups = otherGroupsInSameRow.Sum(m => (int)m.RowType);

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
                            sb.Append(@"<div class=""panel panel-inverse""><div class=""panel-heading""><h4 class=""panel-title"">");
                            sb.Append(groupName);
                            sb.Append(@"</h4></div>");

                            var groupNameID = groupName.Replace(" ", "_");
                            sb.Append(@"<div class=""panel-body"" id=""group-");
                            sb.Append(groupNameID);
                            sb.Append(@""">");
                            sb.Append(html.ValidationMessageSummary(groupName).ToString());
                        }
                    }
                }

                var propertiesShownInRow = new List<string>();
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

                                value = string.Format("{0}, {1}", parentType.FullName, parentType.Assembly.FullName.Substring(0, parentType.Assembly.FullName.IndexOf(',')));
                            }
                            else
                            {
                                // Property name in parent
                                value = html.ViewData.ModelMetadata.PropertyName;
                            }

                            sb.Append(html.Hidden(property.PropertyName, value).ToString());

                            continue;
                        }
                    }

                    var hiddenAttribute = property.GetAttribute<HiddenAttribute>();
                    var containerForID = string.Format("ContainerFor-{0}", html.ViewData.TemplateInfo.GetFullHtmlFieldId(property.PropertyName));
                    var visible = property.IsVisible() && (hiddenAttribute == null || (hiddenAttribute != null && hiddenAttribute.LabelOnly));
                    bool isNestedViewModel = property.IsViewModel();
                    bool isGrid = property.DataTypeName == CustomDataType.Grid || property.DataTypeName == CustomDataType.GridEditable;
                    bool isCheckBox = property.ModelType == typeof(bool) && (string.IsNullOrEmpty(property.TemplateHint) || string.Equals(property.TemplateHint, "Boolean", StringComparison.Ordinal));
                    bool isReadOnlyHyperlink = property.IsReadOnlyHyperlink();
                    bool isContentViewModel = property.ModelType == typeof(Employment.Web.Mvc.Infrastructure.ViewModels.ContentViewModel);

                    // Check if the property is to be excluded from the view entirely
                    if (hiddenAttribute != null && hiddenAttribute.ExcludeFromView)
                    {
                        continue;
                    }


                    if (currentPropertyIndex == 0 && (!isNestedViewModel || isContentViewModel) && !string.IsNullOrEmpty(propertyGroup.Key))
                    {
                        sb.Append(@"<div class=""form-horizontal"">");
                    }

                    if (property.ModelType == typeof(IndicatorType?) || property.ModelType == typeof(IEnumerable<LayoutType>))
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
                            if (!propertiesShownInRow.Any() && (!isNestedViewModel || isContentViewModel)) // Dont show row div if this is a nested view model
                            {
                                sb.Append(@"<div class=""form-group"">");
                            }

                            propertiesShownInRow.Add(property.PropertyName);
                        }
                    }
                    else if (!isNestedViewModel || isContentViewModel) // Dont show row div if this is a nested view model
                    {
                        sb.Append(@"<div class=""form-group"">");
                    }

                    var containerDivTag = new TagBuilder("div"); // div for ContainerFor- id (the container div needs to contain both the label and input in order to hide/show both correctly)
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
                                cols = 2 + 1;   // Label cols are set to 1 when RowType is Quarter.
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

                        if (row == null || row != null && row.RowType != RowType.Dynamic)  // Don't include 'colB' if row type is dynamic
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

                    if (html.ViewData.ModelState != null && html.ViewData.ModelState.ContainsKey(property.PropertyName) && html.ViewData.ModelState[property.PropertyName].Errors.Any())
                    {
                        divTag.AddCssClass("error");
                    }

                    // Div must start before label so the ContainerFor- includes the label and form control ([VisibleIf] hides/shows the ContainerFor- so we need the label inside to hide/show it as well)
                    sb.Append(containerDivTag.ToString(TagRenderMode.StartTag));

                    if (property.HideSurroundingHtml)
                    {
                        sb.Append(divTag.ToString(TagRenderMode.StartTag));
                        sb.Append(html.Editor(property.PropertyName, new { ParentModel = Model }).ToString());
                        sb.Append(@"</div>");
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
                                sb.Append(html.GetLabel(property, html.ViewData.ModelMetadata).ToString());
                            }

                            if (propertyLinksAndButtons.Any())
                            {
                                //sb.Append(@"<br>"));
                            }
                        }

                        // Render label after control for checkboxes
                        if (isCheckBox)
                        {
                            sb.Append(html.GetLabel(property, html.ViewData.ModelMetadata).ToString());
                        }

                        sb.Append(divTag.ToString(TagRenderMode.StartTag));

                        if (propertyLinksAndButtons.Any())
                        {
                            //sb.Append(@"<div class=""ffFix"">"));
                        }

                        sb.Append(html.Editor(property.PropertyName, new { ParentModel = Model, PropertyNameInParentModel = property.PropertyName }).ToString());



                        if (!isNestedViewModel && !isGrid && !isReadOnlyHyperlink)
                        {
                            foreach (var linkOrButton in html.GetPropertyLinksAndButtons(property, true))
                            {
                                sb.Append(linkOrButton.ToString());
                            }
                        }

                        if (propertyLinksAndButtons.Any())
                        {
                            //sb.Append(@"</div>"));
                        }

                        if ((isNestedViewModel && !isGrid))
                        {
                            if (propertyLinksAndButtons.Any())
                            {

                                //@*@:<div class="colB txtR noPad">
                                //@: <div class="panel-footer"> <div class="form-group">*@
                            }


                            foreach (var l in propertyLinksAndButtons)
                            {
                                sb.Append(l.ToString());
                            }


                            if (propertyLinksAndButtons.Any())
                            {
                                //@*@:</div>
                                //@:</div>
                                //@:</div>*@
                            }
                        }
                        sb.Append(divTag.ToString(TagRenderMode.EndTag));//<!--column size div tag for property-->
                    }

                    sb.Append(containerDivTag.ToString(TagRenderMode.EndTag));

                    if (!isNestedViewModel || isContentViewModel)
                    {
                        if (row != null)
                        {
                            if (otherPropertiesInSameRow != null && (otherPropertiesInSameRow.Count() + 1) == propertiesShownInRow.Count())
                            {
                                propertiesShownInRow.Clear();
                                sb.Append(@"</div> <!--form-group div for property (with RowType) is closed here-->");
                            }
                        }
                        else
                        {
                            propertiesShownInRow.Clear();
                            sb.Append(@"</div>  <!--form-group div for property (without RowType) is closed here-->");
                        }
                    }


                    // Buttons assigned to a group should go at end of group

                    currentPropertyIndex++;

                    if (currentPropertyIndex == totalProperties && (!isNestedViewModel || isContentViewModel) && !string.IsNullOrEmpty(propertyGroup.Key))
                    {
                        sb.Append(@"</div> <!--div class=""form-horizontal""-->");
                    }
                }


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
                    Type typeToUse = Model != null ? Model.GetType() : html.ViewData.ModelMetadata.ModelType;
                    // Get buttons for current group
                    var groupButtons = typeToUse.GetAttributes<ButtonAttribute>(b => !string.IsNullOrEmpty(b.GroupName) && string.Equals(b.GroupName, propertyGroup.Key, StringComparison.Ordinal));

                    // Get links for current group
                    var groupLinks = typeToUse.GetAttributes<LinkAttribute>(l => !string.IsNullOrEmpty(l.GroupName) && string.Equals(l.GroupName, propertyGroup.Key, StringComparison.Ordinal));
                    var groupExternalLinks = typeToUse.GetAttributes<ExternalLinkAttribute>(e => !string.IsNullOrEmpty(e.GroupName) && string.Equals(e.GroupName, propertyGroup.Key, StringComparison.Ordinal));
                    IEnumerable<ISplitButtonChild> allActions = groupButtons.Cast<ISplitButtonChild>().Concat(groupLinks.Cast<ISplitButtonChild>()).Concat(groupExternalLinks.Cast<ISplitButtonChild>());
                    List<MvcHtmlString> actionsToRender = html.GetButtonArea(allActions);

                    //if ((groupType == GroupType.FieldSet || groupType == GroupType.Logical) && !string.IsNullOrEmpty(propertyGroup.Key))
                    if ((groupType == GroupType.FieldSet) && !string.IsNullOrEmpty(propertyGroup.Key))
                    {
                        sb.Append(@"</div> <!--panel-body -->");
                    }
                    if (actionsToRender.Any())
                    {
                        sb.Append(@"<div class=""panel-footer""> <div class=""form-group""> 
                <div class=""colB txtR noPad nestedButtons"">");

                        foreach (var item in actionsToRender)
                        {
                            // buttons and links are rendered here.
                            sb.Append(item.ToString());
                        }

                        sb.Append(@"</div>
                </div> 
                </div><!--panel-footer -->   ");
                    }

                    // Only add to fieldset if an actual field set item (will have key name)
                    if (!string.IsNullOrEmpty(propertyGroup.Key))
                    {
                        //if (groupType == GroupType.FieldSet || groupType == GroupType.Logical)
                        if (groupType == GroupType.FieldSet)
                        {
                            // @*@:</fieldset>*@

                            sb.Append(@"</div> <!--panel-inverse-->");

                        }



                        sb.Append(@"</div> <!--Closing DIV for Visible/Hidden for group fieldset-->");


                    }
                }

                if (group != null)
                {
                    // if no other groups in this row, then close Row.
                    if (string.IsNullOrEmpty(group.RowName) || (otherGroupsInSameRow == null) || (otherGroupsInSameRow != null && !otherGroupsInSameRow.Any()))
                    {
                        sb.Append(groupRowDivTag.ToString(TagRenderMode.EndTag));// <!--Row Div-->

                    }
                    else if (otherGroupsInSameRow != null && !string.IsNullOrEmpty(group.RowName) && listOfGroupsRendered[group.RowName] != null && (otherGroupsInSameRow.Count() + 1) == listOfGroupsRendered[group.RowName].Count)
                    {
                        // Close the Row div tag only when the number of groups rendered exceeds the number of groups in same row. 
                        sb.Append(groupRowDivTag.ToString(TagRenderMode.EndTag));// <!--Row Div-->
                        listOfGroupsRendered[group.RowName].Clear();
                    }
                }
            }

            sb.Append(html.ObjectEndForm());

            // Load any currently selected widgets for this View Model
            sb.Append(html.RenderWidgets().ToString());

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Produces HTML for a View Model object (including all nested properties) for an Ajax call.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns>The Ajax HTML for the View Model object.</returns>
        public static MvcHtmlString ObjectForAjax(this HtmlHelper html)
        {
            // TODO: Determine whether to include <form> (would be for [Trigger] only)
            return MvcHtmlString.Empty;
        }
        */ 
    }
}