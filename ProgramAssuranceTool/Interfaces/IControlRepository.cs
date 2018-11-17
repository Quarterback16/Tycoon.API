using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IControlRepository
	{
		/// <summary>
		/// Updates the Control record
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Update( PatControl entity );
		/// <summary>
		/// Gets the Contract record
		/// </summary>
		/// <returns></returns>
		List<PatControl> GetAll();

		/// <summary>
		/// Adds the Control record.
		/// </summary>
		void Add();
	}
}
