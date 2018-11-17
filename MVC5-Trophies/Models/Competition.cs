using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5_Trophies.Models
{
   public class Competition
   {
      public string Name { get; set; }

      public int Frequency { get; set; }

      public int InauguralYear { get; set; }

      public int? FinalYear { get; set; }

      public override string ToString()
      {
         return string.Format( "{0}", Name );
      }

   }
}