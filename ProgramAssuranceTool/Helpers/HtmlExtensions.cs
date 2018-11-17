using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using ProgramAssuranceTool.Infrastructure.Types;

namespace ProgramAssuranceTool.Helpers
{
	public static class HtmlExtensions
	{
		/// <summary>
		/// It displays the required indicator as red asterisk 
		/// If required attribute is not defined it will not be displayed unless you set the forceDisplay to true.
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="html"></param>
		/// <param name="expression"></param>
		/// <param name="labelText">a custom label, if null it will get from the display attribute</param>
		/// <param name="forceDisplay">to force display the required indicator regardless of the required attribute</param>
		/// <returns></returns>
		public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = "", bool forceDisplay = false)
		{
			return LabelHelper( html,
				 ModelMetadata.FromLambdaExpression( expression, html.ViewData ),
				 ExpressionHelper.GetExpressionText( expression ), labelText, forceDisplay );
		}

		private static MvcHtmlString LabelHelper( HtmlHelper html,
			 ModelMetadata metadata, string htmlFieldName, string labelText, bool forceDisplay)
		{
			if ( string.IsNullOrEmpty( labelText ) )
				labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split( '.' ).Last();

			if ( string.IsNullOrEmpty( labelText ) )
				return MvcHtmlString.Empty;

			var isRequired = false;

			if ( metadata.ContainerType != null )
			{
				if ( metadata.PropertyName != null )
					isRequired = metadata.ContainerType.GetProperty( metadata.PropertyName )
										.GetCustomAttributes( typeof( RequiredAttribute ), false )
										.Length == 1;
			}

			var tag = new TagBuilder( "label" );
			tag.Attributes.Add(
				 "for",
				 TagBuilder.CreateSanitizedId(
					  html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName( htmlFieldName )
				 )
			);

			tag.SetInnerText(labelText);

			//shows red asterisk 
			if (isRequired || forceDisplay)
			{
				var star = new TagBuilder("span");
				star.SetInnerText(" *");
				star.MergeAttribute("title", "required");
				star.MergeAttribute("class", "label-required");
				tag.InnerHtml += star.ToString(TagRenderMode.Normal);
			}

			var output = tag.ToString( TagRenderMode.Normal );

			return MvcHtmlString.Create( output );
		}


		/// <summary>Returns an unordered list (ul element) of validation messages that are in the <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.</summary>
		/// <remarks>Custom version of <see cref="ValidationExtensions.ValidationSummary(System.Web.Mvc.HtmlHelper)"/> to use different markup and include property name shortcut anchor.</remarks>
		/// <param name="html">The HTML helper instance that this method extends.</param>
		/// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
		public static MvcHtmlString ValidationMessageSummary(this HtmlHelper html)
		{
			return html.ValidationMessageSummary(false);
		}

		/// <summary>Returns an unordered list (ul element) of validation messages that are in the <see cref="T:System.Web.Mvc.ModelStateDictionary" /> object and optionally displays only model-level errors.</summary>
		/// <remarks>Custom version of <see cref="ValidationExtensions.ValidationSummary(System.Web.Mvc.HtmlHelper, bool)"/> to use different markup and include property name shortcut anchor.</remarks>
		/// <param name="html">The HTML helper instance that this method extends.</param>
		/// <param name="excludePropertyErrors"><c>true</c> to have the summary display model-level errors only, or <c>false</c> to have the summary display all errors.</param>
		/// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
     public static MvcHtmlString ValidationMessageSummary(this HtmlHelper html, bool excludePropertyErrors)
     {
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
		     // Success summary only if model is valid
		     result.AppendLine(MessageSummary(html, MessageType.Success).ToString());

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
		     // Success summary isn't being but it should still be cleared from TempData so as not to duplicate
		     html.ViewContext.TempData.Remove(MessageType.Success.ToString());
	     }

	     result.AppendLine(MessageSummary(html, MessageType.Warning).ToString());
	     result.AppendLine(MessageSummary(html, MessageType.Information).ToString());

	     // Remove ModelState stored in TempData as we are about to show it and don't want it to stick around
	     html.ViewContext.TempData.Remove(PersistModelStateAttribute.TempDataKey);

	     // Error summary (always include; hidden if model is valid but shown by client-side unobtrusive validation if invalid)
	     var headerTag = new TagBuilder("span");
		  headerTag.SetInnerText("Please correct the errors and try again.");

	     var listTag = new TagBuilder("ul");
	     var stringBuilder = new StringBuilder();

	     var modelStates = GetModelStates(html, excludePropertyErrors);

