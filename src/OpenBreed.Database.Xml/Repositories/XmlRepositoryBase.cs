using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Database.Xml.Repositories
{
    public abstract class XmlRepositoryBase<T> : IRepository<T> where T : class, IEntry
    {
        #region Protected Fields

        protected XmlDatabaseMan context;

        #endregion Protected Fields

        #region Protected Constructors

        protected XmlRepositoryBase(XmlDatabaseMan context)
        {
            this.context = context;
        }

        #endregion Protected Constructors

        #region Protected Methods

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

        public abstract void Add(T newEntry);

        public IEntry New(string newId, Type entryType = null)
        {
            if (Find(newId) != null)
                throw new Exception($"Entry with Id '{newId}' already exist.");

            if (entryType == null)
                entryType = EntryTypes.FirstOrDefault();

            var newEntry = Create(entryType);

            newEntry.Id = newId;
            Add(newEntry);
            return newEntry;
        }

        public void Remove(T entry)
        {
            throw new NotImplementedException();
        }

        public void Update(T entry)
        {
            var index = GetIndexOf(entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            ReplaceEntryWithIndex(index, entry);
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


        public IEntry Find(string id)
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

        public abstract string Name { get; }

        public abstract IEnumerable<Type> EntryTypes { get; }

        public abstract IEnumerable<IEntry> Entries { get; }

        public abstract int Count { get; }

        protected abstract T GetEntryWithIndex(int index);

        protected abstract int GetIndexOf(T entry);

        protected abstract void ReplaceEntryWithIndex(int index, T newEntry);

        protected T Create(Type type)
        {
            return Activator.CreateInstance(type) as T;
        }

        #endregion Protected Methods
    }
}