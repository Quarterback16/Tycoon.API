using System.Collections.Generic;

namespace Gerard.HostServer
{
	public class TransactionManager : ITransactionManager
	{
		public readonly ITflService TflService;
		private readonly ILog Log;
		public int ProcessCount { get; set; }

		private readonly IDictionary<string, IProcessTransaction> ProcessingStrategies;

		public TransactionManager(ITflService tflService)
		{
			TflService = tflService;
			Log = new NLogAdaptor();
			ProcessingStrategies = new Dictionary<string, IProcessTransaction>
			{
				{ "SIGN", new PlayerSigningStrategy(TflService) },
				{ "TRADE", new PlayerTradingStrategy(TflService) },
				{ "CUT", new PlayerCutStrategy(TflService) },
				{ "WAIVER", new PlayerCutStrategy(TflService) },
				{ "RETIRED", new PlayerRetirementStrategy(TflService) },
				{ "NEWBIE", new NewbieStrategy(TflService) },
				{ "INJURY", new PlayerInjuredStrategy(TflService) }
			};
			Log.Trace($@"TransactionManager instantiated with {
				ProcessingStrategies.Count
				} strategies");
		}

		public bool ProcessEvent(ExaminationEvent transactionEvent)
		{
			Log.Trace($"Processing: {transactionEvent}");
			if (!ProcessingStrategies[transactionEvent.RecommendedAction]
				.Process(transactionEvent, Log))
				return true;

			Log.Trace($"Handled: {transactionEvent}");
			ProcessCount++;
			return true;
		}
	}
}