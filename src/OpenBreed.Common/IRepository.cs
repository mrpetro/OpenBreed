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

    public interface IRepository<T> : IRepository where T : IEntity
    {

        #region Public Properties

        IEnumerable<IEntity> Entries { get; }
        IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        void Add(T entity);

        T GetById(Int64 id);
        T GetByName(string name);
        void Remove(T entity);
        void Update(T entity);

        #endregion Public Methods
    }
}
