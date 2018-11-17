using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrustructure;

namespace ProgramAssuranceTool.Infrastructure.Ioc.Unity
{
	/// <summary>
	/// Defines a Unity application.
	/// </summary>
	public class UnityMvcApplication : MvcApplication
	{
		/// <summary>
		/// Creates the bootstrapper.
		/// </summary>
		/// <returns></returns>
		protected override IBootstrapper CreateBootstrapper()
		{
			return new UnityBootstrapper();
		}
	}
}