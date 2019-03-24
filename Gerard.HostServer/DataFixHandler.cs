using System;
using Gerard.Messages;
using NLog;
using Shuttle.Esb;

namespace Gerard.HostServer
{
	public class DataFixHandler
		: IMessageHandler<DataFixCommand>
	{
		public readonly DataFixer Fixer;

		public Logger Logger { get; set; }

		public DataFixHandler()
		{
			var lib = new DataLibrarian(
				nflConnection: Utility.NflConnectionString(),
				tflConnection: Utility.TflConnectionString(),
				ctlConnection: Utility.CtlConnectionString(),
				logger: new NLogAdaptor());
			var tfl = new TflService(lib, Logger);
			Fixer = new DataFixer(tfl, new NLogAdaptor());
		}

		public void ProcessMessage(
			IHandlerContext<DataFixCommand> context)
		{
			try
			{
				Console.WriteLine();
				Console.WriteLine(
					$"[DATAFIX] : {context.Message}");
				Console.WriteLine();

				Fixer.ApplyFix(context.Message);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				throw;
			}
		}
	}
}
