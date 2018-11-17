using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Helpers
{
	public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		public IAdwRepository Repository { get; set; }

		public CustomModelMetadataProvider()
		{
			if ( Repository == null )
				Repository = new AdwRepository();
		}

		protected override ModelMetadata CreateMetadata( IEnumerable<Attribute> attributes,
																		Type containerType,
																		Func<object> modelAccessor,
																		Type modelType,
																		string propertyName )
		{
			var metadata = base.CreateMetadata( attributes,
														  containerType,
														  modelAccessor,
														  modelType,
														  propertyName );

            var editlabel = attributes.OfType<EditLabelAttribute>().FirstOrDefault();
            if (editlabel != null)
                metadata.AdditionalValues.Add("EditLabel", editlabel.GetEditLabel());

			var stringLengthAttribute = attributes.OfType<StringLengthAttribute>().FirstOrDefault();
			if ( stringLengthAttribute != null )
				metadata.AdditionalValues.Add( "MaxLength", stringLengthAttribute.MaximumLength );

			var adwCodeListAttribute = attributes.OfType<AdwCodeListAttribute>().FirstOrDefault();
			if ( adwCodeListAttribute != null )
			{
				var items = new List<SelectListItem>();
				if ( adwCodeListAttribute.ShowEmptyValue )
					items.Add( new SelectListItem { Text = "", Value = "" } );
				items.AddRange( !adwCodeListAttribute.IsRelatedCode
										? Repository.ListCode( adwCodeListAttribute.AdwCode )
										: Repository.ListRelatedCode(
											adwCodeListAttribute.AdwRelatedCode, adwCodeListAttribute.AdwSearchCode, false, "n" )
					);
				metadata.AdditionalValues.Add( "AdwCodeList", items );
			}

            var additionalValues = attributes.OfType<HtmlPropertiesAttribute>().FirstOrDefault();

            if (additionalValues != null)
                metadata.AdditionalValues.Add("HtmlAttributes", additionalValues);

			return metadata;
		}
	}
}
