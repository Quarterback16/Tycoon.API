using Tycoon.Logic.Interfaces;

namespace Tycoon.Logic.Services
{
	public sealed class DataCheckCommand : ICommand
	{
		public string FirstName { get; }
		public string Surname { get; }
		public string TeamCode { get; }

		public DataCheckCommand(
			string firstName, 
			string surname, 
			string teamCode)
		{
			FirstName = firstName;
			Surname = surname;
			TeamCode = teamCode;
		}
	}
}
