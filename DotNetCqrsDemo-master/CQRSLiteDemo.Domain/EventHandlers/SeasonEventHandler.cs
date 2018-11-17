using System;
using AutoMapper;
using CQRSlite.Events;
using CQRSLiteDemo.Domain.Events.Seasons;
using CQRSLiteDemo.Domain.ReadModel;
using CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces;

namespace CQRSLiteDemo.Domain.EventHandlers
{
   public class SeasonEventHandler : IEventHandler<SeasonCreatedEvent>
   {
      private readonly IMapper _mapper;
      private readonly ISeasonRepository _seasonRepo;

      public SeasonEventHandler(
           IMapper mapper,
           ISeasonRepository repo )
      {
         _mapper = mapper;
         _seasonRepo = repo;
      }

      public void Handle( SeasonCreatedEvent message )
      {
         SeasonRM season = _mapper.Map<SeasonRM>( message );
         _seasonRepo.Save( season );
      }
   }
}
