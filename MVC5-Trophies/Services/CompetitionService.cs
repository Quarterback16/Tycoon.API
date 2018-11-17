using MVC5_Trophies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5_Trophies.Services
{
   public class CompetitionService
   {
      public List<Competition> GetCompetitions()
      {
         var list = new List<Competition>();
         list.Add(new Competition { Name = "World Cup", Frequency = 4, InauguralYear = 1920 });
         list.Add(new Competition { Name = "Serie A", Frequency = 1, InauguralYear = 1900 });
         return list;
      }
   }
}