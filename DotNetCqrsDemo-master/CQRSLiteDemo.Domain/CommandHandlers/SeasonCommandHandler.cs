using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSLiteDemo.Domain.Commands;
using CQRSLiteDemo.Domain.WriteModel;

namespace CQRSLiteDemo.Domain.CommandHandlers
{
   public class SeasonCommandHandler : ICommandHandler<CreateSeasonCommand>
   {
      private readonly ISession _session;

      public SeasonCommandHandler( ISession session )
      {
         _session = session;
      }

      public void Handle(CreateSeasonCommand command)
      {
         Season season = new Season( command.Id, command.Year );
         _session.Add( season );
         _session.Commit();

      }
   }
}