	     foreach (KeyValuePair<string, ModelState> modelState in modelStates)
	     {
		     foreach (ModelError error in modelState.Value.Errors)
		     {
			     if (!string.IsNullOrEmpty(error.ErrorMessage))
			     {
				     var listItemTag = new TagBuilder("li");

				     if (string.IsNullOrEmpty(modelState.Key))
				     {
					     listItemTag.SetInnerText(error.ErrorMessage);
				     }
				     else
				     {
					     var property = html.ViewData.ModelMetadata.Find(modelState.Key);
					     var displayName = property != null ? property.GetDisplayName() : modelState.Key;

					     var linkTag = new TagBuilder("a");
					     linkTag.Attributes.Add("href", string.Format("#{0}", modelState.Key.Replace('.', '_')));
					     linkTag.SetInnerText(displayName);

					     listItemTag.InnerHtml = string.Format("{0} - {1}", linkTag.ToString(TagRenderMode.Normal), error.ErrorMessage);
				     }

				     stringBuilder.AppendLine(listItemTag.ToString(TagRenderMode.Normal));
			     }
		     }
	     }

	     listTag.InnerHtml = stringBuilder.ToString();

	     var sectionTag = new TagBuilder("section");
	     sectionTag.Attributes.Add("id", "validation-error-summary");
	     sectionTag.AddCssClass(html.ViewData.ModelState.IsValid ? HtmlHelper.ValidationSummaryValidCssClassName : HtmlHelper.ValidationSummaryCssClassName);
	     sectionTag.InnerHtml = headerTag.ToString(TagRenderMode.Normal) + listTag.ToString(TagRenderMode.Normal);

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

	     return new MvcHtmlString(result.ToString());
     }

	  private static MvcHtmlString MessageSummary(HtmlHelper html, MessageType messageType)
	  {
		  var key = messageType.ToString();

		  if (string.IsNullOrEmpty(key))
		  {
			  return new MvcHtmlString(string.Empty);
		  }

		  Dictionary<string, List<string>> messages = null;

		  if (html.ViewContext.TempData.ContainsKey(key))
		  {
			  messages = html.ViewContext.TempData[key] as Dictionary<string, List<string>>;
		  }

		  if (messages != null && messages.Any())
		  {
			  var headerTag = new TagBuilder("h2");
			  headerTag.SetInnerText(string.Format("{0} message", key));

			  var listTag = new TagBuilder("ul");
			  var stringBuilder = new StringBuilder();

			  foreach (var message in messages)
			  {
				  foreach (var m in message.Value)
				  {
					  if (!string.IsNullOrEmpty(m))
					  {
						  var listItemTag = new TagBuilder("li");

						  if (string.IsNullOrEmpty(message.Key))
						  {
							  listItemTag.SetInnerText(m);
						  }
						  else
						  {
							  var property = html.ViewData.ModelMetadata.Properties.FirstOrDefault(p => p.PropertyName == message.Key);
							  var displayName = property != null ? property.GetDisplayName() : message.Key;

							  var linkTag = new TagBuilder("a");
							  linkTag.Attributes.Add("href", string.Format("#{0}", message.Key.Replace('.', '_')));
							  linkTag.SetInnerText(displayName);

							  listItemTag.InnerHtml = string.Format("{0} - {1}", linkTag.ToString(TagRenderMode.Normal), m);
						  }

						  stringBuilder.AppendLine(listItemTag.ToString(TagRenderMode.Normal));
					  }
				  }
			  }

			  listTag.InnerHtml = stringBuilder.ToString();

			  var sectionTag = new TagBuilder("section");

			  switch (messageType)
			  {
				  case MessageType.Success:
					  sectionTag.AddCssClass("msgGood");
					  break;
				  case MessageType.Information:
					  sectionTag.AddCssClass("msgInfo");
					  break;
				  case MessageType.Warning:
					  sectionTag.AddCssClass("msgWarn");
					  break;
			  }

			  sectionTag.InnerHtml = headerTag.ToString(TagRenderMode.Normal) + listTag.ToString(TagRenderMode.Normal);

			  return new MvcHtmlString(sectionTag.ToString(TagRenderMode.Normal));
		  }

		  return new MvcHtmlString(string.Empty);
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
	  /// Find the Model Metadata for a property, including nested properties (delimited by dot).
	  /// </summary>
	  /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
	  /// <param name="propertyName">The property name to find.</param>
	  /// <returns>If found, the model metadata of the property name; otherwise, null.</returns>
	  public static ModelMetadata Find(this ModelMetadata modelMetadata, string propertyName)
	  {
		  if (modelMetadata == null || string.IsNullOrEmpty(propertyName))
		  {
			  return null;
		  }

		  // Check if property is nested
		  if (propertyName.Contains('.'))
		  {
			  // Split nested property name into its nested segments
			  var propertyNameSegments = propertyName.Split('.');

			  var currentModelMetadata = modelMetadata;

			  // Drill down into the Model Metadata to find the property
			  foreach (var propertyNameSegment in propertyNameSegments)
			  {
				  if (currentModelMetadata != null)
				  {
					  // Get matching property for current segment
					  currentModelMetadata = currentModelMetadata.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyNameSegment, StringComparison.InvariantCultureIgnoreCase));
				  }
			  }

			  // Will be populated if final segment (actual property) was found, otherwise null
			  return currentModelMetadata;
		  }

		  // Find matching property name in current Model Metadata, will be populated if found, otherwise null
		  return modelMetadata.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
	  }




	}
}