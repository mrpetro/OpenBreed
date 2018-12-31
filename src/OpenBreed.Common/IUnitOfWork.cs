using OpenBreed.Common.Formats;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : IEntity;
        void Save();
        IEnumerable<IRepository> Repositories{ get; }
    }
}
