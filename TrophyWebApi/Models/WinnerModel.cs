using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrophyWebApi.Models
{
   public class WinnerModel
   {
      public string Name { get; set; }
      public string When { get; set; }

      public override string ToString()
      {
         return string.Format( "{0}:{1}", When, Name );
      }
   }
}