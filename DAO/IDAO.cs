using System.Collections.Generic;

namespace DAO
{
    public interface IDAO<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetBy(object id);
        bool Insert(T t);
        bool Update(T t);
        bool Delete(object id);

    }
}
