using AutoMapper;
using CQRSlite.Commands;
using CQRSLiteDemo.Domain.Commands;
using CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces;
using CQRSLiteDemo.Web.Commands.Requests.Seasons;
using System.Web.Http;

namespace CQRSLiteDemo.Web.Commands.Controllers
{
   [RoutePrefix( "seasons" )]
   public class SeasonController : ApiController
   {
      private readonly IMapper _mapper;
      private readonly ICommandSender _commandSender;
      private readonly ISeasonRepository _seasonRepo;

      public SeasonController(
         ICommandSender commandSender, 
         IMapper mapper, 
         ISeasonRepository seasonRepo )
      {
         _commandSender = commandSender;
         _mapper = mapper;
         _seasonRepo = seasonRepo;
      }

      [HttpPost]
      [Route( "create" )]
      public IHttpActionResult Create( CreateSeasonRequest request )
      {
         var command = _mapper.Map<CreateSeasonCommand>( request );
         _commandSender.Send( command );

         return Ok();
      }
   }
}
