using CSharpFunctionalExtensions;

namespace Tycoon.Logic.Interfaces
{
	public interface ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		Result Handle(TCommand command);
	}
}
