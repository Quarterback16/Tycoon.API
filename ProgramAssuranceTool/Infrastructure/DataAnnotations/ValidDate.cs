using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Infrastructure.DataAnnotations
{
   [AttributeUsage( AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false )]
	public class ValidDate : ValidationAttribute
	{
		protected override ValidationResult IsValid( object value, ValidationContext validationContext )
		{
			if (value == null || value.ToString().Length == 0)
				return ValidationResult.Success;

			DateTime theDate;

			return !DateTime.TryParse( value.ToString(), out theDate ) ? new ValidationResult( ErrorMessage ) : ValidationResult.Success;
		}

	}

	public class ValidDateValidator : DataAnnotationsModelValidator<ValidDate>
	{
		public ValidDateValidator( ModelMetadata metadata, ControllerContext context, ValidDate attribute )
			: base( metadata, context, attribute )
		{
			if (attribute.IsValid( context.HttpContext.Request.Form[ metadata.PropertyName ] )) return;
			var propertyName = metadata.PropertyName;
			context.Controller.ViewData.ModelState[ propertyName ].Errors.Clear();
			context.Controller.ViewData.ModelState[ propertyName ].Errors.Add( attribute.ErrorMessage );
		}
	}
}