﻿namespace Hearthstone_Deck_Tracker.HearthStats.API
{
	public class PostResult
	{
		public bool Retry;
		public bool Success;

		private PostResult(bool success, bool retry)
		{
			Success = success;
			Retry = retry;
		}

		public static PostResult WasSuccess
		{
			get { return new PostResult(true, false); }
		}

		public static PostResult Failed
		{
			get { return new PostResult(false, false); }
		}

		public static PostResult CanRetry
		{
			get { return new PostResult(false, true); }
		}
	}
}