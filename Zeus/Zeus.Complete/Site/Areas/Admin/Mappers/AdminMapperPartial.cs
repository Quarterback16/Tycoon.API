
using System.Collections.Generic;
using System.Security.Claims;
using Employment.Web.Mvc.Area.Admin.ViewModels;
using Employment.Web.Mvc.Area.Admin.ViewModels.User;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Provisioner;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup;
using Employment.Web.Mvc.Infrastructure.Models.AdwLookup;

// Add using for the 'Employment.Web.Mvc.Service.Interfaces' service you will use

namespace Employment.Web.Mvc.Area.Admin.Mappers
{
    /// <summary>
    /// Represents a mapper that is used to map between View Models and Domain Models.
    /// </summary>
    public static partial class AdminMapper
    {


        public static IEnumerable<ClaimViewModel> ToClaimViewModelList(this IEnumerable<Claim> src)
        {
            var dest = new List<ClaimViewModel>();
            if (src != null)
            {
                foreach (var s in src)
                {
                    dest.Add(s.ToClaimViewModel());
                }
            }
            return dest as IEnumerable<ClaimViewModel>;
        }
        public static ProvisionerModel ToProvisionerModel(this DepartmentEmulateUser src)
        {
            var dest = new ProvisionerModel();
            dest.JobNumber = src.JobNumber;
            dest.Reason = src.Reason;
            dest.UserId = src.UserId;
            return dest;
        }

        public static ClaimViewModel ToClaimViewModel(this Claim src)
        {
            var dest = new ClaimViewModel();
            dest.ClaimType = src.Type;
            dest.Value = src.Value;
            dest.HashKey = src.GetHashCode().ToString();
            return dest;
        }


        #region Adw Lookup Mappings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static IPageable<CodeModel> ToPageableCodeModelList(this IList<CodeModel> sourceList)
        {
            if (sourceList == null)
            { return null; }

            var targetList = new Pageable<CodeModel>();
            foreach (var codeModel in sourceList)
            {
                targetList.Add(codeModel);
            }
            return targetList;
        }


        public static ListCodeTypePageMetadata ToListCodeTypePageMetadata(this ListCodeTypeViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new ListCodeTypePageMetadata
            {
                ExactLookup = model.ExactLookup,
                ListType = model.ListType,
                MaxRows = model.MaxRows,
                StartsWith = model.StartFromTableType
            };
        }


        /// <summary>
        ///  Extension method for mapping IList<see cref="CodeTypeModel"/> to IPageable<see cref="CodeTypeViewModel"/>.
        /// </summary>
        /// <param name="sourceList">List of <see cref="CodeTypeModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<CodeTypeViewModel>"/>.</returns>
        public static IPageable<CodeTypeViewModel> ToPageableCodeTypeModelList(this IList<CodeTypeModel> sourceList, IPageable<CodeTypeViewModel> targetList)
        {
            if (sourceList == null)
            { return null; }


            foreach (var codeModel in sourceList)
            {
                var codeTypeViewModel = AdminMapper.MapToCodeTypeViewModel(codeModel);

                targetList.Add(codeTypeViewModel);


            }
            return targetList;
        }


        /// <summary>
        /// Extension method for mapping <see cref="CodeTypeModel"/> to <see cref="CodeTypeViewModel"/>.
        /// </summary>
        /// <param name="codeModel"><see cref="CodeTypeModel"/> instance.</param>
        /// <returns>Instance of <see cref="CodeTypeViewModel"/>.</returns>
        public static CodeTypeViewModel ToCodeTypeViewModel(this CodeTypeModel codeModel)
        {
            if (codeModel == null)
            {
                return null;
            }

            return new CodeTypeViewModel
            {
                CodeType = codeModel.CodeType,
                LongDescription = codeModel.LongDescription,
                ShortDescription = codeModel.ShortDescription
            };

        }


        /// <summary>
        /// Extension method for mapping IList<see cref="RelatedCodeTypeModel"/> to IPageable<see cref="RelatedCodeTypeViewModel"/>.
        /// </summary>
        /// <param name="sourceList">List of <see cref="RelatedCodeTypeModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<RelatedCodeTypeViewModel>"/>.</returns>
        public static IPageable<RelatedCodeTypeViewModel> ToPagableRelatedCodeTypeViewModel(this IList<RelatedCodeTypeModel> sourceList)
        {
            if (sourceList == null)
            {
                return null;
            }

            IPageable<RelatedCodeTypeViewModel> list = new Pageable<RelatedCodeTypeViewModel>();
            foreach (var relatedCodeModel in sourceList)
            {
                var relatedCodeTypeViewModel = relatedCodeModel.ToRelatedCodeTypeViewModel();
                if (relatedCodeTypeViewModel != null)
                {
                    list.Add(relatedCodeTypeViewModel);
                }
            }

            return list;
        }

