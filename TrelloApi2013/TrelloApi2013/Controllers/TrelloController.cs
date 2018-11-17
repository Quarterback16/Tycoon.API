using System.Collections.Generic;
using System.Web.Http;
using TrelloApi2013.Models;

namespace TrelloApi2013.Controllers
{
   [Authorize]
   [RoutePrefix( "api/Trello" )]
   public class TrelloController : ApiController
   {
      private readonly IBoardRepository _repository;

      public TrelloController()
      {
         _repository = new BoardRepository();
      }

      public IEnumerable<Board> GetAll()
      {
         
         return _repository.AllItems;
      }

   }
}