using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC5_Trophies.Models;

namespace MVC5_Trophies.ViewModels
{
   public class CompetitionIndexViewModel
   {
      public IEnumerable<Competition> Competitions { get; set; }
   }
}