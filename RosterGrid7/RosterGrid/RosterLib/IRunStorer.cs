﻿using System;

namespace RosterLib
{
	public interface IRunStorer
	{
		void StoreRun( string stepName, TimeSpan ts );
	}
}