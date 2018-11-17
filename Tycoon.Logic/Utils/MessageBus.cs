using System;
using CSharpFunctionalExtensions;
using Tycoon.Logic.Interfaces;

namespace Logic.Utils
{
	public sealed class MessageBus
	{
		private readonly IServiceProvider _provider;

		public MessageBus(IServiceProvider provider)
		{
			_provider = provider;
		}

		public Result Dispatch(ICommand command)
		{
			Type type = typeof(ICommandHandler<>);
			Type[] typeArgs = { command.GetType() };
			Type handlerType = type.MakeGenericType(typeArgs);

			dynamic handler = _provider.GetService(handlerType);
			Result result = handler.Handle((dynamic)command);

			return result;
		}
	}
}
