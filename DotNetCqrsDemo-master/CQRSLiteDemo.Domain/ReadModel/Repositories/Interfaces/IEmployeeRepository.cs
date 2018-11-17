using System.Collections.Generic;

namespace CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces
{
   public interface IEmployeeRepository : IBaseRepository<EmployeeRM>
    {
        IEnumerable<EmployeeRM> GetAll();
    }
}
