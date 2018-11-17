using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Models;
using ProgramAssuranceTool.Infrastructure.Types;

namespace ProgramAssuranceTool.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions for <see cref="System.Collections.Generic.IList{RelatedCodeModel}" />.
	/// </summary>
	[ExcludeFromCodeCoverage] 
	public static class RelatedCodeModelExtension
	{
		//private static IMappingEngine MappingEngine
		//{
		//	get
		//	{
		//		var containerProvider = DependencyResolver.Current as IContainerProvider;

		//		if ( containerProvider != null )
		//		{
		//			return containerProvider.GetService<IMappingEngine>();
		//		}

		//		return null;
		//	}
		//}

		/// <summary>
		/// Converts the current instance of <see cref="RelatedCodeModel" /> to a <see cref="CodeModel" />.
		/// </summary>
		/// <returns>The current instance of <see cref="RelatedCodeModel" /> as a <see cref="CodeModel" />.</returns>
		public static CodeModel ToCodeModel( this RelatedCodeModel model )
		{
			//return MappingEngine.Map<CodeModel>( model );
			return null;
		}

		/// <summary>
		/// Converts a list of <see cref="RelatedCodeModel" /> to a list of <see cref="CodeModel" />.
		/// </summary>
		/// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
		/// <returns>A list of <see cref="CodeModel" />.</returns>
		public static IList<CodeModel> ToCodeModelList( this IList<RelatedCodeModel> enumerable )
		{
//			return MappingEngine.Map<IList<CodeModel>>( enumerable );
			return null;
		}

		/// <summary>
		/// Converts a list of <see cref="RelatedCodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
		/// </summary>
		/// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
		/// <param name="orderType">The order type to order by.</param>
		/// <param name="displayType">The display type to use for the display text.</param>
		/// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
		public static IEnumerable<SelectListItem> ToOrderedSelectListItem( this IList<RelatedCodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType )
		{
			return ToOrderedSelectListItem( enumerable, orderType, displayType, null );
		}

		/// <summary>
		/// Converts a list of <see cref="RelatedCodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
		/// </summary>
		/// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
		/// <param name="orderType">The order type to order by.</param>
		/// <param name="displayType">The display type to use for the display text.</param>
		/// <param name="selectedCodes">Any selected codes.</param>
		/// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
		public static IEnumerable<SelectListItem> ToOrderedSelectListItem( this IList<RelatedCodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType, object selectedCodes )
		{
			if ( enumerable == null || !enumerable.Any() )
			{
				return Enumerable.Empty<SelectListItem>();
			}

			switch ( orderType )
			{
				case AdwOrderType.ByDominantCode:
					enumerable = enumerable.OrderBy( m => m.DominantCode ).ToList();
					break;
				case AdwOrderType.BySubordinateCode:
					enumerable = enumerable.OrderBy( m => m.SubordinateCode ).ToList();
					break;
				case AdwOrderType.ByCode:
					enumerable = enumerable.First().Dominant ? enumerable.OrderBy( m => m.DominantCode ).ToList() : enumerable.OrderBy( m => m.SubordinateCode ).ToList();
					break;
			}

			var result = enumerable.ToCodeModelList().ToSelectListItem( m => m.Code, m => m.GetDisplayText( displayType ), selectedCodes );

			if ( orderType == AdwOrderType.ByDescription )
			{
				result = result.OrderBy( m => m.Text );
			}

			return result;
		}
	}
}