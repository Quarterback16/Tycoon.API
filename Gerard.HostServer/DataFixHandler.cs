using System;
using Gerard.Messages;
using Shuttle.Esb;

namespace Gerard.HostServer
{
	public class DataFixHandler
		: IMessageHandler<DataFixCommand>
	{
		public void ProcessMessage(
			IHandlerContext<DataFixCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine(
				$"[DATAFIX] : {context.Message}");
			Console.WriteLine();

			//TODO: Railway Oriented code to perform fix
			// 1. get the player
			// 2. check if data is missing
			// 3. Get Shuttle data
			// 4. Update Tfl database
		}
	}
}
