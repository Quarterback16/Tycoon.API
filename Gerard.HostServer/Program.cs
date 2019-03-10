using Shuttle.Core.ServiceHost;

namespace Gerard.HostServer
{
	public class Program
	{
		static void Main()
		{
			ServiceHost.Run<TflHost>();
		}
	}
}
