using System.Collections.Generic;

namespace CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces
{
   public interface ISeasonRepository : IBaseRepository<SeasonRM>
   {
      IEnumerable<SeasonRM> GetAll();
   }
}
