﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	public interface IHtmlReport
	{
		string FileOut { get; set; }

		void Render();
	}
}
