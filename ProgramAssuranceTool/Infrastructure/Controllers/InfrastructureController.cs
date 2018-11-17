using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Types;
using ProgramAssuranceTool.Interfaces;

namespace ProgramAssuranceTool.Infrastructure.Controllers
{
	/// <summary>
	///    Defines the base infrastructure controller which handles the Dependency Injection of common services.
	/// </summary>
	public abstract class InfrastructureController : Controller
	{
		/// <summary>
		///    Whether this is a duplicate POST submit.
		/// </summary>
		public bool IsDuplicateSubmit { get; internal set; }

		/// <summary>
		///    Override page title.
		/// </summary>
		protected string PageTitle
		{
			get { return ViewBag.PageTitle; }
			set { ViewBag.PageTitle = value; }
		}

		protected readonly IVirtualPathService VirtualPathService;

		protected readonly IPatService PatService;

		/// <summary>
		///    Initializes a new instance of the <see cref="InfrastructureController" /> class.
		/// </summary>
		/// <param name="controllerDependencies">Dependencies holder.</param>
		protected InfrastructureController( IControllerDependencies controllerDependencies )
		{
			VirtualPathService = controllerDependencies.VirtualPathService;
			PatService = controllerDependencies.PatService;
		}

		protected string ServerSoftware
		{
			get { return Request.ServerVariables.Get( "SERVER_SOFTWARE" ); }
		}

		#region Validation

		/// <summary>
		///    Validates a view model.
		/// </summary>
		/// <typeparam name="T">The view model type.</typeparam>
		/// <param name="model">The view model instance.</param>
		/// <returns>The model state of the view model.</returns>
		protected ModelStateDictionary ValidateModel<T>( T model )
		{
			T m = model;

			var modelBinder = new ModelBindingContext
				{
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType( () => m, typeof (T) ),
					ValueProvider = new NameValueCollectionValueProvider( new NameValueCollection(), CultureInfo.InvariantCulture )
				};

			ModelBinders.Binders.DefaultBinder.BindModel( ControllerContext, modelBinder );

			return modelBinder.ModelState;
		}

		#endregion

		#region Add Messages

		/// <summary>
		///    Add a success message.
		/// </summary>
		/// <param name="successMessage">The success message.</param>
		protected void AddSuccessMessage( string successMessage )
		{
			AddMessage( MessageType.Success, string.Empty, successMessage );
		}

		/// <summary>
		///    Add a success message against a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to assign the success message to.</param>
		/// <param name="successMessage">The success message.</param>
		protected void AddSuccessMessage( string propertyName, string successMessage )
		{
			AddMessage( MessageType.Success, propertyName, successMessage );
		}

		/// <summary>
		///    Add an information message.
		/// </summary>
		/// <param name="informationMessage">The information message.</param>
		protected void AddInformationMessage( string informationMessage )
		{
			AddMessage( MessageType.Information, string.Empty, informationMessage );
		}

		/// <summary>
		///    Add an information message against a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to assign the information message to.</param>
		/// <param name="informationMessage">The information message.</param>
		protected void AddInformationMessage( string propertyName, string informationMessage )
		{
			AddMessage( MessageType.Information, propertyName, informationMessage );
		}

		/// <summary>
		///    Add a warning message.
		/// </summary>
		/// <param name="warningMessage">The warning message.</param>
		protected void AddWarningMessage( string warningMessage )
		{
			AddMessage( MessageType.Warning, string.Empty, warningMessage );
		}

		/// <summary>
		///    Add a warning message against a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to assign the warning message to.</param>
		/// <param name="warningMessage">The warning message.</param>
		protected void AddWarningMessage( string propertyName, string warningMessage )
		{
			AddMessage( MessageType.Warning, propertyName, warningMessage );
		}

		/// <summary>
		///    Add a message to temp data.
		/// </summary>
		/// <param name="messageType">Type of message.</param>
		/// <param name="propertyName">The name of the property to assign the message to.</param>
		/// <param name="message">The message.</param>
		private void AddMessage( MessageType messageType, string propertyName, string message )
		{
			if (propertyName == null)
			{
				propertyName = string.Empty;
			}

			Dictionary<string, List<string>> messages = null;

			var key = messageType.ToString();

			if (TempData.ContainsKey( key ))
			{
				messages = TempData[ key ] as Dictionary<string, List<string>>;
			}

			if (messages == null)
			{
				messages = new Dictionary<string, List<string>>();
			}

			if (messages.ContainsKey( propertyName ))
			{
				if (!messages[ propertyName ].Contains( message ))
				{
					messages[ propertyName ].Add( message );
				}
			}
			else
			{
				messages.Add( propertyName, new List<string> {message} );
			}

			TempData[ key ] = messages;
		}

