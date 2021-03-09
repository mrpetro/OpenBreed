using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Interface
{
    public interface IReadonlyRepository
    {
        #region Public Properties

        IEnumerable<IEntry> Entries { get; }
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        IEntry Find(string name);

        #endregion Public Methods
    }

    public interface IRepository : IReadonlyRepository
    {
        #region Public Properties

        IEnumerable<Type> EntryTypes { get; }

        #endregion Public Properties

        #region Public Methods

        IEntry New(string newId, Type entryType = null);

        #endregion Public Methods
    }

    public interface IReadonlyRepository<TEntry> : IReadonlyRepository where TEntry : IEntry
    {
        #region Public Methods

        TEntry GetById(string id);

        TEntry GetNextTo(TEntry entry);

        TEntry GetPreviousTo(TEntry entry);

        #endregion Public Methods
    }

    public interface IRepository<TEntry> : IRepository, IReadonlyRepository<TEntry> where TEntry : IEntry
    {
        #region Public Methods

        void Add(TEntry entry);

        void Remove(TEntry entry);

        void Update(TEntry entry);

        #endregion Public Methods
    }
}