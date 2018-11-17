using NServiceBus;

namespace UserService.Messages.Commands
{
   public class CreateNewUserCmd : ICommand
   {
      public string EmailAddress { get; set; }
      public string Name { get; set; }
   }
}
