using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrelloApiEmpty.Models
{
   interface IBoardRepository
   {
      IEnumerable<Board> AllItems { get; }

      Board GetById( int id );

      void Add( Board board );

      bool TryDelete( int id );
   }
}
