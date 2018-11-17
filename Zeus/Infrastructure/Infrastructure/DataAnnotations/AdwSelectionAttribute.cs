using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Extensions;
using System.Web.Mvc;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to provide ADW selection options for string or IEnumerable<string> properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AdwSelectionAttribute : ContingentValidationAttribute, IMetadataAware
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        protected override string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.AdwSelectionAttribute_Invalid; }
        }

        /// <summary>
        /// Adw service.
        /// </summary>
        protected IAdwService AdwService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                if (containerProvider != null)
                {
                    return containerProvider.GetService<IAdwService>();
                }

                return null;
            }
        }

        /// <summary>
        /// The selection type of the class or enumerable property.
        /// </summary>
        public SelectionType SelectionType { get; private set; }

        /// <summary>
        /// The adw type of <see cref="Code" />.
        /// </summary>
        public AdwType AdwType { get; private set; }

        /// <summary>
        /// Adw code.
        /// </summary>
        /// <remarks>
        /// The type is determined by <see cref="AdwType" />.
        /// </remarks>
        public string Code { get; private set; }

        /// <summary>
        /// The name of the dependent property for use when <see cref="AdwType" /> is <see cref="Types.AdwType.RelatedCode" />.
        /// </summary>
        /// <remarks>
        /// Only used if <see cref="AdwType" /> is <see cref="Types.AdwType.RelatedCode" />.
        /// Cannot be used together with <see cref="DependentValue" />. One or the other should be used.
        /// </remarks>
        public new string DependentProperty { get { return base.DependentProperty; } set { base.DependentProperty = value; } }

        /// <summary>
        /// The dependent value for use when <see cref="AdwType" /> is <see cref="Types.AdwType.RelatedCode" />.
        /// </summary>
        /// <remarks>
        /// Only used if <see cref="AdwType" /> is <see cref="Types.AdwType.RelatedCode" />.
        /// Cannot be used together with <see cref="DependentProperty" />. One or the other should be used.
        /// </remarks>
        public new string DependentValue { get { return base.DependentValue != null ? base.DependentValue.ToString() : string.Empty; } set { base.DependentValue = value; } }

        /// <summary>
        /// Whether the <see cref="DependentProperty" /> or <see cref="DependentValue" /> is dominant; otherwise, subordinate (default is true).
        /// </summary>
        /// <remarks>
        /// Only used if <see cref="AdwType" /> is <see cref="Types.AdwType.RelatedCode" />.
        /// </remarks>
        public bool Dominant { get; set; }

        /// <summary>
        /// The display type to indicate what Adw property to use for the display text.
        /// </summary>
        public AdwDisplayType DisplayType { get; set; }

        /// <summary>
        /// The order type to display with.
        /// </summary>
        public AdwOrderType OrderType { get; set; }

        /// <summary>
        /// Whether to use current codes only (default is true).
        /// </summary>
        public bool CurrentCodesOnly { get; set; }

        /// <summary>
        /// The values to exclude from the Adw selection options.
        /// </summary>
        public string[] ExcludeValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.AdwSelectionAttribute" /> class.
        /// </summary>
        /// <param name="selectionType">The <see cref="Types.SelectionType" /> setting.</param>
        /// <param name="adwType">The <see cref="Types.AdwType" /> setting.</param>
        /// <param name="code">The Adw code.</param>
        public AdwSelectionAttribute(SelectionType selectionType, AdwType adwType, string code) : base(null)
        {
            SelectionType = selectionType;
            AdwType = adwType;
            Code = code;
            Dominant = true;
            DisplayType = AdwDisplayType.Default;
            OrderType = AdwOrderType.Default;
            CurrentCodesOnly = true;
        }

        /// <summary>
        /// Format of the error message.
        /// </summary>
        /// <param name="name">Property display name.</param>
        /// <returns>The formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageFormatString;
            }

            return string.Format(ErrorMessageString, name);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentPropertyValue = GetDependentPropertyValue(validationContext.ObjectInstance);

            if (!IsConditionMet(value, dependentPropertyValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Determines whether the specified value of the object meets the condition.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public override bool IsConditionMet(object propertyValue, object dependentPropertyValue)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("AdwSelection.IsConditionMet");

            try
            {
#endif
                dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

                if (AdwType == AdwType.RelatedCode)
                {
                    // Use dependent value as the dependent property value if dependent property is not set and dependent value is set
                    if (string.IsNullOrEmpty(DependentProperty) && !string.IsNullOrEmpty(DependentValue))
                    {
                        dependentPropertyValue = DependentValue;
                    }
                }

                if (PassOnNull && dependentPropertyValue == null)
                {
                    return true;
                }

                if (FailOnNull && dependentPropertyValue == null)
                {
                    return false;
                }

                return AdwType == AdwType.ListCode ? IsValidCode(propertyValue) : IsValidRelatedCode(propertyValue, dependentPropertyValue);
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
        /// Determines whether the property value is a valid Adw List Code.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidCode(object propertyValue)
        {
            if (propertyValue != null)
            {
                var adwService = AdwService;
                var values = propertyValue as IEnumerable<string>;
                
                if ((values == null && SelectionType == SelectionType.None) || SelectionType == SelectionType.Single)
                {
                    return (string.IsNullOrEmpty(propertyValue.ToString()) || (adwService.GetListCodes(Code, CurrentCodesOnly).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Code)).FirstOrDefault(c => c.Code == propertyValue.ToString()) != null));
                }

                if (values != null && (SelectionType == SelectionType.None || SelectionType == SelectionType.Multiple))
                {
                    foreach (var v in values)
                    {
                        if (!(string.IsNullOrEmpty(v) || (adwService.GetListCodes(Code, CurrentCodesOnly).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Code)).FirstOrDefault(c => c.Code == v) != null)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the property value is a valid Adw Related Code.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidRelatedCode(object propertyValue, object dependentPropertyValue)
        {
            dependentPropertyValue = (dependentPropertyValue ?? string.Empty).ToString().Trim();

            if (propertyValue != null)
            {
                var adwService = AdwService;
                var values = propertyValue as IEnumerable<string>;

                if ((values == null && SelectionType == SelectionType.None) || SelectionType == SelectionType.Single)
                {
                    return (string.IsNullOrEmpty(propertyValue.ToString()) || (adwService.GetRelatedCodes(Code, dependentPropertyValue.ToString(), Dominant, CurrentCodesOnly).ToCodeModelList().Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Code)).FirstOrDefault(c => c.Code == propertyValue.ToString()) != null));
                }

                if (values != null && (SelectionType == SelectionType.None || SelectionType == SelectionType.Multiple))
                {
                    foreach (var v in values)
                    {
                        if (!(string.IsNullOrEmpty(v) || (adwService.GetRelatedCodes(Code, dependentPropertyValue.ToString(), Dominant, CurrentCodesOnly).ToCodeModelList().Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Code)).FirstOrDefault(c => c.Code == v) != null)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the Adw codes as a <see cref="IEnumerable{SelectListItem}" />.
        /// </summary>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" />.</returns>
        public IEnumerable<SelectListItem> GetSelectListItems(object container)
        {
            return GetSelectListItems(container, null);
        }

        /// <summary>
        /// Returns the Adw codes as a <see cref="IEnumerable{SelectListItem}" />.
        /// </summary>
        /// <param name="container">The model this object is contained within.</param>
        /// <param name="selectedCodes">The code values that are selected.</param>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" />.</returns>
        public IEnumerable<SelectListItem> GetSelectListItems(object container, object selectedCodes)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("AdwSelection.GetSelectListItems ({0})", Code));

            try
            {
#endif
                if (AdwType == AdwType.RelatedCode)
                {
                    IList<RelatedCodeModel> relatedCodes;

                    if (!string.IsNullOrEmpty(DependentProperty))
                    {
                        var dependentPropertyValue = HandleEnumerableSelectListItem(GetDependentPropertyValue(container));

                        if (dependentPropertyValue == null || string.IsNullOrEmpty(dependentPropertyValue.ToString()))
                        {
                            return Enumerable.Empty<SelectListItem>();
                        }

                        relatedCodes = AdwService.GetRelatedCodes(Code, dependentPropertyValue.ToString(), Dominant, CurrentCodesOnly);
                    }
                    else
                    {
                        relatedCodes = AdwService.GetRelatedCodes(Code, DependentValue, Dominant, CurrentCodesOnly);
                    }

                    return relatedCodes.ToOrderedSelectListItem(OrderType, DisplayType, selectedCodes).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Value));
                }

                var listCodes = AdwService.GetListCodes(Code, CurrentCodesOnly);

                return listCodes.ToOrderedSelectListItem(OrderType, DisplayType, selectedCodes).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Value));
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
        /// Returns a single Adw code as a <see cref="IEnumerable{SelectListItem}" />.
        /// </summary>
        /// <remarks>
        /// Not ready for use.
        /// </remarks>
        /// <param name="container">The model this object is contained within.</param>
        /// <param name="selectedCode">The code value that is selected.</param>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" /> with a single item.</returns>
        public IEnumerable<SelectListItem> GetSelectListItem(object container, string selectedCode)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("AdwSelection.GetSelectListItem ({0})", Code));

            try
            {
#endif
                if (string.IsNullOrEmpty(selectedCode))
                {
                    return Enumerable.Empty<SelectListItem>();
                }

                if (AdwType == AdwType.RelatedCode)
                {
                    RelatedCodeModel relatedCode;

                    if (!string.IsNullOrEmpty(DependentProperty))
                    {
                        var dependentPropertyValue = HandleEnumerableSelectListItem(GetDependentPropertyValue(container));

                        if (dependentPropertyValue == null || string.IsNullOrEmpty(dependentPropertyValue.ToString()))
                        {
                            return Enumerable.Empty<SelectListItem>();
                        }

                        relatedCode = AdwService.GetRelatedCode(Code, dependentPropertyValue.ToString(), selectedCode, Dominant, CurrentCodesOnly);
                    }
                    else
                    {
                        relatedCode = AdwService.GetRelatedCode(Code, DependentValue, selectedCode, Dominant, CurrentCodesOnly);
                    }

                    return relatedCode != null ? new[] { relatedCode }.ToOrderedSelectListItem(OrderType, DisplayType, relatedCode.Dominant ? relatedCode.SubordinateCode : relatedCode.DominantCode).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Value)) : Enumerable.Empty<SelectListItem>();
                }

                var listCode = AdwService.GetListCode(Code, selectedCode, CurrentCodesOnly);

                return listCode != null ? new[] { listCode }.ToOrderedSelectListItem(OrderType, DisplayType, listCode.Code).Where(m => ExcludeValues == null || !ExcludeValues.Contains(m.Value)) : Enumerable.Empty<SelectListItem>();
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
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata.ModelType == typeof(IEnumerable<string>))
            {
                if (SelectionType == SelectionType.Multiple || SelectionType == SelectionType.None)
                {
                    metadata.TemplateHint = "AdwSelectionMultiple";
                }
                else
                {
                    throw new InvalidOperationException("SelectionType must be Multiple or None for a property type of IEnumerable<string>.");
                }
            }
            else if (metadata.ModelType == typeof(string))
            {
                if (SelectionType == SelectionType.Single || SelectionType == SelectionType.None)
                {
                    metadata.TemplateHint = "AdwSelectionSingle";
                }
                else
                {
                    throw new InvalidOperationException("SelectionType must be Single or None for a property type of string.");
                }
            }
            else
            {
                throw new InvalidOperationException("Property type must be either string or IEnumerable<string>.");
            }
        }
    }
}
