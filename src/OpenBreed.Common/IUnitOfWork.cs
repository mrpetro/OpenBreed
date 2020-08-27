using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface.Items;
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
        IRepository GetRepository(string name);
        IRepository GetRepository(Type type);

        void Save();
        IEnumerable<IRepository> Repositories{ get; }
    }
}
