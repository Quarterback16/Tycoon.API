using System;

namespace Gerard.HostServer
{
	public class PlayerTradingStrategy : IProcessTransaction
	{
		private readonly ITflService TflService;

		public PlayerTradingStrategy(ITflService tflService)
		{
			TflService = tflService;
		}

		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			var p = TflService.GetNflPlayer(transactionEvent.PlayerId);

			if (!string.IsNullOrEmpty(p.TeamCode))
				TflService.EndContract(
					p,
					transactionEvent.EventDate
						?? transactionEvent.ExaminationDateTime,
					isRetirement: false);

			return transactionEvent.EventDate != null
				&& (TflService.RecordSigning(
						p,
						transactionEvent.TeamId,
						(DateTime)transactionEvent.EventDate,
						how: "T"));
		}
	}
}