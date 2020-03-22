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
			if (Logger == null)
				Logger = LogManager.GetCurrentClassLogger();

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
				Logger.Info(
					$"[DATAFIX] : {context.Message}");

				//  With the demise of Stattleship this will no longer work
				//Fixer.ApplyFix(context.Message);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				throw;
			}
		}
	}
}
