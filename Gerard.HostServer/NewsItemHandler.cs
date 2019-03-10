using Shuttle.Esb;
using System;
using Gerard.Messages;
using NLog;

namespace Gerard.HostServer
{
	public class NewsItemHandler : IMessageHandler<NewsArticleCommand>
	{
		public readonly ArticleExaminer Examiner;
		private readonly ITransactionManager Manager;

		public Logger Logger { get; set; }

		public void ProcessMessage(
			IHandlerContext<NewsArticleCommand> context)
		{
			try
			{
				var msg = $@"[ARTICLE RECEIVED] : article = '{
					context.Message.ArticleText}'";
				WriteTraceLog(msg);
				Console.WriteLine();
				Console.WriteLine(msg);
				Console.WriteLine();
				var examinationEvent = new ExaminationEvent();
				examinationEvent = Examiner.ExamineArticle(context.Message);
				WriteTraceLog($"{examinationEvent}");

				if (examinationEvent.RecommendedAction != Constants.Result.Ignore)
				{
					WriteInfoLog($"Processing {examinationEvent.RecommendedAction}");
					Manager.ProcessEvent(examinationEvent);
					//TODO: raise the event so subscribers might do something
					//context.Publish(examinationEvent);
				}
				else
					WriteTraceLog($@"{
						examinationEvent
						} ignored - {
						context.Message.ArticleText
						}");
			}
			catch (Exception ex)
			{
				WriteErrorLog($"Article caused :- {ex.Message}", ex);
				throw;
			}
		}

		public NewsItemHandler()
		{
			//Logger = LogManager.GetCurrentClassLogger();
			var assemblyVersion = "2.190307.1";
			WriteInfoLog($@"NewsItemHandler v {
				assemblyVersion
				} =================================================");
			var lib = Utility.TflWs;
			var tfl = new TflService(lib, Logger);
			WriteTraceLog("TFL service created");
			Examiner = new ArticleExaminer(tfl, new NLogAdaptor());
			Manager = new TransactionManager(tfl);
			var msg = "NewsItemHandler instantiated ok";
			WriteTraceLog(msg);
			Console.WriteLine(msg);
		}

		public void WriteInfoLog(string message)
		{
			//Logger.Info(message);
		}

		public void WriteErrorLog(string message, Exception ex)
		{

			//Logger.ErrorException(message, ex);
		}

		public void WriteTraceLog(string message)
		{
			//Logger.Trace(message);
		}
	}
}
