using System.Web;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	///   The interface for getting the virtual path.
	///   Used when setting AJAX URLS for jqQrid data methods
	/// </summary>
    public interface IVirtualPathService
    {
        string ToAbsolute(string virtualPath);
    }

	 /// <summary>
	 ///   The implementation of the Virtual path service
	 /// </summary>
    public class VirtualPathService : IVirtualPathService
    {
		 /// <summary>
		 ///    returns the virtual path for a URL.
		 /// </summary>
		 /// <param name="virtualPath">The virtual path.</param>
		 /// <returns></returns>
        public string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }
    }
}