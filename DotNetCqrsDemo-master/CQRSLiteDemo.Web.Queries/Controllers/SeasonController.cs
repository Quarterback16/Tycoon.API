using System.Web.Http;
using CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces;

namespace CQRSLiteDemo.Web.Queries.Controllers
{
   [RoutePrefix( "seasons" )]
   public class SeasonController : ApiController
   {
      private readonly ISeasonRepository _seasonRepo;

      public SeasonController( ISeasonRepository seasonRepo )
      {
         _seasonRepo = seasonRepo;
      }

      [HttpGet]
      [Route( "all" )]
      public IHttpActionResult GetAll()
      {
         var seasons = _seasonRepo.GetAll();
         return Ok( seasons );
      }
   }
}
