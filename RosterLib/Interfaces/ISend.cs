using Gerard.Messages;

namespace RosterLib.Interfaces
{
	public interface ISend
	{
		void Send(ICommand command);
	}
}
