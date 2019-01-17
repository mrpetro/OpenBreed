using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public interface IUnitOfWork
    {
        string Name { get; }
        IRepository<T> GetRepository<T>() where T : IEntry;
        void Save();
        IEnumerable<IRepository> Repositories{ get; }
    }
}
