namespace ProgramAssuranceTool.Interfaces
{
	public interface IControllerDependencies
	{
		IVirtualPathService VirtualPathService { get; }
		IPatService PatService { get; }
	}

	public class ControllerDependencies : IControllerDependencies
	{
		public ControllerDependencies( IVirtualPathService virtualPathService,
		                               IPatService patService
			)
		{
			VirtualPathService = virtualPathService;
			PatService = patService;
		}

		public IVirtualPathService VirtualPathService { get; private set; }

		public IPatService PatService { get; private set; }
	}
}