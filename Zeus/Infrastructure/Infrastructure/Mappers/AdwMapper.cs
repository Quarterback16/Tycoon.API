
//using Employment.Esc.Adw.Contracts.DataContracts;
//using Employment.Web.Mvc.Infrastructure.Interfaces;
//using Employment.Web.Mvc.Infrastructure.Models;

//namespace Employment.Web.Mvc.Infrastructure.Mappers
//{
//    /// <summary>
//    /// Represents a mapper that is used to map between the Adw Domain Models and the Adw WCF DataContracts.
//    /// </summary>
//    public class AdwMapper 
//    {
//        /// <summary>
//        /// Map between Adw Domain Models and Adw WCF DataContracts.
//        /// </summary>
//        /// <param name="mapper">Mapper configuration.</param>
//        public void Map(IProfileExpression mapper)
//        {
//            mapper.CreateMap<RelatedCodeModel, CodeModel>()
//                .ForMember(dest => dest.Code, options => options.MapFrom(src => src.Dominant ? src.SubordinateCode : src.DominantCode))
//                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Dominant ? src.SubordinateDescription : src.DominantDescription))
//                .ForMember(dest => dest.ShortDescription, options => options.MapFrom(src => src.Dominant ? src.SubordinateShortDescription : src.DominantShortDescription));

//            mapper.CreateMap<User, UserModel>()
//                .ForMember(dest => dest.FirstName, options => options.MapFrom(src => src.FirstName))
//                .ForMember(dest => dest.LastName, options => options.MapFrom(src => src.LastName)) 
//                .ForMember(dest => dest.FullName, options => options.Ignore())
//                .ForMember(dest => dest.UserID, options => options.MapFrom(src => src.UserId))
//                .ForMember(dest => dest.OrganisationCode, options => options.MapFrom(src => src.Organisation));
//        }
//    }
//}
