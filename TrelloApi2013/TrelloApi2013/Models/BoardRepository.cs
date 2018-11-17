using System.Collections.Generic;
using System.Linq;

namespace TrelloApi2013.Models
{
   public class BoardRepository : IBoardRepository
   {
     readonly List<Board> _boards = new List<Board>();

      public IEnumerable<Board> AllItems
      {
         get
         {
            if (_boards.Count != 0) return _boards;
            _boards.Add(new Board { Id = 1, Name = "NFL" });
            _boards.Add(new Board { Id = 2, Name = "Civ V" });
            return _boards;
         }
      }

      public Board GetById(int id)
      {
         return _boards.FirstOrDefault(x => x.Id == id);
      }

      public void Add(Board item)
      {
         item.Id = 1 + _boards.Max(x => (int?)x.Id) ?? 0;
         _boards.Add(item);
      }

      public bool TryDelete(int id)
      {
         var item = GetById(id);
         if (item == null)
         {
            return false;
         }
         _boards.Remove(item);
         return true;
      }
   }

}