		/// <summary>
		///    Add an error message.
		/// </summary>
		/// <remarks>
		///    Almost the same behaviour as using method <see cref="ModelStateDictionary.AddModelError(string, string)" /> on
		///    <see
		///       cref="Controller.ModelState" />
		///    .
		///    The difference is that duplicate errors will not be added.
		/// </remarks>
		/// <param name="errorMessage">The error message.</param>
		protected void AddErrorMessage( string errorMessage )
		{
			AddErrorMessage( string.Empty, errorMessage );
		}

		/// <summary>
		///    Add an error message against a property.
		/// </summary>
		/// <remarks>
		///    Almost the same behaviour as using method <see cref="ModelStateDictionary.AddModelError(string, string)" /> on
		///    <see
		///       cref="Controller.ModelState" />
		///    .
		///    The difference is that duplicate errors will not be added.
		/// </remarks>
		/// <param name="propertyName">The name of the property to assign the error to.</param>
		/// <param name="errorMessage">The error message.</param>
		protected void AddErrorMessage( string propertyName, string errorMessage )
		{
			if (propertyName == null)
			{
				propertyName = string.Empty;
			}

			if (!HasErrorMessage( propertyName, errorMessage ))
			{
				ModelState.AddModelError( propertyName, errorMessage );
			}
		}

		#endregion

		#region Get Messages

		/// <summary>
		///    Get all success messages not assigned to a property.
		/// </summary>
		/// <returns>All success messages not assigned to a property.</returns>
		protected IEnumerable<string> GetSuccessMessages()
		{
			return GetMessages( MessageType.Success, string.Empty );
		}

		/// <summary>
		///    Get all success messages assigned to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the success messages for.</param>
		/// <returns>All success messages assigned to a property.</returns>
		protected IEnumerable<string> GetSuccessMessages( string propertyName )
		{
			return GetMessages( MessageType.Success, propertyName );
		}

		/// <summary>
		///    Get all information messages not assigned to a property.
		/// </summary>
		/// <returns>All information messages not assigned to a property.</returns>
		protected IEnumerable<string> GetInformationMessages()
		{
			return GetMessages( MessageType.Information, string.Empty );
		}

		/// <summary>
		///    Get all information messages assigned to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the information messages for.</param>
		/// <returns>All information messages assigned to a property.</returns>
		protected IEnumerable<string> GetInformationMessages( string propertyName )
		{
			return GetMessages( MessageType.Information, propertyName );
		}

		/// <summary>
		///    Get all warning messages not assigned to a property.
		/// </summary>
		/// <returns>All warning messages not assigned to a property.</returns>
		protected IEnumerable<string> GetWarningMessages()
		{
			return GetMessages( MessageType.Warning, string.Empty );
		}

		/// <summary>
		///    Get all warning messages assigned to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the warning messages for.</param>
		/// <returns>All warning messages assigned to a property.</returns>
		protected IEnumerable<string> GetWarningMessages( string propertyName )
		{
			return GetMessages( MessageType.Warning, propertyName );
		}

		/// <summary>
		///    Get messages from temp data assigned to a property.
		/// </summary>
		/// <param name="messageType">Type of message.</param>
		/// <param name="propertyName">The name of the property to get the messages for.</param>
		/// <returns>All messages of the specific type that are assigned to the specified property.</returns>
		private IEnumerable<string> GetMessages( MessageType messageType, string propertyName )
		{
			if (propertyName == null)
			{
				propertyName = string.Empty;
			}

			Dictionary<string, List<string>> messages = null;

			var key = messageType.ToString();

			if (TempData.ContainsKey( key ))
			{
				messages = TempData[ key ] as Dictionary<string, List<string>>;
			}

			if (messages == null)
			{
				messages = new Dictionary<string, List<string>>();
			}

			if (messages.ContainsKey( propertyName ))
			{
				return messages[ propertyName ];
			}

			return Enumerable.Empty<string>();
		}

		/// <summary>
		///    Get all error messages not assigned to a property.
		/// </summary>
		/// <returns>All error messages not assigned to a property.</returns>
		protected IEnumerable<string> GetErrorMessages()
		{
			return GetErrorMessages( string.Empty );
		}

		/// <summary>
		///    Get all error messages assigned to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the error messages for.</param>
		/// <returns>All error messages assigned to a property.</returns>
		protected IEnumerable<string> GetErrorMessages( string propertyName )
		{
			if (propertyName == null)
			{
				propertyName = string.Empty;
			}

			var errors = ModelState.FirstOrDefault( m => m.Key == propertyName );

			if (errors.Value != null && errors.Value.Errors != null &&
			    errors.Value.Errors.Any( e => !string.IsNullOrEmpty( e.ErrorMessage ) ))
			{
				return errors.Value.Errors.Select( e => e.ErrorMessage );
			}

			return Enumerable.Empty<string>();
		}

