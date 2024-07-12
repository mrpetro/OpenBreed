using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Interface
{
    public interface IReadonlyRepository
    {
        #region Public Properties

        IEnumerable<IDbEntry> Entries { get; }
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        IDbEntry Find(string name);

        #endregion Public Methods
    }

    public interface IRepository : IReadonlyRepository
    {
        #region Public Properties

        IEnumerable<Type> EntryTypes { get; }

        #endregion Public Properties

        #region Public Methods

        IDbEntry New(string newId, Type entryType = null);

        bool Remove(IDbEntry entry);

        #endregion Public Methods
    }

    public interface IReadonlyRepository<TEntry> : IReadonlyRepository where TEntry : IDbEntry
    {
        #region Public Methods

        TEntry GetById(string id);

        TEntry GetNextTo(TEntry entry);

        TEntry GetPreviousTo(TEntry entry);

        #endregion Public Methods
    }

    public interface IRepository<TEntry> : IRepository, IReadonlyRepository<TEntry> where TEntry : IDbEntry
    {
        #region Public Methods

        void Add(TEntry entry);

        bool Remove(TEntry entry);

        void Update(TEntry entry);

        #endregion Public Methods
    }
}