        /// <summary>
        /// Extension method for mapping <see cref="RelatedCodeTypeModel"/> to <see cref="RelatedCodeTypeViewModel"/>.
        /// </summary>
        /// <param name="relatedCodeModel"><see cref="RelatedCodeTypeModel"/> instance.</param>
        /// <returns>Instance of <see cref="RelatedCodeTypeViewModel"/>.</returns>
        public static RelatedCodeTypeViewModel ToRelatedCodeTypeViewModel(this RelatedCodeTypeModel relatedCodeModel)
        {
            if (relatedCodeModel == null)
            {
                return null;
            }

            return new RelatedCodeTypeViewModel
            {
                Relationship = relatedCodeModel.Relationship,
                SubType = relatedCodeModel.SubType,
                SubDescription = relatedCodeModel.SubDescription,
                DomType = relatedCodeModel.DomType,
                DomDescription = relatedCodeModel.DomDescription
            };

        }


        /// <summary>
        /// Extension method for mapping IList<see cref="CodeModel"/> to IPageable<see cref="CodeTypeViewModel"/>.
        /// </summary>
        /// <param name="sourceList">Collection of <see cref="CodeModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<CodeTypeViewModel>"/>.</returns>
        public static IPageable<CodeTypeViewModel> ToPageableCodeTypeViewModelList(this IList<CodeModel> sourceList)
        {
            if (sourceList == null)
            {
                return null;
            }

            var list = new Pageable<CodeTypeViewModel>();

            foreach (var codeModel in sourceList)
            {
                var codeTypeViewModel = codeModel.ToCodeTypeViewModel();
                if (codeTypeViewModel != null)
                {
                    list.Add(codeTypeViewModel);
                }
            }

            return list;
        }


        /// <summary>
        /// Extension method for mapping <see cref="CodeModel"/> to <see cref="CodeTypeViewModel"/>.
        /// </summary>
        /// <param name="codeModel"><see cref="CodeModel"/> instance.</param>
        /// <returns>Instance of <see cref="CodeTypeViewModel"/>.</returns>
        public static CodeTypeViewModel ToCodeTypeViewModel(this CodeModel codeModel)
        {
            if (codeModel == null)
            {
                return null;
            }

            var model = new CodeTypeViewModel()
            {
                // SET Defaults:
                ShowIntColumns = false,
                ShowListCodeColumns = true,


                CodeType = codeModel.Code,
                CurrencyEndDate = codeModel.EndDate,
                CurrencyStartDate = codeModel.StartDate,
                LongDescription = codeModel.Description,
                ShortDescription = codeModel.ShortDescription
            };

            if (codeModel.CurrencyEnd != null)
            {
                model.CurrencyEnd = codeModel.CurrencyEnd.Value;
                model.ShowListCodeColumns = false; // To Hide Date columns.
                model.ShowIntColumns = true;
            }
            if (codeModel.CurrencyStart != null)
            {
                model.CurrencyStart = codeModel.CurrencyStart.Value;
                model.ShowListCodeColumns = false; // To Hide Date columns.
                model.ShowIntColumns = true;
            }

            return model;
        }


        /// <summary>
        /// Extension method for mapping IList<see cref="RelatedCodeViewModel"/> to IPageable<see cref="RelatedCodeModel"/>.
        /// </summary>
        /// <param name="sourceList">Collection of <see cref="RelatedCodeModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<RelatedCodeViewModel>"/>.</returns>
        public static IPageable<RelatedCodeViewModel> ToPageableRelatedCodeViewModel(this IList<RelatedCodeModel> sourceList)
        {
            if (sourceList == null)
            {
                return null;
            }

            var list = new Pageable<RelatedCodeViewModel>();

            foreach (var relatedCodeModel in sourceList)
            {
                var relatedCodeTypeViewModel = relatedCodeModel.ToRelatedCodeViewModel();
                if (relatedCodeTypeViewModel != null)
                {
                    list.Add(relatedCodeTypeViewModel);
                }
            }

            return list;
        }



