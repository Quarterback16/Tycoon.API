using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	public class BalanceReportJob : BaseReport
	{
		public BalanceReportJob()
		{
		}

		/// <summary>
		///   Report can be run but once in the preseason
		/// </summary>
		/// <returns></returns>
		public override bool OkToExecute()
		{
			return true;
		}
	}
}
