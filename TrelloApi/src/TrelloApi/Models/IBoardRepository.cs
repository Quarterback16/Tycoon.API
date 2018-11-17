using System;
using System.Collections.Generic;

namespace TrelloApi.Models
{
    public interface IBoardRepository
    {
       IEnumerable<Board> AllItems { get; }

       Board GetById(int id);

      void Add(Board board);

      bool TryDelete(int id);
    }
}