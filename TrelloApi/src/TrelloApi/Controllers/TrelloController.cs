using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrelloApi.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TrelloApi.Controllers
{
   [Route("api/[controller]")]
   public class TrelloController : Controller
    {
      //static readonly List<Board> _boards = new List<Board>()
      //{
      //   new Board { Id = 1, Name = "NFL" },
      //   new Board { Id = 2, Name = "Civ V" }
      //};

      private readonly IBoardRepository _repository;

      public TrelloController( IBoardRepository repository )
      {
         _repository = repository;
      }

      //[HttpGet()]
      public IEnumerable<Board> GetAll()
      {
         //  get this from a proper repository service
         return _repository.AllItems;
      }

      [HttpGet("{id:int}", Name = "GetByIdRoute")]
      public IActionResult GetById(int id)
      {
         var item = _repository.GetById( id );
         if (item == null)
         {
            return HttpNotFound();
         }

         return new ObjectResult(item);
      }

      [HttpPost]
      public void CreateBoard([FromBody] Board item)
      {
         if (!ModelState.IsValid)
         {
            Context.Response.StatusCode = 400;
         }
         else
         {
            _repository.Add(item);

            string url = Url.RouteUrl("GetByIdRoute", new { id = item.Id },
                Request.Scheme, Request.Host.ToUriComponent());

            Context.Response.StatusCode = 201;
            Context.Response.Headers["Location"] = url;
         }
      }

      [HttpDelete("{id}")]
      public IActionResult DeleteBoard(int id)
      {
         if (_repository.TryDelete(id))
         {
            return new HttpStatusCodeResult(204); // 201 No Content
         }
         else
         {
            return HttpNotFound();
         }
      }
   }
}
