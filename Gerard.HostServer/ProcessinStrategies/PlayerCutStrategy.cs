namespace Gerard.HostServer
{
	internal class PlayerCutStrategy : IProcessTransaction
	{
		private readonly ITflService TflService;

		public PlayerCutStrategy(ITflService tflService)
		{
			TflService = tflService;
		}


		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			var p = TflService.GetNflPlayer(
				transactionEvent.PlayerId);
			var eventDate = transactionEvent.EventDate
							 ?? transactionEvent.ExaminationDateTime;

			if (TflService.IsSameDay(p, eventDate))
			{
				logger.Info($@"Cut for {
					p.PlayerName
					} ignored: Same day contract rule");
				return false;
			}

			if (string.IsNullOrEmpty(p.TeamCode))
			{
				if (logger != null)
					logger.Info($"{p.PlayerName} is already a free agent");
				return true;
			}

			var result = TflService.EndContract(
				p,
				transactionEvent.EventDate ?? transactionEvent.ExaminationDateTime,
				isRetirement: false);
			return result;
		}
	}
}