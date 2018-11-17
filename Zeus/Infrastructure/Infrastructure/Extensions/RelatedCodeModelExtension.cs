using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Collections.Generic.IList{RelatedCodeModel}" />.
    /// </summary>
    public static class RelatedCodeModelExtension
    {
		//private static IMappingEngine MappingEngine
		//{
		//    get
		//    {
		//        var containerProvider = DependencyResolver.Current as IContainerProvider;

		//        if (containerProvider != null)
		//        {
		//            return containerProvider.GetService<IMappingEngine>();
		//        }

		//        return null;
		//    }
		//}

        /// <summary>
        /// Converts the current instance of <see cref="RelatedCodeModel" /> to a <see cref="CodeModel" />.
        /// </summary>
        /// <returns>The current instance of <see cref="RelatedCodeModel" /> as a <see cref="CodeModel" />.</returns>
        public static CodeModel ToCodeModel(this RelatedCodeModel model)
        {
	        var codeModel = new CodeModel();
	        codeModel.Code = ( model.Dominant ? model.SubordinateCode : model.DominantCode);
  	        codeModel.Description = (  model.Dominant ? model.SubordinateDescription : model.DominantDescription);
	        codeModel.ShortDescription = ( model.Dominant ? model.SubordinateShortDescription : model.DominantShortDescription);
	        return codeModel;
        }

	    /// <summary>
	    /// Converts a list of <see cref="RelatedCodeModel" /> to a list of <see cref="CodeModel" />.
	    /// </summary>
	    /// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
	    /// <returns>A list of <see cref="CodeModel" />.</returns>
	    public static IList<CodeModel> ToCodeModelList(this IList<RelatedCodeModel> enumerable)
	    {
		    if (enumerable != null)
		    {
			    IList<CodeModel> codeModels = new List<CodeModel>(enumerable.Count);
			    foreach (var model in enumerable)
			    {
				    var codeModel = new CodeModel();
				    codeModel.Code = (model.Dominant ? model.SubordinateCode : model.DominantCode);
				    codeModel.Description = (model.Dominant ? model.SubordinateDescription : model.DominantDescription);
				    codeModel.ShortDescription = (model.Dominant ? model.SubordinateShortDescription : model.DominantShortDescription);
				    codeModels.Add(codeModel);
			    }
			    return codeModels as IList<CodeModel>;
		    }
		    return new List<CodeModel>() as IList<CodeModel>;
	    }

	    /// <summary>
        /// Converts a list of <see cref="RelatedCodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
        /// </summary>
        /// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
        /// <param name="orderType">The order type to order by.</param>
        /// <param name="displayType">The display type to use for the display text.</param>
        /// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
        public static IEnumerable<SelectListItem> ToOrderedSelectListItem(this IList<RelatedCodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType)
        {
            return ToOrderedSelectListItem(enumerable, orderType, displayType, null);
        }

        /// <summary>
        /// Converts a list of <see cref="RelatedCodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
        /// </summary>
        /// <param name="enumerable">The list of <see cref="RelatedCodeModel" />.</param>
        /// <param name="orderType">The order type to order by.</param>
        /// <param name="displayType">The display type to use for the display text.</param>
        /// <param name="selectedCodes">Any selected codes.</param>
        /// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
        public static IEnumerable<SelectListItem> ToOrderedSelectListItem(this IList<RelatedCodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType, object selectedCodes)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            switch (orderType)
            {
                case AdwOrderType.ByDominantCode:
                    enumerable = enumerable.OrderBy(m => m.DominantCode).ToList();
                    break;
                case AdwOrderType.BySubordinateCode:
                    enumerable = enumerable.OrderBy(m => m.SubordinateCode).ToList();
                    break;
                case AdwOrderType.ByCode:
                    enumerable = enumerable.First().Dominant ? enumerable.OrderBy(m => m.DominantCode).ToList() : enumerable.OrderBy(m => m.SubordinateCode).ToList();
                    break;
            }

            var result = enumerable.ToCodeModelList().ToSelectListItem(m => m.Code, m => m.GetDisplayText(displayType), selectedCodes);

            if (orderType == AdwOrderType.ByDescription)
            {
                result = result.OrderBy(m => m.Text);
            }

            return result;
        }
    }
}