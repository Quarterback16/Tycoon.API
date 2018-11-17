using CSharpFunctionalExtensions;
using Tycoon.Logic.Interfaces;

namespace Tycoon.Logic.Services
{
	internal sealed class DataCheckCommandHandler : ICommandHandler<DataCheckCommand>
	{
		public Result Handle(DataCheckCommand command)
		{
			//TODO:  Call stattle ship and compare data, and update TFL if different

			return Result.Ok();
		}
	}
}
