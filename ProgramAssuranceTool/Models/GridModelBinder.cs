using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Models
{
	[ModelBinder( typeof( GridModelBinder ) )]
	//  Clashes with the MvcJqGrid assembly - go with ours
	public class GridSettings
	{
		public bool IsSearch { get; set; }
		public int PageSize { get; set; }
		public int PageIndex { get; set; }
		public string SortColumn { get; set; }
		public string SortOrder { get; set; }
		public Filter Where { get; set; }
	}

	[DataContract]
	public class Filter
	{
		[DataMember]
		public string groupOp { get; set; }
		[DataMember]
		public Rule[] rules { get; set; }

		public static Filter Create( string jsonData )
		{
			try
			{
				var serializer = new DataContractJsonSerializer( typeof( Filter ) );
				var reader = new System.IO.StringReader( jsonData );
				var ms = new System.IO.MemoryStream( System.Text.Encoding.Default.GetBytes( jsonData ) );
				return serializer.ReadObject( ms ) as Filter;
			}
			catch
			{
				return null;
			}
		}
	}

	[DataContract]
	public class Rule
	{
		[DataMember]
		public string field { get; set; }

		/// <summary>
		///   An indicator representing an Equals (1) (eq) or a Contains (0) operation
		/// </summary>
		[DataMember]
		public string op { get; set; }
		[DataMember]
		public string data { get; set; }
	}

	public class GridModelBinder : IModelBinder
	{
		public object BindModel( ControllerContext controllerContext,
												  ModelBindingContext bindingContext )
		{
			//var _activityRepository = new UserActivityRepository<UserActivity>();
			try
			{
				var request = controllerContext.HttpContext.Request;
				return new GridSettings
				{
					IsSearch = bool.Parse( request[ "_search" ] ?? "false" ),
					PageIndex = int.Parse( request[ "page" ] ?? "1" ),
					PageSize = int.Parse( request[ "rows" ] ?? "20" ),
					SortColumn = request[ "sidx" ] ?? "",
					SortOrder = request[ "sord" ] ?? "desc",
					Where = Filter.Create( request[ "filters" ] ?? "" )
				};
			}
			catch ( Exception ex )
			{
				Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
				return null;
			}
		}
	}
}