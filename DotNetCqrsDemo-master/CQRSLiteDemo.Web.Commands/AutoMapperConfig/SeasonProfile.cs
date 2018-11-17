using System;
using AutoMapper;
using CQRSLiteDemo.Domain.ReadModel;
using CQRSLiteDemo.Web.Commands.Requests.Seasons;
using CQRSLiteDemo.Domain.Events.Seasons;
using CQRSLiteDemo.Domain.Commands;

namespace CQRSLiteDemo.Web.Commands.AutoMapperConfig
{
   public class SeasonProfile : Profile
   {
      public SeasonProfile()
      {
         CreateMap<CreateSeasonRequest, CreateSeasonCommand>()
             .ConstructUsing( x => new CreateSeasonCommand( 
                Guid.NewGuid(), x.Year ) );

         CreateMap<SeasonCreatedEvent, SeasonRM>()
             .ForMember( dest => dest.AggregateID, opt => opt.MapFrom( src => src.Id ) );

      }
   }
}