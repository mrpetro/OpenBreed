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

        IEnumerable<IEntry> Entries { get; }
        IUnitOfWork UnitOfWork { get; }
        string Name { get; }

        IEntry Find(string name);
        IEntry New(string newId);

        #endregion Public Properties
    }

    public interface IRepository<T> : IRepository where T : IEntry
    {
        #region Public Methods

        void Add(T entry);

        T GetById(string id);

        T GetNextTo(T entry);
        T GetPreviousTo(T entry);
        void Remove(T entry);
        void Update(T entry);

        #endregion Public Methods

    }
}
