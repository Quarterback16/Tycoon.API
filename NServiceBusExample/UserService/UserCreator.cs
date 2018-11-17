using NServiceBus;
using NServiceBus.Logging;
using UserService.Messages.Commands;

namespace UserService
{
   public class UserCreator : IHandleMessages<CreateNewUserCmd>
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(UserCreator));

      public void Handle(CreateNewUserCmd message)
      {
         log.InfoFormat("Creating user '{0}' with email '{1}'",
            message.Name,
            message.EmailAddress);
      }
   }
}