		#endregion

		#region Has Messages

		/// <summary>
		///    Has a specific success message already been added.
		/// </summary>
		/// <param name="successMessage">The success message.</param>
		/// <returns>
		///    <c>true</c> if the success message exists; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasSuccessMessage( string successMessage )
		{
			return HasMessage( MessageType.Success, string.Empty, successMessage );
		}

		/// <summary>
		///    Has a specific success message already been added to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to check for the success message.</param>
		/// <param name="successMessage">The success message.</param>
		/// <returns>
		///    <c>true</c> if the success message exists against the property; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasSuccessMessage( string propertyName, string successMessage )
		{
			return HasMessage( MessageType.Success, propertyName, successMessage );
		}

		/// <summary>
		///    Has a specific information message already been added.
		/// </summary>
		/// <param name="informationMessage">The information message.</param>
		/// <returns>
		///    <c>true</c> if the information message exists; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasInformationMessage( string informationMessage )
		{
			return HasMessage( MessageType.Information, string.Empty, informationMessage );
		}

		/// <summary>
		///    Has a specific information message already been added to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to check for the information message.</param>
		/// <param name="informationMessage">The information message.</param>
		/// <returns>
		///    <c>true</c> if the information message exists for the property; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasInformationMessage( string propertyName, string informationMessage )
		{
			return HasMessage( MessageType.Information, propertyName, informationMessage );
		}

		/// <summary>
		///    Has a specific warning message already been added.
		/// </summary>
		/// <param name="warningMessage">The warning message.</param>
		/// <returns>
		///    <c>true</c> if the warning message exists; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasWarningMessage( string warningMessage )
		{
			return HasMessage( MessageType.Warning, string.Empty, warningMessage );
		}

		/// <summary>
		///    Has a specific warning message already been added to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to check for the warning message.</param>
		/// <param name="warningMessage">The warning message.</param>
		/// <returns>
		///    <c>true</c> if the warning message exists for the property; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasWarningMessage( string propertyName, string warningMessage )
		{
			return HasMessage( MessageType.Warning, propertyName, warningMessage );
		}

		/// <summary>
		///    Has a message already been added to a property in temp data.
		/// </summary>
		/// <param name="messageType">Type of message.</param>
		/// <param name="propertyName">The name of the property to check the message for.</param>
		/// <param name="message">The message.</param>
		/// <returns>
		///    <c>true</c> if the message exists for the property; otherwise, <c>false</c>.
		/// </returns>
		private bool HasMessage( MessageType messageType, string propertyName, string message )
		{
			return GetMessages( messageType, propertyName ).Any( m => m == message );
		}

		/// <summary>
		///    Has a specific error message already been added.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///    <c>true</c> if the error message exists; Ootherwise, <c>false</c>.
		/// </returns>
		protected bool HasErrorMessage( string errorMessage )
		{
			return HasErrorMessage( string.Empty, errorMessage );
		}

		/// <summary>
		///    Has a specific error message already been added to a property.
		/// </summary>
		/// <param name="propertyName">The name of the property to check for the error.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///    <c>true</c> if the error message exists for the property; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasErrorMessage( string propertyName, string errorMessage )
		{
			return GetErrorMessages( propertyName ).Any( m => m == errorMessage );
		}

		#endregion

		#region Action Results

		/// <summary>
		///    Returns a 401 (unauthorized) status code result.
		/// </summary>
		/// <returns>The 401 custom error page.</returns>
		protected internal HttpUnauthorizedResult HttpUnauthorized()
		{
			return HttpUnauthorized( null );
		}

		/// <summary>
		///    Returns a 401 (unauthorized) status code result.
		/// </summary>
		/// <param name="statusDescription">A custom status description to include in the result.</param>
		/// <returns>The 401 custom error page.</returns>
		protected internal virtual HttpUnauthorizedResult HttpUnauthorized( string statusDescription )
		{
			return new HttpUnauthorizedResult( statusDescription );
		}

		/// <summary>
		///    Returns a 403 (unauthorized) status code result.
		/// </summary>
		/// <returns>The 403 custom error page.</returns>
		protected internal HttpStatusCodeResult HttpForbidden()
		{
			return HttpUnauthorized( null );
		}

		/// <summary>
		///    Returns a 403 (unauthorized) status code result.
		/// </summary>
		/// <param name="statusDescription">A custom status description to include in the result.</param>
		/// <returns>The 403 custom error page.</returns>
		protected internal virtual HttpStatusCodeResult HttpForbidden( string statusDescription )
		{
			return new HttpStatusCodeResult( 403, statusDescription );
		}


		#endregion
	}
}