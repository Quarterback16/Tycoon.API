using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	/// It is a CheckList repository
	/// </summary>
	public interface ICheckListRepository
	{
		/// <summary>
		/// Gets the checklist by its review Id
		/// </summary>
		/// <param name="id">The check list review Id.</param>
		/// <returns></returns>
		CheckList GetById( int id );

		/// <summary>
		/// Inserts a  new checklist
		/// </summary>
		/// <param name="viewModel">The checklist view model.</param>
	    void Save(CheckList viewModel);

	}
}