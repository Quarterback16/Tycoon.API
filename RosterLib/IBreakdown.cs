using System;

namespace RosterLib
{
	public interface IBreakdown
	{
		void AddLine(string breakdownKey, string line);

		void Dump(
			string breakdownKey, 
			string outputFileName,
			decimal avg);

		void Dump(
			string breakdownKey,
			string outputFileName);
	}
}
