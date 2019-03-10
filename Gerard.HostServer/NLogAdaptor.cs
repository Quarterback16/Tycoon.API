using NLog;

namespace Gerard.HostServer
{
	public class NLogAdaptor : ILog
	{
		public Logger Logger { get; set; }

		public NLogAdaptor()
		{
			Logger = LogManager.GetCurrentClassLogger();
		}

		public NLogAdaptor(Logger logger)
		{
			Logger = logger;
		}

		public void Info(string message)
		{
			Logger.Info(message);
		}

		public void Debug(string message)
		{
			Logger.Debug(message);
		}

		public void Error(string message)
		{
			Logger.Error(message);
		}

		public void Warning(string message)
		{
			Logger.Warn(message);
		}

		public void Trace(string message)
		{
			Logger.Trace(message);
		}
	}
}
