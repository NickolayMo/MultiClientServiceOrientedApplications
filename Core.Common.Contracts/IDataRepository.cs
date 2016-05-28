using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{

    public interface IDataRepository
    {
    }
    public interface IDataRepository<T>:IDataRepository 
        where T: class ,IIdentifiableEntity, new()
    {
        T Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        T Update(T entity);
        T Get(int id);
        IEnumerable<T> Get();
    }
}
