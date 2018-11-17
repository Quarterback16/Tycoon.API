using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrophyWebApi.Models
{
   public class CompetitionModel
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string Frequency { get; set; }

      public int InauguralYear { get; set; }

      public virtual ICollection<WinnerModel> Winners { get; set; }

      public override string ToString()
      {
         return string.Format( "{0}", Name );
      }

   }
}