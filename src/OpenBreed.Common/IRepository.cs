using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository where T : EntityBase
    {
        T GetById(Int64 id);
        T GetByName(string name);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
