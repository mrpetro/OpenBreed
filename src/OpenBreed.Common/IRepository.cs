using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public interface IRepository
    {
        #region Public Properties

        IEnumerable<IEntity> Entries { get; }
        IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties
    }

    public interface IRepository<T> : IRepository where T : IEntity
    {

        #region Public Methods

        void Add(T entry);
        T GetById(long id);

        T GetByName(string name);

        T GetNextTo(T entry);
        T GetPrevTo(T entry);
        void Remove(T entry);
        void Update(T entry);

        #endregion Public Methods

    }
}
