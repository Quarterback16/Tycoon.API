using System.Web.Http;
using TrophyWebApi.Data;
using TrophyWebApi.Models;

namespace TrophyWebApi.Controllers
{
   public class BaseApiController : ApiController
   {
      private ITrophyRepository _repo;
      private ModelFactory _modelFactory;

      public BaseApiController()
      {
      }

      //  Defer creation to just before we need it
      protected ITrophyRepository TrophyRepository
      {
         get { return _repo ?? ( _repo = new TrophyRepository() ); }
      }

      protected ModelFactory ModelFactory
      {
         get { return _modelFactory ?? ( _modelFactory = new ModelFactory() ); }
      }
   }
}