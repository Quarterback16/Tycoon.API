namespace Gerard.HostServer
{
	public static class Constants
	{
		public static class Result
		{
			public const string Ignore = "IGNORE";
			public const string Cut = "CUT";
			public const string Waiver = "WAIVER";
			public const string Retired = "RETIRED";
		}

		#region Roles

		public const string K_ROLE_STARTER = "S";
		public const string K_ROLE_BACKUP = "B";
		public const string K_ROLE_RESERVE = "R";
		public const string K_ROLE_DEEP_RESERVE = "D";
		public const string K_ROLE_INJURED = "I";
		public const string K_ROLE_SUSPENDED = "X";
		public const string K_ROLE_HOLDOUT = "H";

		#endregion Roles
	}
}
