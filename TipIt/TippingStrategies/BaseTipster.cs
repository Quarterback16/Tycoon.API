using System;
using System.Collections.Generic;
using System.Text;
using TipIt.Implementations;
using TipIt.Models;

namespace TipIt.TippingStrategies
{
	public class BaseTipster
	{
		public List<PredictedResult> Predictions { get; set; }

		public readonly TippingContext Context;

		public BaseTipster(
			TippingContext context)
		{
			Context = context;
			Predictions = new List<PredictedResult>();
		}

		protected string Output()
		{
			var sb = new StringBuilder();
			foreach (var prediction in Predictions)
			{
				sb.AppendLine(prediction.Tip());
				Console.WriteLine(prediction.Tip());
			}
			return sb.ToString();
		}
	}
}
