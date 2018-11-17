using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	public class GetGamePrediction
	{
		public GetGamePrediction( PlayerGameProjectionMessage input )
		{
			Process( input );
		}

		private void Process( PlayerGameProjectionMessage input )
		{
			input.Prediction = input.Game.GetPrediction("unit");
		}
	}
}
