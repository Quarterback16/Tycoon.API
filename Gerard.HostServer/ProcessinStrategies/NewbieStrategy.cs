namespace Gerard.HostServer
{
	internal class NewbieStrategy : IProcessTransaction
	{
		private ITflService tflService;

		public NewbieStrategy(ITflService tflService)
		{
			this.tflService = tflService;
		}

		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			//TODO:  Figure out how to handle these
			logger.Info($"NEWBIE {transactionEvent}");
			return true;
		}
	}
}