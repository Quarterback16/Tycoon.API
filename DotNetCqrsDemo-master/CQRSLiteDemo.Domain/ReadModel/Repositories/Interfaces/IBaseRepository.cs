using System.Collections.Generic;

namespace CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces
{
   public interface IBaseRepository<T>
    {
        T GetByID(int id);
        List<T> GetMultiple(List<int> ids);
        bool Exists(int id);
        void Save(T item);
    }
}