        /// <summary>
        /// Extension method for mapping <see cref="RelatedCodeModel"/> to <see cref="RelatedCodeViewModel"/>.
        /// </summary>
        /// <param name="relatedCodeModel"><see cref="RelatedCodeModel"/> instance.</param>
        /// <returns>Instance of <see cref="RelatedCodeViewModel"/>.</returns>
        public static RelatedCodeViewModel ToRelatedCodeViewModel(this RelatedCodeModel relatedCodeModel)
        {
            if (relatedCodeModel == null)
            {
                return null;
            }

            return new RelatedCodeViewModel
            {
                CurrencyEnd = relatedCodeModel.EndDate,
                CurrencyStart = relatedCodeModel.StartDate,
                DominantCode = relatedCodeModel.DominantCode,
                DominantLongDescription = relatedCodeModel.DominantDescription,
                DominantShortDescription = relatedCodeModel.DominantShortDescription,
                //HasCurrencyEnd = relatedCodeModel.
                RelationshipTypeCode = relatedCodeModel.RelatedCode,
                RowPosition = relatedCodeModel.Position,
                SubCode = relatedCodeModel.SubordinateCode,
                SubLongDescription = relatedCodeModel.SubordinateDescription,
                SubShortDescription = relatedCodeModel.SubordinateShortDescription

            };
        }





        /// <summary>
        /// Extension method for mapping IList<see cref="PropertyModel"/> to IPageable<see cref="PropertyViewModel"/>.
        /// </summary>
        /// <param name="sourceList">Collection of <see cref="PropertyModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<PropertyViewModel>"/>.</returns>
        public static IPageable<PropertyViewModel> ToPageablePropertyViewModel(this IList<PropertyModel> sourceList)
        {
            if (sourceList == null)
            {
                return null;
            }

            var list = new Pageable<PropertyViewModel>();

            foreach (var propertyModel in sourceList)
            {
                var propertyViewModel = propertyModel.ToPropertyViewModel();
                if (propertyViewModel != null)
                {
                    list.Add(propertyViewModel);
                }
            }

            return list;
        }

        /// <summary>
        /// Extension method for mapping <see cref="PropertyModel"/> to <see cref="PropertyViewModel"/>.
        /// </summary>
        /// <param name="propertyModel"><see cref="PropertyModel"/> instance.</param>
        /// <returns>Instance of <see cref="PropertyViewModel"/>.</returns>
        public static PropertyViewModel ToPropertyViewModel(this PropertyModel propertyModel)
        {
            if (propertyModel == null)
            {
                return null;
            }

            return new PropertyViewModel
            {
                Code = propertyModel.Code,
                CodeType = propertyModel.CodeType,
                EndDate = propertyModel.EndDate,
                PropertyType = propertyModel.PropertyType,
                StartDate = propertyModel.StartDate,
                Value = propertyModel.Value

            };
        }




        /// <summary>
        /// Extension method for mapping IList<see cref="DeltaModel"/> to IPageable<see cref="DeltaViewModel"/>.
        /// </summary>
        /// <param name="sourceList">Collection of <see cref="DeltaModel"/>.</param>
        /// <returns>Pageable collection of <see cref="IPageable<DeltaViewModel>"/>.</returns>
        public static IPageable<DeltaViewModel> ToPageableDeltaViewModel(this IList<DeltaModel> sourceList)
        {
            if (sourceList == null)
            {
                return null;
            }

            var list = new Pageable<DeltaViewModel>();

            foreach (var deltaModel in sourceList)
            {
                var deltaViewModel = deltaModel.ToDeltaViewModel();
                if (deltaViewModel != null)
                {
                    list.Add(deltaViewModel);
                }
            }

            return list;
        }

        /// <summary>
        /// Extension method for mapping <see cref="DeltaModel"/> to <see cref="DeltaViewModel"/>.
        /// </summary>
        /// <param name="deltaModel"><see cref="DeltaModel"/> instance.</param>
        /// <returns>Instance of <see cref="DeltaViewModel"/>.</returns>
        public static DeltaViewModel ToDeltaViewModel(this DeltaModel deltaModel)
        {
            if (deltaModel == null)
            {
                return null;
            }

            return new DeltaViewModel
            {
                Code = deltaModel.Code,
                CurrencyEnd = deltaModel.CurrencyEndDate != null ? deltaModel.CurrencyEndDate.Value.ToString() : deltaModel.CurrencyEnd,
                CurrencyStart = deltaModel.CurrencyStartDate != null ? deltaModel.CurrencyStartDate.Value.ToString() : deltaModel.CurrencyStart,
                LongDescription = deltaModel.LongDescription,
                ShortDescription = deltaModel.ShortDescription

            };
        }


        #endregion
    }
}