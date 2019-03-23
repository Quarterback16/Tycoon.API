namespace Gerard.HostServer
{
	public sealed class PlayerQuery : IQuery<Player>
	{
		public string TeamCode { get; }
		public string FirstName { get; }
		public string LastName { get; }

		public PlayerQuery(
			string teamCode,
			string firstName,
			string lastName)
		{
			TeamCode = teamCode;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
