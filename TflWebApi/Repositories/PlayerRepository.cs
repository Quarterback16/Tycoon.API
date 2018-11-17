using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RosterLib;

namespace TflWebApi.Repositories
{
   public class PlayerRepository : TflRepository
   {
      public List<NFLPlayer> GetPlayers()
      {
         var ds = DataLibrarian.GetReturners();
         var dt = ds.Tables[0];
         var pList = ( from DataRow dr in dt.Rows select new NFLPlayer( dr ) ).ToList();
         return pList;
      }
   }
}