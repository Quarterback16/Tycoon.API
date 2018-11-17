using System;
using System.Collections.Generic;
using System.Linq;

using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Mappers {

    public static class PageableMapper
    {
        public static Pageable<HistoryModel> ToHistoryModelList(this IEnumerable<HistoryModel> srcList, Pageable<HistoryModel> destList)
        {
            if (srcList!=null && srcList.Any())
            {
                destList.AddRange(srcList);
                //foreach (var src in srcList)
                //{
                //    destList.Add(src);
                //}
            }
            return destList;
        }
    }

    ///// <summary>
    ///// Represents a mapper that is used to map between <see cref="IEnumerable{T}" /> and <see cref="IPageable{T}" />.
    ///// </summary>
    //public class PageableMapper : EnumerableMapperBase<IPageable>, IObjectMapper {
    //    /// <summary>
    //    /// Determines whether the source and destination objects should be handled by this wrapper.
    //    /// </summary>
    //    /// <param name="context">Resolution context of the source and destination objects.</param>
    //    /// <returns><c>true</c> if the source type is <see cref="IEnumerable{T}" /> and the destination type is <see cref="IPageable{T}" />; otherwise, <c>false</c>.</returns>
    //    public override bool IsMatch(ResolutionContext context) {
    //        return IsPageable(context.DestinationType) && TypeExtension.IsEnumerableType(context.SourceType) && !IsPageable(context.SourceType);
    //    }

    //    /// <summary>
    //    /// Maps the source object to the destination object.
    //    /// </summary>
    //    /// <param name="context">Resolution context of the source and destination objects.</param>
    //    /// <param name="mapper">Mapping engine runner.</param>
    //    /// <returns>The mapped object.</returns>
    //    public new object Map(ResolutionContext context, IMappingEngineRunner mapper) {
    //        var obj = base.Map(context, mapper);

    //        return obj;
    //    }

    //    /// <summary>
    //    /// Creates the destination object.
    //    /// </summary>
    //    /// <param name="destElementType">Destination element type.</param>
    //    /// <param name="sourceLength">Length of source object.</param>
    //    /// <returns>The created destination object.</returns>
    //    protected override IPageable CreateDestinationObjectBase(Type destElementType, int sourceLength) {
    //        return (IPageable)DelegateHelper.CreateConstructorDelegate(typeof(Pageable<>).MakeGenericType(new[] { destElementType }))();
    //    }

    //    /// <summary>
    //    /// Set element value in destination object.
    //    /// </summary>
    //    /// <param name="destination">Destination object.</param>
    //    /// <param name="mappedValue">Element value to set in destination object.</param>
    //    /// <param name="index">Index of element.</param>
    //    protected override void SetElementValue(IPageable destination, object mappedValue, int index) {
    //        destination.Add(mappedValue);
    //    }

    //    /// <summary>
    //    /// Determines whether the type is <see cref="IPageable{T}" />
    //    /// </summary>
    //    /// <param name="type">Type to check.</param>
    //    /// <returns><c>true</c> if the type is <see cref="IPageable{T}" />; otherwise, <c>false</c>.</returns>
    //    private bool IsPageable(Type type) {
    //        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IPageable<>)) {
    //            return true;
    //        }

    //        if (type.GetInterfaces().Any(i => i == typeof(IPageable<>))) {
    //            return true;
    //        }

    //        return false;
    //    }
    //}
}