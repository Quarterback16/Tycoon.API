 
  
 
 
 
  
  

/*
Generated code. Do not edit manually.
Admin Mapper.cs
*/
using Employment.Web.Mvc.Area.Admin.ViewModels.User;
using Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup; 
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using System.Security.Claims; 
using System; 
using Employment.Web.Mvc.Infrastructure.Models; 
using Employment.Web.Mvc.Infrastructure.Interfaces;

    

namespace Employment.Web.Mvc.Area.Admin.Mappers
{

    /// <summary>
    /// Represents a mapper for Admin area that is used to map between View Models and Domain Models.
    /// </summary>
    public partial class AdminMapper
    {
        public static CodeTypeViewModel MapToCodeTypeViewModel(CodeTypeModel source)
		 {
			var target = new CodeTypeViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(CodeTypeModel source, CodeTypeViewModel target)
		{
			target.CodeType = source.CodeType;
			target.ShortDescription = source.ShortDescription;
			target.LongDescription = source.LongDescription;

		}
    }
}