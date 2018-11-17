using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Collections.Generic.IList{CodeModel}" />.
    /// </summary>
    public static class CodeModelExtension
    {
        /// <summary>
        /// Converts a list of <see cref="CodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
        /// </summary>
        /// <param name="enumerable">The list of <see cref="CodeModel" />.</param>
        /// <param name="orderType">The order type to order by.</param>
        /// <param name="displayType">The display type to use for the display text.</param>
        /// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
        public static IEnumerable<SelectListItem> ToOrderedSelectListItem(this IList<CodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType)
        {
            return ToOrderedSelectListItem(enumerable, orderType, displayType, null);
        }

        /// <summary>
        /// Converts a list of <see cref="CodeModel" /> to an enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.
        /// </summary>
        /// <param name="enumerable">The list of <see cref="CodeModel" />.</param>
        /// <param name="orderType">The order type to order by.</param>
        /// <param name="displayType">The display type to use for the display text.</param>
        /// <param name="selectedCodes">Any selected codes.</param>
        /// <returns>An enumerable of <see cref="SelectListItem" /> ordered by the specified <paramref name="orderType"/>.</returns>
        public static IEnumerable<SelectListItem> ToOrderedSelectListItem(this IList<CodeModel> enumerable, AdwOrderType orderType, AdwDisplayType displayType, object selectedCodes)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            switch (orderType)
            {
                case AdwOrderType.ByDominantCode:
                case AdwOrderType.BySubordinateCode:
                case AdwOrderType.ByCode:
                    enumerable = enumerable.OrderBy(m => m.Code).ToList();
                    break;
            }

            var result = enumerable.ToSelectListItem(m => m.Code, m => m.GetDisplayText(displayType), selectedCodes);

            if (orderType == AdwOrderType.ByDescription)
            {
                result = result.OrderBy(m => m.Text);
            }

            return result;
        }
    }
}