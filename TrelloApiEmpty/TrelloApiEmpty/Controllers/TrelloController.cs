using System;
using System.Collections.Generic;
using System.Web.Http;
using TrelloApiEmpty.Models;
using TrelloNet;

namespace TrelloApiEmpty.Controllers
{
//   [Authorize]  with this attribute you will prevent anonymous access
   [RoutePrefix( "api/Trello" )]
   public class TrelloController : ApiController
   {
      private readonly IBoardRepository _repository;

      public TrelloController()
      {
         _repository = new BoardRepository();
      }

      public IEnumerable<TrelloNet.Board> GetAll()
      {
         ITrello trello = new Trello( "84ffe047543614a43b06bb341dc3e25f" );
         trello.Authorize( "81fd4d07eb1a4317a05e4cdfaa6a5e1642ed8ac726bf22a8c21194e805952064" );
         var me = trello.Members.Me();
         Console.WriteLine( me.FullName );
         var allMyBoards = trello.Boards.ForMember( me );
         return allMyBoards;
         //return _repository.AllItems;
      }
   }
}