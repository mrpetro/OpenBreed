using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Database.Xml.Repositories
{
    public abstract class XmlReadonlyRepositoryBase<T> : IReadonlyRepository<T> where T : class, IDbEntry
    {
        #region Public Properties

        public abstract string Name { get; }

        public abstract IEnumerable<Type> EntryTypes { get; }

        public abstract IEnumerable<IDbEntry> Entries { get; }

        public abstract int Count { get; }

        #endregion Public Properties

        #region Public Methods

        public T GetNextTo(T entry)
        {
            var index = GetIndexOf(entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < Count)
                return GetEntryWithIndex(index);
            else
                return null;
        }

        public T GetPreviousTo(T entry)
        {
            var index = GetIndexOf(entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return GetEntryWithIndex(index);
            else
                return null;
        }

        public IDbEntry Find(string id)
        {
            return Entries.FirstOrDefault(item => item.Id == id);
        }

        public T GetById(string id)
        {
            var entry = Entries.FirstOrDefault(item => item.Id == id);
            if (entry == null)
                throw new Exception("No entry found with ID: " + id);

            return entry as T;
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract T GetEntryWithIndex(int index);

        protected abstract int GetIndexOf(T entry);

        #endregion Protected Methods
    }


}