using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate if this property meets a certain criteria.
    /// </summary>
    public class RequiredIfAttribute : ContingentValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        protected override string DefaultErrorMessageFormatString
        {
            get
            {
                if (DependentValue == null || (DependentValue is string && string.IsNullOrEmpty(DependentValue.ToString())))
                {
                    if (ComparisonType == ComparisonType.EqualTo)
                    {
                        return DataAnnotationsResources.RequiredIfEmptyAttribute_Invalid;
                    }

                    if (ComparisonType == ComparisonType.NotEqualTo)
                    {
                        return DataAnnotationsResources.RequiredIfNotEmptyAttribute_Invalid;
                    }
                }
                
                return DataAnnotationsResources.RequiredIfAttribute_Invalid;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="value">The value to compare against.</param>
        public RequiredIfAttribute(string dependentProperty, object value) : this(dependentProperty, ComparisonType.EqualTo, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="value">The value to compare against.</param>
        public RequiredIfAttribute(string dependentProperty, ComparisonType comparisonType, object value) : base(dependentProperty, comparisonType, value) { }

        /// <summary>
        /// Returns the client validation rules.
        /// </summary>
        /// <param name="metadata">The model metadata of the object being validated.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>The client validation rules.</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName(), GetOtherDisplayName(metadata.AdditionalValues["ParentModel"])),
                ValidationType = "requiredif"
            };

            clientValidationRule.ValidationParameters.Add("dependentproperty", DependentProperty);
            clientValidationRule.ValidationParameters.Add("comparisontype", ComparisonType);

            var values = DependentValue as object[];

            if (values != null)
            {
                clientValidationRule.ValidationParameters.Add("value", string.Format("[\"{0}\"]", string.Join("\",\"", values)));
            }
            else
            {
                clientValidationRule.ValidationParameters.Add("value", DependentValue);
            }

            clientValidationRule.ValidationParameters.Add("passonnull", PassOnNull.ToString().ToLowerInvariant());
            clientValidationRule.ValidationParameters.Add("failonnull", FailOnNull.ToString().ToLowerInvariant());

            return new[] { clientValidationRule };
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
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, GetOtherDisplayName(validationContext.ObjectInstance)));
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
            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            var failedOnNull = (FailOnNull && dependentPropertyValue == null);

            propertyValue = HandleEnumerableSelectListItem(propertyValue);
            
            var valuesToTestAgainst = DependentValue as object[];

            // If fail on null has failed then we already know the propertyValue is required
            // so drop out and let the next if block check that a value has been provided for it
            if (!failedOnNull && valuesToTestAgainst != null)
            {
                var result = new List<bool>();

                var actualValues = dependentPropertyValue as object[] ?? new[] {dependentPropertyValue};

                foreach (var testValue in valuesToTestAgainst)
                {
                    foreach (var actualValue in actualValues)
                    {
                        result.Add(IsConditionMet(propertyValue, actualValue, testValue));
                    }
                }

                if (ComparisonType == ComparisonType.NotEqualTo)
                {
                    // Negative AND validation (all must be true)
                    if (!result.Contains(false))
                    {
                        if (propertyValue != null)
                        {
                            if (propertyValue is string)
                            {
                                return !string.IsNullOrEmpty(propertyValue.ToString().Trim());
                            }

                            return ComparisonType.NotEqualTo.Compare(propertyValue, DelegateHelper.CreateConstructorDelegate(propertyValue.GetType())());
                        }

                        return false;
                    }
                }
                else
                {
                    // Positive OR validation (at least one must be true)
                    if (result.Contains(true))
                    {
                        if (propertyValue != null)
                        {
                            if (propertyValue is string)
                            {
                                return !string.IsNullOrEmpty(propertyValue.ToString().Trim());
                            }

                            return ComparisonType.NotEqualTo.Compare(propertyValue, DelegateHelper.CreateConstructorDelegate(propertyValue.GetType())());
                        }

                        return false;
                    }
                }

                return true;
            }

            // If fail on null has failed or the condition has been met then the propertyValue is required so check that a value has been provided for it
            if (failedOnNull || IsConditionMet(propertyValue, dependentPropertyValue, DependentValue))
            {
                if (propertyValue != null)
                {
                    if (propertyValue is string)
                    {
                        return !string.IsNullOrEmpty(propertyValue.ToString().Trim());
                    }
                    
                    if (propertyValue.GetType().IsArray)
                    {
                        object[] objArray = propertyValue as object[];

                        var result = new List<bool>();

                        foreach (var obj in objArray)
                        {
                            if (obj is string)
                            {
                                result.Add(!string.IsNullOrEmpty(propertyValue.ToString().Trim()));
                            }
                            else
                            {
                                result.Add(ComparisonType.NotEqualTo.Compare(obj, DelegateHelper.CreateConstructorDelegate(obj.GetType())()));
                            }
                        }

                        if (ComparisonType == ComparisonType.NotEqualTo)
                        {
                            // Negative AND validation (all must be true)
                            return (!result.Contains(false));
                        }
                        
                        // Positive OR validation (at least one must be true)
                        return (result.Contains(true));
                    }
                    
                    // TODO: Handle Enum as its first value is incorrectly considered as empty

                    return ComparisonType.NotEqualTo.Compare(propertyValue, DelegateHelper.CreateConstructorDelegate(propertyValue.GetType())());
                }

                return false;
            }

            return true;
        }
        

        /// <summary>
        /// Checks whether the required condition has been met.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="valueToTestAgainst">The value to test the dependent property value against.</param>
        /// <returns><c>true</c> if the required condition has been met; otherwise, <c>false</c>.</returns>
        protected virtual bool IsConditionMet(object propertyValue, object dependentPropertyValue, object valueToTestAgainst)
        {
            return ComparisonType.Compare(dependentPropertyValue, valueToTestAgainst);
        }

        /// <summary>
        /// Determines whether the property is required.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns><c>true</c> if the property is required; otherwise, <c>false</c>.</returns>
        public bool IsRequired(object propertyValue, object dependentPropertyValue)
        {
            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            if (FailOnNull && dependentPropertyValue == null)
            {
                return false;
            }

            propertyValue = HandleEnumerableSelectListItem(propertyValue);

            var valuesToTestAgainst = DependentValue as object[];

            if (valuesToTestAgainst != null)
            {
                var result = new List<bool>();

                var actualValues = dependentPropertyValue as object[] ?? new[] {dependentPropertyValue};
                
                foreach (var testValue in valuesToTestAgainst)
                {
                    foreach (var actualValue in actualValues)
                    {
                        result.Add(IsConditionMet(propertyValue, actualValue, testValue));
                    } 
                }

                if (ComparisonType == ComparisonType.NotEqualTo)
                {
                    // Negative AND validation (all must be true)
                    if (!result.Contains(false))
                    {
                        return true;
                    }
                }
                else
                {
                    // Positive OR validation (at least one must be true)
                    if (result.Contains(true))
                    {
                        return true;
                    }
                }

                return false;
            }

            if (IsConditionMet(propertyValue, dependentPropertyValue, DependentValue))
            {
                return true;
            }

            return false;
        }
    }
}
