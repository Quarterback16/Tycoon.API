using System;
using System.Collections.Generic;
using System.Web.Http;
using RosterLib;
using TflWebApi.Repositories;

namespace TflWebApi.Controllers
{
    public class PlayerController : ApiController
    {
       public IEnumerable<NFLPlayer> GetPlayers()
       {
          var repo = new PlayerRepository();
          return repo.GetPlayers();
       }
    }
}
